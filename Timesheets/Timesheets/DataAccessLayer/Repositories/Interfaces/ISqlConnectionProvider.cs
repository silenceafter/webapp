using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheets.DataAccessLayer.Interfaces
{
    public interface ISqlConnectionProvider
    {
        public string GetConnectionString();
    }
}
