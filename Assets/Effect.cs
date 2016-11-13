using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

    public AudioClip soundEffect;
    public float activeTime;
    Vector2 startScale;
    public Vector2 goalScale;

	void Start () {
	    if(soundEffect != null) {
            AudioSource.PlayClipAtPoint(soundEffect, Vector2.zero);
        }
        startScale = transform.localScale;
        StartCoroutine("Transformation");
	}

    IEnumerator Transformation() {
        float t = 0;
        while(t < 1) {
            Debug.Log(t);
            t += Time.deltaTime / activeTime;
            transform.localScale = Vector2.Lerp(startScale, goalScale, t);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }

	
}





