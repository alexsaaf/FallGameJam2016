using UnityEngine;
using System.Collections;
using System;

public class GateScript : MonoBehaviour, IActivatable {

    bool open = false;

    public void OnActivated() {
        if (!open) {
            Collider2D col = GetComponent<BoxCollider2D>();
            col.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
