using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseConditionTutorial : Tutorial {


	public override void CheckForSuccessConditions(){
		timer += Time.deltaTime;
		if (timer > minDuration){
			TutorialManager.Instance.CompletedTutorial(); 
		}
	}
}
