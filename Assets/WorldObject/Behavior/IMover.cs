using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMover {

	void PrimaryInteractGameObjectAtDistance(GameObject gameObject, float engageDistance);
	void SecondaryInteractGameObjectAtDistance (GameObject gameObject, float followDistance);
	bool IsInRangeOfPrimaryTarget (GameObject rangeTarget);
	bool IsFacingPrimaryTarget (GameObject rotationTarget);
	void ClearMovement ();





	void AddMoveTarget (GameObject moveTarget, Vector3 movePointLocation);//this is needed only for the group formation manager; otherwise use interaction methods
	void ClearMoveList();
	void SetMoveTrackStarter();//doesn't work right now, but not sure if I care

	void SetManualLookTarget(Vector3 lookAtThis);
	void DisableRotation(); 
	void EnableRotation(); 
	void DisableMoveInput();//i should be able to get rid of this via the state senstive input processing; it's only needed for spawner special
	void EnableMoveInput();//ditto



		
}
