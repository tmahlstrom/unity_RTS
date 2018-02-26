using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalModeDemo : SceneControl {

	protected override IEnumerator SceneEvents(){
		ScreenFader fader = GameObject.FindObjectOfType<ScreenFader>(); 
		fader.BeginFade (-1);
		yield return new WaitForSeconds(5); 
		StageManager.Instance.ToggleWaveActivity(true);
		//StageManager.Instance.VictoryConditionsAreMet(); 
	}

	public override void VictoryDataChange(){
		PlayerData playerData = FindObjectOfType<PlayerData>(); 
		if (playerData){
			playerData.beatNormalDemo = true; 
		}
	}
}
