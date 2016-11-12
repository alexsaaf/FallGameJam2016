using UnityEngine;

public class CameraController : MonoBehaviour {

    public float moveSpeed = 10f;

	void Update () {
        Vector2 movement = transform.right * moveSpeed * Time.deltaTime;
        transform.Translate(movement);	    
	}
}
