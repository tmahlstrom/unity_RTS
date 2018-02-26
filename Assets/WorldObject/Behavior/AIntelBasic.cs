using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS; 

public class AIntelBasic : MonoBehaviour {

	[Header ("AI Parameters")]
    public bool attackAndSpawn;
    public bool attackOnly; 

    protected WorldObject worldObject;
    protected Unit unit; 
    protected Attacker attacker;
    protected Spawner spawner; 
    protected ParamManager paramManager; 

    protected WaitForSeconds shortWait = new WaitForSeconds(0.5f);

	protected virtual void Awake(){
        worldObject = GetComponent<WorldObject> ();
        unit = GetComponent<Unit>(); 
        attacker = GetComponent<Attacker>();
        spawner = GetComponent<Spawner>(); 
        paramManager = GetComponent<ParamManager>(); 
	}


	protected virtual void Start () {
        StartCoroutine ("AIMain"); 
		//StartCoroutine ("AttackNearestEnemyInSight");
        //StartCoroutine ("SpawnWhenPossible");
	}
    

    private IEnumerator AIMain(){
        while (this != null){
            if (unit.unitState.ManuallyInitatedState == false && unit.paramManager.IsDead == false) {
                if (attackAndSpawn){
                    if (!SpawnAttempt()){
                        AttackAttempt(); 
                    }
                }
                if (attackOnly){
                    AttackAttempt(); 
                }
            }
            yield return shortWait;
        }
    }



    private bool SpawnAttempt(){
        if (unit.spawner && unit.spawner.isActiveAndEnabled){
            if (unit.spawner.ReadyToBeginSpawning()){
                if (unit.unitState.GetType() != typeof(SpawnState) && !unit.unitState.StateMidAnimation()){
                    unit.SetUnitState(new SpawnState(unit, false));
                    return true;
                }
            }
        }
        return false; 
    }

    private bool AttackAttempt(){
        Collider targetCollider = null;
        if (paramManager.PlayerOwned){
            targetCollider = worldObject.GetNearestEnemyTargetColliderInRange (paramManager.AggroRange);
        } else {
            targetCollider = WorkManager.DetermineNearestPlayerTargetColliderInRange (transform.position, paramManager.AggroRange);
        }
        if (targetCollider) {
            if (!unit.unitState.StateMidAnimation()){
                if (unit.unitState.GetType() != typeof(AttackState)){
                    unit.SetUnitState(new AttackState(unit, false)); 
                }
                unit.unitState.ReactToColliderFocus(targetCollider);
                return true;
            }
        }
        return false; 
    }



}
