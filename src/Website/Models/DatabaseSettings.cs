using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class Settings
    {
        public string Database { get; set; }


        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (IsContained && Development) { return Container; }
                return _connectionString;
            }
            set { _connectionString = value; }
        }

        public string Container { get; set; }

        public bool IsContained { get; set; }

        public bool Development { get; set; }

    }

   
}
