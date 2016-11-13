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
    [Tooltip ("Must be positive, should probably be approximately 0.01")]
    public float rayOriginOffset; // An offset for the raycasts that check for collisions

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
            RaycastHit2D hitInfoTop = Physics2D.Raycast(new Vector2(rayOriginX, transform.position.y + (height / 2) - rayOriginOffset),
                dir, xSpeed * Time.deltaTime, collisionMask);
            RaycastHit2D hitInfoBottom = Physics2D.Raycast(new Vector2(rayOriginX, transform.position.y - (height / 2) + rayOriginOffset),
                dir, xSpeed * Time.deltaTime, collisionMask);
            float moveDistance = 0;
            if (hitInfoBottom) {
                moveDistance = hitInfoBottom.distance;
            } else if (hitInfoTop) {
                moveDistance = hitInfoTop.distance;
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
            RaycastHit2D hitInfoLeft = Physics2D.Raycast(new Vector2(transform.position.x - (width / 2) + rayOriginOffset, rayOriginY),
                down, ySpeed * Time.deltaTime, collisionMask);
            RaycastHit2D hitInfoRight = Physics2D.Raycast(new Vector2(transform.position.x + (width / 2) - rayOriginOffset, rayOriginY),
                down, ySpeed * Time.deltaTime, collisionMask);
            float moveDistance = 0;
            if (hitInfoLeft) {
                moveDistance = -1 * hitInfoLeft.distance;
            } else if (hitInfoRight) {
                moveDistance = -1 * hitInfoRight.distance;
            } else {
                moveDistance = -1 * ySpeed * Time.deltaTime;
            }
            transform.Translate(0, moveDistance, 0);
        } else {
            Vector2 up = transform.TransformDirection(Vector2.up);
            float rayOriginY = transform.position.y + (height / 2);
            // Raycast up from the top corners of the sprite
            RaycastHit2D hitInfoLeft = Physics2D.Raycast(new Vector2(transform.position.x - (width / 2) + rayOriginOffset, rayOriginY),
                up, ySpeed * Time.deltaTime, collisionMask);
            RaycastHit2D hitInfoRight = Physics2D.Raycast(new Vector2(transform.position.x + (width / 2) - rayOriginOffset, rayOriginY),
                up, ySpeed * Time.deltaTime, collisionMask);
            float moveDistance = 0;
            if (hitInfoLeft) {
                moveDistance = hitInfoLeft.distance;
                jumpTimer = 0;
            } else if (hitInfoRight) {
                moveDistance = hitInfoRight.distance;
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
        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height / 2),
            down, 20, collisionMask);
        return hitInfo.distance < height / 20;
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
