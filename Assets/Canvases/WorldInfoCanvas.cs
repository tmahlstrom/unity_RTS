using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;
using System.Xml.Linq;
using System.Linq;
using UnityEngine.Rendering;
using System.Text;

public class WorldInfoCanvas : MonoBehaviour {


	public Transform unitInfo;
	public RectTransform canvasRect; 
	public float shiftBarHorizontal = 40;
	public int shiftBarVertical = 35;
	public Player player;
	public List<Transform> objectsToFollow;
	WorldObject hpBar;
	public List<RectTransform> unitStatsOnScreen;
	public Dictionary<WorldObject, RectTransform> unitStatsPair = new Dictionary<WorldObject, RectTransform>();
	private float originalHPBarWidth;
	RectTransform hpRect;
	private float originalMPBarWidth;
	RectTransform mpRect;
	private int screenWidth;
    private FloatTextContainer floatTextContainer;


	void Awake(){
		if (unitInfo == null){
			Debug.Log("oh shit");
		}
		player = transform.root.GetComponent< Player >();
		hpRect = this.unitInfo.GetChild(0).GetChild(2) as RectTransform;
		originalHPBarWidth = hpRect.sizeDelta.x;
		mpRect = this.unitInfo.GetChild(1).GetChild(2) as RectTransform;
		originalMPBarWidth = mpRect.sizeDelta.x;
		screenWidth = Screen.width;
        floatTextContainer = GetComponentInChildren<FloatTextContainer>();
	}




	void Start () {
	}
	
	void Update () {
		UpdateUnitStatsPositions ();
	}
		

	public void AssignUnitStats (WorldObject woNeedingStats){
		if (woNeedingStats.hasStatsVisible == false) {
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (Camera.main, woNeedingStats.transform.position);
			screenPoint.y += shiftBarVertical;
			RectTransform unitInfo = Instantiate (this.unitInfo, screenPoint, transform.rotation) as RectTransform;
			if (unitInfo){
				unitInfo.transform.SetParent (this.transform, true);
				if (woNeedingStats.transform.GetComponent<Unit> () == null) {
					unitInfo.transform.GetChild(1).gameObject.SetActive(false);
				}
				if (woNeedingStats.paramManager.MaxManaPoints == 0) {
					unitInfo.transform.GetChild(1).gameObject.SetActive(false);
				}
				Building building = woNeedingStats.GetComponent<Building> ();
				if (building && !building.IsFinishedBuilding) {
					HPBarScript hpBar = unitInfo.transform.GetChild (0).GetComponent<HPBarScript> ();
					hpBar.ActivateConstructingBuildingColor ();
				}
				unitStatsPair.Add (woNeedingStats, unitInfo); 
				if (unitInfo) {
					unitInfo.anchoredPosition = (screenPoint - canvasRect.sizeDelta / 2f);
					woNeedingStats.hasStatsVisible = true;
				}
				UpdateUnitStatsPositions ();
				UpdateWorldObjectStats (woNeedingStats);
			}
		}
	}

	public void ActivateNormalHPTextColor(WorldObject wo){
		RectTransform unitInfo = unitStatsPair [wo]; 
		HPBarScript hpBar = unitInfo.transform.GetChild (0).GetComponent<HPBarScript> ();
		hpBar.ActivateNormalHPBarColor ();
	}

	public void UpdateUnitStatsPositions (){
		foreach (KeyValuePair<WorldObject, RectTransform> pair in unitStatsPair) {
			if (pair.Key != null) {
				Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (Camera.main, pair.Key.transform.position);
				screenPoint.y += shiftBarVertical;
				float shiftX = ((screenPoint.x/(screenWidth))-0.5f) * (shiftBarHorizontal);
				screenPoint.x += shiftX;
				pair.Value.anchoredPosition = (screenPoint - canvasRect.sizeDelta / 2f);
			}
		}
	}

	public void UpdateWorldObjectStats (WorldObject unitWithStats){
		if (unitStatsPair.ContainsKey (unitWithStats)) {
			hpRect = unitStatsPair [unitWithStats].transform.GetChild(0).GetChild(2) as RectTransform;
			float hpBarheight = hpRect.sizeDelta.y;
			float targetHPBarWidth = originalHPBarWidth * unitWithStats.paramManager.HealthPercentage;
			hpRect.sizeDelta = new Vector2 (targetHPBarWidth, hpBarheight);
			float manaPercentage = 0.0f;
			if (unitWithStats.transform.GetComponent<Unit> () != null) {
				if (unitWithStats.transform.GetComponent<Unit> () != null) {
					manaPercentage = unitWithStats.transform.GetComponentInChildren<Unit> ().paramManager.ManaPercentage;
				}
				mpRect = unitStatsPair [unitWithStats].transform.GetChild(1).GetChild(2) as RectTransform;
				float mpBarHeight = mpRect.sizeDelta.y;
				float targetMPBarWidth = originalMPBarWidth * manaPercentage;
				mpRect.sizeDelta = new Vector2 (targetMPBarWidth, mpBarHeight);
				ManaScript manaScript = unitStatsPair [unitWithStats].transform.GetComponentInChildren<ManaScript> ();
				if (manaScript) {
					if (manaPercentage == 1.0f) {
						manaScript.enabled = true;
					} else
						manaScript.enabled = false;
				}
			}
		}
	}

    public void ShowText (int textToShow, Vector3 location, WorldObject worldObject, bool isResource){

        //if (worldObject && unitStatsPair.ContainsKey(worldObject)) { //This is to show all kinds of text, was getting cluttered; below only shows resource text
        if(isResource){
			Vector3 screenLocation = Camera.main.WorldToScreenPoint (location);
			GameObject popUpText = (GameObject)Instantiate (ResourceManager.GetMiscellaneous ("FloatText"));
			bool isOwnUnit = false;
			if (worldObject.player != null && worldObject.player == this.player) {
				isOwnUnit = true;
			}
			popUpText.GetComponent<FloatText> ().SetText (textToShow, isOwnUnit, isResource);
            popUpText.transform.SetParent(floatTextContainer.gameObject.transform);
			popUpText.transform.position = screenLocation;
		}

	}
		 

}
