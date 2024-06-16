using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;


public class QueryHandler
{
    private static SqlConnection _sqlConnection;
    private static SqlCommand _sqlCommand;
    private static SqlDataAdapter _sqlDataAdapter;
    private static DataTable _dataTable;

    public bool ValidateConnection(string connectionString)
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.State == ConnectionState.Open;
            }
        }
        catch (Exception ex)
        {
            LogError(ex, true);
            return false;
        }
    }

    public object ExecuteQuery(string query, string connectionString, bool returnSingleRow = true, int timeout = 3000)
    {
        try
        {
            if (!ValidateConnection(connectionString))
            {
                LogMessage("Failed to establish connection. Please validate the server connection and try again.", true);
                return null;
            }

            _sqlConnection = new SqlConnection(connectionString);
            _sqlCommand = new SqlCommand(query, _sqlConnection)
            {
                CommandType = CommandType.Text,
                CommandTimeout = timeout
            };
            _sqlConnection.Open();
            _sqlCommand.ExecuteNonQuery();

            _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
            _dataTable = new DataTable();
            _sqlDataAdapter.Fill(_dataTable);

            if (returnSingleRow)
            {
                if (_dataTable.Rows.Count > 0)
                {
                    return _dataTable.Rows[0];
                }
            }
            else
            {
                return _dataTable;
            }

            _sqlConnection.Close();
            _sqlConnection.Dispose();
            _sqlCommand.Dispose();
        }
        catch (Exception ex)
        {
            LogError(ex, true);
            return null;
        }

        return null;
    }

    public void ResetID(string tableName, string connectionString)
    {
        ExecuteQuery($"DBCC CHECKIDENT('{tableName}')", connectionString, false);
    }

    public DataTable ExecuteQueryToDataTable(string query, string connectionString)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(query, conn);

                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                return dt;
            }
        }
        catch (Exception ex)
        {
            LogError(ex, true);
            return null;
        }
    }

    public DataRow ExecuteQueryToDataRow(string query, string connectionString, int timeout = 0)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(query, conn)
                {
                    CommandTimeout = timeout
                };

                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
                return null;
            }
        }
        catch (Exception ex)
        {
            LogError(ex, true);
            return null;
        }
    }

    public DataTable ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters, string connectionString)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    var dt = new DataTable();
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                    return dt;
                }
            }
        }
        catch (Exception ex)
        {
            LogError(ex, true);
            return null;
        }
    }

    public int ExecuteBatchQuery(string query, SqlParameter[] parameters, string connectionString)
    {
        try
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    int affectedRows = cmd.ExecuteNonQuery();
                    conn.Close();
                    return affectedRows;
                }
            }
        }
        catch (Exception ex)
        {
            LogError(ex, true);
            return -1;
        }
    }

    private void LogError(Exception ex, bool displayError = true, string title = "Error", string module = "QueryHandler")
    {
        // Implement error logging logic here (e.g., write to a file, database, etc.)
        string logFilePath = "error_log.txt";
        string errorMessage = $"{DateTime.Now}: {module} - {title}: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";

        File.AppendAllText(logFilePath, errorMessage);

        if (displayError)
        {
            LogMessage($"{title}: {ex.Message}", true);
        }
    }

    private void LogMessage(string message, bool displayMessage = false)
    {
        // Implement message logging logic here (e.g., write to a file, database, etc.)
        string logFilePath = "log.txt";
        string logMessage = $"{DateTime.Now}: {message}{Environment.NewLine}";

        File.AppendAllText(logFilePath, logMessage);

        if (displayMessage)
        {
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

