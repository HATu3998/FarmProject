using UnityEngine;

public class PlauerMovementMouse : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    private Vector2 movement;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 targetPosition;
    private bool isMoving = false;
    public float stoppingDistance = 0.1f;
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = rb.position;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMoving = true;
        }
        if (isMoving)
        {
            // T?nh kho?ng c?ch ??n target
            if (Vector2.Distance(rb.position, targetPosition) < stoppingDistance)
            {
                // ?? ??n n?i -> D?ng l?i
                isMoving = false;
            }
        }

        if (isMoving)
        {
            // N?u ?ang di chuy?n:
            // T?nh to?n h??ng di chuy?n
            movement = (targetPosition - rb.position).normalized;
            // B?t animation run
            animator.SetBool("isRun", true);

            // X? l? l?t sprite
            if (movement.x > 0.01f) // Th?m 1 ng??ng nh? ?? tr?nh l?t sprite khi movement.x qu? b?
            {
                spriteRenderer.flipX = false;
            }
            else if (movement.x < -0.01f)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            // N?u kh?ng di chuy?n:
            // ??t movement v? 0 ?? FixedUpdate kh?ng di chuy?n nh?n v?t n?a
            movement = Vector2.zero;
            // T?t animation run
            animator.SetBool("isRun", false);
        }

    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
         
    }
}
