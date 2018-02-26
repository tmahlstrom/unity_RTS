using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconControl3 : MonoBehaviour {

	public UnityEngine.UI.Image icon1Image; 

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void UpdateImage(Sprite passedSprite){
		icon1Image = gameObject.GetComponent<Image> (); 
		icon1Image.sprite = passedSprite;
	}

}