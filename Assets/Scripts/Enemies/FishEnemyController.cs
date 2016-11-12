using UnityEngine;
using System.Collections;

public class FishEnemyController : MonoBehaviour {

    public float xSpeed, ySpeed;
    private int damage = 1;
    public float maxDistance;
    public float xRaycastMargin;
    public LayerMask collisionMask;
    private int currentDirection = -1;
    private float yStartPosition;
    private float width, height;
    private ArrayList raycastHits = new ArrayList();
    

	void Start () {
        yStartPosition = transform.position.y;
        width = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        height = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;
    }
	
	void Update () {
        if (currentDirection == 1) {
            if (transform.position.y > yStartPosition + maxDistance) {
                currentDirection *= -1;
            }
        } else if (currentDirection == -1) {
            if (transform.position.y < yStartPosition - maxDistance) {
                currentDirection *= -1;
            }
        }

        transform.Translate(0, ySpeed * currentDirection * Time.deltaTime, 0);
        transform.Translate(xSpeed * Time.deltaTime * -1, 0, 0);
        CheckCharacterCollisions();
    }

    private void CheckCharacterCollisions() {
        Vector2 dirX = transform.TransformDirection(Vector2.left);
        Vector2 dirY = transform.TransformDirection(Vector2.up);
        // The point on the x axis from which the ray will be cast (left border of collider)
        float rayOriginX = transform.position.x - (width / 2);

        // Raycast everywhere :D :D :D 
        // Seriously, these are raycasts from every corner facing left, upwards and downwards
        RaycastHit2D hitInfoXTop = Physics2D.Raycast(new Vector2(rayOriginX, transform.position.y + (height - xRaycastMargin)),
            dirX, xSpeed * Time.deltaTime, collisionMask);
        RaycastHit2D hitInfoXBottom = Physics2D.Raycast(new Vector2(rayOriginX, transform.position.y - (height - xRaycastMargin)),
            dirX, xSpeed * Time.deltaTime, collisionMask);
        if (hitInfoXTop || hitInfoXBottom) {
            // If colliding with the crab
            //if (hitInfoXTop.collider != null) {
            //    if (hitInfoXTop.collider.tag == "Crab" || hitInfoXTop.collider.tag == "Duck") {
            //        hitInfoXTop.collider.GetComponent<CrabController>().TakeDamage(damage);
            //    }
            //} else if (hitInfoXBottom.collider != null) {
            //    if (hitInfoXBottom.collider.tag == "Crab" || hitInfoXTop.collider.tag == "Duck") {
            //        hitInfoXBottom.collider.GetComponent<CrabController>().TakeDamage(damage);
            //    }
            //}
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Collision!");
        if (other.tag == "Crab" || other.tag == "Duck") {
            other.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }


}
