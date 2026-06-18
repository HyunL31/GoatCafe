using System;
using System.Collections.Generic;

public class GameManager : BaseMonoManager<GameManager>
{
    public PlayerModel PlayerModel { get; private set; } = new PlayerModel();
    public int CurrentSlotIndex { get; private set; } = 0;
    public HashSet<int> SlotIndex {  get; private set; } = new HashSet<int>();

    public Action<int> OnStartGame;

    private void InitSaveSlot()
    {
        for (int i = 0; i < 100; i++)
        {
            if (SaveManager.Instance.HasSaveFile(i))
            {
                SlotIndex.Add(i);
            }
        }

        OnStartGame += (index) =>
        {
            SetCurrentSaveIndex(index);

            if (SaveManager.Instance.HasSaveFile(index))
            {
                LoadData(index);
            }
            else
            {
                LoadDefaultData();
                SaveData();
            }
        };
    }

    public void SaveData()
    {
        SaveManager.Instance.RequestSaveData(CurrentSlotIndex, PlayerModel);
        SlotIndex.Add(CurrentSlotIndex);
    }

    public void LoadData(int index)
    {
        PlayerModel = SaveManager.Instance.RequestLoadData(index);
    }

    public void LoadDefaultData()
    {
        PlayerModel = SaveManager.Instance.GetDefaultData();
    }

    public void SetCurrentSaveIndex(int index)
    {
        CurrentSlotIndex = index;
    }

    public int GetEmptySlotIndex()
    {
        for (int i = 0; i < 100; i++)
        {
            if (!SlotIndex.Contains(i))
            {
                return i;
            }
        }

        return 0;
    }
}