using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour {
    

    private void Awake(){
        AudioSource[] audioSources = GetComponents<AudioSource>();
        AudioSource audioSource = audioSources[Random.Range(0, audioSources.Length)];
        audioSource.enabled = true;            
    }

    void Update () {
		
	}
}
