using UnityEngine;

public class DeathTrigger : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D colli) {
        if(colli.tag == "Crab") {
            GameManager.instance.Died(true);
        }
        if (colli.tag == "Duck") {
            GameManager.instance.Died(false);
        }
        if(colli.tag == "Enemy") {
            Destroy(colli.gameObject);
        }
    }
}
