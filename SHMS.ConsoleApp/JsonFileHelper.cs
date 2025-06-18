using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class JsonFileHelper
{
    public static void SaveToFile<T>(List<T> data, string filePath)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public static List<T> LoadFromFile<T>(string filePath)
    {
        if (!File.Exists(filePath)) return new List<T>();
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json);
    }
}
