using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class GreenFlySpawn : FlySpawn {

    public override void LookForTarget(){
		if (mySpawnerWorldObject && mySpawnerWorldObject.paramManager.PlayerOwned) {
            List<Collider> possibleTargetColliders = new List<Collider>(); 
            possibleTargetColliders = WorkManager.DeterminePlayerUnitTargetsInRange(transform.position, paramManager.AggroRange);
            if (possibleTargetColliders.Count > 0){
                foreach (Collider col in possibleTargetColliders){
                    WorldObject wo = col.GetComponentInParent<WorldObject>();
                    if (wo && wo.paramManager.HealthPercentage != 1f){
                        if (targetCollider){
                            if ((gameObject.transform.position - col.gameObject.transform.position).sqrMagnitude < (gameObject.transform.position - targetCollider.gameObject.transform.position).sqrMagnitude){
                                targetCollider = col;
                            }
                        } else {
                            targetCollider = col;
                        }
                        return; 
                    }
                }
            }
		}
        targetCollider = null; 
	}


    protected override void OnTriggerEnter(Collider encounteredCollider){
        Target tar = encounteredCollider.gameObject.GetComponent<Target> ();
        if (tar){
    		WorldObject colliderWO = encounteredCollider.gameObject.GetComponentInParent<WorldObject> ();
            // Debug.Log (player + "  eeeee    " +  colliderWO.player);
    		// if (colliderWO && WorkManager.AreWorldObjectsOnSameTeam(worldObject, colliderWO) == true) {
            if (colliderWO && colliderWO.player != null){
    			ImpactEffect ();
                InflictDamage (colliderWO, paramManager.AttackDamage); 
    			Spawn colliderSpawn = colliderWO.GetComponent<Spawn> ();
    			if (colliderSpawn == null) {
    				InflictDamage (this.worldObject, this.paramManager.MaxHitPoints);  
    			}
    		}
        }
	}
}
