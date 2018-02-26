using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTutorial : Tutorial {

	
	public override void CheckForSuccessConditions(){
		timer += Time.deltaTime; 

		if (timer > minDuration){
			if (player.selectedObjects.Count > 0){
				Unit unit = player.selectedObjects[0].GetComponent<Unit>(); 
				if (unit){
					TutorialManager.Instance.CompletedTutorial(); 
				}
			}
		}
	}
}
