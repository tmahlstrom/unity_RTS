using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS; 

public class AntSpawn : Spawn {

	GameObject effect;

    public override void PositionOnSurface(){
        RaycastHit hit; 
        Vector3 raySource = transform.position; 
        raySource.y += 1; 
        Ray ray = new Ray(raySource, Vector3.down);
        int layer1 = 8; 
        int layer2 = 13;
        int combinedLayerMask = (1 << layer1) | (1 << layer2); 
        if (Physics.Raycast(ray, out hit, 10, combinedLayerMask)){
            Vector3 position = transform.position; 
            position.y = hit.point.y; 
            transform.position = position;
        }
    }

    public override void AugmentRotationSpeed (Vector3 moveTarget) {
        Vector3 relativePosition = moveTarget - transform.position; 
        Quaternion neededRotation = Quaternion.LookRotation(relativePosition); 
        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, paramManager.RotationSpeed * Time.deltaTime);
	}
	public override bool CheckForArrival(Vector3 targetMove){
        targetMove.y = transform.position.y; 
		if (Vector3.Distance (transform.position, targetMove) < 0.4f) {
			return true; 
		} else 
			return false; 
	}

	public override void ReactToSpawnerDeath(){

	}


	//*******************************************
	//BEGIN PATROL METHODS
	//*******************************************




	public override Vector3 FindPatrolPoint(){ 
		if (mySpawnerWorldObject != null) { 
			patrolOriginPoint = mySpawnerWorldObject.transform.position; 
		} else {
			patrolOriginPoint = transform.position; 
		}
        Vector3 randomDirection = Random.insideUnitSphere * paramManager.PatrolRadius;
		randomDirection.y = 0f; 
		Vector3 almostNextPatrolPoint = randomDirection + patrolOriginPoint;
		if (Vector3.Distance(transform.position, almostNextPatrolPoint) > 0.5){
			return almostNextPatrolPoint;
		} else {
			return FindPatrolPoint(); 
		}
	}


	//*******************************************
	//END PATROL METHODS
	//*******************************************

    //*******************************************
    //BEGIN APPROACH METHODS
    //*******************************************


    public override bool InRangeForAtack(){
		if (targetCollider){
			Vector3 flatTarget = targetCollider.transform.position;
			flatTarget.y = transform.position.y; 
			if (Vector3.Distance (transform.position, flatTarget) < paramManager.AttackRange){
				return true;
			}
		}
		return false; 
	}
    
    public override Vector3 FindOffCenterApproachPoint (Vector3 targetPosition){
		targetPosition.y = transform.position.y;
		Vector3 connectingVector = transform.position - targetPosition;
		Vector3 connectingVector2 = (transform.position + new Vector3(0,1,0)) - targetPosition;
		Vector3 normalVector = (Vector3.Cross (connectingVector, connectingVector2)).normalized;
		Vector3 distanceSensitiveNormalVector = normalVector * connectingVector.magnitude; 
		int navDirectionModifier = Random.Range (0, 2) * 2 - 1;
        float offTargetMovement = Random.Range (paramManager.DegreeOfRandomMovement, -paramManager.DegreeOfRandomMovement); 
        Vector3 desiredApproachPoint = targetPosition + distanceSensitiveNormalVector * navDirectionModifier * offTargetMovement;
		return desiredApproachPoint;  
	}


    //*******************************************
    //END APPROACH  METHODS
    //*******************************************

    //*******************************************
    //BEGIN ATTACK METHODS
    //*******************************************


    public override void AttackMotion(){
        if (targetCollider && transform.position.y < targetCollider.transform.position.y){
            Vector3 jump = Vector3.up * Time.deltaTime * paramManager.MoveSpeed;
            transform.position = transform.position + jump; 
        }
    }
    public override bool AbleToFindNewTarget(){
        base.AbleToFindNewTarget();
        return false;
    }

    protected override void InflictDamage(WorldObject doDamageToThis, int damageAmount) {
		base.InflictDamage (doDamageToThis, damageAmount); 
	}

    //*******************************************
    //END ATTACK  METHODS
    //*******************************************

    //*******************************************
    //BEGIN EFFECT METHODS
    //*******************************************   

	protected override void ImpactEffect() {
		base.ImpactEffect ();
        effect = Instantiate(paramManager.AttackEffectSelf, transform.position, transform.rotation);
		effect.transform.SetParent (ResourceManager.GetDynamicObjects ()); 
	}

	protected override void RemovalEffect() {
		base.RemovalEffect ();
		effect = Instantiate(ResourceManager.GetSpawnEffect("FlyRemoval"), transform.position, transform.rotation);
		effect.transform.SetParent (ResourceManager.GetDynamicObjects ()); 
	}

	public override void ResolveBeingHitByIntercept (){
		base.ResolveBeingHitByIntercept (); 
		effect = Instantiate(ResourceManager.GetSpawnEffect("FlyExplosion"), transform.position, transform.rotation);
		effect.transform.SetParent (ResourceManager.GetDynamicObjects ());
		mySpawner.SubtractDeadSpawnFromPopulationCount (); 
		Destroy (gameObject);
	}

    //*******************************************
    //END EFFECT METHODS
    //*******************************************  
}
