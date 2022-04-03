

using UnityEngine;

[CreateAssetMenu(fileName = "new LootConfig", menuName = "Configs/LootConfig", order = 1)]
public class LootConfig : ScriptableObject
{
    public GameObject model;
    public float damage;
    public float movementSpeed;
    public float healHP;
    public float maxHP;
    public float crit;
}