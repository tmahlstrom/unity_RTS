using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;





public class WorldObject : MonoBehaviour {

	[Header ("Name")] 
	public string objectName;


	private int accumulatedDamage; 
	private float timeSinceDamageAnimation; 

	public Player player;
	public EnemyManager enemyManager;
	public AudioSource audioSource; 


	public bool hasStatsVisible = false;
	public bool currentlySelected = false;
	private bool hoverEffect = false;
	bool hoverFlag = false;


	

	public List<Collider> enemyUnitTargetsInRange = new List<Collider>();
	public Collider nearestEnemyUnitTargetInRange;
	public List<Collider> allyUnitTargetsInRange = new List<Collider>();
	public List<Collider> enemySpawnTargetsInRange = new List<Collider> ();
	public List<Collider> enemyTargetsInRange = new List<Collider> ();
	public Target[] selfTargets;





	protected Bounds objectBounds;
	protected TeamColor[] teamColors;
	protected SelectionManager selectionManager; 
	public WorldObject worldObject; 
	


	public delegate void DeathProtocol();
	public event DeathProtocol OnWorldObjectDeathDelegate;
	private bool deathProtocallIsSetUp = true;

    public delegate void ReviveProtocol();
    public event ReviveProtocol OnWorldObjectReviveDelegate;
    private bool reviveProtocallIsSetUp = false;

    public ParamManager paramManager;
	public AnimationManager animationManager;






	protected virtual void Awake() {
		CalculateBounds ();
        paramManager = GetComponent<ParamManager>(); 
		animationManager = GetComponent<AnimationManager>(); 
		player = transform.GetComponentInParent<Player> ();
		enemyManager = transform.GetComponentInParent<EnemyManager>();
		selfTargets = GetComponentsInChildren<Target>();
		teamColors = GetComponentsInChildren<TeamColor>();
		audioSource = GetComponent<AudioSource>(); 
		selectionManager = GetComponentInChildren<SelectionManager>(); 

		worldObject = this; 
		SetTeamColor();
		
	}


	protected virtual void Start () {

	}

	protected virtual void Update () {

	}


	//*******************************************
	//BEGIN SELECTION METHODS
	//*******************************************


    public void ChangeSelectionStatus (bool toggle){
        if (toggle == true){
            if (currentlySelected == false){//this was inteded to function to only play the clip when an object is selected that wasn't already selected, but because of the liberal use of ClearSelection() in player, this doens't function, but atlas, i'm not even sure if i want that kind of functionality. 
                AudioManager.Instance.Play("SelectionChange");
            }
            currentlySelected = true;
        } else {
            currentlySelected = false;
        }
    }
	public bool HoverEffect {get{return hoverEffect;} 
		set {
			hoverFlag = true;
			if (!currentlySelected && hoverEffect == false) {
				StartCoroutine(HoverEffectResetter()); 
			}
		}
	}

	private IEnumerator HoverEffectResetter(){
		hoverEffect = true; 
		//hoverFlag = true; 
		while (hoverEffect){
			yield return null;
			hoverFlag = false; 
			yield return null;
			if (hoverFlag == false){
				hoverEffect = false; 
			}
		}
	}

	public void TargetedByPlayer(){
		if (selectionManager){
			selectionManager.TargetedEffect();
		}
	}

	//*******************************************
	//END SELECTION METHODS
	//*******************************************


	//*******************************************
	//BEGIN FIND WORLDOBJECTS IN RANGE
	//*******************************************


	public Collider GetNearestEnemyTargetColliderInRange(float range){
		enemyUnitTargetsInRange = WorkManager.DetermineEnemyUnitTargetsInRange (this, range);
		if (enemyUnitTargetsInRange.Count != 0) {
			nearestEnemyUnitTargetInRange = WorkManager.DetermineNearestCollider (this, enemyUnitTargetsInRange);
			return nearestEnemyUnitTargetInRange;
		} else
			return null; 
	}


		
		
	//*******************************************
	//END FIND WORLDOBJECTS IN RANGE
	//*******************************************




	public void CalculateBounds() {
		objectBounds = new Bounds(transform.position, Vector3.zero);
		foreach(Renderer r in GetComponentsInChildren< Renderer >()) {
			objectBounds.Encapsulate(r.bounds);
		}
	}
		

	// public virtual void SetHoverState(GameObject hoverObject) {

	// }
		

	public Bounds GetObjectBounds() {
		return objectBounds;
	}


	public void SetColliders(bool enabled) {
		Collider[] colliders = GetComponentsInChildren< Collider >();
		foreach(Collider collider in colliders) collider.enabled = enabled;
	}


		

	protected void SetTeamColor() {
		if (player) {
			foreach (TeamColor teamColor in teamColors)
				teamColor.GetComponent<Renderer> ().material.color = player.teamColor;
		}
		if (enemyManager) {
			foreach (TeamColor teamColor in teamColors)
				teamColor.GetComponent<Renderer> ().material.color = enemyManager.teamColor;
		}
	}
		



	//*******************************************
	//BEGIN TAKE DAMAGE AND REMOVE UNIT PROCEDURE
	//*******************************************

	public void TakeDamage(int rawDamage, Vector3 impactPoint, WorldObject woDoingDamage) {
		if (rawDamage > 0 && paramManager.IsDead){
			return; 
		}
		int damage = ConsiderDamageMultipliers (rawDamage); 
        paramManager.HPmod(damage);
		ConsiderShowingDamageOnWorldInfoCanvas (damage, impactPoint);
		if (StageManager.Instance){
			foreach (Player p in StageManager.Instance.listOfPlayersWithWorldInfoCanvas) {
				p.worldInfoCanvasScript.UpdateWorldObjectStats(worldObject);
			}
		}
        if (paramManager.IsDead) {
            if (woDoingDamage && woDoingDamage.player) {
                RewardKiller(woDoingDamage.player);
            }
			ManageDeathOfWorldObject ();
		}
	}

    protected virtual void RewardKiller(Player playerWhoKilledMe){

    }



	protected virtual int ConsiderDamageMultipliers (int rawDamage){
		return rawDamage;
	}

	private IEnumerator RememberTimeSinceDamageAnimation(){
        while (this != null && gameObject.activeInHierarchy) {
			timeSinceDamageAnimation += Time.deltaTime; 
			yield return null;
		}
	}

	protected virtual void ConsiderShowingDamageOnWorldInfoCanvas(int damage, Vector3 impactPoint){
		if (damage < 0) {
			ShowDamage (damage);
			return; 
		}
		accumulatedDamage += damage; 
		if (timeSinceDamageAnimation > 0.3f) {
			ShowDamage (accumulatedDamage); 
			accumulatedDamage = 0;
			timeSinceDamageAnimation = 0.0f; 
		}
	}

	protected virtual void ShowDamage (int damageToShow) {
		foreach (Player p in StageManager.Instance.listOfPlayersWithWorldInfoCanvas) {
			p.worldInfoCanvasScript.ShowText (damageToShow, transform.position, this, false); 
		}
	}

    protected virtual void ShowResourceReward(int resourcesToShow) {
        foreach (Player p in StageManager.Instance.listOfPlayersWithWorldInfoCanvas) {
            p.worldInfoCanvasScript.ShowText(resourcesToShow, transform.position, this, true);
        }
    }

	protected virtual void ManageDeathOfWorldObject (){
		StageManager.Instance.allSelectables.Remove (this);
		if (player){
			if (player.selectedObjects.Contains(this)){
				player.selectedObjects.Remove(this);
			}
		}
		currentlySelected = false; 
		DisableTargets();
		DeathAudio(); 
		foreach (Player p in StageManager.Instance.listOfPlayersWithWorldInfoCanvas) {
			if (p.worldInfoCanvasScript.unitStatsPair.ContainsKey(this)) {
				RectTransform unitStats = p.worldInfoCanvasScript.unitStatsPair [this];
				p.worldInfoCanvasScript.unitStatsPair.Remove (this);
				hasStatsVisible = false;
				Destroy (unitStats.gameObject);
			}
		}
		if (OnWorldObjectDeathDelegate != null && deathProtocallIsSetUp){
			OnWorldObjectDeathDelegate ();
			deathProtocallIsSetUp = false; 
		}
		if (enemyManager) { 
            StartCoroutine(DelayedDestroy()); 
		}
        reviveProtocallIsSetUp = true; 
	}

	protected virtual void DropItem (){
                
    }

    protected virtual IEnumerator DelayedDestroy(){
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

	public void ReviveWO (){
		ManageReviveOfWorldObject (); 
	}

	protected virtual void ManageReviveOfWorldObject(){
		gameObject.SetActive (true); 
		StageManager.Instance.allSelectables.Add (this);
		GiveThisWorldInfoCanvasStats (); 
		deathProtocallIsSetUp = true; 
        EnableTargets();

        if (OnWorldObjectReviveDelegate != null && reviveProtocallIsSetUp){
            OnWorldObjectReviveDelegate ();
            reviveProtocallIsSetUp = false; 
        }
	}



    protected virtual void EnableTargets(){
        foreach (Target target in selfTargets){
            //target.enabled = true; 
			target.gameObject.SetActive(true);
        }
    }

    protected virtual void DisableTargets(){
        foreach (Target target in selfTargets){
            target.gameObject.SetActive(false); 
        }
    }

	protected virtual void DeathAudio(){
		if (audioSource){
			if (paramManager.DieAudioClip){
				AudioManager.Instance.Play(paramManager.DieAudioClip, audioSource);
			}
		}
	}


		

	//*******************************************
	//END TAKE DAMAGE AND REMOVE UNIT PROCEDURE
	//*******************************************

	protected virtual void GiveThisWorldInfoCanvasStats(){
		foreach (Player p in StageManager.Instance.listOfPlayersWithWorldInfoCanvas) {
			p.worldInfoCanvasScript.AssignUnitStats (this);
		}
        if (this != null && gameObject.activeInHierarchy) {
            StartCoroutine("RememberTimeSinceDamageAnimation");
        }
	}



    public virtual void MouseClickRight (GameObject hitObject, Vector3 hitPoint) {

    }

    public virtual void MouseClickLeft (Vector3 hitPoint){

    }

	public void SpacebarToWO(){
		SpacebarPressed ();
	}

	public void SpacebarHeldToWO(){
		SpacebarHeld ();
	}


	protected virtual void SpacebarPressed(){ //perhaps should be moved to character state class 

	}

	protected virtual void SpacebarHeld(){ //perhaps should be moved to character state class 

	}

	public virtual void AnimationStartPoint (RTS.EAnimation action){

	}

	public virtual void AnimationClimaxPoint (RTS.EAnimation action){

	}

	public virtual void AnimationEndPoint (RTS.EAnimation action){

	}

}

