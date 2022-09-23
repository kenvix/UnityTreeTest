using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;


public class GlobalGameSystem
{
    public static readonly GlobalGameSystem Instance = new();

    // 建立连接
    private SqliteConnection connection;

    private GlobalGameSystem()
    {
        Debug.Log("GlobalGameSystem: Global game system script initialized");

        var DBFileName = $"{Application.streamingAssetsPath}/ExportedData.db";
        var DBPath = $"data source={DBFileName};FailIfMissing=false;Version=3";

        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);
        if (!File.Exists(DBFileName))
            Debug.LogError($"SqlError: {DBFileName} NOT EXIST!!!");
                
        connection = new(DBPath);
        connection.ConnectionString = DBPath;
        connection.Open();
        Debug.Log("Loaded SQLITE database // " + DBPath);

        // 实例化一个Command
        using var command = connection.CreateCommand();
        // TEST: COUNT TREES
        command.CommandText = "select count(*) as cnt from main.trees;";
        using SqliteDataReader reader = command.ExecuteReader();
        reader.Read();
        Debug.Log(" Current tree num: " + reader.GetInt32(0));
    }

    public SqliteCommand CreateCommand()
    {
        return connection.CreateCommand();
    }

    public void AddTreeItem(SqliteCommand command, string name = "", int dataSetType = 0)
    {
        command.CommandText = "insert into trees (name, triangles, vertices, datasetType, trunk_seed, trunk_length, trunk_radius, trunk_resolution, trunk_axis, trunk_randomness, branch_seed, branch_length, branch_number, branch_resolution, branch_split_proba, branch_randomness, branch_angle, branch_up_attraction, branch_start)" +
                              " values (@name, @triangles, @vertices, @datasetType, @trunk_seed, @trunk_length, @trunk_radius, @trunk_resolution, @trunk_axis, @trunk_randomness, @branch_seed, @branch_length, @branch_number, @branch_resolution, @branch_split_proba, @branch_randomness, @branch_angle, @branch_up_attraction, @branch_start);";
        command.CommandType = CommandType.Text;
        command.Parameters.Add(new SqliteParameter("@name", name));
        command.Parameters.Add(new SqliteParameter("@datasetType", dataSetType));

        command.ExecuteNonQuery();
    }

    ~GlobalGameSystem()
    {
        connection.Close();
    }
}
