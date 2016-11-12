using UnityEngine;
using System.Collections;

public class CrabController : MonoBehaviour, IDamageable {

    public float xSpeed = 10f, ySpeed = 5f;
    public LayerMask collisionMask;
    private float width, height; // The width/height of the BoxCollider2D
    public int numLives = 3;
    public int maxLives;
    private int invincibilityTimer;
    private int maxInvincibilityFrames = 20;
    private int xDirection = 1;

    // ----- JUMP RELATED VARIABLES -----
    private bool isJumping = false; // Is the crab currently rising from the jump?
    private int jumpTimer = 0; // The time until the crab starts falling after a jump
    public int minJumpDuration; // The minimum number of frames that the crab can jump for
    public int maxJumpDuration; // The maximum number of frames that the crab can jump for

    void Start() {
        width = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        height = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;
    }
	
	void Update () {
        // Move left or right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) {
            // Set direction depending on left or right
            if (Input.GetKey(KeyCode.RightArrow)) xDirection = 1;
            if (Input.GetKey(KeyCode.LeftArrow)) xDirection = -1;
            // Directional vector
            Vector2 dir = transform.TransformDirection(Vector2.right) * xDirection;
            float rayOriginX = transform.position.x + (width / 2) * xDirection;
            // Raycast to from corners to left or right, depending on the value of xDirection
            RaycastHit2D hitInfoTop = Physics2D.Raycast(new Vector2(rayOriginX, transform.position.y + height / 2),
                dir, xSpeed * Time.deltaTime, collisionMask);
            RaycastHit2D hitInfoBottom = Physics2D.Raycast(new Vector2(rayOriginX, transform.position.y - height / 2),
                dir, xSpeed * Time.deltaTime, collisionMask);
            float moveDistance = 0;
            if (hitInfoBottom) {
                moveDistance = hitInfoBottom.distance * Time.deltaTime;
            } else if (hitInfoTop) {
                moveDistance = hitInfoTop.distance * Time.deltaTime;
            } else {
                moveDistance = xSpeed * Time.deltaTime;
            }
            // Move
            transform.Translate(xDirection * moveDistance, 0, 0);
        }
        // Jump
        if (Input.GetKey(KeyCode.UpArrow)) {
            if (!isJumping && OnGround()) {
                isJumping = true;
                jumpTimer = maxJumpDuration;
            }
        }
        // If the player releases the jump button, stop the jump
        if (Input.GetKeyUp(KeyCode.UpArrow)) { 
            if (jumpTimer <= minJumpDuration) {
                isJumping = false;
                jumpTimer = 0;
            } else {
                jumpTimer = minJumpDuration;
            }
        }

        // Check if jumping - if not, fall if not standing on ground
        if (!isJumping) {
            Vector2 down = transform.TransformDirection(Vector2.down);
            float rayOriginY = transform.position.y - (height / 2);
            // Raycast down from the bottom corners of the sprite
            RaycastHit2D hitInfoLeft = Physics2D.Raycast(new Vector2(transform.position.x - width / 2, rayOriginY),
                down, xSpeed * Time.deltaTime, collisionMask);
            RaycastHit2D hitInfoRight = Physics2D.Raycast(new Vector2(transform.position.x + width / 2, rayOriginY),
                down, xSpeed * Time.deltaTime, collisionMask);
            float moveDistance = 0;
            if (hitInfoLeft) {
                moveDistance = -1 * hitInfoLeft.distance * Time.deltaTime;
            } else if (hitInfoRight) {
                moveDistance = -1 * hitInfoRight.distance * Time.deltaTime;
            } else {
                moveDistance = -1 * ySpeed * Time.deltaTime;
            }
            transform.Translate(0, moveDistance, 0);
        } else {
            Vector2 up = transform.TransformDirection(Vector2.up);
            float rayOriginY = transform.position.y + (height / 2);
            // Raycast up from the top corners of the sprite
            RaycastHit2D hitInfoLeft = Physics2D.Raycast(new Vector2(transform.position.x - width / 2, rayOriginY),
                up, xSpeed * Time.deltaTime, collisionMask);
            RaycastHit2D hitInfoRight = Physics2D.Raycast(new Vector2(transform.position.x + width / 2, rayOriginY),
                up, xSpeed * Time.deltaTime, collisionMask);
            float moveDistance = 0;
            if (hitInfoLeft) {
                moveDistance = hitInfoLeft.distance * Time.deltaTime;
                jumpTimer = 0;
            } else if (hitInfoRight) {
                moveDistance = hitInfoRight.distance * Time.deltaTime;
                jumpTimer = 0;
            } else {
                moveDistance = ySpeed * Time.deltaTime;
            }
            transform.Translate(0, moveDistance, 0);

            // Count down jump timer
            jumpTimer--;
            // Stop jumping?
            if (jumpTimer <= 0) {
                isJumping = false;
            }
        }

        if (invincibilityTimer > 0) {
            invincibilityTimer--;
        }
    }

    /**
     * Raycasts a short distance straight down. If hit, return true, else return false.
     */
    private bool OnGround() {
        Vector2 down = transform.TransformDirection(Vector2.down);
        // Raycast a very small distance straight down from the bottom of the sprite (to compensate for small errors)
        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height / 2),
            down, 0.065f, collisionMask);
        return hitInfo;
    }

    public void TakeDamage(int damage) {
        if (invincibilityTimer <= 0) {
            numLives--;
            invincibilityTimer = maxInvincibilityFrames;
        }
        if (numLives <= 0) {
            GameManager.instance.Died(true);
        }
    }
}
