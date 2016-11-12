using UnityEngine;
using System.Collections;

public class DuckShotController : MonoBehaviour {

    //The movementVector for the shot
    public Vector3 movementDirection = Vector3.right;

    float movementSpeed = 10;

    int damage = 1;

    //Time until elimitaion
    int eliminationTimer = 0;
    int maxLifeSpan = 150;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(movementDirection * movementSpeed*Time.deltaTime);
        eliminationTimer++;
        if (eliminationTimer >= maxLifeSpan) {
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter2D(Collision2D coll) {
        IDamageable id = coll.transform.GetComponent<IDamageable>();
        if (id != null) {
            id.TakeDamage(damage);
        }
        Debug.Log("YOU MOTHER FUCKER");
        Destroy(gameObject);
    }
}
