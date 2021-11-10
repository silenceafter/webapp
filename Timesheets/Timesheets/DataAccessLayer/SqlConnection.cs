using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;

namespace Timesheets.DataAccessLayer
{
    public class SqlConnection : ISqlConnectionProvider
    {
        private const string _connectionString = "Host=localhost;Port=5432;Database=TimesheetDB;User Id=postgres;Password=bdw";//Data Source=metricsManager.db;Version=3;Pooling=true;Max Pool Size=100;";
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}