using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shotgun", menuName = "Weapons/Shotgun", order = 1)]
public class Shotgun : Weapon
{
    public int pelletCount = 8;
    public float spreadAngle = 15f;
}
