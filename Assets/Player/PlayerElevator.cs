using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElevator : MonoBehaviour {


	public float developmentRate;
	private Vector3 spawnPoint;
	private Quaternion spawnRotation;
	private List<GameObject> blueprintAssignments = new List<GameObject> (); 
	private GameObject blueprintBeingWorkedOn; 
	private float workProgress;
	 

	void Awake () {
		spawnPoint = transform.position + Vector3.forward; 
		spawnRotation = transform.rotation; 
	}
	
	void Update () {
		InitiateWorkOnNextBlueprintAssignment ();
		WorkOnBlueprintAssignments (); 
	}

	public void RegisterBlueprintAssignment (GameObject blueprintAssignment){
		blueprintAssignments.Add (blueprintAssignment); 
	}

	private void InitiateWorkOnNextBlueprintAssignment(){
		if (blueprintAssignments.Count > 0 && blueprintBeingWorkedOn == null) {
			blueprintBeingWorkedOn = blueprintAssignments [0];
		}
	}

	private void WorkOnBlueprintAssignments (){
		if (blueprintBeingWorkedOn != null) {
			workProgress += developmentRate * Time.deltaTime; 
			if (workProgress >= 100.0f) {
				workProgress = 0.0f; 
				FinishDevelopment (); 
			}
		}
	}

	private void FinishDevelopment (){
		blueprintBeingWorkedOn.transform.position = spawnPoint;
		blueprintBeingWorkedOn.transform.rotation = spawnRotation;
		WorldObject revivedWO = blueprintBeingWorkedOn.GetComponent<WorldObject> (); 
		blueprintAssignments.RemoveAt (0); 
		blueprintBeingWorkedOn = null; 
		if (revivedWO) {
			revivedWO.ReviveWO (); 
		}
	}


}
