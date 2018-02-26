using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarScript : MonoBehaviour {


	public void ActivateConstructingBuildingColor(){
		gameObject.transform.GetChild (2).GetComponent<Image> ().color = new Color32 (255, 255, 225, 100); 
		gameObject.transform.GetChild (0).GetComponent<Image> ().color = new Color32 (255, 255, 225, 50); 
	}

	public void ActivateNormalHPBarColor(){
		gameObject.transform.GetChild (2).GetComponent<Image> ().color = new Color32 (0, 255, 23, 255); 
		gameObject.transform.GetChild (0).GetComponent<Image> ().color = new Color32 (0, 135, 34, 255); 
	}

}
