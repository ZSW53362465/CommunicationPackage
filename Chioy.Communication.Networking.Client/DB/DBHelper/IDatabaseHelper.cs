using System.Data;
using System.Data.Common;

namespace Chioy.Communication.Networking.Client.DB.DBHelper
{
    public interface IDatabaseHelper
    {
        void Reset(string p_connectionString);

        DataTable ExecuteQuery(string p_sql);

        DataTable ExecuteQuery(string p_sql, params DbParameter[] p_parameters);

        int ExecuteNonQuery(string p_sql, params DbParameter[] p_parameters);

        bool TestConnection();

        DataTable GetPatientInfoByProcedure(string productid);

        void UploadResultByProcedure(string productid, string check_id, string result);
    }
}