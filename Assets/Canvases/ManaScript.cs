using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
//using UnityEditor;

public class ManaScript : MonoBehaviour {

	public float timeParameter;
	public float minAlphaValue;
	public float maxAlphaValue;
	public float alphaValueAssignment;
	public float speed; 
	Color manaImageColor;


	// Use this for initialization
	void Start () {
		timeParameter = 0;
		manaImageColor = gameObject.transform.GetChild (2).GetComponent<Image> ().color;
	}

	// Update is called once per frame
	void Update () {
		ManaBarFlash ();
		manaImageColor.a = alphaValueAssignment;
		gameObject.transform.GetChild (2).GetComponent<Image> ().color = manaImageColor; 

	}

	private void ManaBarFlash(){
		if (timeParameter < 1.0f) {
			timeParameter += Time.deltaTime * speed; 
			alphaValueAssignment = Mathf.Lerp (minAlphaValue, maxAlphaValue, timeParameter);
		}
		if (timeParameter > 1.0f) {
			float temp = maxAlphaValue;
			maxAlphaValue = minAlphaValue;
			minAlphaValue = temp;
			timeParameter = 0.0f;

		}


	}
}
