using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS; 

public class ItemDropper : MonoBehaviour {

	GameObject drop;
	WorldObject worldObject; 

	private void Awake(){
		worldObject = GetComponent<WorldObject> ();
	}

	private void OnEnable (){
		worldObject.OnWorldObjectDeathDelegate += DetermineItemDrop;
	}

	public void OnDisable (){
		worldObject.OnWorldObjectDeathDelegate -= DetermineItemDrop; 
	}

	private void DetermineItemDrop (){
		if (worldObject.enemyManager) {
			DropItem (ItemType.OrganicMatter); 
		}

		if (worldObject.player) {
			//DropItem (ItemType.Corpse);
		}
	}

	public void DropItem(ItemType item){
		switch (item) {
		case ItemType.OrganicMatter:
			drop = Instantiate (ResourceManager.GetItem ("OrganicMatter"), transform.position, transform.rotation);
			break;
		case ItemType.Corpse:
			drop = Instantiate (ResourceManager.GetItem ("Corpse"), transform.position, transform.rotation);
			drop.GetComponent<Corpse> ().MarkThisCorpse (this.gameObject); 
			break;
		}
		drop.transform.SetParent (ResourceManager.GetDynamicObjects ());
	}


}
