﻿using UnityEngine;
using System.Collections;
using System;

public class FlygandeKompisController : MonoBehaviour, IDamageable {

    // the movement of the Enemy
    [SerializeField, Range(-10, 10)]
    float horizontalSpeed = -5;

   public  Vector3 moveVector = Vector3.right;

    public GameObject poof;
    public GameObject pow;

    //the number of lives
    public int numLive = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(moveVector * horizontalSpeed * Time.deltaTime);
	}

    void OnCollisionEnter2D(Collision2D coll) {
        Debug.Log("We GONT AN COLLISION FFS: " + coll.collider.tag);
        if (coll.collider.tag.Equals("Duck") || coll.collider.tag.Equals("Crab")) {
            IDamageable idmg = coll.collider.transform.GetComponent<IDamageable>();
            idmg.TakeDamage(1);
            Instantiate(pow, transform.position, Quaternion.identity);
        } else {
            Instantiate(poof, transform.position, Quaternion.identity);
        }
       
        Destroy(gameObject);
    }

    public void TakeDamage(int damage) {
        if (--numLive < 1) {
            Instantiate(poof, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
