using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatchery : Building {

	public bool active = true;
	private HatcheryActivityLight activityLight; 


	protected override void Awake(){
		base.Awake(); 
		activityLight = GetComponentInChildren<HatcheryActivityLight>();
	}

	protected override void Start(){
		base.Start();
		StageManager.Instance.allSelectables.Add(this);
		buildingState = new IdleStateB(building, false);
		isFinishedBuilding = true; 
		ReawakenWO();
	}

	protected override void OnEnable(){
    }

    protected override void OnDisable(){
    }

	public void ToggleHatcheryActivity(bool toggle){
		if (toggle == false){
			active = false; 
			if (spawner){
				spawner.enabled = false;
				ToggleActivityLight(false);
			}
		} else {
			active = true; 
			if (spawner){
				spawner.enabled = true; 
				ToggleActivityLight(true);
			}
		}
	}


	private void ToggleActivityLight(bool toggle){
        if (activityLight){
            activityLight.LightActivity(toggle); 
        }
    }


}
