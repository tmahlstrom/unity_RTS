using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS;

public class Spawn : WorldObject {

    public SpawnBaseState spawnState; 
	protected bool hasForcedTarget; 
    public float moveSpeed; 

	public Collider targetCollider; 

	protected Spawn spawn; 
	public Spawner mySpawner; 
	protected WorldObject mySpawnerWorldObject;

	protected Vector3 patrolOriginPoint;
	protected Vector3 patrolPoint; 

    protected Vector3 movePoint;

    protected WaitForSeconds shortWait = new WaitForSeconds(0.1f);

	public Vector3 pushVector; 


	protected override void Awake (){
		base.Awake ();
        paramManager = GetComponent<ParamManager>(); 
		spawn = this;
        spawnState = new PatrolStateS(spawn, false); 
	}



	protected override void Update(){
		base.Update();
		spawnState.UpdateState(); 
	}

		

	public void SetSpawnState (SpawnBaseState newState){
		if (spawnState != null){
        	spawnState.ExitRoutine(spawnState);
		}
		spawnState = newState;
	}


	public virtual void NotifySpawnerOfDeath(){
		if (mySpawner) {
			mySpawner.SubtractDeadSpawnFromPopulationCount ();
			worldObject.OnWorldObjectDeathDelegate -= NotifySpawnerOfDeath;
		}
	}

	public void RememberWhoMadeMe(WorldObject maker){
		if (maker) {
			this.mySpawnerWorldObject = maker;
			this.mySpawner = maker.GetComponent<Spawner> ();
			if (mySpawner) {
				mySpawner.AddNewSpawnToPopulationCount ();
				patrolOriginPoint = mySpawner.transform.position; 
				worldObject.OnWorldObjectDeathDelegate += NotifySpawnerOfDeath;
				worldObject.OnWorldObjectDeathDelegate += DestroyWorldObject; 
			}
		}
	}

	public virtual void ReactToSpawnerDeath(){
		if(enemyManager){
			Invoke("TimedDeath", 7); 
		}
	}


	//*******************************************
	//BEGIN GENERAL MOVEMENT METHODS
	//*******************************************

	public virtual void MoveForward(float speed){
		spawn.transform.position = spawn.transform.position + spawn.transform.forward * Time.deltaTime * speed;
	}

	public virtual bool AugmentRotationNeeded(Vector3 moveTarget){
		Vector3 relativePosition = moveTarget - transform.position; 
        Quaternion neededRotation = Quaternion.LookRotation(relativePosition); 
		if (neededRotation == Quaternion.identity){
			return false; 
		}
		return true;
	}

	public virtual void AugmentRotationSpeed (Vector3 moveTarget) {
		float rotationPercentage = 1.0f - Vector3.Angle(transform.forward, moveTarget-transform.position)/180.0f;
		if (rotationPercentage < 0.99f) {
			Vector3 targetDir = moveTarget - transform.position;
			float step = paramManager.RotationSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
			transform.rotation = Quaternion.LookRotation (newDir);
		}

	}

	public virtual void PositionOnSurface(){

	}


	public virtual void MoveTowardsPointAtSpeed (Vector3 toMoveHere, float speed){
		Debug.Log(transform.position + "   " + toMoveHere);
		Vector3 moveVector = (transform.position - toMoveHere).normalized; 
		Debug.DrawLine(transform.position, toMoveHere, Color.blue, 1);
		transform.position = transform.position - moveVector * Time.deltaTime * speed;
	}



	//*******************************************
	//END GENERAL MOVEMENT METHODS
	//*******************************************



	//*******************************************
	//BEGIN PATROL METHODS
	//*******************************************
 
    public virtual void LookForTarget(){
		if (mySpawnerWorldObject && mySpawnerWorldObject.paramManager.PlayerOwned || player) {
            targetCollider = WorkManager.DetermineNearestEnemyTargetColliderInRange(transform.position, paramManager.AggroRange);
		} else if (mySpawnerWorldObject && !mySpawnerWorldObject.paramManager.PlayerOwned){
            targetCollider = WorkManager.DetermineNearestPlayerTargetColliderInRange(transform.position, paramManager.AggroRange);
        }
	}

	public virtual Vector3 FindPatrolPoint(){ 
		return Vector3.zero; 
	}
       

	public virtual bool CheckForArrival(Vector3 targetMove){
		if (Vector3.Distance (transform.position, targetMove) < 0.4f) {
			return true; 
		} else 
			return false; 
	}

	public void RegisterTargetAssignment (WorldObject targetWO){
		Target target = targetWO.GetComponentInChildren<Target> ();
		if (target) {
			Collider targetCol = target.gameObject.GetComponent<Collider> ();
			targetCollider = targetCol; 
		}
		hasForcedTarget = true; 
	}




	//*******************************************
	//END PATROL METHODS
	//*******************************************

	//*******************************************
	//BEGIN APPROACH METHODS
	//*******************************************

	public virtual bool InRangeForAtack(){
		return false; 
	}

	public virtual Vector3 FindOffCenterApproachPoint (Vector3 targetPosition){
		return targetPosition; 
	}

	//*******************************************
	//END APPROACH METHODS
	//*******************************************

















	//*******************************************
	//BEGIN ATTACK METHODS
	//*******************************************

	public virtual void AttackMotion(){

	}
    public virtual bool AbleToFindNewTarget(){
        if (paramManager.PlayerOwned){
            targetCollider = worldObject.GetNearestEnemyTargetColliderInRange (paramManager.AggroRange);
        } else {
            targetCollider = WorkManager.DetermineNearestPlayerTargetColliderInRange (transform.position,5);
        }           
        if (targetCollider && targetCollider.gameObject.activeInHierarchy == false) {
            targetCollider = null; 
        }
		if (targetCollider != null){
			return true; 
		} else return false; 
    }

	protected virtual void OnTriggerEnter(Collider encounteredCollider){
		// if (encounteredCollider.gameObject.tag == "Barrier"){
		// 	Debug.Log("bounce!!");
		// 	MoveTowardsBarrierCenter(encounteredCollider.transform.position); 
		// 	return; 
		// }
        Target tar = encounteredCollider.gameObject.GetComponent<Target> ();
        if (tar){
    		WorldObject colliderWO = encounteredCollider.gameObject.GetComponentInParent<WorldObject> ();
    		if (colliderWO && WorkManager.AreWorldObjectsOnSameTeam(worldObject, colliderWO) == false) {
    			ImpactEffect ();
                InflictDamage (colliderWO, paramManager.AttackDamage); 
    			Spawn colliderSpawn = colliderWO.GetComponent<Spawn> ();
    			if (colliderSpawn == null) {
    				InflictDamage (this.worldObject, this.paramManager.MaxHitPoints);  
    			}
    		}
        }
	}

	protected virtual void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Barrier"){
			spawnState = new PushedStateS(spawn, false);
			ShieldEffect se = other.gameObject.GetComponent<ShieldEffect>();
			if (se){
				se.ContactEffect(transform.position); 
			}
			return; 
		}
	}

	protected virtual void InflictDamage(WorldObject doDamageToThis, int damageAmount) {
        doDamageToThis.TakeDamage (damageAmount, transform.position, worldObject);
	}
	
	private void DestroyWorldObject(){
		Destroy (gameObject); 
	}

	private void TimedDeath(){
		RemovalEffect(); 
		InflictDamage (this.worldObject, this.paramManager.MaxHitPoints);
	}

	//*******************************************
	//END ATTACK METHODS
	//*******************************************

	//*******************************************
    //BEGIN PUSH METHODS
    //*******************************************  	
	public void SavePushDirection(Vector3 push){
		pushVector = push; 
	}

	public Vector3 UpdatePushDirection(Transform trans){
		Vector3 accPush = pushVector + trans.forward * 2;
				Debug.DrawLine(trans.position, accPush, Color.yellow, .1f); 
				Debug.DrawLine(trans.position, trans.position + trans.forward * 5, Color.yellow, .1f); 


		return accPush; 
	}

	public Vector3 GetPushDirection(){
		return pushVector; 
	}

	//*******************************************
    //BEGIN PUSH METHODS
    //*******************************************  


    //*******************************************
    //BEGIN EFFECT METHODS
    //*******************************************  


	protected virtual void ImpactEffect() {
		
	}

	protected virtual void RemovalEffect() {

	}

	public virtual void ResolveBeingHitByIntercept (){

	}

	protected override void GiveThisWorldInfoCanvasStats (){
		//actually, don't...for now
	}
	
	//*******************************************
    //END EFFECT METHODS
    //*******************************************  

}
