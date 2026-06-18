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
    }

    [Serializable]
    private class SerializableWrapper<T>
    {
        public List<T> _data;
    }

    private Dictionary<string, T> LoadData<T>(string path) where T : GameDataBase
    {
        string resourcePath = $"Json/{path}";
        TextAsset textAsset = LoadUtil.Sync.LoadTextAsset(resourcePath);
        if (textAsset == null)
        {
            this.LogError($"{resourcePath} 경로에 리소스가 없습니다! 다시 확인해주세요!!");
            return null;
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
                    newDictionary.Add(data.Id, data);
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
            return null;
        }
    }
}
