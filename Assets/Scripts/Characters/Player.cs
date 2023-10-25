using System.Security.Cryptography;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    
    [SerializeField] private float speed;
    private Vector3 _moveDirection;
  
    private Rigidbody rb; //NEW jump function

    private Vector2 currentRotation;

    [Header("Camera")]
    [SerializeField, Range(1,20)] private float mouseSensX;
    [SerializeField, Range(1,20)] private float mouseSensY;

    [SerializeField, Range(-90, 0)] private float minViewAngle;
    [SerializeField, Range(0, 90)] private float maxViewAngle;
    
    [SerializeField] private Transform followTarget;

    [Header("Shooting")]
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float projectileForce;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ammoText;
    
    [Header("Ammo")]
    [SerializeField] private int maxAmmo = 30; // this represents the maximum ammo capacity, rn its 30 but we can change
    private int currentAmmo; // used to store the current ammo count of the player. It's not serialized, so you cannot set its value in the Inspector.

    
    
    private Vector2 currentAngle;
    
    

    void Start()
    {
        InputManager.Init(this);
        InputManager.SetGameControl();

        rb = GetComponent<Rigidbody>(); //NEW jump function
        
        
        currentAmmo = maxAmmo; // Initialize ammo to the maximum. so when the game starts the player gets max ammo
    }


    void Update()
    {
        transform.position += transform.rotation * (speed * Time.deltaTime * _moveDirection);
        
        
    }

    
    public void SetMovementDirection(Vector3 currentDirection)
        {
            _moveDirection = currentDirection;
        }
    void OnJump() //NEW JUMP FUNCTION
    {
        if (IsGrounded())
        {
            Jump();
        }
    }
    
    // New JUMP FUNCTION
    public void Jump()
    {
            rb.AddForce(Vector3.up * 5.0f, ForceMode.Impulse); // You can adjust the jump force as needed.
    }

    // New JUMP FUNCTION
    private bool IsGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 0.1f; // Adjust this distance as needed.
        return Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance);
    }

    public void SetLookRotation(Vector2 readValue)
    {
        currentAngle.x += readValue.x * Time.deltaTime * mouseSensX;
        currentAngle.y += readValue.y * Time.deltaTime * mouseSensY;

        currentAngle.y = Mathf.Clamp(currentAngle.y, minViewAngle, maxViewAngle);
        
        transform.rotation = Quaternion.AngleAxis(currentAngle.x, Vector3.up);
        followTarget.localRotation = Quaternion.AngleAxis(currentAngle.y, Vector3.right);
    }


    public void Shoot() // this is called when the play attempts to shoot
    {
        if (currentAmmo > 0) //checks if the player has enough ammo (greater than 0) to fire a shot. If there's ammo, the player can shoot.
        {
            //I GOT THIS FROM YOUR TUTORIAL 
            Rigidbody currentProjectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        
            currentProjectile.AddForce(followTarget.forward * projectileForce, ForceMode.Impulse);
        
            Destroy(currentProjectile.gameObject, 4);
        
            
            
            // Decrease ammo when shooting
            currentAmmo--;

            // Update the UI to display current ammo count
            UpdateAmmoUI();
        }
        
    }

    //RELOAD
    public void Reload()
    {
        if (currentAmmo < maxAmmo) // checks if the current ammo count is less than the maximum ammo capacity.
        {
            int ammoToAdd = maxAmmo - currentAmmo; // Calculate how much ammo to add
            currentAmmo = maxAmmo; // Refill ammo to the maximum
            UpdateAmmoUI(); //updates the UI text element to show the newly reloaded ammo count.
        }

    }
    
    private void UpdateAmmoUI()
    {
        ammoText.text = "Ammo: " + currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }

    
    // AMMO BOX
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AmmoBox")) // if the player has entered a collider tagged as "AmmoBox."
        {
            // Gain some amount of ammo from the box (you can adjust the amount)
            int  ammoToAdd= 10; // Adjust this amount as needed
            currentAmmo += ammoToAdd;
        
            // Make sure not to exceed the maximum ammo capacity
            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;

            UpdateAmmoUI();

            // Disable or remove the ammo box so the player can't gain more ammo from it
            
            Destroy(other.gameObject); // REMOVE THE AMMO BOX WHEN PICKED UP
            
            // Display a debug log message
            Debug.Log("Picked up ammo: " + ammoToAdd + " rounds");
        }
    }

    
    
}
    

