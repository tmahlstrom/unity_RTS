using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FloatText : MonoBehaviour {

	public Animator animator;
    private Text text;


	void Awake () {
		animator = GetComponentInChildren<Animator> ();
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo (0);
		Destroy (gameObject, clipInfo [0].clip.length);
		text = animator.GetComponent<Text>();
	}

    public void SetText(int numberToShow, bool isOwnUnit, bool isResource){
		if (text != null) {
			text.text = Math.Abs(numberToShow).ToString();
            if (isResource){
                animator.Play("ResourceText");
                return; 
            }
			if (numberToShow < 0) {
				animator.Play ("FloatHPHealText");
			}
			if (numberToShow > 0 && isOwnUnit == true) {
				animator.Play ("FloatHPDamageAllyText");
			}
			if (numberToShow > 0 && isOwnUnit == false) {
				animator.Play ("FloatHPDamageEnemyText");
			}
		}
	}
	

}
