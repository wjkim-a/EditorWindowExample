using UnityEngine;

public class MVP_Instraller : MonoBehaviour
{
    [Header("About Player")]
    [SerializeField] private Player _player;
    [SerializeField] private UIPlayerHpBar _uiPlayerHpBar;
    [SerializeField] private UIPlayerGold _uiPlayerGold;

    private PlayerPresenter _playerPresenter;

    private void Start()
    {
        IPlayerHpView[] playerHpViews = new IPlayerHpView[] { _uiPlayerHpBar };
        IPlayerGoldView[] playerGoldViews = new IPlayerGoldView[] { _uiPlayerGold };
        _playerPresenter = new PlayerPresenter(_player.Model, playerHpViews, playerGoldViews);
    }

    private void OnDestroy()
    {
        _playerPresenter?.Dispose();
    }
}
