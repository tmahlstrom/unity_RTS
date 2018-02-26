using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerateStateB : BuildingBaseState {

	private Coroutine regenerateOverTime; 
	private WaitForSeconds regenRate = new WaitForSeconds(.07f); 

	public RegenerateStateB (Building building, bool manualInit) : base (building, manualInit) { 
		regenerateOverTime = building.StartCoroutine(RegenerateOverTime()); 
	}

	public override void UpdateState(){
		if (building.paramManager.HitPoints == building.paramManager.MaxHitPoints){
			building.SetBuildingState(new IdleStateB(this.building, false));
		}
	}

	private IEnumerator RegenerateOverTime(){
		int step = building.paramManager.MaxHitPoints / 1000;
		if (step<1){
			step = 1; 
		}
		while (building.paramManager.HitPoints < building.paramManager.MaxHitPoints) {
			building.worldObject.TakeDamage (-step, building.transform.position, building.worldObject); 
			yield return regenRate;
		}
	}

}
