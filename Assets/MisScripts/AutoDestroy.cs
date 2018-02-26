using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

    public float duration = 3.0f;

	// Use this for initialization
	void Start () {

		Destroy (gameObject, duration);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
