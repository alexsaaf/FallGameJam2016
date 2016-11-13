using UnityEngine;
using System.Collections;

/**
 * The controller script for a simple enemy that walks in one direction until it hits an obstacle
 * or reaches an edge, then turns.
 */
public class TestEnemy1Controller : MonoBehaviour {

    public int damage = 1;
    public float xSpeed = 5f;
    public LayerMask collisionMask;
    // Raycasting horizontally from the corners was problematic, as it registered collision with the ground
    // This variable moves the origin of the raycasts up/down so that it does not do this.
    public float xRaycastMargin; 
    private float verticalCheckLength;
    private int currentDirection = -1; // Start walking left, set to 1 to go right
    private float width, height;
    private SpriteRenderer spriteRenderer;


    void Start() {
        width = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        height = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;
        verticalCheckLength = height / 10;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	void Update () {
        ManageHorizontalMovement();
        ManageVerticalMovement();
	}

    /**
     * Walk in one direction until another collider is detected in front of it, then turn around
     */
    private void ManageHorizontalMovement() {
        Vector2 dirX = currentDirection * transform.TransformDirection(Vector2.right);
        // The point on the x axis from which the ray will be cast (left or right edge of collider, depending on currentDirection)
        float rayOriginX = transform.position.x + currentDirection * (width / 2);
        RaycastHit2D hitInfoXTop = Physics2D.Raycast(new Vector2(rayOriginX, transform.position.y + (height - xRaycastMargin)),
            dirX, xSpeed * Time.deltaTime, collisionMask);
        RaycastHit2D hitInfoXBottom = Physics2D.Raycast(new Vector2(rayOriginX, transform.position.y - (height - xRaycastMargin)),
            dirX, xSpeed * Time.deltaTime, collisionMask);
        if (hitInfoXTop || hitInfoXBottom) {
            transform.Translate(hitInfoXTop.distance * Time.deltaTime * currentDirection, 0, 0);
            currentDirection *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            // If colliding with the crab
            if (hitInfoXTop.collider != null) {
                if (hitInfoXTop.collider.tag == "Crab") {
                    hitInfoXTop.collider.GetComponent<CrabController>().TakeDamage(damage);
                }
            }
            if (hitInfoXBottom.collider != null) {
                if (hitInfoXBottom.collider.tag == "Crab") {
                    hitInfoXBottom.collider.GetComponent<CrabController>().TakeDamage(damage);
                }
            } 
        } else {
            transform.Translate(xSpeed * Time.deltaTime * currentDirection, 0, 0);
        }
    }

    /**
     * Raycast from the bottom corners straight down. If hit, keep walking. If not, an edge is reached, then it turns around.
     */
    private void ManageVerticalMovement() {
        Vector2 down = transform.TransformDirection(Vector2.down);
        float rayOriginY = transform.position.y - (height / 2);
        RaycastHit2D hitInfoYLeft = Physics2D.Raycast(new Vector2(transform.position.x - width / 2, rayOriginY),
            down, verticalCheckLength, collisionMask);
        RaycastHit2D hitInfoYRight = Physics2D.Raycast(new Vector2(transform.position.x + width / 2, rayOriginY),
            down, verticalCheckLength, collisionMask);

        if (!hitInfoYLeft || !hitInfoYRight) {
            currentDirection *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        } 
    }
}
