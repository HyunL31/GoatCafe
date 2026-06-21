using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDataManager : BaseMonoManager<UIDataManager>
{
    public Dictionary<string, MainMenuUIData> MainMenuUIDataList {  get; private set; }

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
        LoadMainMenuUIData();
    }

    private void LoadMainMenuUIData()
    {
        MainMenuUIDataList = LoadData<MainMenuUIData>(DataUtil.UIData.MainMenuUI.File);
    }

    [Serializable]
    private class SerializableWrapper<T>
    {
        public List<T> m_data;
    }

    private Dictionary<string, T> LoadData<T>(string path) where T : UIDataBase
    {
        string resourcePath = $"Json/{path}";
        TextAsset textAsset = LoadUtil.Sync.LoadTextAsset(resourcePath);

        if (textAsset == null)
        {
            this.LogError($"{path} 경로에 리소스가 없습니다! 다시 확인해주세요!!");
            return null;
        }

        try
        {
            string jsonData = textAsset.text;
            string wrapperData = "{\"m_data\":" + jsonData + "}";
            SerializableWrapper<T> wrapper = JsonUtility.FromJson<SerializableWrapper<T>>(wrapperData);
            if (wrapper.m_data != null)
            {
                Debug.Log($"{resourcePath}의 데이터가 {wrapper.m_data.Count}만큼 로드 되었습니다!!");
                Dictionary<string, T> newDictionary = new(wrapper.m_data.Count);
                foreach (T data in wrapper.m_data)
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
