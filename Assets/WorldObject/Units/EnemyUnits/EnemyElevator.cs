using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElevator : MonoBehaviour {

	private Animator animator;
	private AnimatorClipInfo[] riseClipInfo;
	private AnimatorClipInfo[] dropClipInfo;
	private bool isElevated; 

	private void Awake () {
		animator = GetComponentInChildren<Animator> ();
    }



    public void RaiseElevator(){
		animator.SetBool ("IsElevated", true); 
	}

	public void LowerElevator(){
		animator.SetBool ("IsElevated", false); 
	}

}
