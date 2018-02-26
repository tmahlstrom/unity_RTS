using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using RTS;

public class DragSelectionHandler: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	[SerializeField]
	Image selectionBoxImage;

	Vector2 startPosition;
	Rect selectionRect;
	public List<WorldObject> boxSelectedObjects;
	private bool rightClickDragInputOccurring; 
	private bool leftClickDragInputOccurring; 
	Player player;
	Vector2 originalClickLocation; 
	Vector2 originalClickLocationModifier; 
	public Camera dragCamera;


	 
	void Start () {
		player = GetComponentInParent<Player>();
		rightClickDragInputOccurring = false;
	}

	void Update(){
		if (Input.GetMouseButtonDown(0)){
			originalClickLocation = Input.mousePosition; 
		}
			
	}

	public bool IsRightClickDragInputOccurring () {
		if (rightClickDragInputOccurring) {
			return true;
		} else {
			return false;
		}
	}

	public bool IsLeftClickDragInputOccurring () {
		if (leftClickDragInputOccurring) {
			return true;
		} else {
			return false;
		}
	}

		
	public void OnBeginDrag(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			leftClickDragInputOccurring = true; 
			selectionBoxImage.gameObject.SetActive (true);
			startPosition = originalClickLocation;
			selectionRect = new Rect ();
		}
		if (eventData.button == PointerEventData.InputButton.Right) {
			rightClickDragInputOccurring = true;
			if (player.selectedObjects.Count != 0 && player.selectedObjects [0] != null) {
				if (player.selectedObjects.Count != 0 && player.selectedObjects [0].GetComponent< Mover > () != null) {
					foreach (WorldObject unit in player.selectedObjects) {
						if (unit.player == player && unit != null) {
							unit.GetComponent< Mover > ().SetMoveTrackStarter ();
						}
					}
				}
			}
		}
	}

	public void OnDrag(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			if (eventData.position.x < startPosition.x) {
				selectionRect.xMin = eventData.position.x;
				selectionRect.xMax = startPosition.x;
			} else {
				selectionRect.xMin = startPosition.x;
				selectionRect.xMax = eventData.position.x;
			}

			if (eventData.position.y < startPosition.y) {
				selectionRect.yMin = eventData.position.y;
				selectionRect.yMax = startPosition.y;
			} else {
				selectionRect.yMin = startPosition.y;
				selectionRect.yMax = eventData.position.y;
			}

			selectionBoxImage.rectTransform.offsetMin = selectionRect.min;
			selectionBoxImage.rectTransform.offsetMax = selectionRect.max;
		}
		if (eventData.button == PointerEventData.InputButton.Right) {
			
		}

	}

	public void OnEndDrag(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			selectionBoxImage.gameObject.SetActive (false);
			boxSelectedObjects.Clear ();
			foreach (WorldObject worldObject in StageManager.Instance.allSelectables) {
				if (worldObject && (selectionRect.Contains (Camera.main.WorldToScreenPoint (worldObject.transform.position))) || SelectionRectOverlapsWithWORect (worldObject)) {
					boxSelectedObjects.Add (worldObject);
				}
			}
			player.ConsiderBoxSelectedObjects ();
			leftClickDragInputOccurring = false;
		}

		if (eventData.button == PointerEventData.InputButton.Right) {
			rightClickDragInputOccurring = false;
		}
	}

	private bool SelectionRectOverlapsWithWORect(WorldObject targetWO){
		Selector selector = targetWO.GetComponentInChildren<Selector> (); 
		if (selector != null) {
			Rect selectorRect = new Rect (targetWO.transform.position.x, targetWO.transform.position.y, selector.selectorSize, selector.selectorSize);
			Vector3 woScreenPosition = dragCamera.WorldToScreenPoint (targetWO.transform.position);
			selectorRect.center = woScreenPosition; 
			if (selectionRect.Overlaps (selectorRect)){
				return true; 
			}
		}
		return false;
	}


	/// <summary>
	/// below function is not currently in use, but probably will need to be in order to do the 'tantative selection indicator' for the drag selection
	/// </summary>
	private void UpdateSelectorRects (WorldObject targetWO){
		foreach (WorldObject worldObject in StageManager.Instance.allSelectables) {
			Selector selector = worldObject.GetComponentInChildren<Selector> (); 
			if (selector) {
				Rect selectorRect = new Rect (targetWO.transform.position.x, targetWO.transform.position.y, selector.selectorSize, selector.selectorSize);
				Vector3 woScreenPosition = dragCamera.WorldToScreenPoint (targetWO.transform.position);
				selectorRect.center = woScreenPosition; 
				if (selectionRect.Overlaps (selectorRect)) {
					 
				}
			}
		}
	}

}

