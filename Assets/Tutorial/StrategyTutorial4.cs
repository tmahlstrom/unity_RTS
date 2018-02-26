using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyTutorial4 : Tutorial {

	private bool enemyHasSpawned = false; 
	EnemyUnits enemyUnits; 

	protected override void Awake(){
		base.Awake();
		enemyUnits = GameObject.FindObjectOfType<EnemyUnits>(); 
	}

	public override void CheckForSuccessConditions(){
		timer += Time.deltaTime;
		if (!waveSent){
			SendNextWave();
		}
		if (enemyUnits){
			Unit[] units = enemyUnits.GetComponentsInChildren<Unit>(); 
			if (units.Length > 0){
				enemyHasSpawned = true;
			}
			if (enemyHasSpawned && units.Length == 0){
				TutorialManager.Instance.CompletedTutorial(); 
			}
		}
	}


	private void SendNextWave(){
		SceneControl.Instance.TriggerSceneEvents(0);
		waveSent = true; 
	}
}
