using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class SpecialTutorial : Tutorial {

	bool laserTriggered; 
	Units units; 

	protected override void Awake(){
		base.Awake();
		if (player){
			units = player.GetComponentInChildren<Units>();
		}
	}
	public override void CheckForSuccessConditions(){
		if (!waveSent){
			SendWave();
		} 
		timer += Time.deltaTime;
		if (!laserTriggered){
			InterceptSpawnLaser laser = units.GetComponentInChildren<InterceptSpawnLaser>();
			if (laser && laser.isShowingLaser){
				laserTriggered = true; 
			}
		}
		if (laserTriggered && timer > minDuration){
			StageManager.Instance.EndRepeatingWave(); 
			TutorialManager.Instance.CompletedTutorial();
		}

	}

	private void SendWave(){
		StageManager.Instance.StartRepeatingWave(); 
		waveSent = true; 
	}
}
