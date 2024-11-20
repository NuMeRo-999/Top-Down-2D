using UnityEngine;

public class BloodParticles : MonoBehaviour
{
    [SerializeField] GameObject bloodParticlesPrefab;
    public GameObject[] bloodStainPrefabs; // Array de prefabs de manchas de sangre

    public void SpawnBloodBurst()
    {
        if (bloodParticlesPrefab != null)
        {
            // Instanciar las partículas en la posición del enemigo
            GameObject bloodParticles = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);

            // Acceder al sistema de partículas para configurar el burst
            ParticleSystem ps = bloodParticles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                var burst = new ParticleSystem.Burst(0f, 50, 100); // Configurar el burst (tiempo 0, entre 50 y 100 partículas)
                emission.SetBursts(new ParticleSystem.Burst[] { burst });
            }

            // Destruir las partículas después de su duración
            // Destroy(bloodParticles, 2f);
        }
        else
        {
            Debug.LogWarning("Blood particles prefab is not assigned!");
        }
    }

    public void SpawnBloodParticlesAndStain()
    {
        // Generar partículas de sangre
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
            // Seleccionar un prefab aleatorio del array
            GameObject randomBloodStain = bloodStainPrefabs[Random.Range(0, bloodStainPrefabs.Length)];

            // Instanciar la mancha de sangre seleccionada
            GameObject bloodStain = Instantiate(randomBloodStain, transform.position, Quaternion.identity);
            bloodStain.transform.Rotate(0f, 0f, Random.Range(0f, 360f)); // Rotación aleatoria para variedad
        }
        else
        {
            Debug.LogWarning("Blood stain prefabs array is empty or not assigned!");
        }
    }
}
