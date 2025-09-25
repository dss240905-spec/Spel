using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private float footstepInterval = 0.4f;

    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Image fillColor;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text diamondText;
    [SerializeField] private TMP_Text silvercoinText;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private GameObject coinEffect,dustParticles;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private AudioClip healthPickupSound;
    [SerializeField] private AudioClip playerHurt;

    private float horizontalValue;
    private float rayDistanse = 0.25f;
    private float footstepTimer;
    
    private bool isGrounded;
    private bool canMove;
    
    public int diamondsCollected = 0;
    public int silvercoinsCollected = 0;
    public int coinsCollected = 0;
    public int maxHealth = 3;
    private int currentHealth = 0;

    private bool canDash = true;
    private bool isDashing; 
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    public HealthSystemManager healthUI;
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        // Get components with null checks
        rgbd = GetComponent<Rigidbody2D>();
        if (rgbd == null) Debug.LogError("Rigidbody2D missing from player!", this);
        
        rend = GetComponent<SpriteRenderer>();
        if (rend == null) Debug.LogError("SpriteRenderer missing from player!", this);
        
        anim = GetComponent<Animator>();
        if (anim == null) Debug.LogError("Animator missing from player!", this);
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) Debug.LogError("AudioSource missing from player!", this);

        // Check if UI elements are assigned
        if (coinText == null) Debug.LogError("Coin Text not assigned!", this);
        if (fillColor == null) Debug.LogError("Fill Color not assigned!", this);

        // Only proceed if essential components exist
        if (rgbd != null)
        {
            canMove = true;
            
            // Safe UI updates
            if (coinText != null)
                coinText.text = "" + coinsCollected;
            if (diamondText != null)
                diamondText.text = "" + diamondsCollected;
            if (silvercoinText != null)
                silvercoinText.text = "" + silvercoinsCollected;
            
        }
        canMove = true;
        currentHealth = maxHealth;

          //  healthUI.SetHealth(currentHealth);

        coinText.text = "" + coinsCollected;
        if (diamondText != null)
            diamondText.text = "" + diamondsCollected;
        if (silvercoinText != null)
            silvercoinText.text = "" + silvercoinsCollected;
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();        
    }

    void Update()
    {
        if (rgbd == null) return; // Don't run if rigidbody is missing

        if (isDashing){
            return;
        }

        horizontalValue = Input.GetAxis("Horizontal");
        if (horizontalValue < 0)
        {
            FlipSprite(true);
        }
        if (horizontalValue > 0)
        {
            FlipSprite(false);
        }
        
        if (Input.GetButtonDown("Jump") && CheckIfGrounded() == true)
        {
            Jump();
        }
        
        // Safe animator updates
        if (anim != null)
        {
            anim.SetFloat("MoveSpeed", Mathf.Abs(rgbd.linearVelocity.x));
            anim.SetFloat("VerticalSpeed", rgbd.linearVelocity.y);
            anim.SetBool("IsGrounded", CheckIfGrounded());
        }

       if(Input.GetKeyDown(KeyCode.LeftShift) && canDash){
        StartCoroutine(Dash());
       }
        if (CheckIfGrounded() && Mathf.Abs(horizontalValue) > 0.1f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayeFootstep();
                footstepTimer = footstepInterval;
            }
        }
       else
        {
            footstepTimer = 0f; //reset when not moving.
        }
    }

    private void FixedUpdate()
    {
        if (rgbd == null) return; // Safety check

        if (isDashing){
            return;
        }

        if(!canMove)
        {
            return;
        }
        rgbd.linearVelocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (audioSource == null) return;

        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coinsCollected++;
            if (coinText != null)
                coinText.text = "" + coinsCollected;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(pickupSound, 0.5f);
            if (coinEffect != null)
                Instantiate(coinEffect, other.transform.position, Quaternion.identity);
        }
        if (other.CompareTag("Diamond"))
        {
            Destroy(other.gameObject);
            diamondsCollected++;
            if (diamondText != null)
                diamondText.text = "" + diamondsCollected;
        }
        if (other.CompareTag("SilverCoin"))
        {
            Destroy(other.gameObject);
            silvercoinsCollected++;
            if (silvercoinText != null)
                silvercoinText.text = "" + silvercoinsCollected;
        }
        if (other.CompareTag("Health"))
        {
            RestoreHealth(other.gameObject);
        }
    }

    private void FlipSprite(bool direction)
    {
        if (rend != null)
            rend.flipX = direction;
    }

    private void Jump()
    {
        if (rgbd == null) return;
        
        rgbd.AddForce(new Vector2(0, jumpForce));
        
        if (audioSource != null && jumpSounds != null && jumpSounds.Length > 0)
        {
            int randomvalue = Random.Range(0, jumpSounds.Length);
            audioSource.PlayOneShot(jumpSounds[randomvalue], 0.2f);
        }
        
        if (dustParticles != null)
            Instantiate(dustParticles, transform.position, dustParticles.transform.localRotation);
    }

private IEnumerator Dash()
{
    if (rgbd == null || tr == null) yield break;

    // Determine dash direction based on input
    Vector2 dashDirection = Vector2.zero;

    // Left
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        dashDirection = Vector2.left;
    // Right
    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        dashDirection = Vector2.right;
    // Down (only if in air)
    else if (!isGrounded && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        dashDirection = Vector2.down;

    // If no direction is pressed, don't dash
    if (dashDirection == Vector2.zero)
        yield break;

    canDash = false;
    isDashing = true;

    float originalGravity = rgbd.gravityScale;
    rgbd.gravityScale = 0f;

    rgbd.linearVelocity = dashDirection.normalized * dashingPower;

    tr.emitting = true;
    yield return new WaitForSeconds(dashingTime);
    tr.emitting = false;

    rgbd.gravityScale = originalGravity;
    isDashing = false;

    yield return new WaitForSeconds(dashingCooldown);
    canDash = true;
}



    public void TakeDamage( int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
       
        
        healthUI.SetHealth(currentHealth);

        if (playerHurt != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(playerHurt, 1f);
        }

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void TakeKnockBack(float knockbackForce, float upwards)
    {
        if (rgbd == null) return;
        
        canMove = false;
        rgbd.AddForce(new Vector2(knockbackForce, upwards));
        Invoke("CanMoveAgain", 0.25f);
    }

    private void CanMoveAgain() 
    {
        canMove = true;
    }
  
    private void Respawn()
    {
       currentHealth = maxHealth;
       healthUI.SetHealth(maxHealth);
        if (spawnPosition != null)
            transform.position = spawnPosition.position;
        if (rgbd != null)
            rgbd.linearVelocity = Vector2.zero;
    }

    private void RestoreHealth(GameObject healthPickup)
    {
        if (currentHealth >= maxHealth)
            return;

        //Get how much health this pickup should restore
        int healthToRestore = healthPickup.GetComponent<HealthPickUp>().healthAmount;

        //Add it to current health
        currentHealth += healthToRestore;

        //Clamp so we never exceed max health
        currentHealth += Mathf.Clamp(currentHealth, 0, maxHealth);

        //Update the hearts UI once
        healthUI.SetHealth(currentHealth);

        //Remove the pickup from the scene
        Destroy(healthPickup);

        if (healthPickupSound != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(healthPickupSound, 0.7f);
        }
    }
   
    private bool CheckIfGrounded()
    {
        if (leftFoot == null || rightFoot == null)
        {
            Debug.LogWarning("Foot transforms not assigned for ground check!");
            return false;
        }

        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, rayDistanse, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, rayDistanse, whatIsGround);
        
        // Debug visualization
        Debug.DrawRay(leftFoot.position, Vector2.down * rayDistanse, Color.red);
        Debug.DrawRay(rightFoot.position, Vector2.down * rayDistanse, Color.red);
        
        return (leftHit.collider != null || rightHit.collider != null);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            other.transform.SetParent(null);
        }
    }

    private void PlayeFootstep()
    {
        if (footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            audioSource.pitch = Random.Range(0.9f, 1.1f); //small pitch variation for realism
            audioSource.PlayOneShot(footstepSounds[randomIndex], 0.7f);
        }
    }

}
