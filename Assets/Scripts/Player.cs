using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config vars
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float deathKickMin = 15f;
    [SerializeField] float deathKickMax= 25f;

    // state
    bool isAlive = true;

    // cached refs
    Rigidbody2D rigidbody2D;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    float initialGravityScale;


    // Start is called before the first frame update
    void Start ()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        initialGravityScale = rigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!isAlive)
        {
            return;
        }
            ClimbLadder();
            Jump();
            Run();
            FlipSprite();
            Die();
    }


    private void ClimbLadder()
    {
        var isTouchingLadder = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (!isTouchingLadder)
        {
            animator.SetBool("isClimbing", false);
            rigidbody2D.gravityScale = initialGravityScale;
            return;
        }
        var deltaY = Input.GetAxis("Vertical") * climbSpeed;
        Vector2 playerClimbVelocity = new Vector2(rigidbody2D.velocity.x, deltaY);
        rigidbody2D.velocity = playerClimbVelocity;
        rigidbody2D.gravityScale = 0;


        var playerHasVertSpeed = Mathf.Abs(rigidbody2D.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVertSpeed);
    }

    private void Jump()
    {
        var isTouchingGround = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!isTouchingGround)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0f, jumpHeight);
            rigidbody2D.velocity += jumpVelocity;
        }
    }

    private void Run ()
    {
        // Move character
        var deltaX = Input.GetAxis("Horizontal") * movementSpeed;
        Vector2 playerVelocity = new Vector2 (deltaX, rigidbody2D.velocity.y);
        rigidbody2D.velocity = playerVelocity;


        // set running anim
        bool playerHasHorizSpeed = Mathf.Abs (rigidbody2D.velocity.x) > Mathf.Epsilon;
        animator.SetBool ("isRunning", playerHasHorizSpeed);

    }

    private void FlipSprite ()
    {
        bool playerHasHorizSpeed = Mathf.Abs (rigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x), transform.localScale.y);
        }

    }

    private void Die()
    {
        var isTouchingHazard = bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"));
        if (isTouchingHazard)
        {
            animator.SetTrigger("die");
            isAlive = false;
            rigidbody2D.velocity = new Vector2(Random.Range(deathKickMin, deathKickMax), Random.Range(deathKickMin, deathKickMax));
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            feetCollider.enabled = false;
        }
    }

}