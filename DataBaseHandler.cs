using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PalletTrace
{
    internal class DataBaseHandler
    {
        #region Fields
        private SqlConnection conPallet;
        public string palletConfig = ConfigurationManager.ConnectionStrings["Pallet"].ConnectionString;
        #endregion

        #region PublicMethods
        public void CloseDatabaseConnection()
        {
            if (conPallet.State != 0)
            {
                conPallet.Close();
            }
        }

        public void RunStoredProcedure(string storedProcedureName, string parameterName, int parameter, out DataTable values)
        {
            values = new DataTable();


            OpenDatabaseConnectionCommand(storedProcedureName, out SqlCommand sqlCommand);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = storedProcedureName;
            sqlCommand.Parameters.AddWithValue(parameterName, parameter);


            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(values);

            CloseDatabaseConnection();
        }

        public void RunStoredProcedure(string storedProcedureName, string parameterName, string parameter, out DataTable values)
        {
            values = new DataTable();


            OpenDatabaseConnectionCommand(storedProcedureName, out SqlCommand sqlCommand);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = storedProcedureName;
            sqlCommand.Parameters.AddWithValue(parameterName, parameter);


            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(values);

            CloseDatabaseConnection();
        }
        #endregion

        #region PrivateMethods
        private void OpenDatabaseConnectionCommand(string query, out SqlCommand sqlCommand)
        {
            conPallet = new SqlConnection(palletConfig);
            conPallet.Open();
            sqlCommand = new SqlCommand(query, conPallet);
        }
        #endregion
    }
}
