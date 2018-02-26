using UnityEngine;
using System.Collections;
using RTS;
using System.Collections.Generic;

public class HealSpawn : FlySpawn {


	protected override void Awake (){
		base.Awake ();
	}

	protected override void Update () {
		base.Update ();
	}

	// protected override IEnumerator PeriodicallyLookForTargets(){
	// 	while (this != null && this.gameObject.activeInHierarchy) {
			
	// 		if (targetCollider == null || !targetCollider.gameObject.activeInHierarchy) {
    //             targetCollider = worldObject.GetLowestHPAllyUnitTargetInRange (paramManager.AggroRange);
	// 		} else {
	// 			WorldObject targetColWO = targetCollider.GetComponentInParent<WorldObject> ();
	// 			if (targetColWO && targetColWO.paramManager.HealthPercentage == 1.0f && this.hasForcedTarget == false) {
    //                 targetCollider = worldObject.GetLowestHPAllyUnitTargetInRange (paramManager.AggroRange);
	// 			}
	// 		}
	// 		yield return new WaitForSeconds (0.2f); 
	// 	}
	// 	yield return new WaitForSeconds (2.0f); 
	// }

	protected override void OnTriggerEnter(Collider encounteredCollider){
		if (encounteredCollider = targetCollider) {
			ImpactEffect ();
			WorldObject colliderWO = encounteredCollider.gameObject.GetComponentInParent<WorldObject> ();
			if (colliderWO) {
				InflictDamage (colliderWO, paramManager.AttackDamage); 
                Spawn colliderSpawn = colliderWO.GetComponent<Spawn> ();
                if (colliderSpawn == null) {
                    InflictDamage (this.worldObject, this.paramManager.MaxHitPoints);  
                }
			}
		}

	}



	protected override void InflictDamage (WorldObject doDamageToThis, int damageAmount) {
		base.InflictDamage (doDamageToThis, damageAmount) ; 
	}

	protected override void ImpactEffect() {
		effect = Instantiate(ResourceManager.GetSpawnEffect("FlyExplosion"), transform.position, transform.rotation);
		effect.transform.SetParent (ResourceManager.GetDynamicObjects ()); 
	}

	protected override void RemovalEffect() {
		effect = Instantiate(ResourceManager.GetSpawnEffect("FlyRemoval"), transform.position, transform.rotation);
		effect.transform.SetParent (ResourceManager.GetDynamicObjects ()); 
	}

}
