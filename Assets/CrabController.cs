using UnityEngine;
using System.Collections;

public class CrabController : MonoBehaviour {

    public float xSpeed = 10f, ySpeed;
    public float fallSpeed = 5f;
    public LayerMask collisionMask;
    private float radius;
    private bool isJumping = false;
    private int jumpTimer = 0;

    void Start() {
        // Assuming that the object is circular -> same scale for x and y
        radius = transform.localScale.x * GetComponent<CircleCollider2D>().radius;
    }
	
	void Update () {
        // Move right
	    if (Input.GetKey(KeyCode.RightArrow)) {
            Vector2 right = transform.TransformDirection(Vector2.right);
            if (!Physics.Raycast(transform.position, right, xSpeed)) {
                transform.Translate(xSpeed * Time.deltaTime, 0, 0);
            }
        }
        // Move left
        if (Input.GetKey(KeyCode.LeftArrow)) {
            Vector2 left = transform.TransformDirection(Vector2.left);
            if (!Physics.Raycast(transform.position, left, xSpeed)) {
                transform.Translate(-1 * xSpeed * Time.deltaTime, 0, 0);
            }
        }

        // Jump
        if (Input.GetKey(KeyCode.UpArrow)) {
            if (!isJumping && OnGround()) {
                isJumping = true;
                jumpTimer = 100;
            }
        }

        // Check if jumping - if not, fall if not standing on ground
        if (!isJumping) {
            Vector2 down = transform.TransformDirection(Vector2.down);
            // Raycast down from the bottom of the sprite
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - radius), 
                down, fallSpeed * Time.deltaTime, collisionMask);
            if (!hitInfo) {
                transform.Translate(0, -1 * fallSpeed * Time.deltaTime, 0);
            } else {
                transform.Translate(0, -1 * (hitInfo.distance) * Time.deltaTime, 0);
            }
        } else {
            Vector2 up = transform.TransformDirection(Vector2.up);
            // Raycast up from the top of the sprite
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + radius), 
                up, fallSpeed * Time.deltaTime, collisionMask);
            if (!hitInfo) {
                transform.Translate(0, fallSpeed * Time.deltaTime, 0);
            } else {
                transform.Translate(0, (hitInfo.distance) * Time.deltaTime, 0);
            }
            // Count down jump timer
            jumpTimer--;
            // Stop jumping?
            if (jumpTimer == 0) {
                isJumping = false;
            }
        }
        
    }
    // THIS DOES NOT WORK AT ALL
    private bool OnGround() {
        Vector2 down = transform.TransformDirection(Vector2.down);
        // Raycast down from the bottom of the sprite
        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - radius),
            down, fallSpeed * Time.deltaTime, collisionMask);
        Debug.Log(hitInfo.distance);
        return hitInfo.distance == 0;


       
    }
}
