using UnityEngine;

public class BloodParticles : MonoBehaviour
{
    [SerializeField] GameObject bloodParticlesPrefab;
    public GameObject particleContainer;
    public GameObject[] bloodStainPrefabs; // Array de prefabs de manchas de sangre

    public void SpawnBloodBurst()
    {
        if (bloodParticlesPrefab != null)
        {
            GameObject bloodParticles = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);

            ParticleSystem ps = bloodParticles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                var burst = new ParticleSystem.Burst(0f, 50, 100);
                emission.SetBursts(new ParticleSystem.Burst[] { burst });
            }

            Destroy(bloodParticles, 2f);
        }
        else
        {
            Debug.LogWarning("Blood particles prefab is not assigned!");
        }
    }

    public void SpawnBloodParticlesAndStain()
    {
        if (bloodParticlesPrefab != null)
        {
            GameObject bloodParticles = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(bloodParticles, 2f);
        }
        else
        {
            Debug.LogWarning("Blood particles prefab is not assigned!");
        }

        // Generar la mancha de sangre en el suelo de forma aleatoria
        if (bloodStainPrefabs != null && bloodStainPrefabs.Length > 0)
        {
            GameObject randomBloodStain = bloodStainPrefabs[Random.Range(0, bloodStainPrefabs.Length)];

            GameObject bloodStain = Instantiate(randomBloodStain, transform.position, Quaternion.identity);
            bloodStain.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        }
        else
        {
            Debug.LogWarning("Blood stain prefabs array is empty or not assigned!");
        }
    }
}
