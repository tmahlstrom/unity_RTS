using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveTutorial2 : Tutorial {

	Units units; 

	protected override void Awake(){
		base.Awake();
		if (player){
			units = player.GetComponentInChildren<Units>();
		}
	}
	public override void CheckForSuccessConditions(){
		int counter = 0; 
		if (player && units){
			Unit[] toCount = units.GetComponentsInChildren<Unit>();
			foreach (Unit unit in toCount){
				if (unit.paramManager.IsDead == false){
					counter++; 
				}
			}
			if (counter > 2){
				TutorialManager.Instance.CompletedTutorial(); 
			}
		}
	}
}
