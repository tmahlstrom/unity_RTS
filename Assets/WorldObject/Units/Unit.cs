using UnityEngine;
using System.Collections;
using RTS;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.AI;

public class Unit : WorldObject {

    public UnitBaseState unitState; 
	public Unit unit; 
	public Mover mover; 
	public Builder builder;
    public NavMeshAgent navAgent;
    public Attacker attacker;
    public Reviver reviver;
    public Spawner spawner;
	public HatcheryInteracter hatcheryInteracter; 
    protected List<string> listOfBuildables = new List<string>();

    protected WaitForSeconds shortWait = new WaitForSeconds(1.2f);


    protected override void Awake() {
		base.Awake();
		unit = this; 
		mover = GetComponent<Mover> (); 
		builder = GetComponent<Builder> (); 
		worldObject.OnWorldObjectDeathDelegate += LetStageManagerKnowOfUnitDeath;
        navAgent = GetComponent<NavMeshAgent>();
        attacker = GetComponent<Attacker>();
        reviver = GetComponent<Reviver>(); 
        spawner = GetComponent<Spawner>(); 
		hatcheryInteracter = GetComponent<HatcheryInteracter>(); 

        unitState = new IdleState(unit, false); 
        if(enemyManager && navAgent){
            RandomizeNavAgentPriority();
        }
	}

	protected override void Start () {
		base.Start(); 
		if (StageManager.Instance){
			StageManager.Instance.allSelectables.Add(this);
			GiveThisWorldInfoCanvasStats ();
			StageManager.Instance.DealWithUnitCountChange (this, 1);
		}
	}

	protected override void Update () {
		base.Update ();
        if (unitState != null){
            unitState.UpdateState();
        }
	}

    public void SetUnitState (UnitBaseState newState){
        unitState.ExitRoutine();
		unitState = newState;
	}
	
	public void SetColliderFocus (Collider colFocus){
		unitState.ReactToColliderFocus(colFocus);
	}
		
    protected virtual void RandomizeNavAgentPriority (){
        int priority = UnityEngine.Random.Range(50, 100);
        navAgent.avoidancePriority = priority; 
    }


	private void LetStageManagerKnowOfUnitDeath(){
		StageManager.Instance.DealWithUnitCountChange (this, -1);
	}

		
	public override void MouseClickRight(GameObject hitObject, Vector3 hitPoint) {
        unitState.MouseClickRight(hitObject, hitPoint); 
    }


    public override void MouseClickLeft (Vector3 hitPoint){
        unitState.MouseClickLeft(hitPoint); 
    }


	protected override void SpacebarPressed(){
		if (player.selectedObjects.Count == 1){
        	unitState.SpaceBar();
		}
	}



	public void ChangeMana(int manaChange) {
        paramManager.MPmod(manaChange); 
		if (player) {
			player.UpdateStatsForThisWorldObject (worldObject);
		}

	}

	protected override void ManageDeathOfWorldObject(){
		base.ManageDeathOfWorldObject (); 
		if (player) {
			player.RemoveFromControlGroups (this);
            DisableTargets();
            Corpse corpse = gameObject.AddComponent<Corpse>() as Corpse;  
		}  
		SetUnitState(new DeadState(this, false));
	}

    protected override void RewardKiller(Player playerWhoKilledMe) {
        playerWhoKilledMe.ObtainOrganics(paramManager.RewardForKill);
        ShowResourceReward(paramManager.RewardForKill);
    }

	protected override void ManageReviveOfWorldObject(){
		base.ManageReviveOfWorldObject();
		StageManager.Instance.DealWithUnitCountChange (this, 1);
        TakeDamage (-paramManager.MaxHitPoints, transform.position, worldObject);
		//ChangeMana (-paramManager.MaxManaPoints);
		if (player) {
			player.AddMeToOpenControlGroup (this); 
		}
        Corpse corpse = gameObject.GetComponent<Corpse>();
        if (corpse) {
            Destroy(corpse);
        }
        EnableTargets();
		SetUnitState(new IdleState(this, false));
    }




	//*******************************
	//BEGIN CURSOR METHODS
	//*******************************
	// public override void SetHoverState(GameObject hoverObject) {
	// 	base.SetHoverState(hoverObject);
	// }
	//*******************************
	//END CURSOR METHODS
	//*******************************

	public override void AnimationClimaxPoint (RTS.EAnimation action){
        if (unitState != null){
            unitState.AnimationClimaxEvent(action);
        }
	}

	public override void AnimationEndPoint (RTS.EAnimation action){
        if (unitState != null){
            unitState.AnimationCompletionEvent(action);
        }
	}



}
