using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class CursorManager : MonoBehaviour {

	private CursorState activeCursorState;


	public Texture2D defaultCursor;
	public Texture2D[] allyCursors;
	public Texture2D[] enemyCursors;
	public Texture2D transparentCursor; 
	Texture2D newCursorTexture = null;
	IEnumerator coroutine;
	private Vector2 cursorPositionModifier; 

	private bool cursorChangeIsEnabled; 

	void Awake (){
		cursorChangeIsEnabled = true; 
		cursorPositionModifier = new Vector2 (10f, 10f); 
	}
		
	void Start() { 
		SetMouseCursor (CursorState.DefaultCursor);
	}


	public void DisableCursor(){
		RegisterCursorState (CursorState.TransparentCursor); 
		cursorChangeIsEnabled = false; 
	}

	public void EnableCursor(){
		cursorChangeIsEnabled = true; 
	}

	private void SetMouseCursor(CursorState newCursorState) {
		switch (newCursorState) {
		case CursorState.DefaultCursor:
			newCursorTexture = defaultCursor;
			break;
		case CursorState.TransparentCursor:
			newCursorTexture = transparentCursor;
			break;
		case CursorState.AllyCursor:
			coroutine = CycleCursors (allyCursors, CursorState.AllyCursor);
			if (coroutine != null) {
				StartCoroutine (coroutine); 
			}
			break;
		case CursorState.EnemyCursor:
			coroutine = CycleCursors (enemyCursors, CursorState.EnemyCursor);
			if (coroutine != null) {
				StartCoroutine (coroutine); 
			}
			break;
		}
		Cursor.SetCursor (newCursorTexture, cursorPositionModifier, CursorMode.Auto); 
	}


	public void RegisterCursorState(CursorState maybeNewState) {
		if(cursorChangeIsEnabled && activeCursorState != maybeNewState) {
			activeCursorState = maybeNewState;
			SetMouseCursor (maybeNewState);
		}
	}

	private IEnumerator CycleCursors(Texture2D[] cursorArray, CursorState cursorState){
		int currentFrame = 0;
		int cycleDirection = 1; 
		while (activeCursorState == cursorState) {
			newCursorTexture = cursorArray [currentFrame];
			Cursor.SetCursor (newCursorTexture, cursorPositionModifier, CursorMode.Auto);
			yield return new WaitForSeconds (0.08f);
			currentFrame += cycleDirection;
			if (currentFrame >= cursorArray.Length) {
				currentFrame = cursorArray.Length - 1;
				cycleDirection = -1; 
			}
			if (currentFrame < 0) {
				currentFrame = 0;
				cycleDirection = 1; 
			}
		} 
	}
}