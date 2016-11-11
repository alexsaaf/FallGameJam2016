using UnityEngine;


/// <summary>
/// Basic script that fades the scenes in or out
/// When swapping scene, call BeginFade and then 
/// swap scene after the time returned
/// </summary>
public class SceneFading : MonoBehaviour {

    public Texture fadeOutTexture;
    public float fadeTime;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;
    
    void OnGui() {
        //Fade the alpha in or out
        alpha += fadeDir * 1 / fadeTime * Time.deltaTime;

        //Clamp the alpha value
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade(int direction) {
        fadeDir = direction;
        return fadeTime;
    }
	
    void OnLevelWasLoaded() {
        BeginFade(-1);
    }
}

