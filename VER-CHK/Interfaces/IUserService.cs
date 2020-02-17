using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VER_CHK.Models.Users;

namespace VER_CHK.Interfaces.Users
{
    public interface IUserService
    {
        Task<User> Authenticate(string userName, string password);
        Task<List<User>> GetAll();
        Task<User> Get(string userName);
        Task<User> Create(User user, string password);
        Task Update(User user, string password = null);
        Task Delete(string userName);
    }
}
