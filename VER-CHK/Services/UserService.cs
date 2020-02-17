using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VER_CHK.Helpers;
using VER_CHK.Interfaces;
using VER_CHK.Interfaces.Users;
using VER_CHK.Models.Users;

namespace VER_CHK.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _context;

        public UserService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _context = database.GetCollection<User>(settings.CollectionName);
        }

        public async Task<User> Authenticate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            var user = await (await _context.FindAsync(x => x.UserName == userName)).FirstOrDefaultAsync();

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public async Task<List<User>> GetAll() =>
            await (await _context.FindAsync(user => user.UserName != null)).ToListAsync();

        public async Task<User> Get(string userName) =>
           await (await _context.FindAsync(user => user.UserName == userName)).FirstOrDefaultAsync();

        public async Task<User> Create(User user, string password)
        {
            //validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (await ((await _context.FindAsync(x => x.UserName == user.UserName)).FirstOrDefaultAsync()) != null)
                throw new AppException("UserName \"" + user.UserName + "\" is already taken");

            if (await ((await _context.FindAsync(x => x.Email == user.Email)).FirstOrDefaultAsync()) != null)
                throw new AppException("Email \"" + user.Email + "\" is already taken");

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.InsertOneAsync(user);
            return user;
        }

        public async Task Update(User userParam, string password = null)
        {
            var user = await (await _context.FindAsync(x => x.UserName == userParam.UserName)).FirstOrDefaultAsync();

            if (user == null)
                throw new AppException("User not found");

            // update email if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Email))
            {
                // throw error if the new email is already taken
                if ((await (await _context.FindAsync(x => x.Email == userParam.Email)).FirstOrDefaultAsync()) != null)
                    throw new AppException("Email \"" + user.Email + "\" is already taken");

                user.Email = userParam.Email;
            }

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            await _context.ReplaceOneAsync(user => user.UserName == userParam.UserName, user);
        }

        public async Task Delete(string userName) =>
            await _context.DeleteOneAsync(user => user.UserName == userName);

        // private helper methods
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
