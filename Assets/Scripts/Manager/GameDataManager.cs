using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : BaseMonoManager<GameDataManager>
{
    private void Start()
    {
        if (Instance == this)
        {
            LoadAllData();
        }
    }

    public void ReloadAllData()
    {
        LoadAllData();
    }

    private void LoadAllData()
    {
        LoadDialogueData("Dialogue");
    }

    [Serializable]
    private class SerializableWrapper<T>
    {
        public List<T> _data;
    }

    public Dictionary<string, DialogueData> DialogueDataList { get; private set; } = new Dictionary<string, DialogueData>();

    private Dictionary<string, T> LoadData<T>(string path) where T : GameDataBase
    {
        string resourcePath = $"JsonOutput/{path}";
        TextAsset textAsset = LoadUtil.Sync.LoadTextAsset(resourcePath);
        if (textAsset == null)
        {
            this.LogError($"{resourcePath} 경로에 리소스가 없습니다! 다시 확인해주세요!!");
            return new Dictionary<string, T>();
        }

        try
        {
            string jsonData = textAsset.text;
            string wrapperData = "{\"m_data\":" + jsonData + "}";
            SerializableWrapper<T> wrapper = JsonUtility.FromJson<SerializableWrapper<T>>(wrapperData);
            if (wrapper._data != null)
            {
                Debug.Log($"{resourcePath}의 데이터가 {wrapper._data.Count}만큼 로드 되었습니다!!");
                Dictionary<string, T> newDictionary = new(wrapper._data.Count);
                foreach (T data in wrapper._data)
                {
                    newDictionary.Add(data.ID, data);
                }
                return newDictionary;
            }
            else
            {
                this.LogError($"{resourcePath}의 데이터가 없습니다 다시 확인해주세요!!");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return new Dictionary<string, T>();
    }

    public void LoadDialogueData(string tableName)
    {
        DialogueDataList = LoadData<DialogueData>(tableName);
    }

    public DialogueData GetDialogueData(string id)
    {
        if (DialogueDataList == null || string.IsNullOrEmpty(id))
        {
            return null;
        }

        return DialogueDataList.TryGetValue(id, out var data) ? data : null;
    }
}
