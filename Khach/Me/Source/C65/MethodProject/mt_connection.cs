using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C65.Method
{
    class mt_connection
    {
        public string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();

        public void Connection()
        {
            connection.ConnectionString = connectionSTR;
        }
    }
}
