using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public LineRenderer aimLine;
    public Transform weaponHolder;
    public bool isAiming = false;
    
    private WeaponSystem weaponSystem;
    private PlayerMovement playerMovement;
    private Animator animator;
    

    void Start()
    {
        weaponSystem = GetComponent<WeaponSystem>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }


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
            mousePosition.z = 0; 

            Vector3 direction = (mousePosition - transform.position).normalized;

            if (Vector3.Distance(transform.position + weaponHolder.position, mousePosition) > 0.1f) 
            {
                weaponHolder.right = direction;
                
                animator.SetInteger("WeaponID",(int)weaponSystem.equippedWeapon.type);
                playerMovement.speed = 3f;
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
            playerMovement.speed = 5f;
            isAiming = false;
            aimLine.enabled = false;
        }
        animator.SetBool("isAiming", isAiming);

    }
}
