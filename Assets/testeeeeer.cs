using UnityEngine;
using System.Collections;

public class testeeeeer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("Beep");
	}
	
	// Update is called once per frame
	void Update () {

    }


    IEnumerator Beep() {
        while (true) {

        Debug.Log("GetAxis: " + Input.GetAxis("CrabHorizontal"));
        Debug.Log("GetAxisRaw: " + Input.GetAxisRaw("CrabHorizontal"));
            yield return new WaitForSeconds(0.5f);
        }
    }
}
