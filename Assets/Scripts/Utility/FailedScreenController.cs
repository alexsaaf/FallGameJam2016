using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FailedScreenController : MonoBehaviour {

    private Text whoDied;

	void Start () {

        whoDied = GameObject.Find("WhoDied").GetComponent<Text>();

        if (whoDied != null) { 
            if (GameManager.instance.result.crabDied) {
                whoDied.text = "The crab died!";
            } else {
                whoDied.text = "The duck died!";
            }
        }
	}

    public void RestartLevel() {
        GameManager.instance.LoadLevel(GameManager.instance.result.levelIndex);
    }

    //Main menu should be on build index 0. ALWAYS!
    public void ToMainMenu() {
        GameManager.instance.LoadLevel(0);
    }
}
