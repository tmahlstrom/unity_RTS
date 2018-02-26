using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTutorial : Tutorial {
 
	private bool enemyHasSpawned = false; 

	private EnemyUnits enemyUnits; 

	protected override void Awake(){
		base.Awake();
		enemyUnits = GameObject.FindObjectOfType<EnemyUnits>(); 
	}
	public override void CheckForSuccessConditions(){
		if (!waveSent){
			SendWave();
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

	private void SendWave(){
		StageManager.Instance.SendSingleWave(); 
		waveSent = true; 
	}

}
