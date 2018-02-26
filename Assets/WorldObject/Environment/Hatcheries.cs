using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatcheries : MonoBehaviour {

	Hatchery[] totalHatcheries; 

	private void Awake (){
		totalHatcheries = GetComponentsInChildren<Hatchery>();
	}

	public int GetHatcheryCount(){
		int count = 0; 
		if (totalHatcheries != null && totalHatcheries.Length > 0){
			foreach (Hatchery hatchery in totalHatcheries){
				if (hatchery.gameObject.activeInHierarchy && hatchery.active){
					count += 1; 
				}
			}
			return count; 
		}
		return 999;  
	}

}
