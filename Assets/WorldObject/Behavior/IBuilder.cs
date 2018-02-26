using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder {

	GameObject GOfocus {get;}

	bool CheckForRepeatCommand(GameObject hitObject);

	void InitiateBuildingPlacement();
	bool CheckToCancelBuildingPlacement();

	bool CheckToGiveNewBuildOrder ();
	bool CheckToGiveResumeBuildOrder(GameObject hitObject);

	bool CheckIfReadyToBeginBuilding();
	void BuildExecution();


	void ResetBuilder();

}
