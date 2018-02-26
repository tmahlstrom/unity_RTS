using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTutorial4 : Tutorial {

    Buildings buildings; 

	protected override void Awake(){
		base.Awake();
		if (player){
			buildings = player.GetComponentInChildren<Buildings>();
		}
	}
	public override void CheckForSuccessConditions(){
		int antMound = 0;
		int vine = 0; 
		if (buildings){
			Building[] buildingObjects = buildings.GetComponentsInChildren<Building>(); 
			if (buildingObjects.Length > 1){
				foreach (Building building in buildingObjects){
					if (building.GetComponent<AntMound>() && building.IsFinishedBuilding){
						antMound++; 
					}
					if (building.GetComponent<Vine>() && building.IsFinishedBuilding){
						vine++; 
					}
				}
			}
			if (antMound > 2 && vine > 2){
				TutorialManager.Instance.CompletedTutorial(); 
			}
		}
	}

}
