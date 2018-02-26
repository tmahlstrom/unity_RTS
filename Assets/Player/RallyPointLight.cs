using UnityEngine;
using System.Collections;
using System;

public class RallyPointLight : MonoBehaviour {

	public float timeParameter;
	public float minIntensity;
	public float maxIntensity;
	public float intensity; 
	public float speed; 


	// Use this for initialization
	void Start () {
		//StartCoroutine ("Flicker");
		timeParameter = 0;
		intensity = maxIntensity;
	

	}
	
	// Update is called once per frame
	void Update () {
		FadeAround ();
		gameObject.GetComponent<Light> ().intensity = intensity;
	}

//	private IEnumerator Flicker(){
//		while (true){
//			gameObject.GetComponent<Light>().enabled = true;
//			yield return new WaitForSeconds (flickerSpeed);
//			gameObject.GetComponent<Light> ().intensity = lowIntensity;
//
//			enabled = false;
//			yield return new WaitForSeconds (flickerSpeed);
//		}
//	}

	private void FadeAround(){
		if (timeParameter < 1.0f) {
			timeParameter += Time.deltaTime * speed; 
			intensity = Mathf.Lerp (minIntensity, maxIntensity, timeParameter);
		}
		if (timeParameter > 1.0f) {
			float temp = maxIntensity;
			maxIntensity = minIntensity;
			minIntensity = temp;
			timeParameter = 0.0f;
		
		}
	
	
	}
}
