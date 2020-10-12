using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config vars
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] float jumpHeight = 5f;
    // state

    // cached refs
    Rigidbody2D rigidbody2D;
    Animator animator;
    Collider2D collider2D;
    float initialGravityScale;


    // Start is called before the first frame update
    void Start ()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        initialGravityScale = rigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update ()
    {
        ClimbLadder();
        Jump();
        Run();
        FlipSprite();
    }

    private void ClimbLadder()
    {
        var isTouchingLadder = collider2D.IsTouchingLayers(LayerMask.GetMask("Ladder"));
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
        var isTouchingGround = collider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
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

}