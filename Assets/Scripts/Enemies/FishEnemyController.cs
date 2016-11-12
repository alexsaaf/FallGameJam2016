using UnityEngine;
using System.Collections;

public class FishEnemyController : MonoBehaviour {

    public float xSpeed, ySpeed;
    public float maxDistance;
    public LayerMask collisionMask;
    private int currentDirection = -1;
    private float yStartPosition;
    private float width, height;
    private int damage = 1;


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
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Crab" || other.tag == "Duck") {
            other.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }


}
