using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VanSon.Connection
{
    class connection_database
    {
        public string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        public OleDbConnection conn = new OleDbConnection();
        public void connection_project()
        {
            conn.ConnectionString = connectionSTR;
        }
    }
}
