using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerInfo", menuName = "Scriptable Objects/Player/SO_PlayerInfo")]
public class SO_PlayerInfo : ScriptableObject
{
    [SerializeField] private float _maxHp;
    [SerializeField] private float _damage;
    [SerializeField] private long _startGold;

    public float MaxHp => _maxHp;
    public float Damage => _damage;
    public long StartGold => _startGold;
}
