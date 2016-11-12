using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private DuckController duck;
    private CrabController crab;

    private Image duckHealth1, duckHealth2, duckHealth3;
    private Image crabHealth1, crabHealth2, crabHealth3;
    private Text timerText;

	void Start () {
        //Get the characters
        crab = GameObject.Find("CrabCharacter").GetComponent<CrabController>();
        //duck = GameObject.Find("Duck").GetComponent<DuckController>();
        
        //Find the duckHealth images
        //This must be uncommented when there is a duck prefab =)
        /*
        duckHealth1 = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        duckHealth2 = transform.GetChild(0).GetChild(3).GetComponent<Image>();
        duckHealth3 = transform.GetChild(0).GetChild(5).GetComponent<Image>();
        */
           
        //Find the crabhealth images
        crabHealth1 = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        crabHealth2 = transform.GetChild(1).GetChild(3).GetComponent<Image>();
        crabHealth3 = transform.GetChild(1).GetChild(5).GetComponent<Image>();

        timerText = transform.GetChild(2).GetComponent<Text>();
    }

	void Update () {
        //UpdateDuckHealth();
        UpdateCrabHealth();
        UpdateTimer();
	}

    void UpdateTimer() {
        float time = Time.time - GameManager.instance.startTime;
        int roundedTime = Mathf.RoundToInt(time);
        timerText.text = roundedTime.ToString() + " s";
    }

    //This is very ugly code... but hey, #GameJam!
    void UpdateDuckHealth() {
        //Replace with the actual health later
        int health = 2;
        Color color = duckHealth1.color;
        //Set the visible health symbols accordingly to the health
        switch (health) {
            case 0:
                duckHealth1.color = new Color(color.r, color.g, color.b, 0);
                duckHealth2.color = new Color(color.r, color.g, color.b, 0);
                duckHealth3.color = new Color(color.r, color.g, color.b, 0);
                break;
            case 1:
                duckHealth1.color = new Color(color.r, color.g, color.b, 1);
                duckHealth2.color = new Color(color.r, color.g, color.b, 0);
                duckHealth3.color = new Color(color.r, color.g, color.b, 0);
                break;
            case 2:
                duckHealth1.color = new Color(color.r, color.g, color.b, 1);
                duckHealth2.color = new Color(color.r, color.g, color.b, 1);
                duckHealth3.color = new Color(color.r, color.g, color.b, 0);
                break;
            case 3:
                duckHealth1.color = new Color(color.r, color.g, color.b, 1);
                duckHealth2.color = new Color(color.r, color.g, color.b, 1);
                duckHealth3.color = new Color(color.r, color.g, color.b, 1);
                break;
        }
    }

    //This is very ugly code... but hey, #GameJam!
    void UpdateCrabHealth() {
        int health = crab.numLives;
        Color color = crabHealth1.color;
        //Set the visible health symbols accordingly to the health
        switch (health) {
            case 0:
                crabHealth1.color = new Color(color.r, color.g, color.b, 0);
                crabHealth2.color = new Color(color.r, color.g, color.b, 0);
                crabHealth3.color = new Color(color.r, color.g, color.b, 0);
                break;
            case 1:
                crabHealth1.color = new Color(color.r, color.g, color.b, 1);
                crabHealth2.color = new Color(color.r, color.g, color.b, 0);
                crabHealth3.color = new Color(color.r, color.g, color.b, 0);
                break;
            case 2:
                crabHealth1.color = new Color(color.r, color.g, color.b, 1);
                crabHealth2.color = new Color(color.r, color.g, color.b, 1);
                crabHealth3.color = new Color(color.r, color.g, color.b, 0);
                break;
            case 3:
                crabHealth1.color = new Color(color.r, color.g, color.b, 1);
                crabHealth2.color = new Color(color.r, color.g, color.b, 1);
                crabHealth3.color = new Color(color.r, color.g, color.b, 1);
                break;
        }
    }
}
