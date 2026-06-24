using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : BaseMonoManager<SaveManager>
{
    public PlayerModel CurrentPlayerModel { get; private set; } = new PlayerModel();
    public string CurrentSlotIndex { get; private set; }
    public HashSet<string> SlotIndex { get; private set; } = new HashSet<string>();

    public Action OnSaveClear;

    // 저장
    public void SaveData()
    {
        RequestSaveData(CurrentSlotIndex, CurrentPlayerModel);
        SlotIndex.Add(CurrentSlotIndex);
    }

    // 로드 게임
    public void LoadData(string index)
    {
        CurrentPlayerModel = RequestLoadData(index);
    }

    public void LoadDefaultData()
    {
        CurrentPlayerModel = GetDefaultData();
    }

    // 현재 슬롯 인덱스로 시작
    public void SetCurrentSaveIndex(string index)
    {
        CurrentSlotIndex = index;
    }

    // 처음 시작 시
    public void LoadOrCreatePlayerData(string index)
    {
        SetCurrentSaveIndex(index);

        if (HasSaveFile(index))
        {
            LoadData(index);
        }
        else
        {
            LoadDefaultData();
            SaveData();
        }

        GameManager.Instance.StartGame();
    }


    // 파일 주소 가져오기
    private string GetPath(string slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"GOAT{slotIndex}.json");
    }

    // 세이브 요청
    public void RequestSaveData(string slotIndex, PlayerModel data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slotIndex), json);
    }

    // 가져오기 요청
    public PlayerModel RequestLoadData(string slotIndex)
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

    // 기본 데이터
    public PlayerModel GetDefaultData()
    {
        var newPlayerData = new PlayerModel();

        newPlayerData.Day = 1;
        newPlayerData.Coin = 0;
        newPlayerData.Stamina = 100;
        newPlayerData.WalkSpeed = 3f;
        newPlayerData.RunSpeed = 5f;

        return newPlayerData;
    }

    // 파일 있는지 없는지 검사
    public bool HasSaveFile(string slotIndex)
    {
        return File.Exists(GetPath(slotIndex));
    }
}