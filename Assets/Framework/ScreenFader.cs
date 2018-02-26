using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour {

	public Texture2D texture;
	public float fadeSpeed = 0.8f; 
	
	private int drawDepth = 1000;
	private float alpha = 1.0f; 
	private int fadeDirection = 1; 




	public float BeginFade (int direction){
		fadeDirection = direction;
		return fadeSpeed; 
	}

	private void OnGUI(){
		alpha += fadeDirection * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha); 
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth; 
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), texture); 
	}

}
