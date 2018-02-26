using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 
using UnityEngine.SceneManagement;


public class TutorialScene : SceneControl {

	Player player; 
	Unit[] units; 
	TutorialManager tutMan; 

	private void Awake(){
		player = FindObjectOfType<Player>();
		units = player.GetComponentsInChildren<Unit>();		
	}







	protected override IEnumerator SceneEvents(){
		yield return new WaitForSeconds(0.1f);
		KillWorkerUnits(); 
		player.cursorManagerScript.DisableCursor(); 
		//ReviveHer();
		yield return new WaitForSeconds(1.7f);
		ScreenFader fader = GameObject.FindObjectOfType<ScreenFader>(); 
		fader.BeginFade (-1);
		TutorialManager.Instance.StartTutorial(); 
		player.cursorManagerScript.EnableCursor();
	}

	private void KillWorkerUnits(){
		units = player.GetComponentsInChildren<Unit>();
		foreach(Unit unit in units){
			if (unit.gameObject.name != "Her"){
				unit.TakeDamage(unit.paramManager.MaxHitPoints, Vector3.zero, null); 
			}
		}
	}

	private void ReviveHer(){
		foreach(Unit unit in units){
			if (unit.name == "Her"){
				unit.worldObject.ReviveWO(); 
			} 
		}
	}


	public override void TriggerSceneEvents(int number){
		if (number == 0){
			StageManager.Instance.SendSingleWave(); 
		}
		if (number == 1){
			StageManager.Instance.SendSingleWave(); 
		}
		if (number == 2){
			StageManager.Instance.ToggleWaveActivity(true); 
		}
		
	}

}
