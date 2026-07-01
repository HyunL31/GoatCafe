using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : BaseMonoManager<SaveManager>
{
    public PlayerModel CurrentPlayerModel { get; private set; } = new PlayerModel();
    public string CurrentSlotIndex { get; private set; }
    public HashSet<string> SlotIndex { get; private set; } = new HashSet<string>();

    public event Action OnSetGoatSpeed;
    public Action OnSetStamina;
    public Action OnSaveClear;

    private void Start()
    {
        GetAllSaveSlots();
    }

    public void GetAllSaveSlots()
    {
        SlotIndex.Clear();

        string path = Application.persistentDataPath;

        if (!Directory.Exists(path))
        {
            return;
        }

        string[] files = Directory.GetFiles(path, "GOAT*.json");

        foreach(string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            if (fileName.StartsWith("GOAT"))
            {
                string slotIndex = fileName.Substring(4);
                SlotIndex.Add(slotIndex);
            }
        }
    }

    // 저장
    public void SaveData()
    {
        if (StoreManager.Instance != null)
        {
            StoreManager.Instance.SaveStoreData();
        }

        RequestSaveData(CurrentSlotIndex, CurrentPlayerModel);

        SlotIndex.Add(CurrentSlotIndex);
        Debug.Log("데이터 저장");
    }

    // 로드 게임
    public void LoadData(string index)
    {
        CurrentPlayerModel = RequestLoadData(index);

        OnSetStamina?.Invoke();
        OnSetGoatSpeed?.Invoke();
        GameManager.Instance.InitDuration();

        if (StoreManager.Instance != null)
        {
            StoreManager.Instance.InitItemEffect();
            StoreManager.Instance.LoadSaveStore();
        }

        Debug.Log("데이터 로드");
    }

    public void LoadDefaultData()
    {
        CurrentPlayerModel = GetDefaultData();
        OnSetStamina?.Invoke();
        OnSetGoatSpeed?.Invoke();
        GameManager.Instance.InitDuration();
        StoreManager.Instance.InitItemEffect();
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
            Debug.Log($"{CurrentPlayerModel.Day}, {CurrentPlayerModel.Coin}, {CurrentPlayerModel.Stamina}");
        }
        else
        {
            LoadDefaultData();
            SaveData();
            Debug.Log($"{CurrentPlayerModel.Day}, {CurrentPlayerModel.Coin}, {CurrentPlayerModel.Stamina}");
        }
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
    public void DeleteSaveData(string slotIndex)
    {
        string path = GetPath(slotIndex);

        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
                Debug.Log($"[SaveManager] 세이브 파일 삭제 완료: {path}");
            }
            catch (IOException e)
            {
                Debug.LogError($"[SaveManager] 파일 삭제 중 오류 발생: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"[SaveManager] 삭제할 세이브 파일이 존재하지 않습니다: {path}");
        }
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
        newPlayerData.Coin = 5000;
        newPlayerData.StolenItemCount = 0;
        newPlayerData.Stamina = 100;
        newPlayerData.WalkSpeed = 3f;
        newPlayerData.RunSpeed = 5f;
        newPlayerData.Ending = "";

        return newPlayerData;
    }

    // 파일 있는지 없는지 검사
    public bool HasSaveFile(string slotIndex)
    {
        return File.Exists(GetPath(slotIndex));
    }

    public void RemoveSlotIndex(string slotName)
    {
        if(SlotIndex.Contains(slotName))
        {
            SlotIndex.Remove(slotName);
        }
        else
        {
            Debug.Log($"[SaveManager] {slotName} 는 SlotIndex에 존재하지 않습니다.");
        }
    }

    public void SavePlayerPoint(int point)
    {
        if (CurrentPlayerModel == null)
        {
            return;
        }

        CurrentPlayerModel.Coin += point;

        if(CurrentPlayerModel.Coin <= 0)
        {
            CurrentPlayerModel.Coin = 0;
        }
    }
}