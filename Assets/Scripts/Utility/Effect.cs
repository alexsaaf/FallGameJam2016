﻿using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

    public AudioClip soundEffect;
    public float activeTime;
    Vector2 startScale;
    public Vector2 goalScale;

	void Start () {
	    if(soundEffect != null) {
            AudioSource.PlayClipAtPoint(soundEffect, transform.position,1);
        }
        startScale = transform.localScale;
        StartCoroutine("Transformation");
	}

    IEnumerator Transformation() {
        float t = 0;
        while(t < 1) {
            t += Time.deltaTime / activeTime;
            transform.localScale = Vector2.Lerp(startScale, goalScale, t);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }

	
}





