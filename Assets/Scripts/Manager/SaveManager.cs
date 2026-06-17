using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : BaseMonoManager<SaveManager>
{
    public Action OnSaveClear;

    private string GetPath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"GOAT{slotIndex}.json");
    }

    public void RequestSaveData(int slotIndex, PlayerModel data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slotIndex), json);
    }

    public PlayerModel RequestLoadData(int slotIndex)
    {
        string path = GetPath(slotIndex);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerModel data = JsonUtility.FromJson<PlayerModel>(json);

            return data;
        }
        else
        {
            var playerData = GetDefaultData();
            return playerData;
        }
    }

    public PlayerModel GetDefaultData()
    {
        var newPlayerData = new PlayerModel();

        newPlayerData.Day = 1;
        newPlayerData.Gold = 0;

        return newPlayerData;
    }

    public bool HasSaveFile(int slotIndex)
    {
        return File.Exists(GetPath(slotIndex));
    }
}