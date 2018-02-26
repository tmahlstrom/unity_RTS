using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReviver  {

	void IssueReviveCommand(); 
	void ResetReviver(); 
	void ReviveExecution();
	void ReviveComplete();
	bool CheckIfReadyToBeginReviving();
	bool CheckForRepeatCommand(GameObject hitObject);
}
