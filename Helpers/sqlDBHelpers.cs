using Npgsql;

namespace MODUL_PAA_2043.Helpers
{
    public class SqlDBHelpers
    {
        private NpgsqlConnection connection;
        private string __constr;

        public SqlDBHelpers(string pCOnstr)
        {
            __constr = pCOnstr;
            connection = new NpgsqlConnection();
            connection.ConnectionString = __constr;
        }

        public NpgsqlCommand getNpgsqlCommand(string query)
        {
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = query;
            cmd.CommandType = System.Data.CommandType.Text;
            return cmd;

        }

        public void closeConnection()
        {
            connection.Close();
        }

    }
}
