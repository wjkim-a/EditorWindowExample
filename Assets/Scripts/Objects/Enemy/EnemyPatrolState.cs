using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private Enemy _enemy;
    private Transform _detectTarget;
    private Transform[] _wayPoints; 

    public EnemyPatrolState(Enemy enemy, Transform detectTarget, Transform[] wayPoints)
    {
        _enemy = enemy;
        _detectTarget = detectTarget;
        _wayPoints = wayPoints;
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        //patrol
        Vector3 target = _wayPoints[_enemy.CurWayIndex].position;
        Vector2 direction = (target - _enemy.transform.position).normalized;

        float yRotation = direction.x > 0 ? 180f : 0f;
        _enemy.transform.localRotation = Quaternion.Euler(_enemy.transform.localRotation.x, yRotation, _enemy.transform.localRotation.z);

        _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position, target, _enemy.MoveSpeed * Time.deltaTime);

        if (Vector2.Distance(_enemy.transform.position, target) < 0.1f) 
        {
            _enemy.CurWayIndex = (_enemy.CurWayIndex + 1) % _wayPoints.Length;
        }

        //detect
        if(Vector2.Distance(_enemy.transform.position, _detectTarget.position) <= _enemy.DetectRange)
        {
            _enemy.ChangeState(eEnemyStateType.Chase);
        }
    }
}
