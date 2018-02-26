using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;
using System;


public class ControlCanvas : MonoBehaviour {


	public Sprite scoutImage;
	public Sprite healerImage;
	public Sprite catcherImage;
	public Sprite unitType4Image;

	private Player player;
	private Transform highlightImage1;
	private Transform highlightImage2;
	private Transform highlightImage3;
	private Transform highlightImage4;
	private Transform unitHealth1; 
	private Transform unitHealth2; 
	private Transform unitHealth3; 
	private Transform unitHealth4;
	private String hpText1;
	private String hpText2;
	private String hpText3;
	private String hpText4;
	private Button[] iconReferences;

	void Start () {
		player = transform.root.GetComponent< Player >();
		iconReferences = gameObject.GetComponentsInChildren<Button>(); 

		highlightImage1 = this.gameObject.transform.GetChild (0).GetChild (0);
		highlightImage2 = this.gameObject.transform.GetChild (1).GetChild (0);
		highlightImage3 = this.gameObject.transform.GetChild (2).GetChild (0);
		highlightImage4 = this.gameObject.transform.GetChild (3).GetChild (0);

		unitHealth1 = this.gameObject.transform.GetChild (0).GetChild (1);
		unitHealth2 = this.gameObject.transform.GetChild (1).GetChild (1);
		unitHealth3 = this.gameObject.transform.GetChild (2).GetChild (1);
		unitHealth4 = this.gameObject.transform.GetChild (3).GetChild (1);

		hpText1 = this.gameObject.transform.GetChild (0).GetChild(1).GetChild(2).GetComponent<Text>().text; 
		hpText2 = this.gameObject.transform.GetChild (1).GetChild(1).GetChild(2).GetComponent<Text>().text; 
		hpText3 = this.gameObject.transform.GetChild (2).GetChild(1).GetChild(2).GetComponent<Text>().text; 
		hpText4 = this.gameObject.transform.GetChild (3).GetChild(1).GetChild(2).GetComponent<Text>().text;

		//I can make this more accurate in the future




	}
	
	void Update () {
		if (iconReferences != null) {
			SetControlImages ();
			SetHighlightImages ();
			UpdateHealthBars ();
			UpdateHealthNumbers ();
		}
	}


	public void SetControlImages (){
		
		//ICON 1 
		if (player.controlGroup1.Count == 1) {
			WorldObject unit = player.controlGroup1 [0]; 
			string unitType = DetermineUnitType (unit);
			if (unitType == "scout") {
				if (iconReferences != null && iconReferences[0] != null) {
					iconReferences [0].GetComponent<IconControl1> ().UpdateImage (scoutImage);
				}
			}
			if (unitType == "healer") {
				if (iconReferences != null && iconReferences[0] != null) {
					iconReferences [0].GetComponent<IconControl1> ().UpdateImage (healerImage);
				}
			}
			if (unitType == "catcher") {
				if (iconReferences != null && iconReferences[0] != null) {
					iconReferences [0].GetComponent<IconControl1> ().UpdateImage (catcherImage);
				}
			}
		} else {
			iconReferences [0].GetComponent<IconControl1>().UpdateImage(null);
		}

		//ICON 2 
		if (player.controlGroup2.Count == 1) {
			WorldObject unit = player.controlGroup2 [0]; 
			string unitType = DetermineUnitType (unit);
			if (unitType == "scout") {
				if (iconReferences != null && iconReferences[1] != null) {
					iconReferences [1].GetComponent<IconControl2> ().UpdateImage (scoutImage);
				}
			}
			if (unitType == "healer") {
				if (iconReferences != null && iconReferences[1] != null) {
					iconReferences [1].GetComponent<IconControl2> ().UpdateImage (healerImage);
				}
			}
			if (unitType == "catcher") {
				if (iconReferences != null && iconReferences[1] != null) {
					iconReferences [1].GetComponent<IconControl2> ().UpdateImage (catcherImage);
				}
			}
		} else {
			iconReferences [1].GetComponent<IconControl2>().UpdateImage(null);
		}

		//ICON 3
		if (player.controlGroup3.Count == 1) {
			WorldObject unit = player.controlGroup3 [0]; 
			string unitType = DetermineUnitType (unit);
			if (unitType == "scout") {
				if (iconReferences != null && iconReferences[2] != null) {
					iconReferences [2].GetComponent<IconControl3> ().UpdateImage (scoutImage);
				}
			}
			if (unitType == "healer") {
				if (iconReferences != null && iconReferences[2] != null) {
					iconReferences [2].GetComponent<IconControl3> ().UpdateImage (healerImage);
				}
			}
			if (unitType == "catcher") {
				if (iconReferences != null && iconReferences[2] != null) {
					iconReferences [2].GetComponent<IconControl3> ().UpdateImage (catcherImage);
				}
			}
		} else {
			iconReferences [2].GetComponent<IconControl3>().UpdateImage(null);
		}

		//ICON 4
		if (player.controlGroup4.Count == 1) {
			WorldObject unit = player.controlGroup4 [0]; 
			string unitType = DetermineUnitType (unit);
			if (unitType == "scout") {
				if (iconReferences != null && iconReferences[3] != null) {
					iconReferences [3].GetComponent<IconControl4> ().UpdateImage (scoutImage);
				}
			}
			if (unitType == "healer") {
				if (iconReferences != null && iconReferences[3] != null) {
					iconReferences [3].GetComponent<IconControl4> ().UpdateImage (healerImage);
				}
			}
			if (unitType == "catcher") {
				if (iconReferences != null && iconReferences[3] != null) {
					iconReferences [3].GetComponent<IconControl4> ().UpdateImage (catcherImage);
				}
			}
		} else {
			iconReferences [3].GetComponent<IconControl4>().UpdateImage(null);
		}
	}


	public string DetermineUnitType (WorldObject unit){
		string unitType;
		if (unit != null) {
			if (unit.GetComponentInChildren<AntChild> () != null) {
				unitType = "scout";
				return unitType;
			}
			if (unit.GetComponentInChildren<PlantChild> () != null) {
				unitType = "healer";
				return unitType;
			}
			if (unit.GetComponentInChildren<Catcher> () != null) {
				unitType = "catcher";
				return unitType;
			}
			else {
				return null; 
			}
		} else {
			return null;
		}
		
	}

	public void SetHighlightImages (){

		if (player.controlGroup1.Count >= 1 && player.controlGroup1[0] != null) {
			if (player.controlGroup1 [0].currentlySelected) {
				//if (highlightImage1 
				highlightImage1.gameObject.SetActive (true);
			} else if (!player.controlGroup1 [0].currentlySelected) {
				highlightImage1.gameObject.SetActive (false);
			} 
		}
		else if (player.controlGroup1.Count == 0) {
			if (highlightImage1.gameObject ?? null) {
				highlightImage1.gameObject.SetActive (false);
			}
		}

		if (player.controlGroup2.Count >= 1 && player.controlGroup2[0] != null) {
			if (player.controlGroup2 [0].currentlySelected) {
				highlightImage2.gameObject.SetActive (true);
			} else if (!player.controlGroup2 [0].currentlySelected) {
				highlightImage2.gameObject.SetActive (false);
			}
		}
		else if (player.controlGroup2.Count == 0) {
			if (highlightImage2.gameObject ?? null) {
				highlightImage2.gameObject.SetActive (false);
			}
		}

		if (player.controlGroup3.Count >= 1 && player.controlGroup3[0] != null) {
			if (player.controlGroup3 [0].currentlySelected) {
				highlightImage3.gameObject.SetActive (true);
			} else if (!player.controlGroup3 [0].currentlySelected) {
				highlightImage3.gameObject.SetActive (false);
			}
		}
		else if (player.controlGroup3.Count == 0) {
			if (highlightImage3.gameObject ?? null) {
				highlightImage3.gameObject.SetActive (false);
			}
		}

		if (player.controlGroup4.Count >= 1 && player.controlGroup4[0] != null) {
			if (player.controlGroup4 [0].currentlySelected) {
				highlightImage4.gameObject.SetActive (true);
			} else if (!player.controlGroup4 [0].currentlySelected) {
				highlightImage4.gameObject.SetActive (false);
			}
		}
		else if (player.controlGroup4.Count == 0) {
			if (highlightImage4.gameObject ?? null) {
				highlightImage4.gameObject.SetActive (false);
			}
		}
	}

	public void UpdateHealthBars () {


		if (player.controlGroup1.Count == 1 && player.controlGroup1[0] != null) {
			unitHealth1.GetComponent<Slider> ().value = player.controlGroup1 [0].paramManager.HealthPercentage * 100;
			hpText1 = (player.controlGroup1 [0].paramManager.HitPoints.ToString()) + "/" + (player.controlGroup1 [0].paramManager.MaxHitPoints.ToString());
			unitHealth1.gameObject.SetActive (true);
		}
		else if (player.controlGroup1.Count == 0) {
			unitHealth1.gameObject.SetActive (false);
		}

		if (player.controlGroup2.Count == 1 && player.controlGroup2[0] != null) {
			unitHealth2.GetComponent<Slider> ().value = player.controlGroup2 [0].paramManager.HealthPercentage * 100;
			hpText2 = (player.controlGroup2 [0].paramManager.HitPoints.ToString()) + "/" + (player.controlGroup2 [0].paramManager.MaxHitPoints.ToString());
			unitHealth2.gameObject.SetActive (true);
		}
		else if (player.controlGroup2.Count == 0) {
			unitHealth2.gameObject.SetActive (false);
		}

		if (player.controlGroup3.Count == 1 && player.controlGroup3[0] != null) {
			unitHealth3.GetComponent<Slider> ().value = player.controlGroup3 [0].paramManager.HealthPercentage * 100;
			hpText3 = (player.controlGroup3 [0].paramManager.HitPoints.ToString()) + "/" + (player.controlGroup3 [0].paramManager.MaxHitPoints.ToString());
			unitHealth3.gameObject.SetActive (true);
		}
		else if (player.controlGroup3.Count == 0) {
			unitHealth3.gameObject.SetActive (false);
		}

		if (player.controlGroup4.Count == 1 && player.controlGroup4[0] != null) {
			unitHealth4.GetComponent<Slider> ().value = player.controlGroup4 [0].paramManager.HealthPercentage * 100;
			hpText4 = (player.controlGroup4 [0].paramManager.HitPoints.ToString()) + "/" + (player.controlGroup4 [0].paramManager.MaxHitPoints.ToString());
			unitHealth4.gameObject.SetActive (true);
		}
		else if (player.controlGroup4.Count == 0) {
			unitHealth4.gameObject.SetActive (false);
		}
	}

	public void UpdateHealthNumbers (){
		if (player.controlGroup1.Count == 1) {
			this.gameObject.transform.GetChild (0).GetChild (1).GetChild (2).GetComponent<Text> ().text = hpText1;
		}
		if (player.controlGroup2.Count == 1) {
			this.gameObject.transform.GetChild (1).GetChild (1).GetChild (2).GetComponent<Text> ().text = hpText2;
		}
		if (player.controlGroup3.Count == 1) {
			this.gameObject.transform.GetChild (2).GetChild (1).GetChild (2).GetComponent<Text> ().text = hpText3;
		}
		if (player.controlGroup4.Count == 1) {
			this.gameObject.transform.GetChild (3).GetChild (1).GetChild (2).GetComponent<Text> ().text = hpText4;
		}


	}

}