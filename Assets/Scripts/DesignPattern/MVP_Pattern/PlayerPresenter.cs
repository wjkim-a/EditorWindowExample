using System;
using UnityEngine;

public class PlayerPresenter : IDisposable
{
    private IPlayerModel _playerModel;
    private IPlayerHpView[] _playerHpViews;
    private IPlayerGoldView[] _playerGoldViews;

    public PlayerPresenter(IPlayerModel playerModel, IPlayerHpView[] playerHpViews, IPlayerGoldView[] playerGoldViews)
    {
        _playerModel = playerModel;
        _playerHpViews = playerHpViews;
        _playerGoldViews = playerGoldViews;

        _playerModel.OnUpdateHpAction += OnUpdateHP;
        _playerModel.OnUpdateGoldAction += OnUpdateGold;

        _playerModel.Init();
    }

    private void OnUpdateHP(float curHp, float MaxHp)
    {
        for (int i = 0; i < _playerHpViews.Length; i++)
            _playerHpViews[i].UpdateHpUI(curHp, MaxHp);
    }
    
    private void OnUpdateGold(long curGold)
    {
        for (int i = 0; i < _playerHpViews.Length; i++)
            _playerGoldViews[i].UpdateGoldUI(curGold);
    }

    public void Dispose()
    {
        _playerModel.OnUpdateHpAction -= OnUpdateHP;
    }
}
