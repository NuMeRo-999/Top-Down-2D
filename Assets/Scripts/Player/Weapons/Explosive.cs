using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Explosive")]
public class Explosive : Weapon
{
    public GameObject prefab;
    public GameObject explosionEffectPrefab;
    public GameObject explosionMarkPrefab;
    public float throwDistance = 2f;
    public float moveTime = 0.5f;
    public float detonationTime = 3f;
    public float explosionRadius = 5f;
}

