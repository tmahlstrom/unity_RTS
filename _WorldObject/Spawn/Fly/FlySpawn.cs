using UnityEngine;
using System.Collections;
using RTS;
using System.Collections.Generic;

public class FlySpawn : Spawn {
	
	protected GameObject effect;





	protected override void Awake (){
		base.Awake ();
	}


		

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





    //*******************************************
    //BEGIN GENERAL MOVEMENT METHODS
    //*******************************************



    public override void AugmentRotationSpeed (Vector3 moveTarget) {
        Vector3 relativePosition = moveTarget - transform.position; 
        Quaternion neededRotation = Quaternion.LookRotation(relativePosition); 
        transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, paramManager.RotationSpeed * Time.deltaTime);
    }



    //*******************************************
    //END GENERAL MOVEMENT METHODS
    //*******************************************

    //*******************************************
    //BEGIN PATROL METHODS
    //*******************************************
	public override Vector3 FindPatrolPoint(){ 
		if (mySpawnerWorldObject != null) { 
			patrolOriginPoint = mySpawnerWorldObject.transform.position; 
		} else {
			patrolOriginPoint = transform.position + Vector3.up * 0.01f;
            //patrolOriginPoint.y = 0;  
		}
        Vector3 randomDirection = Random.insideUnitSphere * paramManager.PatrolRadius;
        Vector3 almostNextPatrolPoint = randomDirection + patrolOriginPoint; 
        almostNextPatrolPoint.y = patrolOriginPoint.y + Random.Range (1.5f, 2f);
        patrolPoint = almostNextPatrolPoint; 
        return patrolPoint;
	}


		
    //*******************************************
    //END PATROL METHODS
    //*******************************************

    //*******************************************
    //BEGIN APPROACH TARGET METHODS
    //*******************************************


    public override Vector3 FindOffCenterApproachPoint (Vector3 targetPosition){
        Vector3 connectingVector = transform.position - targetPosition;
        Vector3 connectingVector2 = (transform.position + new Vector3(0,1,0)) - targetPosition;
        Vector3 normalVector = (Vector3.Cross (connectingVector, connectingVector2)).normalized;
        Vector3 distanceSensitiveNormalVector = normalVector * connectingVector.magnitude; 
        int navDirectionModifier = Random.Range (0, 2) * 2 - 1;
        float offTargetMovement = Random.Range (paramManager.DegreeOfRandomMovement, -paramManager.DegreeOfRandomMovement); 
        Vector3 desiredApproachPoint = targetPosition + distanceSensitiveNormalVector * navDirectionModifier * offTargetMovement;
        return desiredApproachPoint;  
    }

    public override bool InRangeForAtack(){
		if (targetCollider){
			if (Vector3.Distance (transform.position, targetCollider.transform.position) < paramManager.AttackRange){
				return true;
			}
		}
		return false; 
	}


    //*******************************************
    //END APPROACH TARGET METHODS
    //*******************************************



    //*******************************************
    //BEGIN IMPACT & RESOLUTION METHODS
    //*******************************************

	protected override void InflictDamage (WorldObject doDamageToThis, int damageAmount) {
		base.InflictDamage (doDamageToThis, damageAmount) ; 
	}

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
        effect = Instantiate(paramManager.SpawnRayEffectSelf, transform.position, transform.rotation);
		effect.transform.SetParent (ResourceManager.GetDynamicObjects ());
		if (mySpawner) {
			mySpawner.SubtractDeadSpawnFromPopulationCount (); 
		}
		Destroy (gameObject);
	}

    //*******************************************
    //END IMPACT & RESOLUTION METHODS
    //*******************************************

}
