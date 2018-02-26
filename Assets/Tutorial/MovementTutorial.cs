using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class MovementTutorial : Tutorial {

	private int counter; 
	private string baseText; 

	private void Start(){
		baseText = explanationText; 
		explanationText = baseText + "\n" + counter.ToString() + "/3";
	}

	public override void CheckForSuccessConditions(){		
		timer += Time.deltaTime;
		if (timer > minDuration && UnitSelected()){
			if (Input.GetMouseButtonDown (1)) {
				GameObject hitObject = WorkManager.FindHitObject(Input.mousePosition);
				if (hitObject && hitObject.name == "Ground"){
					counter += 1; 
					explanationText = baseText + "\n" + counter.ToString() + "/3"; 
					TutorialManager.Instance.InstantUpdateText();
				}
			}
		}
		if (counter >= 3){
			TutorialManager.Instance.CompletedTutorial(); 
		}
	}

	private bool UnitSelected(){
		if (player.selectedObjects.Count > 0){
			Unit unit = player.selectedObjects[0].GetComponent<Unit>();
			if (unit){
				return true; 
			}
		}
		return false; 
	}


}
