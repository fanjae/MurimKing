using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHP = 30f;
    public float attackDamage = 10f;
    public float moveSpeed = 0f;
}