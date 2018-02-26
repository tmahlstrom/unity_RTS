using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSetter : MonoBehaviour {
    
    PositionTracker positionTracker; 
    Vector3 positionTrackerLocal; 

	void Start () {
        positionTracker = transform.parent.GetComponentInChildren<PositionTracker>();
        //if (positionTracker)
            //positionTrackerLocal = positionTracker.transform.localPosition;
        //offset = new Vector3(0,0,-1.0f); 
	}
	
	void Update () {
        if (positionTracker){
            positionTrackerLocal = positionTracker.transform.position;
            //positionTrackerLocal.z -= 0.01f; 
            //positionTracker.transform.localPosition = positionTrackerLocal;
            //trackerPosition = positionTracker.transform.position; 
            //trackerPosition.loz -= 1.0f; 
            transform.position = positionTrackerLocal;
            //positionTracker.transform.position; 
        }
	}
}
