using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTutorial2 : Tutorial {

	public override void CheckForSuccessConditions(){
		timer += Time.deltaTime;
		if (player.selectedObjects.Count == 2 && (timer > minDuration)){
			bool onlyBuilders = true; 
			foreach (WorldObject wo in player.selectedObjects){
				Builder builder = wo.GetComponentInChildren<Builder>(); 
				if (builder == null){
					onlyBuilders = false;
				}
			}
			if (onlyBuilders){
				TutorialManager.Instance.CompletedTutorial(); 
			}
		}
	}
}
