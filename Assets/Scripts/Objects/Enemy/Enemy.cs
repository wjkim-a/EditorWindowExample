using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : ObjectBase
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _detectRange;
    [SerializeField] private float _damage;
    [SerializeField] private float _maxHp;
    [SerializeField] private long _rewardValue;
    [SerializeField] private UnityEvent OnStartAction;
    [SerializeField] private UnityEvent OnDieAction;
    [SerializeField] private Transform[] _wayPoints;

    private Player _player;
    private int _curWayIndex = 0;
    private EnemyState _curState;  
    private Dictionary<eEnemyStateType, EnemyState> _enemyStates;
    private float _curHp;
    private bool _isImmune = false;

    public float MoveSpeed => _moveSpeed;
    public float DetectRange => _detectRange;
    public int CurWayIndex
    {
        get {  return _curWayIndex; }
        set { _curWayIndex = value; }
    }

    private void Update()
    {
        _curState?.Update();
    }
    private void OnDisable()
    {
        OnStartAction.RemoveAllListeners();
        OnDieAction.RemoveAllListeners();
    }

    private void Start()
    {
        OnStartAction?.Invoke();
    }

    private void OnEnable()
    {
        InitEnemy();
    }

    public void SetUp()
    {
        FindPlayer();
        SetStates();

        ChangeState(eEnemyStateType.Patrol);
    }

    public void SetUp(Transform[] wayPoints, UnityAction dieAction)
    {
        _wayPoints = wayPoints;
        OnDieAction.AddListener(dieAction);

        FindPlayer();
        SetStates();

        ChangeState(eEnemyStateType.Patrol);
    }

    private void InitEnemy()
    {
        _curHp = _maxHp;
        OffInmmune();
    }

    private void FindPlayer()
    {
        if (_player == null)
            _player = FindAnyObjectByType<Player>();
    }

    private void SetStates()
    {
        _enemyStates = new Dictionary<eEnemyStateType, EnemyState>
        {
            [eEnemyStateType.Patrol] = new EnemyPatrolState(this, _player.transform, _wayPoints),
            [eEnemyStateType.Chase] = new EnemyChaseState(this, _player.transform),
        };

    }

    public void ChangeState(eEnemyStateType stateType)
    {
        if (_enemyStates.ContainsKey(stateType))
        {
            _curState?.Exit();
            _curState = _enemyStates[stateType];
            _curState?.Enter();
        }
        else
        {
            Debug.LogError($"need to set state : {stateType}");
        }
    }

    public override void OnTakeDamage(float damage)
    {
        if (_isImmune)
            return;

        base.OnTakeDamage(damage);

        _curHp -= damage;
        _isImmune = true;
        
        if(_curHp <= 0)
        {
            _curHp = 0;
            Die();
        }

        Invoke(nameof(OffInmmune), 1f);
    }

    private void OffInmmune()
    {
        _isImmune = false;
    }

    private void Die()
    {
#if TESTPLAY
        OnStartAction.AddListener(Die);
#endif
        ObjectManager.Instance.Player.Model.AddGold(_rewardValue);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player.TrfBottomCheck.position.y > transform.position.y)
                return;

            player?.OnTakeDamage(_damage);
        }
    }
}
