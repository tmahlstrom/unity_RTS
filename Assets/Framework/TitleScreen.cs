using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using UnityEngine.Video; 

public class TitleScreen : MonoBehaviour {

	PlayerData playerData; 

	private void Awake(){
		playerData = FindObjectOfType<PlayerData>();
	}

	private void Start(){
		StartCoroutine(TitleScreenControl());
		DetermineVideo(); 
		ConsiderEnablingHardModeButon(); 
	}


	private IEnumerator TitleScreenControl(){
		yield return new WaitForSeconds(0.5f); 
		MainCanvas.Instance.FadeIn(.2f);
	}
	public void ChangeScene(string levelName){
		SceneManager.LoadScene(levelName); 
	}
	
	private void ConsiderEnablingHardModeButon(){
		if (playerData){
			if (playerData.beatNormalDemo){
				GameObject buttonGO = GameObject.Find("Hard Mode Button"); 
				if (buttonGO){
					Button button = buttonGO.GetComponent<Button>(); 
					if (button){
						button.interactable = true; 
					}
				}
			}
		}
	}

	private void DetermineVideo(){
		GameObject canvas = GameObject.Find("TitleScreenCanvas");
		if (canvas && playerData){
			VideoPlayer[] videos = canvas.GetComponentsInChildren<VideoPlayer>(); 
			if (videos.Length > 1){
				if (!playerData.beatHardDemo){
					videos[0].enabled = true;
					videos [1].enabled = false; 
				} else {
					videos[1].enabled = true;
					videos [0].enabled = false; 
				}
			}
		}
	}
}
