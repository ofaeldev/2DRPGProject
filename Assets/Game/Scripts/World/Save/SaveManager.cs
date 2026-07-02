using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private const string FileName = "savegame.json";

    private static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, FileName);
    }

    public static void Save(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), json);
    }

    public static GameSaveData Load()
    {
        string path = GetPath();

        if (!File.Exists(path))
            return new GameSaveData();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameSaveData>(json) ?? new GameSaveData();
    }

    public static void Delete()
    {
        string path = GetPath();

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
