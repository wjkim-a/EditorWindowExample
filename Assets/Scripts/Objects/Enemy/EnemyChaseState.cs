using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Enemy _enemy;
    private Transform _detectTarget;

    public EnemyChaseState(Enemy enemy, Transform chaseTarget)
    {
        _enemy = enemy;
        _detectTarget = chaseTarget;
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        Vector2 direction = (_detectTarget.position - _enemy.transform.position).normalized;

        float yRotation = direction.x > 0 ? 180f : 0f;
        _enemy.transform.localRotation = Quaternion.Euler(_enemy.transform.localRotation.x, yRotation, _enemy.transform.localRotation.z);

        Vector2 nextPos = new Vector2
            (
                _enemy.transform.position.x + direction.x * _enemy.MoveSpeed * Time.deltaTime,
                _enemy.transform.position.y
            );

        _enemy.transform.position = nextPos;

        if (Vector2.Distance(_enemy.transform.position, _detectTarget.position) > _enemy.DetectRange)
        {
            _enemy.ChangeState(eEnemyStateType.Patrol);
        }
    }
}
