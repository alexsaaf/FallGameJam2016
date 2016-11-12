using UnityEngine;

public class Goal : MonoBehaviour {


	void Start () {
	
	}
	

	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D colli) {
        if(colli.collider.tag == "Crab" || colli.collider.tag == "Duck") {
            GameManager.instance.Won();
        }

    }
}
