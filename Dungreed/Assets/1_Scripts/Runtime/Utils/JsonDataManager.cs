using System.IO;
using System.Text;
using UnityEngine;

public class JsonDataManager : Singleton<JsonDataManager>
{
    private static readonly string ExtensionName = ".json";

    new protected void Awake()
    {
        base.Awake();
    }

    public void SaveToJson(string path, string fileName, object obj)
    {
        Debug.Log(Path.Combine(Application.dataPath, path, fileName + ExtensionName));
        File.WriteAllText(Path.Combine(Application.dataPath, path, fileName + ExtensionName), JsonUtility.ToJson(obj, true));
    }

    public bool LoadFromJson<T>(string path, string fileName, out T val)
    {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, path, fileName + ExtensionName), Encoding.UTF8);

        val = JsonUtility.FromJson<T>(json);
        if (val is T) return true;

        return false;
    }
}
