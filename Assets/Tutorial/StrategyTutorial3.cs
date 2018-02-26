using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyTutorial3 : Tutorial {



	public override void CheckForSuccessConditions(){
		timer += Time.deltaTime;
		if (timer > minDuration){
			TutorialManager.Instance.CompletedTutorial(); 
		}
	}
}
