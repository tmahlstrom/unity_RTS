using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.PostProcessing; 

public class MainCanvas : MonoBehaviour {



	private static MainCanvas instance; 
	public static MainCanvas Instance {
		get {
			if (instance == null){
				instance = GameObject.FindObjectOfType<MainCanvas>(); 
			} 
			if (instance == null){
				Debug.Log("error: no main canvas"); 
			}
			return instance; 
		}
	}

	public Text notificationText;
	public Text waveText; 
	public Text countDownText;
	public bool extraStageUnlocked = false; 


	private ScreenFader screenFader; 

	private void Awake(){
		screenFader = GetComponent<ScreenFader>();
	}

	private void OnEnable(){
        StageManager.AWaveHasBeenCompleted += UpdateWaveText;
    }

	private void Start(){
		UpdateWaveText(); 
	}







    public void UpdateWaveText(){
		if (StageManager.Instance){
			if (waveText) {
				if (StageManager.Instance.currentWave > StageManager.Instance.wavesToComplete){
					waveText.text = "Wave: COMPLETE"; 
					return; 
				}
				waveText.text = "Wave: " + (StageManager.Instance.currentWave).ToString() + " of " + StageManager.Instance.wavesToComplete.ToString();
			}
		}
    }

	public void UpdateCountdownText(float time){
		if (countDownText){
			countDownText.text = "Spawning begins in: " + Mathf.Floor(time).ToString(); 
			if (time <= 0f){
				countDownText.text = ""; 
			}
		}
    }



	public void DefeatNotification(){
		string defeat = "Defeat"; 
		notificationText.text = defeat;
		IEnumerator coroutine = FadeTextToFullAlpha(3f, notificationText);
		StartCoroutine(coroutine); 
	}

	public void VictoryNotification(){
		string victory = "Victory"; 
		Color color = Color.yellow;
		color.a = 0; 
		notificationText.color = color;
		notificationText.text = victory;
		IEnumerator coroutine = FadeTextToFullAlpha(3f, notificationText);
		StartCoroutine(coroutine); 
	}

	public void FadeOut(float speed){
		screenFader.fadeSpeed = speed; 
		screenFader.BeginFade(1); 
	}

	public void FadeIn(float speed){
		screenFader.fadeSpeed = speed; 
		screenFader.BeginFade(-1); 
	}


	private IEnumerator FadeTextToFullAlpha(float duration, Text t){
        t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a);
        while (t.color.a < 1.0f)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a + (Time.deltaTime / duration));
            yield return null;
        }
    }
 
    private IEnumerator FadeTextToZeroAlpha(float duration, Text t){
        t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a);
        while (t.color.a > 0.0f)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - (Time.deltaTime / duration));
            yield return null;
        }
    }

	public void ReturnToTitle(){
		SceneManager.LoadScene("TitleScreen");
	}

	public void Quit(){
		Application.Quit(); 
	}

	public void TogglePPEffects(bool toggle){
		GameObject cam = GameObject.Find("Main Camera");
		if (cam){
			// PostProcessingBehaviour pp = cam.GetComponent<PostProcessingBehaviour>(); 
			// if (pp){
			// 	pp.enabled = toggle; 
			// }
		}
	}

}
