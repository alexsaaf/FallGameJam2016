using UnityEngine;
using System.Collections;

/// <summary>
/// The game manager handles the overall game logic and flow
/// Responsible for changing scenes and keeping track of data
/// between scenes.
/// </summary>
public class GameManager : MonoBehaviour {

    #region variables
    public static GameManager instance;

    SceneFader fader;

    #endregion
     
    void Awake() {
        if(instance = null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        fader = GetComponent<SceneFader>();
    }

    //Loads the level after this one in the build settings
    public void LoadNextLevel() {
        
    }

    //Loads the specified scene
    public void LoadScene(int buildIndex) {
        
    }


}
