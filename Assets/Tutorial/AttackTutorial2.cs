using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class AttackTutorial2 : Tutorial {

	private bool enemyHasSpawned = false; 
	private string baseText; 

	private EnemyUnits enemyUnits; 
	private int counter; 

	protected override void Awake(){
		base.Awake();
		enemyUnits = GameObject.FindObjectOfType<EnemyUnits>(); 
	}
	private void Start(){
		baseText = explanationText; 
		explanationText = baseText + "\n" + counter.ToString() + "/2";
	}
	public override void CheckForSuccessConditions(){
		if (!waveSent){
			SendWave();
		}
		CheckIfEnemiesSpawned();
		if (PlayerUnitSelected()){
			if (RightClickedOnEnemy()){
				counter += 1;
				explanationText = baseText + "\n" + counter.ToString() + "/2"; 
				TutorialManager.Instance.InstantUpdateText();
			}
		}

		if (enemyHasSpawned && counter > 1){
			StopWave(); 
			TutorialManager.Instance.CompletedTutorial(); 
		}
	}

	private void SendWave(){
		StageManager.Instance.StartRepeatingWave(); 
		waveSent = true; 
	}

	private void StopWave(){
		StageManager.Instance.EndRepeatingWave(); 
	}
	
	private bool CheckIfEnemiesSpawned(){
		if (enemyHasSpawned == false){
			Unit[] units = enemyUnits.GetComponentsInChildren<Unit>(); 
			if (units.Length > 0){
				enemyHasSpawned = true;
				return true; 
			}
		}
		return false; 
	}


	private bool PlayerUnitSelected(){
		if (player.selectedObjects.Count > 0){
			Player player2 = player.selectedObjects[0].player;
			if (player2 == player){
				return true; 
			}
		}
		return false;
	}

	private bool RightClickedOnEnemy(){
		if (Input.GetMouseButtonDown (1)) {
			GameObject hitObject = WorkManager.FindHitObject(Input.mousePosition);
			if (hitObject){
				WorldObject wo = hitObject.GetComponentInParent<WorldObject>(); 
				if (wo && wo.name == "Wolf(Clone)"){
					return true; 
				}
			}
		}
		return false; 
	}

}
