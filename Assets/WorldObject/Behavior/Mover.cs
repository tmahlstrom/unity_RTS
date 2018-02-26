using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS;
using System.Runtime.InteropServices;
using System.Threading;


public class Mover : MonoBehaviour, IMover {


    //*******************************************
    //BEGIN INTERFACE METHODS
    //*******************************************


    public bool CheckForFollowAllyCommand(GameObject hitObject){
        WorldObject hitWorldObject = hitObject.GetComponentInParent<WorldObject> ();
        if (hitWorldObject && WorkManager.AreWorldObjectsOnSameTeam (hitWorldObject, worldObject) == true) {
            SecondaryInteractGameObjectAtDistance (hitObject, 1.2f);
            return true;
        }
        return false; 
    }

    public void DisableMoveInput() {
        acceptingMoveInput = false;
    }

    public void EnableMoveInput() {
        acceptingMoveInput = true;
    }


    public void SetMoveTrackStarter(){//this doesn't work right now, not sure if i care
        if (this.gameObject.activeSelf) {
            StartCoroutine ("SetMoveTrack");
        }
    }

    public void AddMoveTarget (GameObject moveTarget, Vector3 movePointInput){
        if (!acceptingMoveInput) {
            return; 
        }
        CheckToClearMovePointList (); 
        Vector3 movePointOutput = Vector3.zero; 
        if (moveTarget != null) {
            ClearMoveList ();
            Vector3 closeEnoughPoint = SlightlyShortenedDistance (moveTarget);
            movePointOutput = closeEnoughPoint; 
            //movePointLocation = moveTarget.transform.position; 
        } else {       
            NavMeshHit hit; 
            if (NavMesh.SamplePosition(movePointInput, out hit, 1.0f, NavMesh.AllAreas)){
                movePointOutput = hit.position; 
            }
        }
        if (movePointOutput == Vector3.zero){
            movePointOutput = movePointInput;
        }
        listOfMovePoints.Add (movePointOutput);
    }
    
    public void ClearMoveList (){
        if (listOfMovePoints.Count != 0) {
            listOfMovePoints.Clear ();
            mayStillNeedToRotate = false;
        }
    }

    public void SetManualLookTarget(Vector3 lookAtThis){//this is used to register look targets in case movement is not involved...there is now some redundancy...
        SetLookTarget(lookAtThis);
        TurnToLookTarget();
    }

    public void PrimaryInteractGameObjectAtDistance(GameObject gameObject, float engageDistance){
        if (acceptingMoveInput) {
            this.primaryInteractDistance = engageDistance;
            this.primaryTarget = gameObject;
        }
        if (IsInRangeOfPrimaryTarget(primaryTarget)){//if you're already within range, look at it immediately (if you're not within distance, this is set when you are in distance)
            SetLookTarget(gameObject.transform.position);
            //TurnToLookTarget();
        }
    }

    public void SecondaryInteractGameObjectAtDistance (GameObject gameObject, float followDistance){
        if (acceptingMoveInput){
            this.secondaryInteractDistance = followDistance;
            this.secondaryTarget = gameObject;
        }
    }

    public bool IsInRangeOfPrimaryTarget (GameObject rangeTarget){
        if (primaryTarget && primaryTarget == rangeTarget){
            Vector3 flatPosition = transform.position; 
            flatPosition.y = 0f; 
            Vector3 flatTarget = primaryTarget.transform.position;
            flatTarget.y = 0f; 
            if (Vector3.Distance (flatPosition, flatTarget) < (primaryInteractDistance + 0.3)) {
               return true;
            }
        }
        return false; 
    }

    public bool IsFacingPrimaryTarget (GameObject rotationTarget){
        if (primaryTarget && primaryTarget == rotationTarget){
            if (mayStillNeedToRotate == false){
                return true;
            }
        }
        return false; 
    }

    public void DisableRotation(){
        rotationAllowed = false; 
    }

    public void EnableRotation(){
        rotationAllowed = true;
    }

    public void ClearMovement (){
        ClearMoveList(); 
        primaryTarget = null;
        primaryInteractDistance = 0;
        secondaryTarget = null;
        secondaryInteractDistance = 0;
        mayStillNeedToRotate = false;
        SetLookTarget(transform.position); 
    }

    //*******************************************
    //END INTERFACE METHODS
    //*******************************************

    private bool moving, rotating = false;
    private GameObject primaryTarget; 
    private GameObject secondaryTarget;
    private float primaryInteractDistance = 1.0f;
    private float secondaryInteractDistance = 2.5f;

    private bool interactCoroutineStarted = false; 
    private bool moveAndRotationModifierCoroutineStarted = false; 
    private bool considerRotationCoroutineStarted = false;
    private bool considerGivingUpCoroutineStarted = false;

    private bool acceptingMoveInput = true;
    private bool rotationAllowed = true; 
    private bool readyToMove, mayStillNeedToRotate = false;

    private List<Vector3> listOfMovePoints = new List<Vector3>();
    private NavMeshAgent navAgent; 
    private NavMeshObstacle navObstacle; 
    private Vector3 destination;
    private float stoppingDistance;
    private Quaternion finalDesiredRotation;

    private WorldObject worldObject;
    private Unit unit; 
    private Player player;
    private DragSelectionHandler dragSelectionHandler;

    private ParamManager paramManager;


    private void Awake (){

        paramManager = GetComponent<ParamManager>(); 
        navAgent = GetComponent<NavMeshAgent> ();
        if (navAgent) {
            navAgent.speed = paramManager.MoveSpeed; 
        }
        navObstacle = GetComponent<NavMeshObstacle> (); 
        worldObject = GetComponentInParent<WorldObject> (); 
        unit = GetComponent<Unit>(); 
        stoppingDistance = GetComponent<NavMeshAgent> ().stoppingDistance; 
        player = GetComponentInParent<Player> ();
        if (player){
            dragSelectionHandler = player.GetComponentInChildren<DragSelectionHandler> (); //if GetRightClickDragInputOccurring() returns true, selection is not cleared when adding another point
        }
    }

    private void Start(){
        NavMeshHit hit; 
        if (NavMesh.SamplePosition(transform.position, out hit, 5, NavMesh.AllAreas)){
            Vector3 pos = transform.position;
            pos.y = hit.position.y; 
            transform.position = pos; 
        }
    }

    private void OnEnable (){
        worldObject.OnWorldObjectDeathDelegate += ClearMoveList;
        worldObject.OnWorldObjectDeathDelegate += DisableRotation;
        worldObject.OnWorldObjectReviveDelegate += EnableRotation; 
        ResetMovement (); 
    }

    private void OnDisable (){
        worldObject.OnWorldObjectDeathDelegate -= ClearMoveList; 
        worldObject.OnWorldObjectDeathDelegate -= DisableRotation;
        worldObject.OnWorldObjectReviveDelegate -= EnableRotation; 

    }

    
    private void Update () {
        
        if (listOfMovePoints.Count > 0 && !moving) {
            FigureOutDestination ();
            StartCoroutine ("PrepareToMove");
        }

        if (listOfMovePoints.Count > 0 && readyToMove) {
            moving = true;
            mayStillNeedToRotate = false;
            MakeMove (destination);
        }

        else if (listOfMovePoints.Count == 0 && moving) {
            moving = false;
            readyToMove = false;
            if (navObstacle){
                navAgent.enabled = false;
                navObstacle.enabled = true;
            }
            mayStillNeedToRotate = true;
        }
            
        if ((primaryTarget || secondaryTarget) && !interactCoroutineStarted) {
            StartCoroutine ("InteractWithTargets");
        }

        if (!moving && !considerRotationCoroutineStarted) {
            StartCoroutine ("ConsiderRotation");
        }

        if (moving) {
            if (!considerGivingUpCoroutineStarted) {
                StartCoroutine ("ConsiderGivingUpOnMovePoint");
            }
            if (!moveAndRotationModifierCoroutineStarted) {
                StartCoroutine ("ModifyMoveAndRotationSpeed"); 
            }

        }

        if (mayStillNeedToRotate && rotationAllowed) {
            TurnToLookTarget ();
        }

    }

    private void ResetMovement (){
        ClearMovement();
        interactCoroutineStarted = false; 
        considerRotationCoroutineStarted = false; 
        considerGivingUpCoroutineStarted = false;
        moveAndRotationModifierCoroutineStarted = false;
    }




    //*******************************************
    //BEGIN ADD MOVE POINT METHODS
    //*******************************************
    private void MoveToPointOnGround(Vector3 hitPoint) {
        secondaryTarget = null;
        primaryTarget = null;
        if (player && worldObject.currentlySelected && player.selectedObjects [0] == worldObject) {
            if (player.groupFormationManager) {
                //WorldObject hitWorldObject = hitObject.GetComponentInParent<WorldObject> (); //This was for making a special attack formation, but would need some work
                player.groupFormationManager.DetermineFormationGroup (null, hitPoint);
            }
        }
    }

    private IEnumerator SetMoveTrack (){
        while (player.dragSelectionHandlerScript.IsRightClickDragInputOccurring()) {
            RaycastHit hit;
            Ray ray = player.mainCamera.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (ray, out hit)) {
                if (hit.transform.name == "Ground" && player.selectedObjects[0] == worldObject) {
                    player.groupFormationManager.DetermineFormationGroup (null, hit.point);
                }
            }
            yield return new WaitForSeconds (0.1f);
        }
    }



    // WHILE AN INTERESTING METHOD, THIS WAS MESSING WITH THE NAVMESH AI; OVERRIDE, NON TARGET OBJECT MOVE POINTS, THERE IS NOW ONLY THE 'SMOOTHER', ...
    // WHCIH IS APPROPRIATE BECAUSE THEY ARE, I BELEIVE, ONLY EVER 'GROUND' MOVE POINTS. 
    // UPDATE: I BELIEVE I SHOULD RE-INCORPORATE THIS METHOD, ALONG WITH A CHECK WEHTER THE DERIVED POINT IS NAVAGATABLE, AND IF NOT, TO RESORT TO THE 
    // ORIGINAL; IT WAS ONLY A PROBLEM WHEN THESE POINTS WERE NOT REACHABLE. HOW THIS WILL WORK WHEN THERE IS CROWDING, I'M NOT SURE
    // UPDATE, I NOW REALIZE THAT HAVING THIS SHORTEN THE DISTANCE BY ONLY A LITTLE--WELL WITHIN THE UNREACABLE RANGE--IT MAKES THE 'NEAREST AVAILABLE POINT ON NAV MESH' ACTUALL REASONABLE AND MORE CONSISTENT.
    // IT MAY STILL CAUSE SOME PROBLEMS, THOUGH, SO I'LL HAVE TO KEEP AN EYE OUT.
    // UPDATE, INDEED IT DOES CAUSE SOME PROBLEMS. I NEED TO CHECK IF THE NEW MOVEPOINT IS NAVAGATABLE, OTHERWISE USE THE OLD ONE
    private Vector3 SlightlyShortenedDistance (GameObject moveTarget){
        Vector3 moveDirection = moveTarget.transform.position - transform.position;
        Vector3 unitDirectionVector = moveDirection.normalized;
        Vector3 desiredMovePosition = Vector3.zero; 
        desiredMovePosition = moveTarget.transform.position - unitDirectionVector * 0.1f;
        NavMeshHit hit;
        bool blocked = NavMesh.Raycast(transform.position, desiredMovePosition, out hit, NavMesh.AllAreas);
        Debug.DrawLine(transform.position, desiredMovePosition, blocked ? Color.red : Color.green);
        if (blocked){
            Debug.DrawRay(hit.position, Vector3.up, Color.red);
        }
        return desiredMovePosition;
    }
        
    //*******************************************
    //END ADD MOVE POINT METHODS
    //*******************************************

    //*******************************************
    //BEGIN PROCESS MOVE POINT METHODS
    //*******************************************

    private void FigureOutDestination(){
        if (listOfMovePoints.Count > 0) {
            destination = listOfMovePoints [0];
        }
    }

    private IEnumerator PrepareToMove (){ 
        if (navObstacle){
            if (navObstacle.enabled == true) { 
                navObstacle.enabled = false; 
                yield return null;
            }
        }
        navAgent.enabled = true;
        readyToMove = true; 
    }

    private void MakeMove(Vector3 moveToThis) {
        navAgent.SetDestination(moveToThis);
        CheckForArrival();
        FigureOutDestination();
    }


    private void CheckForArrival(){

        float movementSmoother = 0.0f;
        if (listOfMovePoints.Count > 1) {
            movementSmoother = 0.2f; 
        }
        Vector3 flatPosition = transform.position; 
        flatPosition.y = 0f; 
        Vector3 flatDestination = destination;
        flatDestination.y = 0f; 
        if (Vector3.Distance(flatPosition, flatDestination) <= (DetermineDesiredDistanceFromTarget() + movementSmoother)) {
            RemoveMovePoint (); 
        }
    }

    private float DetermineDesiredDistanceFromTarget(){
        float desiredDistance = 0.1f;
        return desiredDistance; 
    }

    private void RemoveMovePoint(){
        if (listOfMovePoints.Count != 0) {
            listOfMovePoints.Remove(listOfMovePoints[0]); 
        }
        if (listOfMovePoints.Count == 0) {
            unit.unitState.SelfExitState(RTS.EAnimation.Move);
            SetLookTarget (destination); 
        }
    }

    private void CheckToClearMovePointList (){ 
        if (listOfMovePoints.Count >0){
            if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift)) {
                if (dragSelectionHandler && dragSelectionHandler.IsRightClickDragInputOccurring () == false) {
                    ClearMoveList ();
                }
            }
        }
    }

    private IEnumerator ConsiderGivingUpOnMovePoint(){
        considerGivingUpCoroutineStarted = true;
        Vector3 positionCheck = transform.position; 
        yield return new WaitForSeconds (1.0f); 
        if (transform.position == positionCheck) {
            RemoveMovePoint ();
            Debug.Log (worldObject.name + " had to give up on move point"); 
        }
        considerGivingUpCoroutineStarted = false;
    }

    private IEnumerator ModifyMoveAndRotationSpeed (){
        moveAndRotationModifierCoroutineStarted = true;
        while (moving){
            Vector3 positionCheck = transform.position; 
            yield return new WaitForSeconds (0.01f);
            Vector3 movementVector = transform.position - positionCheck; 
            if (movementVector != Vector3.zero) {
                float rotationPercentage = 1.0f - Vector3.Angle (transform.forward, movementVector) / 180.0f;
                PenalizeMoveSpeed (rotationPercentage); 
                if (rotationPercentage < 0.80f) {
                    float step = paramManager.RotationSpeed * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards (transform.forward, movementVector, step, 0.0F);
                    transform.rotation = Quaternion.LookRotation (newDir);
                }
            }
        }
        moveAndRotationModifierCoroutineStarted = false;
    }

    private void PenalizeMoveSpeed (float rotationPercentage){
        if (rotationPercentage < 0.8f) {
            //rotationPercentage = 0.8f; //now that i have animations, it looks better without this buffer
        }
        navAgent.speed = paramManager.MoveSpeed * rotationPercentage;
    }
        



    //*******************************************
    //END PROCESS MOVE POINT METHODS
    //*******************************************


    //*******************************************
    //BEGIN ROTATION METHODS
    //*******************************************



    private void SetLookTarget (Vector3 destination){
        if (destination - transform.position != Vector3.zero) {
            destination.y = transform.position.y;
            finalDesiredRotation = Quaternion.LookRotation (destination - transform.position);
            mayStillNeedToRotate = true; 
        }
    }

    private IEnumerator ConsiderRotation (){
        considerRotationCoroutineStarted = true;
        if (primaryTarget && primaryTarget.activeInHierarchy) {
            SetLookTarget (primaryTarget.transform.position);
        }
        else {
            FaceNearestEnemyOrFollowTarget ();
        }
        yield return new WaitForSeconds (0.4f); 
        considerRotationCoroutineStarted = false; 
    }

    private void FaceNearestEnemyOrFollowTarget(){
        Collider nearestEnemyTarget = worldObject.GetNearestEnemyTargetColliderInRange(15.0f);
        if (nearestEnemyTarget) {
            WorldObject nearestEnemy = nearestEnemyTarget.GetComponentInParent<WorldObject> ();
            if (nearestEnemy) {
                SetLookTarget (nearestEnemy.transform.position); 
            }
        } else if (secondaryTarget) {
            SetLookTarget (secondaryTarget.transform.position);
        }
    }

    private void TurnToLookTarget() { //the demoninator was arriaved at via trial and error, and may need some further attention
        transform.rotation = Quaternion.Slerp(transform.rotation, finalDesiredRotation, paramManager.RotationSpeed * Time.deltaTime);
        if (Quaternion.Angle(transform.rotation, finalDesiredRotation) < 5.0f) {
            mayStillNeedToRotate = false;
        }
    }


    //*******************************************
    //END ROTATION METHODS
    //*******************************************


    //*******************************************
    //BEGIN INTERACTION METHODS
    //*******************************************


    private IEnumerator InteractWithTargets (){
        interactCoroutineStarted = true; 
        ClearMoveList ();

        if (primaryTarget && !IsInRangeOfPrimaryTarget(primaryTarget)) {
            AddMoveTarget (primaryTarget, primaryTarget.transform.position);
        }

        if (primaryTarget == null) {
            if (secondaryTarget && Vector3.Distance (transform.position, secondaryTarget.transform.position) >  secondaryInteractDistance) {
                AddMoveTarget (secondaryTarget, secondaryTarget.transform.position);
            }
        }

        if (listOfMovePoints.Count == 0) {
            unit.unitState.SelfExitState(RTS.EAnimation.Move);
        }
        yield return new WaitForSeconds (0.1f);
        interactCoroutineStarted = false;
    }

    

    //*******************************************
    //END INTERACTION METHODS
    //*******************************************






}