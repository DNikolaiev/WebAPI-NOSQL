using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces
{
    public interface IDatabaseSettings
    {
        string Database { get; set; }
        string ConnectionString { get; set; }
        string Container { get; set; }
        bool IsContained { get; set; }
        bool Development { get; set; }
    }
}
