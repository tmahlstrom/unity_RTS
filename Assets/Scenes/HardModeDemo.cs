using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class HardModeDemo : SceneControl {

	public float countdownDuration; 

	private WaitForSeconds shortWait = new WaitForSeconds(0.1f); 


	protected override void Start(){
		base.Start(); 
		if (countdownDuration > 0){
			StartCoroutine(PerformCountdown()); 
		}
	}
	protected override IEnumerator SceneEvents(){
		ScreenFader fader = GameObject.FindObjectOfType<ScreenFader>(); 
		fader.BeginFade (-1);
		yield return new WaitForSeconds(5); 
		// StageManager.Instance.ToggleWaveActivity(true);
	}

	private IEnumerator PerformCountdown(){
		if (MainCanvas.Instance != null){
			while (countdownDuration > 0){
				MainCanvas.Instance.UpdateCountdownText(countdownDuration); 
				countdownDuration -= Time.deltaTime; 
				yield return null; 
			}
			MainCanvas.Instance.UpdateCountdownText(0f);
			StageManager.Instance.FixedSpawnRate(0.1f); 
			StageManager.Instance.ToggleWaveActivity(true);
		}
	}

	public override void VictoryDataChange(){
		PlayerData playerData = FindObjectOfType<PlayerData>(); 
		if (playerData){
			playerData.beatHardDemo = true; 
		}
	}


}
