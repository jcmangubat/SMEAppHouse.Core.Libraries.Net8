using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SMEAppHouse.Core.Patterns.EF.Helpers;

public static class SQLServerOperationsExtensions
{
    public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database,
        string sqlQry,
        CommandType sqlCmdType)
        where T : class
    {
        return await SqlQueryAsync<T>(database, sqlQry, sqlCmdType, 60, null, true);
    }

    public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database,
        string sqlQry,
        CommandType sqlCmdType,
        SqlParameter[] sqlQryParams)
        where T : class
    {
        return await SqlQueryAsync<T>(database, sqlQry, sqlCmdType, 60, sqlQryParams, true);
    }

    public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database,
        string sqlQry,
        CommandType sqlCmdType,
        int sqlCmdTimeout,
        SqlParameter[] sqlQryParams,
        bool renewConn = true) where T : class
    {
        var conn = (SqlConnection)database.GetDbConnection();

        if (renewConn)
            conn = new SqlConnection(conn.ConnectionString);

        return await SqlServerUtil.SqlGetObjectList<T>(conn, sqlQry, sqlCmdType, sqlCmdTimeout,
            sqlQryParams);
    }
}