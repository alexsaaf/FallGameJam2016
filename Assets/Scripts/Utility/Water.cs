using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    Camera mainCamera;

	void Start () {
        LevelInfo info = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
        transform.position = new Vector2(transform.position.x, info.seaLevel - transform.localScale.y / 2);
        mainCamera = Camera.main;
    }

	void Update () {
        Vector2 newPos = new Vector2(mainCamera.transform.position.x, transform.position.y);
        transform.position = newPos;
	}
}
