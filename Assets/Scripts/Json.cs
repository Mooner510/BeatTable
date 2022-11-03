using System.IO;
using System.Text;
using UnityEngine;

public static class Json {
    public static void CreateJsonFile(string fileName, object obj) {
        var fileStream = new FileStream($"{fileName}.json", FileMode.Create);
        var json = JsonUtility.ToJson(obj);
        var data = Encoding.UTF8.GetBytes(json);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public static T LoadJsonFile<T>(string fileName) {
        var fileStream = new FileStream($"{fileName}.json", FileMode.Open);
        var data = new byte[fileStream.Length]; 
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        var jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
}