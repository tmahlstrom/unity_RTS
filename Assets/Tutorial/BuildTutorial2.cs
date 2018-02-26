using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTutorial2 : Tutorial {


	Buildings buildings; 

	protected override void Awake(){
		base.Awake();
		if (player){
			buildings = player.GetComponentInChildren<Buildings>();
		}
	}
	public override void CheckForSuccessConditions(){
		timer += Time.deltaTime; 
		if (buildings){
			Building[] buildingObjects = buildings.GetComponentsInChildren<Building>(); 
			if (buildingObjects.Length > 0 && buildingObjects[0].IsFinishedBuilding){
				if (timer > minDuration){
					TutorialManager.Instance.CompletedTutorial(); 
				}
			}
		}
	}
}
