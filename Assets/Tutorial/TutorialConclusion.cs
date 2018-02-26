using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialConclusion : Tutorial {

	private bool enemyHasSpawned = false; 

	public override void CheckForSuccessConditions(){

		if (!waveSent){
			SendWave();
		}

		timer += Time.deltaTime;
		if (timer > minDuration){
			TutorialManager.Instance.CompletedTutorial(); 
		}
	}

	private void SendWave(){
		StageManager.Instance.ToggleWaveActivity(true); 
		waveSent = true; 
	}
}
