using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// The game manager handles the overall game logic and flow
/// Responsible for changing scenes and keeping track of data
/// between scenes.
/// </summary>
[RequireComponent(typeof(SceneFader)), RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {

    #region variables
    //Information about the level just completed
    public Result result;

    //The music to play by default
    public AudioClip defaultMusic;

    //Instance used for quick access to the GameManager
    public static GameManager instance;

    //The timepoint that the level started
    public float startTime;

    //The fader used to fade between scenes
    SceneFader fader;

    private AudioSource audioSource;

    private LevelInfo levelInfo;

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
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = defaultMusic;
        audioSource.Play();

        //We need the fader for later
        fader = GetComponent<SceneFader>();
        //Create the result object
        result = new Result();
    }

    //Called by character when they die.
    public void Died(bool _crabDied) {
        result.time = Time.time - startTime;        
        result.crabDied = _crabDied;
        result.levelIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(1);
    }

    //Loads the level after this one in the build settings
    //DEPRECATED: Will most likely not be used at all.
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


    void OnLevelWasLoaded() {
        startTime = Time.time;
        GameObject levelInfoObject = GameObject.Find("LevelInfo");
        LevelInfo info = null;
        if(levelInfoObject != null) {
            info = levelInfoObject.GetComponent<LevelInfo>();
        }
        //Log if there is no levelinfo. Not necessarily a problem.
        if(info == null) {
            Debug.Log("There is no levelinfo in this level!");
        } else {
            //If there is music for the level, play it
            if(info.levelMusic != null) {
                audioSource.clip = info.levelMusic;
                audioSource.Play();
            } else {
                //If the clip is already the default, do nothing
                if(audioSource.clip != defaultMusic) {
                    audioSource.clip = defaultMusic;
                    audioSource.Play();
                }
            }
        }
    }
}


public class Result {
    public bool crabDied = false;
    public float time = 0;
    public int levelIndex;
}
