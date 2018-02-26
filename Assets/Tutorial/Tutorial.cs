using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public int order;
	
	[TextArea(3,10)]
	public string explanationText; 

	protected Player player; 

	public float minDuration; 
	protected float timer = 0f;
	protected bool waveSent = false; 

	protected virtual void Awake(){
		TutorialManager.Instance.tutorials.Add(this);
		player = GameObject.FindObjectOfType<Player>();
	}

	public virtual void CheckForSuccessConditions(){

	}


}
