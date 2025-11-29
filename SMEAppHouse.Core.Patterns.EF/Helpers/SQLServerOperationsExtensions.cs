using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SMEAppHouse.Core.Patterns.EF.Helpers;

/// <summary>
/// Extension methods for executing raw SQL queries against SQL Server databases.
/// </summary>
public static class SQLServerOperationsExtensions
{
    /// <summary>
    /// Default command timeout in seconds for SQL queries.
    /// </summary>
    private const int DefaultCommandTimeout = 60;
    /// <summary>
    /// Executes a raw SQL query asynchronously and returns the results as a list of objects.
    /// Uses default timeout of 60 seconds and creates a new connection.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <param name="database">The database facade.</param>
    /// <param name="sqlQry">The SQL query to execute.</param>
    /// <param name="sqlCmdType">The command type (Text, StoredProcedure, etc.).</param>
    /// <returns>A list of objects of type T.</returns>
    /// <exception cref="ArgumentNullException">Thrown when database or sqlQry is null.</exception>
    public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database,
        string sqlQry,
        CommandType sqlCmdType)
        where T : class
    {
        return await SqlQueryAsync<T>(database, sqlQry, sqlCmdType, DefaultCommandTimeout, null, true);
    }

    /// <summary>
    /// Executes a raw SQL query asynchronously with parameters and returns the results as a list of objects.
    /// Uses default timeout of 60 seconds and creates a new connection.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <param name="database">The database facade.</param>
    /// <param name="sqlQry">The SQL query to execute.</param>
    /// <param name="sqlCmdType">The command type (Text, StoredProcedure, etc.).</param>
    /// <param name="sqlQryParams">The SQL parameters to use with the query.</param>
    /// <returns>A list of objects of type T.</returns>
    /// <exception cref="ArgumentNullException">Thrown when database or sqlQry is null.</exception>
    public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database,
        string sqlQry,
        CommandType sqlCmdType,
        SqlParameter[] sqlQryParams)
        where T : class
    {
        return await SqlQueryAsync<T>(database, sqlQry, sqlCmdType, DefaultCommandTimeout, sqlQryParams, true);
    }

    /// <summary>
    /// Executes a raw SQL query asynchronously with full control over timeout, parameters, and connection reuse.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <param name="database">The database facade.</param>
    /// <param name="sqlQry">The SQL query to execute.</param>
    /// <param name="sqlCmdType">The command type (Text, StoredProcedure, etc.).</param>
    /// <param name="sqlCmdTimeout">The command timeout in seconds.</param>
    /// <param name="sqlQryParams">The SQL parameters to use with the query.</param>
    /// <param name="renewConn">If true, creates a new connection; otherwise, reuses the existing connection.</param>
    /// <returns>A list of objects of type T.</returns>
    /// <exception cref="ArgumentNullException">Thrown when database or sqlQry is null.</exception>
    public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade database,
        string sqlQry,
        CommandType sqlCmdType,
        int sqlCmdTimeout,
        SqlParameter[] sqlQryParams,
        bool renewConn = true) where T : class
    {
        if (database == null)
            throw new ArgumentNullException(nameof(database));
        if (string.IsNullOrWhiteSpace(sqlQry))
            throw new ArgumentNullException(nameof(sqlQry));

        var conn = (SqlConnection)database.GetDbConnection();

        if (renewConn)
        {
            if (string.IsNullOrWhiteSpace(conn.ConnectionString))
                throw new InvalidOperationException("Connection string is not available.");

            conn = new SqlConnection(conn.ConnectionString);
        }

        return await SqlServerUtil.SqlGetObjectList<T>(conn, sqlQry, sqlCmdType, sqlCmdTimeout,
            sqlQryParams);
    }
}