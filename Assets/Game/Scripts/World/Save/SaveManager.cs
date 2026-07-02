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
        string path = GetPath();
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"[SaveManager] Jogo salvo com sucesso em: {path}");
    }

    public static GameSaveData Load()
    {
        string path = GetPath();

        if (!File.Exists(path))
        {
            Debug.Log($"[SaveManager] Nenhum save encontrado em: {path}. Criando novo save.");
            return new GameSaveData();
        }

        Debug.Log($"[SaveManager] Carregando save de: {path}");
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameSaveData>(json) ?? new GameSaveData();
    }

    public static void Delete()
    {
        string path = GetPath();

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[SaveManager] Save deletado com sucesso em: {path}");
        }
    }
}
