using Chioy.Communication.Networking.Common;

namespace Chioy.Communication.Networking.Client.DB.Models
{
    public class DataBaseSoft
    {
        public static DatabaseEnum TransDatabaseSoft(DatabaseSoft p_soft, bool p_isAdvancedSetting = false)
        {
            var e = DatabaseEnum.OleDb;

            if (!p_isAdvancedSetting)
            {
                switch (p_soft)
                {
                    case DatabaseSoft.SQLServer:
                        e = DatabaseEnum.SQLServer;
                        break;
                    case DatabaseSoft.Oracle:
                        e = DatabaseEnum.Oracle;
                        break;
                    case DatabaseSoft.MySql:
                        e = DatabaseEnum.MySql;
                        break;
                    case DatabaseSoft.PostgreSQL:
                        e = DatabaseEnum.PostgreSQL;
                        break;
                }
            }

            return e;
        }
    }
    public enum DatabaseSoft
    {
        SQLServer,
        Oracle,
        MySql,
        PostgreSQL
    }
}
