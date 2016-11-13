using UnityEngine;

public class Goal : MonoBehaviour {


	void Start () {
	
	}
	

	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D colli) {
        if(colli.tag == "Crab" || colli.tag == "Duck") {
            GameManager.instance.Won();
        }

    }
}
