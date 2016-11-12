using UnityEngine;
using System.Collections;

public class DuckShotController : MonoBehaviour {

    //The movementVector for the shot
    public Vector3 movementDirection;

    float movementSpeed = 7;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(movementDirection * movementSpeed);
	}

    void OnCollissionEnter2D(CircleCollider2D collider) {

    }
}
