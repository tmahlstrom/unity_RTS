using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker {

	void ResetAttacker(); 
	void AttackExecution();
	void AttackIterationComplete();
	bool CheckIfReadyToBeginAttacking();
	bool CheckForRepeatCommand(Collider hitCollider);
	bool AttackProcessStarted();
}
