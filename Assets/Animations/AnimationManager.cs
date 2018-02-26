using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS; 

public class AnimationManager : MonoBehaviour {

	private Animator animator;
    private WorldObject worldObject;
    private ParamManager paramManager; 

    private float specialAttackAnimationDuration;
    private float attackAnimationDuration; 
    private float spawnAnimationDuration; 

    void Awake() {
        animator = GetComponent<Animator>();
        worldObject = GetComponent<WorldObject>();
        paramManager = GetComponent<ParamManager>();
    }

    private void Start() {
        GetClipInformation(); 
    }

    private void GetClipInformation(){
        if (animator){
            RuntimeAnimatorController runAnim = animator.runtimeAnimatorController;
            if (runAnim){
                for (int i = 0; i < runAnim.animationClips.Length; i++){
                    if (runAnim.animationClips[i].name == "SpecialAttack"){
                        specialAttackAnimationDuration = runAnim.animationClips[i].length;
                    }
                    if (runAnim.animationClips[i].name == "Attack"){
                        attackAnimationDuration = runAnim.animationClips[i].length;
                    }
                    if (runAnim.animationClips[i].name == "Spawn"){
                        spawnAnimationDuration = runAnim.animationClips[i].length;
                    }
                }
            }
        }
    }


    public void ChangeAnimation(RTS.EAnimation action, bool toggle){
        if (!animator){
            return; 
        }
        switch (action) {

            case EAnimation.Idle:
                if (toggle == true){
                    animator.SetFloat("MoveSpeed", 0);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Spawn", false);
                    animator.SetBool("Build", false);
                    animator.SetBool("Die", false);
                    animator.SetBool("Revive", false);
                }
                break; 

            
            case EAnimation.Move:
                if (toggle == true) {
                    animator.SetFloat("MoveSpeed", paramManager.MoveSpeed);
                } else {
                    animator.SetFloat("MoveSpeed", 0);
                }
               break;

            case EAnimation.Rotate:
                if (toggle == true) {
                    animator.SetBool("Rotate", true);
                } else {
                    animator.SetBool("Rotate", false);
                }
                break;


            case EAnimation.Attack:
                if (toggle == true) {
                    float animationScaleFactor = (attackAnimationDuration) / (paramManager.AttackAnimationDuration);
                    animator.SetFloat("AttackSpeed", animationScaleFactor);
                    animator.SetBool("Attack", true);
                } else {
                    animator.SetBool("Attack", false);
                }
                break;

            case EAnimation.Spawn:
                if (toggle == true) {
                    float animationScaleFactor = (spawnAnimationDuration) / (paramManager.SpawnAnimationDuration);
                    animator.SetFloat("SpawnSpeed", animationScaleFactor);
                    animator.SetBool("Spawn", true);
                    if (spawnAnimationDuration < 0.01f){
                        SpawnExecutionEvent();
                    }
                        
                } else {
                    animator.SetBool("Spawn", false);
                }
                break;

            case EAnimation.SpawnerSpecial:
                if (toggle == true) {
                    float animationScaleFactor = (specialAttackAnimationDuration) / (paramManager.GetSpawnerSpecialDuration());
                    animator.SetFloat("SpecialAttackSpeed", animationScaleFactor);
                    animator.SetBool("Special", true);
                } else {
                    animator.SetBool("Special", false); 
                }
                break;

            case EAnimation.Build:
                if (toggle == true){
                    animator.SetBool("Build", true);
                } else {
                    animator.SetBool("Build", false);
                }
                break;

            case EAnimation.Die:
                if (toggle == true){
                    animator.SetBool("Die", true);
                } else {
                    animator.SetBool("Die", false);
                }
                break;

            case EAnimation.Revive:
                if (toggle == true){
                    animator.SetBool("Revive", true);
                } else {
                    animator.SetBool("Revive", false);
                }
                break;

            case EAnimation.Sit:
                if (toggle == true){
                    animator.SetBool("Sit", true);
                } else {
                    animator.SetBool("Sit", false);
                }
                break;
        }
    }


    //*******************************
    //BEGIN ANIMATION EVENT NOTIFIERS
    //*******************************


    private void AttackAnimationClimax (){
        worldObject.AnimationClimaxPoint(RTS.EAnimation.Attack);
    }

    public void AttackAnimationFinish(){ //this could be used (with some work) to create a continuous animation
        worldObject.AnimationEndPoint(RTS.EAnimation.Attack);
    }

    private void SpawnExecutionEvent(){
        worldObject.AnimationClimaxPoint(RTS.EAnimation.Spawn);
    }

    private void SpawnAnimationEnd(){
        worldObject.AnimationEndPoint(RTS.EAnimation.Spawn);
    }

    public void SpecialAttackStartPoint(){
        worldObject.AnimationStartPoint(RTS.EAnimation.SpawnerSpecial);
    }

    public void SpecialAttackEndPoint(){
        worldObject.AnimationEndPoint(RTS.EAnimation.SpawnerSpecial);
    }

    public void ReviveClimaxEvent(){
        worldObject.AnimationClimaxPoint(RTS.EAnimation.Revive);
    }

    public void BuildAnimationClimax(){
        worldObject.AnimationClimaxPoint(RTS.EAnimation.Build);
    }

    //*******************************
    //END ANIMATION EVENT NOTIFIERS
    //*******************************

}
