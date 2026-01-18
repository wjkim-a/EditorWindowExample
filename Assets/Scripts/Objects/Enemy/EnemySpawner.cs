using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _maxEnemyCount;
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private float _spawnCheckDelay;

    private WaitForSeconds _checkDelay;
    private int _curAliveEnemyCount = 0;

    private void Start()
    {
        SetUp();
        StartCoroutine(SpawnEnemy());
    }

    private void SetUp()
    {
        _checkDelay = new WaitForSeconds(_spawnCheckDelay);
    }

    private IEnumerator SpawnEnemy()
    {
        while (_enemyPrefab != null)
        {
            if (_curAliveEnemyCount < _maxEnemyCount)
            {
                //Get or Create Enemy
                Enemy newEnemy = ObjectManager.Instance.CreatObjWithUsePool(_enemyPrefab, transform);
                newEnemy.transform.localPosition = Vector3.zero;

                newEnemy.SetUp(_wayPoints, delegate
                {
                    if (_curAliveEnemyCount > 0)
                        _curAliveEnemyCount--;

                    ObjectManager.Instance.PushToPool(newEnemy);
                });

                _curAliveEnemyCount++;
            }

            yield return _checkDelay;
        }
    }
}
