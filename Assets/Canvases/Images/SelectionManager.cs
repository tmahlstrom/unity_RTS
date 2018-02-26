using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SelectionManager : MonoBehaviour {

    public bool halo;
    public bool orb; 
    public bool range;
    public bool hoverEffect; 

    private bool hoverEffectStarted; 

    private float hoverEffectFloat; 



    private WorldObject worldObject; 
   	private SelectionIndicatorHalo selectionIndicatorHalo; 
    private SelectionIndicatorOrb selectionIndicatorOrb; 
    private RangeIndicator rangeIndicator; 
    private SelectorWhenUp selectorWhenUp;
    private SelectorWhenDown selectorWhenDown;
    private Selector selector; 
    private float hoverIntensity = 0.4f; 


	private void Awake (){
        worldObject = GetComponentInParent<WorldObject>();
		selectionIndicatorHalo = GetComponentInChildren<SelectionIndicatorHalo>(true); 
        //selectionIndicatorOrb = GetComponentInChildren<SelectionIndicatorOrb> (true);
        rangeIndicator = GetComponentInChildren<RangeIndicator>(true);

        selector = GetComponentInChildren<Selector>(); 
        selectorWhenUp = GetComponentInChildren<SelectorWhenUp>();
        selectorWhenDown = GetComponentInChildren<SelectorWhenDown>();
	}

    private void Start(){
        worldObject.OnWorldObjectDeathDelegate += SelectorWhenDown;
        worldObject.OnWorldObjectReviveDelegate += SelectorWhenUp; 
        SelectorWhenUp(); 
    }

    private void Update(){
        if (halo && selectionIndicatorHalo){
            if (worldObject.currentlySelected && selectionIndicatorHalo.gameObject.activeSelf == false) {
                selectionIndicatorHalo.gameObject.SetActive(true);
            } else if (!worldObject.currentlySelected && selectionIndicatorHalo.gameObject.activeSelf == true){
                if (!hoverEffectStarted){
                    selectionIndicatorHalo.gameObject.SetActive(false);
                }
            }
        }
        if (orb && selectionIndicatorOrb){
            if (worldObject.currentlySelected && selectionIndicatorOrb.gameObject.activeSelf == false) {
                selectionIndicatorOrb.gameObject.SetActive(true);
            } else if (!worldObject.currentlySelected && selectionIndicatorOrb.gameObject.activeSelf == true){
                selectionIndicatorOrb.gameObject.SetActive(false);
            }
        }
        if (range && rangeIndicator){
            if (worldObject.currentlySelected && rangeIndicator.gameObject.activeSelf == false) {
                rangeIndicator.gameObject.SetActive(true);
            } else if (!worldObject.currentlySelected && rangeIndicator.gameObject.activeSelf == true){
                rangeIndicator.gameObject.SetActive(false);
            }
        }

        if (hoverEffect){
            if (worldObject.currentlySelected && hoverEffectStarted){
                StopHoverEffect(); 
            } else if (!worldObject.currentlySelected){
                if (worldObject.HoverEffect && !hoverEffectStarted){
                    StartHoverEffect();
                }
                else if (!worldObject.HoverEffect && hoverEffectStarted){
                    StopHoverEffect(); 
                }
            }
        }
    }

    public void TargetedEffect(){
        StartCoroutine(FlashSelection()); 
    }

    private IEnumerator FlashSelection(){
        if (selectionIndicatorHalo){
            selectionIndicatorHalo.gameObject.SetActive(true);
            Light light = selectionIndicatorHalo.GetComponent<Light>();
            float toMaxIntensity = 1 - hoverIntensity; 
            if (light){
                float blinkRate = 0.05f;
                for (float t = 0f; t < blinkRate; t += Time.deltaTime) {
                    light.intensity = hoverIntensity + ((t / blinkRate) * toMaxIntensity);
                    yield return null;
                }
                for (float t = 0f; t < blinkRate; t += Time.deltaTime) {
                    light.intensity = hoverIntensity - ((t / blinkRate) * toMaxIntensity);
                    yield return null;
                }  
                for (float t = 0f; t < blinkRate; t += Time.deltaTime) {
                    light.intensity = hoverIntensity + ((t / blinkRate) * toMaxIntensity);
                    yield return null;
                }   
                for (float t = 0f; t < blinkRate; t += Time.deltaTime) {
                    light.intensity = hoverIntensity - ((t / blinkRate) * toMaxIntensity);
                    yield return null;
                }       
            }
            if (!worldObject.currentlySelected && ! worldObject.HoverEffect){
                selectionIndicatorHalo.gameObject.SetActive(false);
            } else if (!worldObject.currentlySelected){
                light.intensity = hoverIntensity; 
            }
        }
    }

    private void StartHoverEffect(){
        hoverEffectStarted = true; 
        if (selectionIndicatorHalo){
            Light light =selectionIndicatorHalo.gameObject.GetComponent<Light>();
            if (light){
                light.intensity = hoverIntensity;
            }
        }
        selectionIndicatorHalo.gameObject.SetActive(true);
    }

    private void StopHoverEffect(){

        if (selectionIndicatorHalo){
            Light light =selectionIndicatorHalo.gameObject.GetComponent<Light>();
            if (light){
                light.intensity = 1f;
            }
        }
        if (!worldObject.currentlySelected){
            selectionIndicatorHalo.gameObject.SetActive(false);
        }
        hoverEffectStarted = false; 
    }

    protected virtual void SelectorWhenUp(){
        if (selectorWhenUp){
            selectorWhenUp.gameObject.SetActive(true);
        }
        if (selectorWhenDown){
            selectorWhenDown.gameObject.SetActive(false);
        }
    }

    protected virtual void SelectorWhenDown(){
        if (selectorWhenDown){
            selectorWhenDown.gameObject.SetActive(true);
        }
        if (selectorWhenUp){
            selectorWhenUp.gameObject.SetActive(false);
        }
    }

    public void EnableSelection(){
        selector.gameObject.SetActive(true); 
    }

    public void DisableSelection(){
        selector.gameObject.SetActive(false); 
    }

}