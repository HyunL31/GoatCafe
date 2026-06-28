using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameOptionPresenter : BasePresenter<GameOptionPresenter, GameOptionUI>
{
    public override UIType UIType_This { get;  } = UIType.GameOptionUI;


    private GameOptionUI _gameOptionUI;

    private GameObject _prefab_menuButton;

    private Sprite _sprite_background;
    private Sprite _sprite_menuTab;
    private Sprite _sprite_optionTab;

    private Sprite _sprite_button;
    private Sprite _sprite_buttonHighlighted;
    private Sprite _sprite_buttonSelected;

    public override void InitUI(GameOptionUI ui)
    {
        _gameOptionUI = ui;
        SetUI().Forget();
    }
    protected async override UniTaskVoid SetUI()
    {

    }

    protected async override UniTask LoadAssetAsync()
    {
        
    }

    protected override void LoadData()
    {
        
    }


    protected override void SetUIData()
    {
        
    }

}