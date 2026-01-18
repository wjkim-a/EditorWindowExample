using System;
using TMPro;

public class PlayerModel : IPlayerModel
{
    private float _playerMaxHP;
    private float _playerCurHP;
    private long _hasGold;

    public event Action<float, float> OnUpdateHpAction;
    public event Action<long> OnUpdateGoldAction;
    public event Action OnDieAction;

    public PlayerModel(float playerCurHP, float playerMaxHP, long hasGold)
    {        
        _playerCurHP = playerCurHP;
        _playerMaxHP = playerMaxHP;
        _hasGold = hasGold;
    }

    public void Init()
    {
        UpdateHP(_playerCurHP);
        UpdateGold(_hasGold);
    }

    public void TakeDamage(float damage)
    {
        _playerCurHP -= damage;

        if (_playerCurHP < 0) 
        {
            _playerCurHP = 0;
            OnDieAction?.Invoke();
        }

        OnUpdateHpAction?.Invoke(_playerCurHP, _playerMaxHP);
    }

    public void UpdateHP(float hp)
    {
        _playerCurHP = hp;

        if (_playerCurHP <= 0)
        {
            _playerCurHP = 0;
            OnDieAction.Invoke();
        }

        OnUpdateHpAction?.Invoke(_playerCurHP, _playerMaxHP);
    }

    public void AddGold(long gold)
    {
        _hasGold += gold;
        OnUpdateGoldAction?.Invoke(_hasGold);
    }

    public void UpdateGold(long gold)
    {
        _hasGold = gold;
        OnUpdateGoldAction?.Invoke(_hasGold);
    }
}
