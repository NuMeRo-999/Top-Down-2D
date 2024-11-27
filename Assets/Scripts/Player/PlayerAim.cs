using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public LineRenderer aimLine;
    public Transform weaponHolder;
    public bool isAiming = false;
    
    private WeaponSystem weaponSystem;
    private PlayerMovement playerMovement;
    private Inventory inventory;
    private Animator animator;
    

    void Start()
    {
        weaponSystem = GetComponent<WeaponSystem>();
        playerMovement = GetComponent<PlayerMovement>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        Aim();
    }

    void Aim()
    {
        if (Input.GetMouseButton(1))
        {
            isAiming = true;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; 

            Vector3 direction = (mousePosition - transform.position).normalized;
            // Debug.Log(Vector3.Distance(transform.position + weaponHolder.position, mousePosition));
            // Debug.Log(Vector3.Distance(transform.position + weaponHolder.position, mousePosition) > 0.1f);
            if (Vector3.Distance(transform.position + weaponHolder.position, mousePosition) > 0.1f) 
            {
                weaponHolder.right = direction;


                if (!inventory.selectedItem.isHealingItem) 
                    animator.SetInteger("WeaponID",(int)weaponSystem.equippedWeapon.type);
                else
                    animator.SetInteger("WeaponID", 10);

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
