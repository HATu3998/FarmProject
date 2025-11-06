using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        bool isRuning = movement.x != 0 || movement.y != 0;
        animator.SetBool("isRun", isRuning);
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }

    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
