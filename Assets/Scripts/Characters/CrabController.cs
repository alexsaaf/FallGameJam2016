using UnityEngine;
using System.Collections;

public class CrabController : MonoBehaviour {

    public float xSpeed = 10f, ySpeed = 5f;
    public LayerMask collisionMask;
    private float radius;
    public int numLives;
    public int maxLives;

    // ----- JUMP RELATED VARIABLES -----
    private bool isJumping = false; // Is the crab currently rising from the jump?
    private int jumpTimer = 0; // The time until the crab starts falling after a jump
    public int minJumpDuration; // The minimum number of frames that the crab can jump for
    public int maxJumpDuration; // The maximum number of frames that the crab can jump for

    void Start() {
        // Assuming that the object is circular -> same scale for x and y
        radius = transform.localScale.x * GetComponent<CircleCollider2D>().radius;
    }
	
	void Update () {
        // Move right
	    if (Input.GetKey(KeyCode.RightArrow)) {
            Vector2 right = transform.TransformDirection(Vector2.right);
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x + radius, transform.position.y), 
                right, xSpeed * Time.deltaTime, collisionMask);
            if (!hitInfo) {
                transform.Translate(xSpeed * Time.deltaTime, 0, 0);
            } else {
                transform.Translate(hitInfo.distance * Time.deltaTime, 0, 0);
            }
        }
        // Move left
        if (Input.GetKey(KeyCode.LeftArrow)) {
            Vector2 left = transform.TransformDirection(Vector2.left);
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x - radius, transform.position.y),
                left, xSpeed * Time.deltaTime, collisionMask);
            if (!hitInfo) {
                transform.Translate(-1 * xSpeed * Time.deltaTime, 0, 0);
            } else {
                transform.Translate(-1 * hitInfo.distance * Time.deltaTime, 0, 0);
            }
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
            // Raycast down from the bottom of the sprite
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - radius),
                down, ySpeed * Time.deltaTime, collisionMask);
            if (!hitInfo) {
                transform.Translate(0, -1 * ySpeed * Time.deltaTime, 0);
            } else {
                transform.Translate(0, -1 * hitInfo.distance * Time.deltaTime, 0);
            }
        } else {
            Vector2 up = transform.TransformDirection(Vector2.up);
            // Raycast up from the top of the sprite
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + radius),
                up, ySpeed * Time.deltaTime, collisionMask);
            if (!hitInfo) {
                transform.Translate(0, ySpeed * Time.deltaTime, 0);
            } else {
                transform.Translate(0, hitInfo.distance * Time.deltaTime, 0);
                jumpTimer = 0; // If hit something above, stop ascending. isJump is set to false after else case 
            }
            // Count down jump timer
            jumpTimer--;
            // Stop jumping?
            if (jumpTimer <= 0) {
                isJumping = false;
            }
        }
    }

    /**
     * Raycasts a short distance straight down. If hit, return true, else return false.
     */
    private bool OnGround() {
        Vector2 down = transform.TransformDirection(Vector2.down);
        // Raycast a very small distance straight down from the bottom of the sprite (to compensate for small errors)
        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - radius),
            down, 0.065f, collisionMask);
        return hitInfo;
    }
}
