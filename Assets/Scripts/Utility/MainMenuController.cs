using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public void StartGame() {
        GameManager.instance.LoadLevel(3);
    }

}
