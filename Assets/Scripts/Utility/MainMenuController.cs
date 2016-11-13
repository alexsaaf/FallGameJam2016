using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public void StartGame() {
        GameManager.instance.LoadLevel(3);
    }

    void Update() {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("CrabJump")) {
            StartGame();
        }
    }

}
