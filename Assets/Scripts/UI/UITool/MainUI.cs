using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class MainUI : BaseUI<MainUI>
{
    [SerializeField] private Image Image_Background;

    [SerializeField] private List<Image> Images = new();
    [SerializeField] private List<Transform> Transform_Buttons = new();

    private Dictionary<Transform, BaseButton> MenuButtons = new();

    public void SetBackground(Sprite sprite_background)
    {
        Image_Background.sprite = sprite_background;
    }

    public void SetAndCreateButton(int index, GameObject buttonPrefab, Sprite buttonSprite, string buttonText, TMP_FontAsset buttonFontAsset, Action buttonAction)
    {
        if (index < 0 || index >= Transform_Buttons.Count)
        {
            this.LogError($"인덱스의 수치가 잘못되었습니다!! 인덱스 : {index}");
            return;
        }

        if (Transform_Buttons[index] == null)
        {
            this.LogError($"{index}번째 트랜스폼 데이터가 없습니다!!");
            return;
        }

        if (this.InstantiateAndGetComponent(buttonPrefab, Transform_Buttons[index], out BaseButton buttonComponent) == false)
        {
            return;
        }

        if (buttonComponent.SetButtonData(buttonSprite, buttonText, buttonFontAsset,  buttonAction) == false)
        {
            Destroy(buttonComponent.gameObject);
            return;
        }

        MenuButtons.Add(Transform_Buttons[index], buttonComponent);
    }

    public void SetImage(int index, Sprite imageSprite)
    {
        if(index < 0 || index >= Images.Count)
        {
            this.LogError($"인덱스의 수치가 잘못되었습니다!! 인덱스 : {index}");
            return;
        }

        if (Images[index] == null)
        {
            this.LogError($"{index}번째 이미지 컴포넌트가 없습니다!!");
            return;
        }

        if(imageSprite == null)
        {
            this.LogWarning($"이미지가 null입니다!! {index}번째 이미지 컴포넌트를 비활성화 합니다!!");
            Images[index].ActiveFalse();
            return;
        }

        Images[index].sprite = imageSprite;
        Images[index].ActiveTrue();
    }
}
