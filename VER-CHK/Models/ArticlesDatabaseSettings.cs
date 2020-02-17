using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VER_CHK.Interfaces;

namespace VER_CHK.Models
{
    public class ArticlesDatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
