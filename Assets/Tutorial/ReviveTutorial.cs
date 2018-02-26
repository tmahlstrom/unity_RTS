using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class ReviveTutorial : Tutorial {

	public override void CheckForSuccessConditions(){
		if (StageManager.Instance.allSelectables.Count > 2){
			TutorialManager.Instance.CompletedTutorial(); 
		}
	}
}
