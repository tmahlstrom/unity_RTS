using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour {
	private static SceneControl instance; 
	public static SceneControl Instance {
		get {
			if (instance == null){
				instance = GameObject.FindObjectOfType<SceneControl>(); 
			} 
			if (instance == null){
				Debug.Log("No scene controller found"); 
			}
			return instance; 
		}
	}

	public Transform[] sceneLocations; 


	protected virtual void Start(){
		Invoke("BeginSceneEvents", 0.1f);
	}

	private void BeginSceneEvents(){
		StartCoroutine(SceneEvents()); 
	}

	protected virtual void Update(){

	}
	protected virtual IEnumerator SceneEvents(){
		yield return null; 
	}
	public virtual void TriggerSceneEvents(int number){
		
	}

	public virtual void VictoryDataChange(){
		
	}
}
