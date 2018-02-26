using System.Collections;
using UnityEngine;
using RTS;

public class BuildingBaseState {

	protected Building building;
	protected bool manuallyInitatedState = false;
    public bool ManuallyInitatedState { get {return manuallyInitatedState; }}

    public BuildingBaseState(Building building, bool manualInit){
		this.building = building;
        this.manuallyInitatedState = manualInit;
	}


    public virtual void UpdateState(){

    }


	public virtual void SelfExitState(RTS.EAnimation state){
        manuallyInitatedState = false;      
	}

    public virtual void ExitRoutine(BuildingBaseState state){

    }


}
