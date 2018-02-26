using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using UnityEngine.AI;

public class GroupFormationManager : MonoBehaviour {

	[SerializeField]
	private float formationSpacing = 2.0f;

	private Player player; 
	private List<WorldObject> selectedObjects; //





	void Awake () {
		player = GetComponent<Player> ();
		selectedObjects = player.GetSelectedObjects ();
		if (gameObject.transform.Find("CursorCanvas") ?? null) {
		}
	}


	public void DetermineFormationGroup (WorldObject targetedWorldObject, Vector3 formationCenter){
		Vector3 selectedCenter = UnitsSelectedCenter ();
		Vector3 connectingVector = formationCenter - selectedCenter;
		Vector3 spacingVector = Vector3.Scale (connectingVector.normalized, new Vector3 (formationSpacing, 0, formationSpacing));
		Vector3 unitPosition1 = Vector3.zero;
		Vector3 unitPosition2 = Vector3.zero;
		Vector3 unitPosition3 = Vector3.zero;
		Vector3 unitPosition4 = Vector3.zero;
		List<Vector3> movePointAssignments = new List<Vector3> ();
		if (selectedObjects.Count == 1) {
			unitPosition1 = formationCenter;
		}
		if (selectedObjects.Count == 2) {
			if (targetedWorldObject != null) {
				spacingVector = Vector3.Scale (connectingVector.normalized, new Vector3 (formationSpacing * 0.4f, 0, formationSpacing * 0.4f));
			}
			//IF HORIZONTAL 
			unitPosition1 = formationCenter + Quaternion.Euler (0, 90, 0) * spacingVector;
			unitPosition2 = formationCenter + Quaternion.Euler (0, -90, 0) * spacingVector;
		}
		if (selectedObjects.Count == 3) {
			if (targetedWorldObject != null) {
				spacingVector = Vector3.Scale (connectingVector.normalized, new Vector3 (formationSpacing * 0.5f, 0, formationSpacing * 0.5f));
				unitPosition1 = formationCenter;
				unitPosition2 = formationCenter + Quaternion.Euler (0, 70, 0) * spacingVector;
				unitPosition3 = formationCenter + Quaternion.Euler (0, -70, 0) * spacingVector;
			} else if (targetedWorldObject == null) {
				//IF TRIANGLE 
				unitPosition1 = formationCenter + spacingVector;
				unitPosition2 = formationCenter + Quaternion.Euler (0, 90, 0) * spacingVector;
				unitPosition3 = formationCenter + Quaternion.Euler (0, -90, 0) * spacingVector;
			}
		}
		if (selectedObjects.Count == 4) {
			if (targetedWorldObject != null) {
				spacingVector = Vector3.Scale (connectingVector.normalized, new Vector3 (formationSpacing * 0.5f, 0, formationSpacing * 0.5f));
				unitPosition1 = formationCenter+ Quaternion.Euler (0, 90, 0) * spacingVector * 0.8f;
				unitPosition2 = formationCenter + Quaternion.Euler (0, -90, 0) * spacingVector * 0.8f;
				unitPosition3 = formationCenter + Quaternion.Euler (0, 70, 0) * spacingVector * 2.0f;
				unitPosition4 = formationCenter + Quaternion.Euler (0, -70, 0) * spacingVector * 2.0f;
			} else if (targetedWorldObject == null) {
				//IF SQUARE
				unitPosition1 = formationCenter + spacingVector;
				unitPosition2 = formationCenter + Quaternion.Euler (0, 90, 0) * spacingVector;
				unitPosition3 = formationCenter + Quaternion.Euler (0, -90, 0) * spacingVector;
				unitPosition4 = formationCenter - spacingVector;
			}
		}


		if (unitPosition1 != Vector3.zero) {
			movePointAssignments.Add (unitPosition1);
		}
		if (unitPosition2 != Vector3.zero) {
			movePointAssignments.Add (unitPosition2);
		}
		if (unitPosition3 != Vector3.zero) {
			movePointAssignments.Add (unitPosition3);
		}
		if (unitPosition4 != Vector3.zero) {
			movePointAssignments.Add (unitPosition4);
		}

		for (int i = 0; i < selectedObjects.Count; i++) {
            Mover mover = selectedObjects [i].GetComponent<Mover> (); 
			if (mover) {
				mover.AddMoveTarget(null, movePointAssignments[i]);
			}
		}
	}

	public Vector3 UnitsSelectedCenter (){
		Vector3 sumOfVectors = new Vector3(); 
		for (int i = 0; i < selectedObjects.Count; i++) {
			if (selectedObjects [i] != null) {
				sumOfVectors += selectedObjects [i].transform.position;
			}
		}
		Vector3 averageOfVectors = new Vector3();
		averageOfVectors = sumOfVectors / (selectedObjects.Count); 
		return averageOfVectors;
	}
}
