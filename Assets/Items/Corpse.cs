using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse	: Item {

	public float maxCollectionProgress = 100.0f;
	public float collectionProgress = 0.0f;
	private bool isFinishedBeingCollected;
	public GameObject deadGameObject; 



	public void MarkThisCorpse (GameObject deadGameObject){
		this.deadGameObject = deadGameObject; 
	}

	public bool IsFinishedBeingCollected() {
		return isFinishedBeingCollected;
	}

	public float GetCollectionPercentage() {
		return collectionProgress / maxCollectionProgress;
	}

	public void BeCollected(int amount) {
		collectionProgress += amount;
		if(collectionProgress >= maxCollectionProgress) {
			CompleteMyCollection ();
		}
	}

	protected virtual void CompleteMyCollection(){
		isFinishedBeingCollected = true;
	}



}
