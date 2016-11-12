using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public float pushDistance;
    public GameObject activator;

    bool activated = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D col) {
        if (!activated && col.gameObject.CompareTag("Crab")) {
            transform.position = new Vector3(transform.position.x, transform.position.y - pushDistance, transform.position.z);
            activated = true;
            activator.GetComponent<IActivatable>().OnActivated();
        }
    }
}
