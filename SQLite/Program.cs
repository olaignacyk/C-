using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Data.Sqlite;

class Program
{
    static (List<List<string>>, List<string>) ReadCSVFile(string filePath, char separator)
    {
        List<List<string>> data = new List<List<string>>();
        List<string> columnNames = new List<string>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine();
            if (headerLine != null)
            {
                columnNames.AddRange(headerLine.Split(separator));
            }

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                List<string> row = new List<string>(line.Split(separator));
                data.Add(row);
            }
            return (data, columnNames);
        }
    }

    static Dictionary<string, (string Type, bool CanBeNull)> GetColumnTypes(List<List<string>> data, List<string> columnNames)
    {
        Dictionary<string, (string Type, bool CanBeNull)> columnTypes = new Dictionary<string, (string, bool)>();

        foreach (var columnName in columnNames)
        {
            int intCount = 0;
            int doubleCount = 0;
            int nullCount = 0;

            int columnIndex = columnNames.IndexOf(columnName);
            foreach (var row in data)
            {
                string value = row[columnIndex];

                if (value == null)
                {
                    nullCount++;
                }
                else if ( value== ""){
                    nullCount++;
                }
                else if (int.TryParse(value, out _))
                {
                    intCount++;
                }
                else if (double.TryParse(value, out _))
                {
                    doubleCount++;
                }
            }

            string columnType;
            bool canBeNull;

            if (intCount == data.Count - nullCount)
            {
                columnType = "INTEGER";
                canBeNull = nullCount > 0;
            }
            else if (doubleCount == data.Count - nullCount)
            {
                columnType = "REAL";
                canBeNull = nullCount > 0;
            }
            else if (nullCount == data.Count - nullCount){
                columnType ="NULL";
                canBeNull = nullCount > 0;  
            }
            
            else{
                columnType = "TEXT";
                canBeNull = nullCount > 0;
            }


            columnTypes.Add(columnName, (columnType, canBeNull));
        }

        return columnTypes;
    }

    static void CreateTableFromCSVData(List<List<string>> csvData, List<string> columnNames, string tableName, SqliteConnection connection)
{
    StringBuilder createTableQuery = new StringBuilder();
    createTableQuery.AppendFormat("CREATE TABLE IF NOT EXISTS {0} (", tableName);
    for (int i = 0; i < columnNames.Count; i++)
    {
        string columnName = columnNames[i];
        var columnType = GetColumnTypes(csvData, columnNames)[columnName];
        createTableQuery.AppendFormat("{0} {1}", columnName, columnType.Type);

        if (i < columnNames.Count - 1)
        {
            createTableQuery.Append(", ");
        }
    }

    createTableQuery.Append(");");

    using (SqliteCommand command = new SqliteCommand(createTableQuery.ToString(), connection))
    {
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }
}

static void FillTableWithData(List<List<string>> data, string tableName, SqliteConnection connection)
{
    StringBuilder insertQuery = new StringBuilder();
    insertQuery.AppendFormat("INSERT INTO {0} VALUES ", tableName);

    for (int i = 0; i < data.Count; i++)
    {
        insertQuery.Append("(");
        for (int j = 0; j < data[i].Count; j++)
        {
            if (data[i][j] == "")
            {
                insertQuery.Append("NULL");
            }
            else
            {
                insertQuery.AppendFormat("'{0}'", data[i][j]); 
            }

            if (j < data[i].Count - 1)
            {
                insertQuery.Append(", ");
            }
        }
        insertQuery.Append(")");

        if (i < data.Count - 1)
        {
            insertQuery.Append(", ");
        }
    }

    using (SqliteCommand command = new SqliteCommand(insertQuery.ToString(), connection))
    {
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }
}

static void PrintTableData(string tableName, SqliteConnection connection)
{
    string query = $"SELECT * FROM {tableName} WHERE FirstName IS NULL";

    using (SqliteCommand command = new SqliteCommand(query, connection))
    {
        connection.Open();
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(reader.GetName(i) + "\t");
            }
            Console.WriteLine();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetValue(i).ToString() + "\t");
                }
                Console.WriteLine();
            }
        }
        connection.Close();
    }
}



    public static void Main(string[] args)
    {
        string filePath = "test.csv";
        char separator = ',';
        var result = ReadCSVFile(filePath, separator);
        foreach (var row in result.Item1)
        {
            foreach (var item in row)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("Nazwy kolumn:");
        foreach (var columnName in result.Item2)
        {
            Console.WriteLine(columnName);
        }

        var csvData = result.Item1;
        var columnNames = result.Item2;

        var res = GetColumnTypes(csvData, columnNames);
        foreach (var column in res)
        {
            Console.WriteLine(column);
        }

        string tableName = "People";
        string connectionString = "Data Source=Baza_.db"; 

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            CreateTableFromCSVData(csvData, columnNames, tableName, connection);
            FillTableWithData(csvData, tableName, connection);
            PrintTableData(tableName, connection);
        }
    }
}
