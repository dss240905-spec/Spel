using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


    public HealthSystemManager healthUI;
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;
    private AudioSource audioSource;


    
    void Start()
    {
        canMove = true;
        currentHealth = maxHealth;

            healthUI.SetHealth(currentHealth);

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
        anim.SetFloat("MoveSpeed", Mathf.Abs(rgbd.linearVelocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.linearVelocity.y);
        anim.SetBool("IsGrounded", CheckIfGrounded());

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
        if(!canMove)
        {
            return;
        }
        rgbd.linearVelocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coinsCollected++;
            coinText.text = "" + coinsCollected;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(pickupSound, 0.5f);
            Instantiate(coinEffect, other.transform.position, Quaternion.identity);
        }
        if (other.CompareTag("Diamond"))
        {
            Destroy(other.gameObject);
            diamondsCollected++;
            diamondText.text = "" + diamondsCollected;
        }
        if (other.CompareTag("SilverCoin"))
        {
            Destroy(other.gameObject);
            silvercoinsCollected++;
            silvercoinText.text = "" + silvercoinsCollected;
        }
        if (other.CompareTag("Health"))
        {
            RestoreHealth(other.gameObject);
        }        
    }

    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }
    private void Jump()
    {
        rgbd.AddForce(new Vector2(0, jumpForce));
        int randomvalue = Random.Range(0, jumpSounds.Length);
        audioSource.PlayOneShot(jumpSounds[randomvalue], 0.5f);
        Instantiate(dustParticles, transform.position, dustParticles.transform.localRotation);
    }

    public void TakeDamage(int damageGiven)
    {
        currentHealth -= damageGiven;
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
        transform.position = spawnPosition.position;
        
        currentHealth = maxHealth;
        transform.position = spawnPosition.position;
        rgbd.linearVelocity = Vector2.zero;
        healthUI.SetHealth(currentHealth);
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
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down,rayDistanse, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down,rayDistanse, whatIsGround);
        
        
        //Debug.DrawRay(leftFoot.position, Vector2.down * rayDistanse, Color.blue, 0.25f);
        //Debug.DrawRay(rightFoot.position, Vector2.down * rayDistanse, Color.red, 0.25f);
        
        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        { 
            return true;
        }
        else 
        { 
            return false;
        }
      
     
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
        if (other.gameObject.CompareTag("Player")&& other.transform.position.y > transform.position.y)
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
