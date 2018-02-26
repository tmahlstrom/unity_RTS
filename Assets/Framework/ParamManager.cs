using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;


public class ParamManager : MonoBehaviour {

    [SerializeField]
    private int hitPoints = 42, maxHitPoints = 42, manaPoints = 24, maxManaPoints = 24; 
    [SerializeField]
    private bool isDead; 
    [SerializeField]
    private int minAttackDamage = 1, maxAttackDamage = 3;
    [SerializeField]
    private float attackCooldown = 2.0f, attackAnimationDuration = 1.0f, attackRange = 2.0f, moveSpeed = 3.0f, rotationSpeed = 3.0f; 
    [SerializeField]
    private GameObject attackEffectSelf, attackEffectOther, buildEffectSelf, spawnEffectSelf, spawnRayEffectSelf; 
    [SerializeField]
    private float spawnAnimationDuration = 1.0f; 
    [SerializeField]
    private int spawnPerCreate = 1, maxSpawnPopulation = 5;
    [SerializeField]
    private float buildSpeed = 30.0f, spawnCooldown = 1.0f, spawnerSpecialDurationFactor = 0.05f, spawnerSpecialEffectRate = 0.05f;  
    [SerializeField]
    private List<GameObject> buildablesList = new List<GameObject>(); 
    [SerializeField]
    private List<GameObject> spawnablesList = new List<GameObject>(); 
    [SerializeField]
    private float attackMoveVelocity = 2.0f, tryingToAttackMoveVelocity = 5.0f, patrolVelocity = 400f, aggroRange = 5.0f, patrolRadius = 2.0f, degreeOfRandomMovement = 1.0f; 
    [SerializeField]
    private bool playerOwned;
    [SerializeField]
    private int productionCost;
    [SerializeField]
    private int rewardForKill; 
    [SerializeField]
    private AudioClip attackAudioClip, buildAudioClip, dieAudioClip, specialAudioClip, spawnAudioClip; 



    public int AttackDamage {get{return Random.Range (minAttackDamage, maxAttackDamage);}}
    public float AttackCooldown {get{return attackCooldown;}}
    public float AttackAnimationDuration {get{return attackAnimationDuration;}}
    public float AttackRange {get{return attackRange;}}
    public GameObject AttackEffectSelf {get{return attackEffectSelf;}}
    public GameObject AttackEffectOther {get{return attackEffectOther;}}
    public float AttackMoveVelocity {get{return attackMoveVelocity;}}
    public float ApproachMoveVelocity {get{return tryingToAttackMoveVelocity;}}
    public float PatrolVelocity {get{return patrolVelocity;}}
    public float AggroRange {get{return aggroRange;}}
    public float PatrolRadius {get{return patrolRadius;}}
    public float DegreeOfRandomMovement {get{return degreeOfRandomMovement;}}


    public int HitPoints { get {return hitPoints;}}
    public int MaxHitPoints { get {return maxHitPoints;}}
    public int ManaPoints { get {return manaPoints;}}
    public int MaxManaPoints { get {return maxManaPoints;}}
    public bool IsDead { get {return isDead;}}

    public float HealthPercentage {get {if (maxHitPoints > 0){return ((float)hitPoints / (float)maxHitPoints);} else {return 0.0f;}}}
    public float ManaPercentage {get {if (maxManaPoints > 0){return ((float)manaPoints / (float)maxManaPoints);} else {return 0.0f;}}}

    public float SpawnAnimationDuration{get{return spawnAnimationDuration;}}
    public int SpawnPerCreate { get {return spawnPerCreate;}}
    public int MaxSpawnPopulation {get {return maxSpawnPopulation;}}
    public float SpawnCooldown {get {return spawnCooldown;}}
    public float SpawnerSpecialDurationFactor { get{return spawnerSpecialDurationFactor;}}
    public float SpawnerSpecialEffectRate { get{return spawnerSpecialEffectRate;}}
    public GameObject SpawnEffectSelf {get{return spawnEffectSelf;}}
    public GameObject SpawnRayEffectSelf {get{return spawnRayEffectSelf;}}


    public float BuildSpeed{get{return buildSpeed;}}
    public GameObject BuildEffectSelf {get{return buildEffectSelf;}}

    public float MoveSpeed {get{return moveSpeed;}}
    public float RotationSpeed {get{return rotationSpeed;}}

    public List<GameObject> BuildablesList { get{return buildablesList;}}
    public int ProductionCost {get{return productionCost;}}
    public int RewardForKill {get{return rewardForKill;}}
    public List<GameObject> SpawnablesList { get{return spawnablesList;}}
    public AudioClip AttackAudioClip {get{return attackAudioClip;}}
    public AudioClip BuildAudioClip {get{return buildAudioClip;}}
    public AudioClip DieAudioClip {get{return dieAudioClip;}}
    public AudioClip SpecialAudioClip {get{return specialAudioClip;}}
    public AudioClip SpawnAudioClip {get{return spawnAudioClip;}}


    public bool PlayerOwned{get{return playerOwned;} set {playerOwned = value;}}

    //*******************************
    //BEGIN DELEGATES
    //*******************************

    public delegate void HPChangeProtocol();
    public event HPChangeProtocol OnHPChangeDelegate; 



    //*******************************
    //END DELEGATES
    //*******************************

    //*******************************
    //BEGIN RUNTIME SETTERS
    //*******************************
    private void Awake(){
        Player player = GetComponentInParent<Player>();
        if (player){
            playerOwned = true; 
        }
    }

    //*******************************
    //END RUNTIME SETTERS
    //*******************************

    //*******************************
    //BEGIN MODIFICATION METHODS
    //*******************************


    public void HPmod(int damage) {
        hitPoints -= damage;
        if (isDead && hitPoints > 0){
            isDead = false; 
        }
        if (hitPoints > maxHitPoints) {
            hitPoints = maxHitPoints; 
        }
        if (hitPoints <= 0) {
            hitPoints = 0;
            isDead = true;
        }
        if (OnHPChangeDelegate != null){
            OnHPChangeDelegate();
        }
    }

    public void MPmod(int change) {
        manaPoints += change;
        if (manaPoints > maxManaPoints) {
            manaPoints = maxManaPoints; 
        }
        if (manaPoints <= 0) {
            manaPoints = 0;
        }
    }
    //*******************************
    //END MODIFICATION METHODS
    //*******************************


    //*******************************
    //BEGIN COMPLEX GETTERS
    //*******************************

    public float GetSpawnerSpecialDuration(){
        return (maxManaPoints * spawnerSpecialDurationFactor);
    }

    //*******************************
    //END COMPLEX GETTERS
    //*******************************

}
