using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class Attacker : MonoBehaviour, IAttacker {



    //*******************************************
    //BEGIN INTERFACE 
    //*******************************************

    public Collider attackTargetCollider; 

    public bool CheckIfReadyToBeginAttacking(){
        if (AttackCooldownComplete() && attackProcessStarted == false){
            if (unit.attacker.attackTargetCollider){
                return true; 
            }
        }
        return false; 
        
    }

    public void BeginAttackProcess(){
        attackProcessStarted = true; 
    }

    public void AttackIterationComplete(){
        attackProcessStarted = false; 
        currentAttackRechargeTime = 0.00f;
    }


    public bool AttackProcessStarted(){
        return attackProcessStarted; 
    }

    

    public virtual void AttackExecution(){ //called by animator
        if (attackTargetCollider) {
            WorldObject attackWO = attackTargetCollider.GetComponentInParent<WorldObject>();
            if (attackWO){
                InflictDamage(attackWO);
                EnemyEffect(attackWO);
                SelfEffect();
                AudioEffect();
            } else {print(worldObject.name + " appears to have attacked a collider with unreachable WO");}
        }
    }

    public void ResetAttacker(){
        attackTargetCollider = null;
        currentAttackRechargeTime = 0.00f;        
        attackProcessStarted = false;
    }


    public void AttackThisCol(Collider targetCol){
        if (targetCol != attackTargetCollider){
            ResetAttacker();
        }
        this.attackTargetCollider = targetCol;
    }

    public bool CheckForRepeatCommand(Collider hitCollider){
        if (attackTargetCollider && hitCollider == attackTargetCollider){
            return true;
        }
        return false; 
    }

    //*******************************************
    //END INTERFACE 
    //*******************************************

    private WorldObject worldObject; 
    private Unit unit; 
    private ParamManager paramManager; 



    private float currentAttackRechargeTime = 0.0f;

    private bool attackProcessStarted; 

    private void Awake() {
        worldObject = GetComponent<WorldObject>(); 
        unit = GetComponent<Unit>();
        paramManager = GetComponent<ParamManager>(); 
    }

    private void Start(){
    }


    private void Update() {
        currentAttackRechargeTime += Time.deltaTime;
    }

    //*******************************************
    //BEGIN INITIALIZATION METHODS
    //*******************************************

    private bool AttackCooldownComplete() {
        if (currentAttackRechargeTime > paramManager.AttackCooldown) {
            return true;
        } else {return false;}
    }


    //*******************************************
    //END INITIALIZATION METHODS
    //*******************************************



    //*******************************************
    //BEGIN EXECUTION AND WRAP-UP METHODS
    //*******************************************







    private void SelfEffect(){
        AttackEffectSelfPositionSetter effectPositioner = GetComponentInChildren<AttackEffectSelfPositionSetter>();
        if (effectPositioner && paramManager.AttackEffectSelf){
            GameObject effect = Instantiate(paramManager.AttackEffectSelf, effectPositioner.transform.position, effectPositioner.transform.rotation);
            effect.transform.SetParent (ResourceManager.GetDynamicObjects());
        }
    }

    private void InflictDamage (WorldObject attackWO){
        attackWO.TakeDamage(paramManager.AttackDamage, attackWO.transform.position, worldObject);
    }

    private void EnemyEffect(WorldObject attackWO){
        AttackEffectOtherPositionSetter effectPositioner = attackWO.GetComponentInChildren<AttackEffectOtherPositionSetter>();
        if (effectPositioner && paramManager.AttackEffectOther){
            GameObject effect = Instantiate(paramManager.AttackEffectOther, effectPositioner.transform.position, effectPositioner.transform.rotation);
            effect.transform.SetParent (ResourceManager.GetDynamicObjects());
        }
    }

    private void AudioEffect(){
        if (worldObject.audioSource){
            if (paramManager.AttackAudioClip){
                AudioManager.Instance.Play(paramManager.AttackAudioClip, worldObject.audioSource);
                // worldObject.audioSource.clip = paramManager.AttackAudioClip; 
                // worldObject.audioSource.Play(); 
            }
        }
    }

    //*******************************************
    //END EXECUTION AND WRAP-UP METHODS
    //*******************************************





}
