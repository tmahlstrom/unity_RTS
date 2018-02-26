using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo2Conductor : SceneControl {

	Player player; 
	Unit[] units; 
	private WaitForSeconds fiveSecondWait = new WaitForSeconds(5); 

	private void Awake(){
		player = FindObjectOfType<Player>();
	}

	protected override IEnumerator SceneEvents(){
		Unit her = GetReferenceToHer(); 
		if (her){
			her.SetUnitState(new SitState(her, true));
		}
		yield return new WaitForSeconds(2); 
		ScreenFader fader = GameObject.FindObjectOfType<ScreenFader>(); 
		CameraControl.Instance.UpdateCameraTargetPosition(); 
		fader.BeginFade (-1);
		yield return new WaitForSeconds(2);
		if (her){
			her.SetUnitState(new IdleState(her, true));
		}
		yield return new WaitForSeconds(.75f);
		if (her){
			her.mover.AddMoveTarget(null, sceneLocations[0].transform.position);
			her.SetUnitState(new MoveState(her, true));
		}
		CameraControl.Instance.UpdateCameraTargetPosition();
		CameraControl.Instance.UpdateCameraTargetPosition(99f, 1);

		//StageManager.Instance.ToggleWaveActivity(true);
		//StageManager.Instance.VictoryConditionsAreMet(); 

		StartCoroutine(WaitForHatcheryDeactivation()); 
	}

	private IEnumerator WaitForHatcheryDeactivation(){
		Hatcheries hatcheries = GameObject.FindObjectOfType<Hatcheries>();
		int count = 99; 
		if (hatcheries){
			while (count > 0){
				count = hatcheries.GetHatcheryCount();
				yield return fiveSecondWait; 
			}
		}
		if (count == 0){
			StageManager.Instance.ToggleWaveActivity(true);
		}
	}


	private Unit GetReferenceToHer(){
		Unit her = null; 
		if (player){
			units = player.GetComponentsInChildren<Unit>(); 
			if (units.Length > 0){
				her = units[0]; 
			}
		}
		return her; 
	}

	private void UpdateCameraPosition(){
		CameraControl.Instance.UpdateCameraTargetPosition(); 
	}
}
