using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Common
{
    public enum ProductType
    {
        BMD = 0,
        KRTCD
    }
    public enum DatabaseEnum
    {
        SQLServer,
        Oracle,
        MySql,
        OleDb,
        PostgreSQL
    }
    public enum BindingType
    {
        TCP,
        HTTP,
    }
    public enum Protocol
    {
        WebService,
        Ftp,
        Http,
        DB,
        Wcftcp
    }
}
