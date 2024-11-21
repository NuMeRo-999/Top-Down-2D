using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public LineRenderer aimLine;
    public Transform weaponHolder; // Donde se posicionará el arma equipada
    public bool isAiming = false; // Variable para verificar si se está apuntando

    void Update()
    {
        Aim();
    }

    void Aim()
    {
        if (Input.GetMouseButton(1)) // Clic derecho sostenido
        {
            isAiming = true;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Aseguramos que esté en el plano 2D

            Vector3 direction = (mousePosition - transform.position).normalized;

            if (Vector3.Distance(transform.position + weaponHolder.position, mousePosition) > 0.1f) 
            {
                weaponHolder.right = direction;

                aimLine.enabled = true;
                aimLine.SetPosition(0, transform.position + direction);
                aimLine.SetPosition(1, mousePosition);
            }
            else
            {
                aimLine.enabled = false;
            }
        }
        else
        {
            isAiming = false;
            aimLine.enabled = false;
        }
    }
}
