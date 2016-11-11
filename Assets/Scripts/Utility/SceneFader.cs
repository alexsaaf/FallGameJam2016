using UnityEngine;


/// <summary>
/// Basic script that fades the scenes in or out
/// When swapping scene, call BeginFade and then 
/// swap scene after the time returned
/// </summary>
public class SceneFader : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;
    
    //Function called by unity
    void OnGUI() {
        //Fade the alpha in or out
        alpha += fadeDir * 1 / fadeSpeed * Time.deltaTime;

        //Clamp the alpha value
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    //Start fading in direction. Also returns the time it will take to fade.
    public float BeginFade(int direction) {
        fadeDir = direction;
        return fadeSpeed;
    }
	
    //Automatically called by unity when a scene has been loaded
    void OnLevelWasLoaded() {
        BeginFade(-1);
    }
}

