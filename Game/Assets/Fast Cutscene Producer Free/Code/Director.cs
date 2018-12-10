using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.IO;
using System.IO.Compression;


#if UNITY_EDITOR
using UnityEditor;
#endif



[ExecuteInEditMode]
public partial class Director : MonoBehaviour {

	public string standaloneDialogueEditorPath;


	public Font comicsFont;
	public int comicsFontSize = 24;

	public Font subtitlesFont;
	public int subtitlesFontSize = 40;

	public Font subtitlesHeaderFont;
	public int subtitlesHeaderFontSize = 40;

	public bool hasActorNameHeader = false;
	public Sprite subtitlesBackgroundSprite;


	public float verticalPadding = 0;
	public float horizontalPadding = 0;


	public bool hasText;

	public bool hasCustomSubtitlesStyle;
	public bool hasCustomComicsStyle;

	public bool hasBackgroundImage;

	Producer producer;

	void Start()
	{
		producer = GetComponentInParent<Producer> ();

		if (Application.isPlaying) {

			if (hasText) {				

				if (hasCustomComicsStyle) {
					setComicsStyle ();
				}

				if (hasCustomSubtitlesStyle) {
					setSubtitlesStyle ();

					if (hasActorNameHeader) {
						setSubtitlesHeaderStyle ();
					}
				}
			}

			StartCoroutine (inGameStart ());
		}
	}


	IEnumerator inGameStart()
	{
		Actor[] actors = GetComponents<Actor>();

		Line[] lines = GetComponentsInChildren<Line>();

		Dictionary<string, Actor> actorDictionary = new Dictionary<string, Actor>();
		foreach(Actor actor in actors)
		{
			actor.gameObject.SetActive(true);
			actorDictionary.Add(actor.actorName, actor);
		}

		producer.subtitlesHeader.gameObject.SetActive (hasActorNameHeader);

		for (int i = 0; i < lines.Length; i++) {

			if (lines [i].enabled == false) {
				continue;
			}
			string parserName = lines [i]["ACTOR"];
			if (lines[i] ["ASYNC"] == "TRUE") {
				StartCoroutine (actorDictionary [parserName].parse (lines [i]));
				yield return null; // So that it actually starts before the following one.
			} else {
				yield return StartCoroutine (actorDictionary [parserName].parse (lines [i]));
			}
		}
	}


	public void setSubtitlesStyle() {
		if (subtitlesFont != null) {
			producer.subtitlesText.font = subtitlesFont;
		}

		producer.subtitlesText.fontSize = subtitlesFontSize;

		if (hasBackgroundImage) {
			if (subtitlesBackgroundSprite != null) {
				producer.subtitlesBackgroundImage.sprite = subtitlesBackgroundSprite;
				producer.subtitlesBackgroundImage.enabled = true;
			} else {
				producer.subtitlesBackgroundImage.sprite = null;
				producer.subtitlesBackgroundImage.enabled = false;
			}
		}
	}


	public void setSubtitlesHeaderStyle() {
		producer.subtitlesHeader.gameObject.SetActive (true);

		if (subtitlesHeaderFont != null) {
			producer.subtitlesHeader.font = subtitlesHeaderFont;
		} 
		producer.subtitlesHeader.fontSize = subtitlesHeaderFontSize;

	}


	public void setComicsStyle() {
		if (hasCustomComicsStyle && comicsFont != null) {
			producer.leftBottomText.font = producer.leftTopText.font = producer.rightTopText.font = producer.rightBottomText.font = comicsFont;
		} 

		if (hasCustomComicsStyle) {
			producer.leftBottomText.fontSize = producer.leftTopText.fontSize = producer.rightTopText.fontSize = producer.rightBottomText.fontSize = comicsFontSize;
		} 
	}
		

	public GameObject createGameObjectIfNotPresent(string name, Transform parent) {
		if (parent != null) {
			Transform[] children = parent.GetComponentsInChildren<Transform> (true);

			foreach (Transform child in children) {
				if (child.name == name) {
					return child.gameObject;
				}
			}

			GameObject g = new GameObject(name);
			g.transform.SetParent (parent,false);
			return g;
		} else {
			GameObject[] rootGameObjects = SceneManager.GetActiveScene ().GetRootGameObjects ();

			foreach (GameObject rootGameObject in rootGameObjects) {
				if (rootGameObject.name == name) {
					return rootGameObject;
				}
			}

			return new GameObject (name);

		}
	}


#if UNITY_EDITOR





	public int hasText_subtitlesBalloonsOrBoth;
	public bool isInLine;

	public bool isLayoutFlexible = true;
	public LineEditor.ScriptLayout scriptLayout = LineEditor.ScriptLayout.tall;

	[NonSerialized]public string dialogueEditorTempFileNameArg;
	[NonSerialized]public string highlightActorName;
	[NonSerialized]public string highlightLineName;
	[NonSerialized]public double startHighlightTime = 0;

	public List<GameObject> cutsceneScriptParts;

	GameObject currentCutsceneScriptPartValue;


	void Awake() {
		EditorApplication.update += checkCurrentPartHighlight;

		initializeCutsceneScriptParts ();

		if (cutsceneScriptParts.Count == 0) {
			createNewCutsceneScriptPart ("CutsceneScript");
		} else {
			currentCutsceneScriptPartValue = cutsceneScriptParts [cutsceneScriptParts.Count - 1];
		}
	}


	public void initializeCutsceneScriptParts() {
		cutsceneScriptParts = new List<GameObject> ();

		Transform[] children = GetComponentsInChildren<Transform> ();
		foreach (Transform child in children) {
			if (child.GetComponents<Line>().Length > 0 || child.name == "CutsceneScript") {
				cutsceneScriptParts.Add (child.gameObject);
			}
		}

	}


	void OnDestroy() {
		EditorApplication.update -= checkCurrentPartHighlight;
	}


	public GameObject currentCutsceneScriptPart {
		set {
			Selection.activeGameObject = currentCutsceneScriptPartValue = value;
		}
		get {
			if (cutsceneScriptParts.Contains (Selection.activeGameObject)) {
				currentCutsceneScriptPartValue = Selection.activeGameObject;
			}
			return currentCutsceneScriptPartValue;
		}
	}


	public GameObject createNewCutsceneScriptPart() {
		createNewCutsceneScriptPart("CutsceneScript_Part " + (cutsceneScriptParts.Count+1));
		Actor actor = GetComponent<Actor> ();
		if (actor) {
			actor.addLineToCutsceneScript ();
		}
		return currentCutsceneScriptPart;
	}


	public GameObject createNewCutsceneScriptPart(string partName) {
		currentCutsceneScriptPart = new GameObject (partName);
		currentCutsceneScriptPart.transform.SetParent (transform, false);
		cutsceneScriptParts.Add (currentCutsceneScriptPart);
		return currentCutsceneScriptPart;
	}



	void checkCurrentPartHighlight() {
		if (cutsceneScriptParts.Contains (Selection.activeGameObject)) {
			currentCutsceneScriptPart = Selection.activeGameObject;
			checkHighlight (highlightLineName);
		} else if (Selection.activeGameObject == gameObject) {
			checkHighlight (highlightActorName);
		}
	}


	void checkHighlight(string s) {
		if (startHighlightTime == 0) {
			if (s != null && s != "") {
				startHighlightTime = EditorApplication.timeSinceStartup;

				Highlighter.Highlight ("Inspector", s);
			}
		} else if (EditorApplication.timeSinceStartup - startHighlightTime > 10
		            || (EditorApplication.timeSinceStartup - startHighlightTime > 2 && Highlighter.activeVisible)) {
			startHighlightTime = 0;
			highlightActorName = highlightLineName = "";

			Highlighter.Stop ();
		}
	}


	public Actor getActorByName(string actorName) {
		Actor[] actors = GetComponents<Actor> ();
		foreach (Actor actor in actors) {
			if (actor.actorName.ToUpper() == actorName.ToUpper()) {
				return actor;
			}
		}
		return null;
	}

#endif

}














