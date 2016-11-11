using UnityEngine;
using UnityEngine.UI;

public class ClearedScreenController : MonoBehaviour {

    void Start() {
        GameObject.Find("TimeValue").GetComponent<Text>().text = GameManager.instance.result.time.ToString() + " s";
    }

    public void NextLevel() {
        GameManager.instance.LoadLevel(GameManager.instance.result.levelIndex + 1);
    }

    public void ToMainMenu() {
        GameManager.instance.LoadLevel(0);
    }
}
