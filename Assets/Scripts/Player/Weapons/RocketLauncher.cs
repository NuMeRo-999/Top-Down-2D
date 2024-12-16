using UnityEngine;

[System.Serializable]

[CreateAssetMenu(menuName = "Weapons/RocketLauncher")]

public class RocketLauncher : Weapon
{
    public GameObject rocketPrefab;
    public GameObject explosionEffectPrefab;
    public GameObject wallExplosionEffectPrefab;
    public GameObject explosionMarkPrefab;
    public float explosionRadius = 2f;
    public float speed;
    public float maxRange;
}

