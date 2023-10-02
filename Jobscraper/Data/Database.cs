using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Jobscraper.Data
{
    public class Database
    {
        public const SQLiteConnection connection = null;

        public Database()
        {
            SQLiteConnection _connection = new SQLiteConnection("data.db");
            _connection.CreateTable<Ad>();
        }

        public bool ContainsAd(string URL)
        {
            return false;
        }
    }
}
