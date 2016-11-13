using UnityEngine;
using UnityEngine.UI;

public class ClearedScreenController : MonoBehaviour {

    void Start() {
        GameObject.Find("TimeValue").GetComponent<Text>().text = GameManager.instance.result.time.ToString() + " s";
    }

    void Update() {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("CrabJump")) {
            NextLevel();
        }
        if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("CrabBack")) {
            ToMainMenu();
        }
    }

    public void NextLevel() {
        GameManager.instance.LoadLevel(GameManager.instance.result.levelIndex + 1);
    }

    public void ToMainMenu() {
        GameManager.instance.LoadLevel(0);
    }
}
