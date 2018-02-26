using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using RTS; 


public class TutorialManager : MonoBehaviour {

	public List<Tutorial> tutorials = new List<Tutorial>(); 
	public Text explantionText; 
	public float fadeTime; 
	IEnumerator textFadeCoroutine; 

	private bool textFadeCoroutineStarted = false; 
	private bool flag = false; 

	private static TutorialManager instance; 
	public static TutorialManager Instance {
		get {
			if (instance == null){
				instance = GameObject.FindObjectOfType<TutorialManager>(); 
			} 
			return instance; 
		}
	}
	private Tutorial currentTutorial; 



	public void StartTutorial(){
		SetNextTutorial(0);
	}

	private void Update(){
		if (currentTutorial){
			currentTutorial.CheckForSuccessConditions(); 
		}
	}

	public void SetNextTutorial(int currentOrder){
		currentTutorial = GetTutorialByOrder(currentOrder); 
		if (!currentTutorial){
			CompletedAllTutorials();		
			return; 
		}
		if (textFadeCoroutineStarted){
			this.StopAllCoroutines(); 
		}
		textFadeCoroutine = UpdateExplanationText(); 
		StartCoroutine(UpdateExplanationText()); 
	}

	public void InstantUpdateText(){
		explantionText.text = currentTutorial.explanationText;
	}
	public void CompletedTutorial(){
		SetNextTutorial(currentTutorial.order + 1);
		//MainCanvas.Instance.DefeatNotification(); 
		//StageManager.Instance.VictoryConditionsAreMet(); 
	}

	private void CompletedAllTutorials(){
		Tutorial emptyTut = gameObject.AddComponent<Tutorial>();
		emptyTut.explanationText = " "; 
		currentTutorial = emptyTut;
		StartCoroutine(UpdateExplanationText()); 
	}

	public Tutorial GetTutorialByOrder(int order){
		for (int i = 0; i < tutorials.Count; i++){
			if (tutorials[i].order == order){
				return tutorials[i]; 
			}
		}
		return null; 
	}


	private IEnumerator UpdateExplanationText(){
		textFadeCoroutineStarted = true; 

		IEnumerator disappearCoroutine = FadeTextToZeroAlpha(fadeTime, explantionText);
		StartCoroutine(disappearCoroutine); 
		yield return new WaitForSeconds(fadeTime + 0.3f);//this has to be slightly longer than fadeTime to be sure the coroutines don't start fighting over the alpha

		explantionText.text = currentTutorial.explanationText; 

		IEnumerator appearCoroutine = FadeTextToFullAlpha(fadeTime, explantionText);
		StartCoroutine(appearCoroutine); 
		yield return new WaitForSeconds(fadeTime);
	
		textFadeCoroutineStarted = false; 
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


}
