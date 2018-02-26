using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class Vine : Building {


    protected List<Fruit> fruitList = new List<Fruit>(); 
    protected int fruitVisibilityParameter;
    protected Coroutine fruitCoroutine; 
    protected WaitForSeconds shortWait = new WaitForSeconds(0.5f); 

    protected override void Start() {
        base.Start();
        buildingSpaceIsFree = false; //buildings that need to go on a plant base should have a default value of false, unlike other buildings
        CreateFruitList();
        DetermineFruitVisibilityParameter(); 
        paramManager.OnHPChangeDelegate += DetermineFruitVisibility;
        if (autoComplete){
            StageManager.Instance.allSelectables.Add(this);
            buildingState = new IdleStateB(building, false);
            isFinishedBuilding = true; 
		    GiveThisWorldInfoCanvasStats ();
            ReawakenWO();
            worldObject.TakeDamage(-paramManager.MaxHitPoints, transform.position, worldObject); 
        }
    }

    protected virtual void CreateFruitList(){
        Fruit[] fruitArray = GetComponentsInChildren<Fruit>();
        foreach (Fruit fruit in fruitArray){
            fruitList.Add(fruit);
        }
    }

    protected virtual void DetermineFruitVisibilityParameter(){
        if (fruitList.Count>0){
            fruitVisibilityParameter = paramManager.MaxHitPoints / fruitList.Count;
        } else {print(gameObject.name + " has no fruit!");} 
    }


    private void DetermineFruitVisibility(){
        for (int i = 0; i < fruitList.Count; i++){
            if (paramManager.HitPoints >= i * fruitVisibilityParameter){
                fruitList[i].gameObject.SetActive(true);
            } else {
                fruitList[i].gameObject.SetActive(false);
            }
        }
    }

    public override void RememberWhoMadeYou(WorldObject creator){
        this.creator = creator; 
        if (creator){
            SetBuildingMaterial(creator.player.notAllowedMaterial);
        }
    }

    protected override void ReawakenWO(){
        base.ReawakenWO(); 
        int layerMask = 1 << 23;
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(.5f,.5f,.5f), transform.rotation, layerMask); 
        foreach(Collider col in cols){
            PlantBase pb = col.GetComponent<PlantBase>();
            if (pb){
                pb.spaceAvailable = false; 
            }
        }
    }



    protected override void OnTriggerStay (Collider other){
        if (!constructionHasBegun && buildingSpaceIsFree == false) {
            PlantBase plantBase = other.GetComponent<PlantBase>(); 
            if (plantBase && plantBase.spaceAvailable){
                buildingSpaceIsFree = true;
                if (creator) {
                    SetBuildingMaterial(creator.player.allowedMaterial);
                }
            }
        }
    }

    // protected override void OnTriggerStay(Collider other){
        
    // }

    protected override void OnTriggerExit (Collider other){
        if (!constructionHasBegun && buildingSpaceIsFree == true) {
            PlantBase plantBase = other.GetComponent<PlantBase>(); 
            if (plantBase){
                buildingSpaceIsFree = false;
                if (creator) {
                    SetBuildingMaterial(creator.player.notAllowedMaterial);
                }
            }
        }
    }



}
