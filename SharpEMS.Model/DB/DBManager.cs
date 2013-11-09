using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqliteORM;


namespace SharpEMS.Model.DB
{
    class DBManager
    {
        static void Main()
        {
            DbConnection.Initialise("data source=database3.db");
        }
    }
}
