using UnityEngine;
using System.Collections;

enum RayDirectionSearch {
    UP,
    RIGHT,
    LEFT,
    DOWN
}

public class DuckController : MonoBehaviour {
    [SerializeField, Range(0, 10)]
    double verticalSpeed = 5;
    [SerializeField, Range(0, 10)]
    double horizontalSpeed = 5;

    public LayerMask collisionMask;

    BoxCollider2D collider;

    float seaLevel;

	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider2D>();
        seaLevel = -5;
	}

    /// <summary>
    /// This method retuns a vector of  direction that was gien and given the speed it returns a vetor to move in that direction
    ///  and shortens it if it will collide with someting in the given distance
    /// </summary>
    /// <param name="rayCount">This is the number of rays that will ge cast to check for collision</param>
    /// <param name="direction">This is the direction of where you want the raycast</param>
    /// <param name="bounds">This is the bound of your collider</param>
    /// <param name="vs">This is the vertical speed, to move with</param>
    /// <param name="hs">This is the horizontal speed to move with</param>
    /// <param name="tf">This is the transform of the object</param>
    /// <returns>Returns a vector of the max movement vector (gievn deltaTime) to translate to</returns>
    private Vector3 CalculateMoveVectorCollision(int rayCount, RayDirectionSearch direction, Bounds bounds, float vs,float hs, Transform tf) {
        float distance;
        float jumpLength;
        // calculate the jumpdistance dpending on the number of rays and direktion
        if (direction == RayDirectionSearch.DOWN || direction == RayDirectionSearch.UP) {
            jumpLength = (float)(bounds.extents.x * 2) / (rayCount-1);
            distance = vs * Time.deltaTime;
        } else {
            jumpLength = (float)(bounds.extents.y * 2) / (rayCount-1);
            distance = hs * Time.deltaTime;

        }
        Vector3 rayDirection;
        Vector3 rayJump = Vector3.zero;
        Vector3 startPos;
        // setup the ray search direktion, a vector for jumping and start position depending on the
        // search direktion
        switch (direction) {
            case RayDirectionSearch.UP:
                rayDirection = tf.up;
                rayJump.x = jumpLength;
                startPos = new Vector3(bounds.min.x, bounds.max.y, 0);
                break;
            case RayDirectionSearch.DOWN:
                rayDirection = tf.up * (-1);
                rayJump.x = jumpLength;
                startPos = new Vector3( bounds.min.x,  bounds.min.y, 0);
                break;
            case RayDirectionSearch.LEFT:
                rayDirection = tf.right * (-1);
                rayJump.y = jumpLength * (-1);
                startPos = new Vector3( bounds.min.x,  bounds.max.y, 0);
                break;
            default:
                rayDirection = tf.right;
                rayJump.y = jumpLength * (-1);
                startPos = new Vector3( bounds.max.x,  bounds.max.y, 0);
                break;
        }
        Vector3 moveVector = rayDirection*distance;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < rayCount; i++) {
            Debug.DrawRay(startPos, rayDirection, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(startPos, rayDirection, distance, collisionMask);
            if (hit.collider != null) {
                if (direction == RayDirectionSearch.DOWN || direction == RayDirectionSearch.UP) {
                    if (minDistance > Mathf.Abs(startPos.y - hit.point.y)) {
                        minDistance = Mathf.Abs(startPos.y - hit.point.y);
                        moveVector.y = startPos.y - hit.point.y;
                    }
                }
                else {
                    if (minDistance  > Mathf.Abs(startPos.x - hit.point.x)) {
                        minDistance = Mathf.Abs(startPos.x - hit.point.x);
                        moveVector.x = startPos.x - hit.point.x;
                    }

                }
            }
            startPos += rayJump;
        }

        return moveVector;
    }

    void FixedUpdate() {
        if (Input.GetAxisRaw("Vertical") > 0) {
            Vector3 movementVector = CalculateMoveVectorCollision(3, RayDirectionSearch.UP,collider.bounds, (float)verticalSpeed, (float)horizontalSpeed,transform);
            transform.Translate(movementVector);
        }
        if (Input.GetAxisRaw("Vertical") < 0) {
            Vector3 movementVector = CalculateMoveVectorCollision(3, RayDirectionSearch.DOWN, collider.bounds, (float)verticalSpeed, (float)horizontalSpeed, transform);
            if ((transform.position + movementVector).x < seaLevel) {
                movementVector.x = transform.position.x - seaLevel;
            }
            transform.Translate(movementVector);
        }
        if (Input.GetAxisRaw("Horizontal") > 0) {
            Vector3 movementVector = CalculateMoveVectorCollision(3, RayDirectionSearch.RIGHT, collider.bounds, (float)verticalSpeed, (float)horizontalSpeed, transform);
            transform.Translate(movementVector);
        }
        if (Input.GetAxisRaw("Horizontal") < 0) {
            Vector3 movementVector = CalculateMoveVectorCollision(3, RayDirectionSearch.LEFT, collider.bounds, (float)verticalSpeed, (float)horizontalSpeed, transform);
            transform.Translate(movementVector);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetAxisRaw("Fire1") > 1) {

        }
	}
}
