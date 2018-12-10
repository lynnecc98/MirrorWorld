using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.AnimatedValues;
#endif

[ExecuteInEditMode]
public class Actor : MonoBehaviour
{


	public string actorName;
	public Transform actorTransform;

	public Vector3 pivotRotationCenter;
	public float topBalloonXdistance, topBalloonYdistance, bottomBalloonXdistance, bottomBalloonYdistance;


	public Animator animator;
	public List<string> usedAnimationStateNames;

	public const float periodForFades = 0.1f;

	//
	// ComicsTalk
	//
	public RectTransform comicsPanelRectTransform; // Set by PanelEnterExit
	public Camera actorTransformCamera;
	public Producer producer;

	[HideInInspector]
	public Director director;

	void Awake() {
		director = GetComponent<Director> ();
	}


	public IEnumerator parse(Line line)
	{
		float startTime = Time.time;
		yield return new WaitForSeconds(float.Parse(line["INTERVAL"]));

		if (line ["ANIMATION"] != "") {
			string stateName = line ["ANIMATION"].Replace ("_-_-", " ");
			animator.SetBool ("cutscene", true);
			animator.CrossFade( stateName, 0.25f, 0 );
		}

		switch (line["ACTION"])
		{
		case "comicsTalk":
			yield return StartCoroutine (parseComicsTalk (line));
			break;
		case "subtitles":
			yield return StartCoroutine (parseSubtitle (line));
			break;
		case "enter":
			if (actorTransform.gameObject.activeSelf) {
				Debug.LogWarning ("Enter cutscene line: " + actorName + " is already enabled.");
			} else {
				actorTransform.gameObject.SetActive (true);
				CanvasGroup canvasGroup = actorTransform.GetComponent<CanvasGroup> ();
				if (canvasGroup) {
					yield return StartCoroutine(Fade.In (canvasGroup.gameObject, (line["FADETIME"] == "") ? periodForFades : float.Parse(line["FADETIME"])));
				}
			}
			break;
		case "exit":
			if (!actorTransform.gameObject.activeSelf) {
				Debug.LogWarning ("Exit cutscene line: " + actorName + " is already disabled.");
			} else {
				CanvasGroup canvasGroup = actorTransform.GetComponent<CanvasGroup> ();
				if (canvasGroup) {
					yield return StartCoroutine(Fade.Out (canvasGroup.gameObject, (line["FADETIME"] == "") ? periodForFades : float.Parse(line["FADETIME"])));
				}
				actorTransform.gameObject.SetActive (false);
			}
			break;
		case "goTo":
			yield return StartCoroutine(goTo(float.Parse(line["X"]), float.Parse(line["Y"]), float.Parse(line["Z"]),
				float.Parse(line["SPEED"]), bool.Parse(line["DESACCEL"]) , bool.Parse(line["KEEPORIGINALROTATION"]) ));
			break;
		case "goToAccelerating":
			yield return StartCoroutine(goToAccelerating(float.Parse(line["X"]), float.Parse(line["Y"]), float.Parse(line["Z"]),
				float.Parse(line["SPEED"]), float.Parse(line["ACCELERATION"]) , bool.Parse(line["KEEPORIGINALROTATION"])));
			break;
		case "goToTeleporting":
			yield return StartCoroutine(goToTeleporting(float.Parse(line["X"]), float.Parse(line["Y"]), float.Parse(line["Z"])));
			break;
		case "rotateTowards":
			yield return StartCoroutine(rotateTowards(float.Parse(line["X"]), float.Parse(line["Y"]), float.Parse(line["Z"]), 
				float.Parse(line["SPEED"])));
			break;
		case "rotateBy":
			yield return StartCoroutine(rotateBy(float.Parse(line["ANGLE"]), float.Parse(line["SPEED"])));
			break;
		case "rotateTo":
			yield return StartCoroutine(rotateTo(float.Parse(line["ANGLE"]), float.Parse(line["SPEED"])));
			break;
		}


		if (line ["ANIMATION"] != "") {
			animator.CrossFade(usedAnimationStateNames [0], 0.25f, 0); // Plays default animation
			animator.SetBool ("cutscene", false);
		}

	}


	//
	// Talk
	//

	//
	// Talk -> Subtitles
	//

	public IEnumerator parseSubtitle(Line line)
	{
		Producer producer = GetComponentInParent<Producer> ();
		producer.subtitlesHeader.text = actorName;
		Text subtitlesText = producer.subtitlesText;

		subtitlesText.text = line ["TEXT"];

		CanvasGroup textCanvasGroup = subtitlesText.GetComponentInParent<CanvasGroup> ();
		textCanvasGroup.GetComponent<RectTransform>().pivot = getSubtitlesPivot(line);

		yield return StartCoroutine(Fade.In(textCanvasGroup.gameObject, periodForFades));

		if (line["BLINKRATE"] != "") {
			StartCoroutine (blinkSubtitles (line, subtitlesText));
		}

		if (line["CLICKWAIT"] != "") {
			float clickWaitTimer = float.Parse (line ["CLICKWAIT"]);
			while (Input.GetMouseButtonUp (0) == false && clickWaitTimer > 0) {
				clickWaitTimer -= Time.deltaTime;
				yield return null;
			}
		}
		else
		{			

			// Using the 5 letters per word convention
			int numberOfWords = line ["TEXT"].Length / 5;
			float timeToRead = producer.readingStartDelay + numberOfWords / producer.wordsPerSecond;
			yield return new WaitForSeconds(timeToRead);
		}

		yield return StartCoroutine(Fade.Out(textCanvasGroup.gameObject, periodForFades));

	}


	public virtual Vector2 getSubtitlesPivot(Line line) {
		return (line ["XPIVOT"] == "") ? new Vector2 (0.5f, 0) : new Vector2 (float.Parse (line ["XPIVOT"]), float.Parse (line ["YPIVOT"]));
	}


	public IEnumerator blinkSubtitles(Line line, Text subtitlesText) {
		CanvasGroup fadeCanvasGroup = subtitlesText.transform.parent.GetComponent<CanvasGroup>();
		CanvasGroup blinkCanvasGroup = subtitlesText.GetComponent<CanvasGroup>();

		float blinkPeriod = float.Parse (line ["BLINKRATE"]), blinkTimer = 0;

		while (fadeCanvasGroup.alpha > 0) {
			if (blinkTimer <= 0) {
				blinkCanvasGroup.alpha = (blinkCanvasGroup.alpha == 0) ? 1 : 0;
				blinkTimer = blinkPeriod;
			} else {
				blinkTimer -= Time.deltaTime;
			}
			yield return null;
		}

		blinkCanvasGroup.alpha = 1;
	}


	//
	// Talk -> Comics Balloons
	//

	Camera viewportCamera;

	Transform pivotRotationTransform, pivotPositionTransform;

	public IEnumerator parseComicsTalk(Line line)
	{
		producer = GetComponentInParent<Producer> ();

		float clickWaitTimer = (line["CLICKWAIT"] != "") ? float.Parse (line ["CLICKWAIT"]) : -1;

		GameObject baloonPanel = getBalloonPanel(line ["XTYPE"], line ["YTYPE"]);
		baloonPanel.GetComponentInChildren<Text>().text = line ["TEXT"];
		RectTransform balloonRectTransform = baloonPanel.GetComponent<RectTransform>();


		if (line["XPIVOT"] == "")
		{
			if (actorTransformCamera != null) {
				viewportCamera = actorTransformCamera;
			} else {
				if(Camera.main != null) {
					viewportCamera = Camera.main;
				}
				else {
					viewportCamera =  Camera.current;
				} 
			}

			if (comicsPanelRectTransform == null) {
				comicsPanelRectTransform = producer.transform as RectTransform;
			}

			if (pivotRotationTransform == null) {
				pivotRotationTransform = director.createGameObjectIfNotPresent ("pivotRotationTransform", actorTransform).transform;
				pivotPositionTransform = director.createGameObjectIfNotPresent ("pivotPositionTransform", pivotRotationTransform).transform;
			}

			pivotRotationTransform.localPosition = pivotRotationCenter;
			pivotPositionTransform.localPosition = getPivotLocalPosition (line ["XTYPE"], line ["YTYPE"]); 

			balloonRectTransform.position = getUpdatedBalloonPosition();
		}
		else {
			balloonRectTransform.pivot = new Vector2 (float.Parse (line ["XPIVOT"]), float.Parse (line ["YPIVOT"]));
			balloonRectTransform.anchoredPosition = Vector3.zero; // anchoredPosition is the Pos X and Pos Y that appear on the Inspector.
		} 


		StartCoroutine(Fade.In(baloonPanel, periodForFades));

		if (clickWaitTimer != -1) {
			while (Input.GetMouseButtonUp (0) == false && clickWaitTimer > 0) {
				clickWaitTimer -= Time.deltaTime;
				if (line ["XPIVOT"] == "") {
					balloonRectTransform.position = getUpdatedBalloonPosition ();
				}
				yield return null;
			}
		}
		else
		{
			// Using the 5 letters per word convention
			int numberOfWords = line ["TEXT"].Length / 5;
			float timeToRead = producer.readingStartDelay + numberOfWords / producer.wordsPerSecond;

			if (line ["XPIVOT"] == "") {
				while (timeToRead > 0) {
					timeToRead -= Time.deltaTime;

					balloonRectTransform.position = getUpdatedBalloonPosition ();
					yield return null;
				}
			} else {
				yield return new WaitForSeconds(timeToRead);
			}
		}

		yield return StartCoroutine(Fade.Out(baloonPanel, periodForFades));
	}


	Vector3 getUpdatedBalloonPosition() {
		// Make the in-world pivot transform armature turn towards the camera
		pivotRotationTransform.LookAt(viewportCamera.transform, pivotRotationTransform.parent.up);
		pivotRotationTransform.localEulerAngles = new Vector3(0, pivotRotationTransform.localEulerAngles.y, 0);
		// viewportCamera was externally set to render to the texture in comicsPanelRectTransform
		// pivotPositionTransform is locally attached to the actorTransform and changes its position together with it.
		comicsPanelRectTransform.pivot = viewportCamera.WorldToViewportPoint (pivotPositionTransform.position);
		// Changing the pivot of comicsPanelRectTransform also changes its pixel position in screen space. 
		// Although visually the panel continues at the same place, its position now points to (original position in pixels + pivot offset in pixels), 
		// and it is this new changed position, the screen space position of the in-world pivot,  that the balloon RectTransform must be assigned to.
		return comicsPanelRectTransform.position;
	}
		


	GameObject getBalloonPanel(string xType, string yType)
	{
		Text textGameObject = (xType == "LEFT") ?
			(yType == "BOTTOM") ? producer.leftBottomText : producer.leftTopText :
			(yType == "BOTTOM") ? producer.rightBottomText : producer.rightTopText;

		Vector2 pivot = (xType == "LEFT") ?
			(yType == "BOTTOM") ? Vector2.one : Vector2.right :
			(yType == "BOTTOM") ? Vector2.up : Vector2.zero;

		GameObject textPanel = textGameObject.transform.parent.gameObject;
		textPanel.GetComponent<RectTransform>().pivot = pivot;

		return textPanel;
	}


	Vector3 getPivotLocalPosition(string xType, string yType)
	{
		return (xType == "LEFT") ?
			(yType == "BOTTOM") ? new Vector3(bottomBalloonXdistance, bottomBalloonYdistance,0) : new Vector3(topBalloonXdistance, topBalloonYdistance,0) :
			(yType == "BOTTOM") ? new Vector3(-bottomBalloonXdistance, bottomBalloonYdistance,0) : new Vector3(-topBalloonXdistance, topBalloonYdistance,0) ;
	}	


	//
	// Move
	// (Coordinates are always local.)
	//

	//
	// Move -> Go To
	//

	public IEnumerator goTo(float x, float y, float z, float speedMagnitude, bool desaccel, bool keepOriginalRotation)
	{
		Vector3 targetPosition = (actorTransform.parent != null) ? actorTransform.parent.TransformPoint(x, y, z) : new Vector3(x, y, z);

		if (!keepOriginalRotation) {
			actorTransform.LookAt (targetPosition, (actorTransform.parent != null) ? actorTransform.parent.up : Vector3.up);
		}

		Vector3 speed = Vector3.Normalize(targetPosition - actorTransform.position) * speedMagnitude;

		Vector3 origin = actorTransform.position;
		float startingDistanceFromTarget = Vector3.Distance(origin, targetPosition);
		float time = startingDistanceFromTarget / speedMagnitude;
		const float timeToDesaccel = 1f;
		float desaccelSpeed = speedMagnitude / (timeToDesaccel * 1.4f); // Empyrical choices.

		float distanceFromOrigin = 0;
		do
		{
			yield return null;
			actorTransform.position += speed * Time.deltaTime;

			distanceFromOrigin = Vector3.Distance(actorTransform.position, origin);
			time -= Time.deltaTime;

			if (desaccel && time < timeToDesaccel && time > 0.01f)
			{
				speed -= desaccelSpeed * speed.normalized * Time.deltaTime;
			}

		} while (distanceFromOrigin < startingDistanceFromTarget);

		actorTransform.position = targetPosition;
	}


	public IEnumerator goToAccelerating(float x, float y, float z, float speedMagnitude, float accelerationMagnitude, bool keepOriginalRotation)
	{
		Vector3 targetPosition = (actorTransform.parent != null) ?  actorTransform.parent.TransformPoint(x, y, z) : new Vector3(x, y, z);

		if (!keepOriginalRotation) {
			actorTransform.LookAt (targetPosition,  (actorTransform.parent != null) ? actorTransform.parent.up : Vector3.up);
		}

		Vector3 speed = Vector3.Normalize(targetPosition - actorTransform.position) * speedMagnitude;
		Vector3 acceleration = speed.normalized * accelerationMagnitude;

		Vector3 origin = actorTransform.position;
		float startingDistanceFromTarget = Vector3.Distance(origin, targetPosition);

		float distanceFromOrigin = 0;
		do
		{
			yield return null;
			actorTransform.position += speed * Time.deltaTime;
			speed += acceleration * Time.deltaTime;
			distanceFromOrigin = Vector3.Distance(actorTransform.position, origin);
		} while (distanceFromOrigin < startingDistanceFromTarget);

		actorTransform.position = targetPosition;
	}


	public IEnumerator goToTeleporting(float x, float y, float z)
	{
		actorTransform.position = (actorTransform.parent != null) ? actorTransform.parent.TransformPoint(x, y, z) : new Vector3(x, y, z);
		yield break;
	}


	//
	// Move -> Rotate
	//

	public IEnumerator rotateBy(float angle, float speed)
	{

		Quaternion initialRotation = actorTransform.rotation;
		actorTransform.Rotate(Vector3.up, angle);
		Quaternion finalRotation = actorTransform.rotation;
		actorTransform.rotation = initialRotation;

		speed *= Mathf.Sign (angle);
		for (float angleRotated = 0, angleOffset = 0;   Math.Abs(angleRotated) < Math.Abs(angle);  angleRotated += angleOffset)
		{
			yield return null;
			angleOffset = speed * Time.deltaTime;
			actorTransform.Rotate(Vector3.up, angleOffset);
		}

		actorTransform.rotation = finalRotation;
	}

	public IEnumerator rotateTo(float angle, float speed) // Assumes the starting angle is zero.
	{
		Quaternion targetLocalRotation = Quaternion.Euler(0, angle, 0);
		do {
			actorTransform.localRotation = Quaternion.RotateTowards (actorTransform.localRotation, targetLocalRotation, speed * Time.deltaTime);
			yield return null;

		} while (actorTransform.localRotation != targetLocalRotation);

		actorTransform.localRotation = targetLocalRotation;
	}

	public IEnumerator rotateTowards(float x, float y, float z, float speed)
	{


		Vector3 target = (actorTransform.parent != null) ? actorTransform.parent.TransformPoint(x, y, z) : new Vector3(x, y, z);
		Vector3 previousPosition;

		Quaternion currentRotation, targetRotation;
		do {

			currentRotation = actorTransform.rotation;
			actorTransform.LookAt (target, (actorTransform.parent != null) ? actorTransform.parent.up : Vector3.up);
			targetRotation = actorTransform.rotation;
			actorTransform.rotation = currentRotation;


			actorTransform.rotation = Quaternion.RotateTowards (actorTransform.rotation, targetRotation, speed * Time.deltaTime);
			previousPosition = actorTransform.position;
			yield return null;

		} while (actorTransform.rotation != targetRotation || actorTransform.position != previousPosition);

		actorTransform.rotation = targetRotation;
	}

	#if UNITY_EDITOR

	public AnimatorController animatorController;
	public bool hasAnimations;

	public bool canUseRelativeCoordinates;


	void Start()
	{		
		if (Application.isPlaying) {
			if (!warnActorNotUsed ()) {
				if (usedAnimationStateNames != null && usedAnimationStateNames.Count > 1) { // The idle animation is always there
					addCondition ();
				}
			}
		}
	}
		

	bool warnActorNotUsed() {
		Line[] lines = GetComponentsInChildren<Line> ();
		foreach (Line line in lines) {
			if (line.actor == this) {
				return false;
			}
		}

		Debug.LogWarning ("Actor \"" + actorName + "\" is not used in cutscene \"" + gameObject.name + "\""); 
		return true;
	}


	void addCondition() {
		AnimatorControllerParameter[] animatorControllerParameters = animatorController.parameters;

		bool hasCutscene = false;
		foreach (AnimatorControllerParameter animatorControllerParameter in animatorControllerParameters) {
			if (animatorControllerParameter.name == "cutscene") {
				hasCutscene = true;
				break;
			}
		}

		if (!hasCutscene) {
			animatorController.AddParameter ("cutscene", AnimatorControllerParameterType.Bool);
		}
		animator.SetBool ("cutscene", false);

		foreach(AnimatorControllerLayer layer in animatorController.layers) {
			foreach (ChildAnimatorState state in layer.stateMachine.states) {
				if (usedAnimationStateNames.Contains (state.state.name)) {
					foreach (AnimatorStateTransition transition in state.state.transitions) {
						if (usedAnimationStateNames.Contains ( transition.destinationState.name)) {
							hasCutscene = false;
							foreach(AnimatorCondition condition in transition.conditions) {
								if (condition.parameter == "cutscene") {
									hasCutscene = true;
									break;
								}
							}
							if (!hasCutscene) {
								transition.AddCondition (AnimatorConditionMode.IfNot, 0, "cutscene");
							}
						}
					}
				}
			}
		}
	}


	public Line importLineToCutsceneScript(string lineData) {
		
		GameObject cutsceneScriptGameObject = director.currentCutsceneScriptPart;
		Line newLine = cutsceneScriptGameObject.AddComponent<Line>();

		if (lineData != "") {
			newLine.importLine (lineData);
		} else {
			newLine.setDefaultLine (this);
		}

		Selection.activeGameObject = cutsceneScriptGameObject;
		return newLine;
	}


	public Line addLineToCutsceneScript() {
		return importLineToCutsceneScript ("");
	}


	public Line changeAtorFromCutsceneScript(Line line, string newActorName) {
		line.actor = director.getActorByName (newActorName);
		line ["ACTOR"] = line.actor.actorName;
		return line;
	}


	public void updateActorTransform(Transform newActorTransform) {
		actorTransform = newActorTransform;
		if(actorTransform) {
			animator = actorTransform.GetComponentInChildren<Animator> (true);
			if (animator) {
				animatorController = animator.runtimeAnimatorController as AnimatorController;
				if (animatorController) {
					usedAnimationStateNames = new List<string> ();
					usedAnimationStateNames.Add (animatorController.layers [0].stateMachine.defaultState.name);
				}
			}

			if(!animator || !animatorController) {
				hasAnimations = false;
			}
		}
	}


	#endif

}

