using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBaseState {

	protected Spawn spawn;
    protected bool manuallyInitatedState = false;
    public bool ManuallyInitatedState { get {return manuallyInitatedState; }}

    protected WaitForSeconds veryShortWait =  new WaitForSeconds(0.05f);
    protected WaitForSeconds shortWait = new WaitForSeconds(0.1f);
    protected WaitForSeconds mediumWait = new WaitForSeconds(1f);

    public SpawnBaseState(Spawn spawn, bool manualInit){
		this.spawn = spawn;
        this.manuallyInitatedState = manualInit;
	}

    public virtual void UpdateState(){
        
    }

	public virtual void ExitRoutine(SpawnBaseState state){

    }

}
