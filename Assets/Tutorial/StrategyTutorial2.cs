using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyTutorial2 : Tutorial {

	private bool enemyHasSpawned = false; 

	EnemyUnits enemyUnits; 
	protected override void Awake(){
		base.Awake();
		enemyUnits = GameObject.FindObjectOfType<EnemyUnits>(); 
	}

	public override void CheckForSuccessConditions(){
		if (!waveSent){
			SendWave();
		}
		timer += Time.deltaTime;
		if (enemyUnits){
			Unit[] units = enemyUnits.GetComponentsInChildren<Unit>(); 
			if (units.Length > 0){
				enemyHasSpawned = true;
			}
			if (enemyHasSpawned && units.Length == 0 && timer > minDuration){
				TutorialManager.Instance.CompletedTutorial(); 
			}
		}
	}

	private void SendWave(){
		SceneControl.Instance.TriggerSceneEvents(0);
		waveSent = true; 
	}
}
