using UnityEngine;
using System.Collections;

enum RayDirectionSearch {
    UP,
    RIGHT,
    LEFT,
    DOWN
}

public class DuckController : MonoBehaviour {
    //The vertical-movementspeed for the duck
    [SerializeField, Range(0, 10),Tooltip("The movement in the y-axel.")]
    double verticalSpeed = 5;
    //The horizontal movement-speed for the duck
    [SerializeField, Range(0, 10), Tooltip("The movement in the x-axel.")]
    double horizontalSpeed = 5;

    //The life of the Duck
    public int numLives = 3;

    //A timer for invincibility
    int invincTimer = 0;
    [SerializeField, Range(0, 50), Tooltip("The number of frames the duck should be invincible")]
    int invincibleFrames = 10;

    //A collision-Mast to mask out the duck from the raycasting
    public LayerMask collisionMask;

    //The collider for the duck
    BoxCollider2D collider;

    float seaLevel;

    //If the duck is in a dive
    bool inDiving = false;

    //The distance that the duck can dive into the sea.
    float diveDepth = 2;

    bool keepDiving = false;
    bool rising = false;
    [SerializeField, Range(1, 30), Tooltip("The number of iterations that the duck will keep the depth of the dive to thes rise")]
    int downTime = 5;


	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider2D>();
        //LevelInfo li = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
        //seaLevel = li.seaLevel;
        //diveDepth = li.diveDepth;
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
    private Vector3 CalculateMoveVectorCollision(int rayCount, RayDirectionSearch direction, Bounds bounds, float vs, float hs, Transform tf) {
        float distance;
        float jumpLength;
        Vector3 startPosOffset = Vector3.zero;
        float percentOffset = 0.02f;
        // calculate the jumpdistance dpending on the number of rays and direktion
        if (direction == RayDirectionSearch.DOWN || direction == RayDirectionSearch.UP) {
            jumpLength = (float)(bounds.extents.x * 2*(1-percentOffset)) / (rayCount - 1);
            distance = vs * Time.deltaTime;
            startPosOffset.x = bounds.extents.x * 2 * (percentOffset/2);
        } else {
            jumpLength = (float)(bounds.extents.y * 2*(1-percentOffset)) / (rayCount - 1);
            distance = hs * Time.deltaTime;
            startPosOffset.y = bounds.extents.y * 2 * (percentOffset/2);

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
                startPos = new Vector3(bounds.min.x, bounds.max.y, 0) + startPosOffset;
                break;
            case RayDirectionSearch.DOWN:
                rayDirection = tf.up * (-1);
                rayJump.x = jumpLength;
                startPos = new Vector3(bounds.min.x, bounds.min.y, 0) + startPosOffset;
                break;
            case RayDirectionSearch.LEFT:
                rayDirection = tf.right * (-1);
                rayJump.y = jumpLength * (-1);
                startPos = new Vector3(bounds.min.x, bounds.max.y, 0) - startPosOffset;
                break;
            default:
                rayDirection = tf.right;
                rayJump.y = jumpLength * (-1);
                startPos = new Vector3(bounds.max.x, bounds.max.y, 0) - startPosOffset;
                break;
        }
        Vector3 moveVector = rayDirection * distance;
        float minDistance = Mathf.Infinity;
        RaycastHit2D tmp = new RaycastHit2D();
        for (int i = 0; i < rayCount; i++) {
            Debug.DrawRay(startPos, rayDirection, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(startPos, rayDirection, distance, collisionMask);
            if (hit.collider != null) {
                if (direction == RayDirectionSearch.DOWN || direction == RayDirectionSearch.UP) {
                    if (minDistance > hit.distance) {
                        minDistance = hit.distance;
                        moveVector.y = hit.distance*rayDirection.y;
                        tmp = hit;
                    }
                }
                else {
                    if (minDistance > hit.distance) {
                        minDistance = hit.distance;
                        moveVector.x = hit.distance * rayDirection.x;
                    }
                }
            }
            startPos += rayJump;
        }
        if (tmp.collider != null) {
            OnCollision(tmp);
        }
        return moveVector;
    }

    void FixedUpdate() {
        if (!inDiving) {
            if (collider.bounds.min.y < seaLevel) {
                Debug.Log("To low, Duck is under the surface.");
                transform.Translate(CalculateMoveVectorCollision(3, RayDirectionSearch.UP, collider.bounds, (float)verticalSpeed, (float)horizontalSpeed, transform));
            }
            else {
                // Movement uppwards
                if (Input.GetAxisRaw("Vertical") > 0) {
                    Vector3 movementVector = CalculateMoveVectorCollision(3, RayDirectionSearch.UP, collider.bounds, (float)verticalSpeed, (float)horizontalSpeed, transform);
                    transform.Translate(movementVector);
                }
                //Movement Downwards
                if (Input.GetAxisRaw("Vertical") < 0) {
                    Vector3 movementVector = CalculateMoveVectorCollision(3, RayDirectionSearch.DOWN, collider.bounds, (float)verticalSpeed, (float)horizontalSpeed, transform);
                    //Check if the duck will be below the seaLevel
                    if ((collider.bounds.min.y + movementVector.y) < seaLevel && collider.bounds.min.y > seaLevel) {
                        movementVector.y = collider.bounds.min.y - seaLevel;
                    }
                    transform.Translate(movementVector);
                }
                //Movement righ
                if (Input.GetAxisRaw("Horizontal") > 0) {
                    Vector3 movementVector = CalculateMoveVectorCollision(3, RayDirectionSearch.RIGHT, collider.bounds, (float)verticalSpeed, (float)horizontalSpeed, transform);
                    transform.Translate(movementVector);
                }
                //Movement left
                if (Input.GetAxisRaw("Horizontal") < 0) {
                    Vector3 movementVector = CalculateMoveVectorCollision(3, RayDirectionSearch.LEFT, collider.bounds, (float)verticalSpeed, (float)horizontalSpeed, transform);
                    transform.Translate(movementVector);
                } 
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!inDiving) {
            if (Input.GetAxisRaw("Fire1") > 0) {
                keepDiving = true;
                if (!inDiving) {
                    StartCoroutine(DivingRoutine(diveDepth,new Vector3((float)horizontalSpeed,(float)verticalSpeed,0)));
                    inDiving = true;
                }
            } 
        }
        else {
            if (Input.GetAxisRaw("Fire1")  <= 0) {
                keepDiving = false;
            }
        }
        if (invincTimer > 0) {
            invincTimer -= 1;
        }
	}

    IEnumerator DivingRoutine(float _diveDepth, Vector3 diveSpeed) {
        while (keepDiving && !rising) {
            Vector3 yMovement = CalculateMoveVectorCollision(3, RayDirectionSearch.DOWN, collider.bounds, diveSpeed.y, diveSpeed.x, transform);
            if (yMovement.y == 0) {
                rising = true;
            }
            if ((collider.bounds.min.y+yMovement.y) < seaLevel+(-1)*_diveDepth) {
                yMovement.y = (seaLevel + (-1)*_diveDepth) - collider.bounds.min.y;
                rising = true;
            }
            transform.Translate(yMovement);
            transform.Translate(CalculateMoveVectorCollision(3, RayDirectionSearch.RIGHT, collider.bounds, diveSpeed.y, diveSpeed.x, transform));
            yield return  null;
        }
        if (collider.bounds.min.y < seaLevel) {
            rising = true;
            for (int i = 0; i < downTime; i++) {
                transform.Translate(CalculateMoveVectorCollision(3, RayDirectionSearch.RIGHT, collider.bounds, 0, diveSpeed.x, transform));
                yield return null;
            }

            Debug.Log("Befor rising, the rising boolean: " + rising);
            while (rising && (collider.bounds.min.y < seaLevel)) {
                transform.Translate(CalculateMoveVectorCollision(3, RayDirectionSearch.UP, collider.bounds, diveSpeed.y, diveSpeed.x, transform));
                transform.Translate(CalculateMoveVectorCollision(3, RayDirectionSearch.RIGHT, collider.bounds, diveSpeed.y, diveSpeed.x, transform));
                yield return null;
            } 
        }
        Debug.Log("Not diving anymore");
        rising = false;
        inDiving = false;
    }

    void OnCollision(RaycastHit2D hit) {
        if (hit.transform.tag.Equals("Enemy")) {
            Debug.Log("Hit an enemy");
        }
        Debug.Log("OnCollisionEnter triggerd.");
    }

    void TakeDamage(int dmg) {
        if (invincTimer == 0) {
            int tmp = numLives - dmg;
            if (tmp < 1) {
                numLives = 0;
                GameManager.instance.Died(false);
            }
            else {
                numLives = tmp;
            }
            invincTimer = invincibleFrames;
        }
    }
}
