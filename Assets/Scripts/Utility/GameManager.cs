using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// The game manager handles the overall game logic and flow
/// Responsible for changing scenes and keeping track of data
/// between scenes.
/// </summary>
[RequireComponent(typeof(SceneFader))]
public class GameManager : MonoBehaviour {

    #region variables

    public Result result;

    public static GameManager instance;

    SceneFader fader;

    #endregion

    //Singleton pattern
    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        //We need the fader for later
        fader = GetComponent<SceneFader>();
        result = new Result();
    }

    public void Died(bool _crabDied) {
        result.crabDied = _crabDied;
        result.levelIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(1);
    }

    //Loads the level after this one in the build settings
    public void LoadNextLevel() {
        int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine("LoadSceneRoutine", buildIndex);
    }

    //Loads the specified scenes
    public void LoadLevel(int buildIndex) {
        StartCoroutine("LoadSceneRoutine", buildIndex);
    }

    //Coroutine for starting fade and changing scene when fade is done
    IEnumerator LoadSceneRoutine(int buildIndex) {
        float fadeTime = fader.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(buildIndex);
    }

}


public class Result {
    public bool crabDied = false;
    public float time = 0;
    public int levelIndex;
}
