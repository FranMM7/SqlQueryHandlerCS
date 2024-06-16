Sure, I'll update the `README` file to reflect the new project name, `SqlQueryHandler`.

# SqlQueryHandler

SqlQueryHandler is a robust and efficient C# library for handling SQL queries and stored procedures. This library provides functionalities for executing SQL queries, validating connections, executing stored procedures, and logging errors or messages.

## Features

- Validate SQL Server connections.
- Execute SQL queries and return results as `DataTable` or `DataRow`.
- Execute SQL stored procedures with parameters.
- Execute batch queries with parameters.
- Log errors and messages with an option to display them to the user.

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/frankmejia7/SqlQueryHandler-.git
    ```

2. Open the project in Visual Studio.

3. Build the project to generate the `SqlQueryHandler.dll`.

4. Reference the `SqlQueryHandler.dll` in your .NET project.

## Usage

### 1. Validate Connection

```csharp
using SqlQueryHandler;

var queryHandler = new QueryHandler();
bool isConnected = queryHandler.ValidateConnection("YourConnectionStringHere");

if (isConnected)
{
    Console.WriteLine("Connection successful.");
}
else
{
    Console.WriteLine("Failed to connect.");
}
```

### 2. Execute SQL Query

```csharp
using SqlQueryHandler;

var queryHandler = new QueryHandler();
string query = "SELECT * FROM YourTable";
var result = queryHandler.ExecuteQuery(query, "YourConnectionStringHere");

if (result is DataTable dt)
{
    Console.WriteLine($"Rows returned: {dt.Rows.Count}");
}
else if (result is DataRow dr)
{
    Console.WriteLine($"First row data: {dr[0]}");
}
```

### 3. Execute Stored Procedure

```csharp
using System.Data;
using System.Data.SqlClient;
using SqlQueryHandler;

var queryHandler = new QueryHandler();
string storedProcedure = "YourStoredProcedureName";
SqlParameter[] parameters = {
    new SqlParameter("@ParameterName", SqlDbType.VarChar) { Value = "ParameterValue" }
};

DataTable result = queryHandler.ExecuteStoredProcedure(storedProcedure, parameters, "YourConnectionStringHere");

Console.WriteLine($"Stored Procedure returned: {result.Rows.Count} rows");
```

### 4. Execute Batch Query

```csharp
using System.Data;
using System.Data.SqlClient;
using SqlQueryHandler;

var queryHandler = new QueryHandler();
string query = "UPDATE YourTable SET Column1 = @Param1 WHERE Column2 = @Param2";
SqlParameter[] parameters = {
    new SqlParameter("@Param1", SqlDbType.Int) { Value = 1 },
    new SqlParameter("@Param2", SqlDbType.VarChar) { Value = "Value" }
};

int rowsAffected = queryHandler.ExecuteBatchQuery(query, parameters, "YourConnectionStringHere");

Console.WriteLine($"Batch query affected: {rowsAffected} rows");
```

### 5. Log Messages and Errors

```csharp
using SqlQueryHandler;

var queryHandler = new QueryHandler();

// Log a simple message
queryHandler.LogMessage("This is a log message.", displayMessage: true);

// Log an error
try
{
    // Some code that throws an exception
}
catch (Exception ex)
{
    queryHandler.LogError(ex, displayError: true);
}
```

## Contributing

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -am 'Add some feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Create a new Pull Request.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

This `README` provides a comprehensive guide for using the SqlQueryHandler library. If you have any issues or questions, feel free to open an issue on the GitHub repository or contact the maintainer.
