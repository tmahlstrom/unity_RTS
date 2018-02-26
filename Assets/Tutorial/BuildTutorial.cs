using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTutorial : Tutorial {


	Buildings buildings; 

	protected override void Awake(){
		base.Awake();
		if (player){
			buildings = player.GetComponentInChildren<Buildings>();
		}
	}

	public override void CheckForSuccessConditions(){
		if (buildings){
			Building[] buildingObjects = buildings.GetComponentsInChildren<Building>(); 
			if (buildingObjects.Length > 1){
				TutorialManager.Instance.CompletedTutorial(); 
			}
		}
	}
}
