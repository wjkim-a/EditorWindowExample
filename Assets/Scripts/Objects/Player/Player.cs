using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class Player : ObjectBase
{
    [Header("info")]
    [SerializeField] private SO_PlayerInfo _playerInfo;

    [Header("CheckEnemy")]
    [SerializeField] private Transform _trfEnemyCheck;
    [SerializeField] private float _enemyCheckDistance;
    [SerializeField] private LayerMask _enemyCheckMask;

    private PlayerModel _model;
    private PlayerController _controller;

    public PlayerModel Model => _model;
    public Transform TrfBottomCheck => _trfEnemyCheck;

    private void Awake()
    {
        ObjectManager.Instance.Player = this;
        _model = new PlayerModel(_playerInfo.MaxHp, _playerInfo.MaxHp, _playerInfo.StartGold);
        _model.OnDieAction += delegate { SceneManager.LoadScene(0); };
        _controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        CheckEnemy();
    }

    private void CheckEnemy()
    {
        RaycastHit2D hit = Physics2D.Raycast(_trfEnemyCheck.position, Vector2.down, _enemyCheckDistance, _enemyCheckMask);
        
        if(hit)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (enemy == null)
                return;
            
            if (_trfEnemyCheck.position.y < enemy.transform.position.y)
                return;

            enemy.OnTakeDamage(_playerInfo.Damage);

            _controller.BounceUp();
        }
    }

    public override void OnTakeDamage(float damage)
    {
        base.OnTakeDamage(damage);
        _model.TakeDamage(damage);
    }
}
