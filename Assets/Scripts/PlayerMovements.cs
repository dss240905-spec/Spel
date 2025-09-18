using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillColor;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text diamondText;
    [SerializeField] private TMP_Text silvercoinText;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private GameObject coinEffect,dustParticles;
    [SerializeField] private TrailRenderer tr;

    private float horizontalValue;
    private float rayDistanse = 0.25f;
    private bool isGrounded;
    private bool canMove;
    
    private int startingHealth = 5;
    private int currentHealth = 0;
    public int diamondsCollected = 0;
    public int silvercoinsCollected = 0;
    public int coinsCollected = 0;

    private bool canDash = true;
    private bool isDashing; 
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;
    private AudioSource audioSource;


    
    void Start()
    {
        canMove = true;
        currentHealth = startingHealth;
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
        anim.SetFloat("MoveSpeed", Mathf.Abs(rgbd.linearVelocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.linearVelocity.y);
        anim.SetBool("IsGrounded", CheckIfGrounded());

       if(Input.GetKeyDown(KeyCode.LeftShift) && canDash){
        StartCoroutine(Dash());
       }
    }



    private void FixedUpdate()
    {
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




   private IEnumerator Dash()
{
    canDash = false;
    isDashing = true;
    float originalGravity = rgbd.gravityScale;
    rgbd.gravityScale = 0f;
    rgbd.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
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
        currentHealth -= damageAmount ;
        UpdateHealthBar();

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
       currentHealth = startingHealth;
        UpdateHealthBar();
        transform.position = spawnPosition.position;
       rgbd.linearVelocity = Vector2.zero;
    }
    private void RestoreHealth(GameObject healthPickup)
    {
        if (currentHealth >= startingHealth)
        {
            return;
        }
        else
        {
            int healthToRestore = healthPickup.GetComponent<HealthPickUp>().healthAmount;
            currentHealth += 3;
            UpdateHealthBar() ;
            Destroy(healthPickup);
            if(currentHealth>=startingHealth)
            {
                currentHealth = startingHealth;
            }
        }
    }
    private void UpdateHealthBar()
    {
        
        healthSlider.value = currentHealth;
        if(currentHealth >= 2)
        {
            fillColor.color = Color.green;
        }
        else
        {

            fillColor.color = Color.red;
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

}
