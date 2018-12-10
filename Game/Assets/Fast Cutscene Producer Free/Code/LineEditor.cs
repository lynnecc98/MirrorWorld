
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Line))]
[CanEditMultipleObjects]
public partial class LineEditor : Editor {

	protected Line line;
	protected List<string> actorNames = new List<string>();

	int lineNumber;

	public enum ScriptLayout { tall, balanced, wide };
	ScriptLayout scriptLayout;

	void Awake() {
		line = (serializedObject.targetObject as Line);

		Actor[] actors = line.transform.parent.GetComponentsInChildren<Actor> ();
		foreach(Actor actor in actors) {
			if (actor.actorTransform && actor.actorName.Trim () != "") {
				actorNames.Add (actor.actorName);
			}
		}
	}


	public override void OnInspectorGUI() {
		if (line.actor.director.isLayoutFlexible) {
			scriptLayout = (Screen.width < 515) ? scriptLayout = ScriptLayout.tall :
				(Screen.width < 950) ? scriptLayout = ScriptLayout.balanced :
				ScriptLayout.wide;		
		} else {
			scriptLayout = line.actor.director.scriptLayout;
		}
		serializedObject.Update ();	


		if (line.isActiveAndEnabled == false) {
			DirectorEditor.enableMessage ();
			serializedObject.ApplyModifiedProperties ();
			return;
		}
		if (line == null || ((string)line).Length == 0){
			return;
		}


		EditorGUILayout.BeginHorizontal ();
		if (scriptLayout == ScriptLayout.tall) {
			colorBar ();
			EndH ();
			BeginH ();
			actorName ();
		} else {
			actorName ();
			EndH ();
			BeginH ();
			colorBar ();
		}

		if (scriptLayout == ScriptLayout.tall || scriptLayout == ScriptLayout.balanced) {
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
		} 
		moveEnterExitTalkPause ();
		EditorGUILayout.EndHorizontal ();
		BeginHorizontal = true;



		//
		//
		// Main switch
		//
		//

		if (scriptLayout == ScriptLayout.wide && line.moveEnterExitOrTalk != 3) {
			moveLineAndRunSimultaneously ();
		} else {
			EditorGUILayout.Separator ();
		}

		switch (line.moveEnterExitOrTalk) {
		case 0:			
			OnInspectorGUI_Move ();
			break;
		case 3:
			OnInspectorGUI_Talk ();
			break;
		default:
			OnInspectorGUI_EnterExit ();
			break;
		}

		if (scriptLayout != ScriptLayout.wide || line.moveEnterExitOrTalk == 3) {
			moveLineAndRunSimultaneously ();
		}

		BeginH ();
		EditorGUILayout.EndHorizontal (); // End of a great horizontal line, if it was in-line.

		if (GUI.changed && !Application.isPlaying) {
			EditorUtility.SetDirty (line);
			EditorSceneManager.MarkSceneDirty (line.gameObject.scene);
		}

		serializedObject.ApplyModifiedProperties ();
	}

	void OnInspectorGUI_EnterExit () {
		if (line.actor.actorTransform.GetComponent<CanvasGroup> ()) {
			BeginH ();
			GUILayout.Label ("Custom Fade Time", GUILayout.ExpandWidth(false));
			bool hasCustomFade = EditorGUILayout.Toggle ((line ["FADETIME"] != ""), GUILayout.Width (20));
			if (hasCustomFade) {
				if (line ["FADETIME"] == "") {
					line ["FADETIME"] = Actor.periodForFades.ToString ();
				}
				line ["FADETIME"] = EditorGUILayout.FloatField (float.Parse (line ["FADETIME"]), GUILayout.Width (50)).ToString ();;
			} else if (line ["FADETIME"] != "") {
				line ["FADETIME"] = "";
			}
			EndH ();
		}
	}


	void colorBar() {

		lineNumber = ((new List<Line> (line.GetComponents<Line> ())).IndexOf (line) + 1);


		int totalWidth = (scriptLayout == ScriptLayout.tall) ? 240  : 140; 
		int textureWidth = totalWidth - ( getNumberOfAlgarisms(lineNumber) - 1 ) * 6;

		int textureHeight = 8;
		int textureY = (scriptLayout == ScriptLayout.tall) ? 0 : 3;

		GUILayout.Label("", GUILayout.Width(0));
		GUI.DrawTexture(new Rect (GUILayoutUtility.GetLastRect ().x, GUILayoutUtility.GetLastRect ().y + textureY, textureWidth, textureHeight), line.inspectorColorTextures[line["ACTION"]]);
			
		GUIStyle guiStyle = new GUIStyle ();
		guiStyle.normal.textColor = line.actionTextColors [line ["ACTION"]];
		guiStyle.fontSize = 9;
		guiStyle.fontStyle = FontStyle.Bold;
		GUI.Label (new Rect (GUILayoutUtility.GetLastRect ().x + textureWidth + 1, GUILayoutUtility.GetLastRect ().y + textureY - 1, 10, 10), lineNumber.ToString (), guiStyle);
		if (scriptLayout == ScriptLayout.wide) {
			GUILayout.Label ("", GUILayout.Width (totalWidth + 5));
		}
	}


	void actorName() {

		GUILayout.Label ("Actor", GUILayout.ExpandWidth (false));

		int selectedActorIndex = actorNames.IndexOf (line.actor.actorName); 
		selectedActorIndex = EditorGUILayout.Popup (selectedActorIndex, actorNames.ToArray(), (scriptLayout == ScriptLayout.tall) ? GUILayout.ExpandWidth (true) : GUILayout.Width (204) );
		string newActorName = actorNames [selectedActorIndex];
		if (newActorName != line.actor.actorName) {
			line.actor.changeAtorFromCutsceneScript(line, newActorName);
			return;
		}

		if (GUILayout.Button ("View Actor", GUILayout.ExpandWidth (false))) {
			line.actor.director.highlightActorName = line.actor.actorName;
			Selection.activeGameObject = line.actor.gameObject;
		}

		GUILayout.Label (" ", GUILayout.Width (7));

	}


	void moveLineAndRunSimultaneously() {

		string[] lineNames = getLineNames ();
		if (lineNames.Length > 1) {
			if (scriptLayout == ScriptLayout.tall) {
				EditorGUILayout.Separator ();
			} else {
				EditorGUILayout.EndHorizontal ();
			}
			moveLineToBefore ();
		}

		BeginH ();

		GUILayout.Label ("Run simultaneously with lines below", GUILayout.ExpandWidth (false));

		int w = (scriptLayout == ScriptLayout.wide) ? 28 : 20;
		line ["ASYNC"] = EditorGUILayout.Toggle (bool.Parse (line ["ASYNC"].ToLower ()), GUILayout.Width (w)).ToString ().ToUpper ();

		EndH ();

	}

	void moveEnterExitTalkPause() {


		int selected;
		if (line.actor.director.hasText) {
			selected = GUILayout.Toolbar (line.moveEnterExitOrTalk, new string[] {"Move","Enter","Exit","Talk"}, GUILayout.Width(240));
			if (selected != line.moveEnterExitOrTalk) {
				line.moveEnterExitOrTalk = selected;
			}
		} else {
			if (line.moveEnterExitOrTalk == 3) {
				line.moveEnterExitOrTalk = 0;
			}
			selected = GUILayout.Toolbar (line.moveEnterExitOrTalk, new string[] {"Move","Enter","Exit"	}, GUILayout.Width(240));			
			if (selected != line.moveEnterExitOrTalk) {
				line.moveEnterExitOrTalk = selected;
			}
		}
		EndH ();


		BeginH ();
		numericTextField ("Pause", "INTERVAL", GUILayout.MaxWidth(40));
		GUILayout.Label ("second(s) before starting", GUILayout.ExpandWidth (false));

	}


	int getNumberOfAlgarisms(int i) {
		return getNumberOfAlgarisms(i, 0);
	}


	int getNumberOfAlgarisms(int i, int numberOfAlgarisms) {
		return (i>0) ? getNumberOfAlgarisms (i/10, numberOfAlgarisms+1) : numberOfAlgarisms;
	}


	string[] getLineNames() {
		Line[] lines = line.GetComponents<Line> ();

		List<string> lineNames = new List<string> ();
		for (int i = 0; i < lines.Length; i++) {
			if (lines [i].actor) {
				lineNames.Add("Line " + (i + 1) + " " + lines [i]["ACTOR"] + " " + lines [i]["ACTION"]);
			}
		}
		return lineNames.ToArray();
	}


	void numericTextField(string label, string key, GUILayoutOption guiLayoutOption) {
		GUILayout.Label (label, GUILayout.ExpandWidth (false));
		line [key] = EditorGUILayout.FloatField (float.Parse (line [key]), guiLayoutOption).ToString ();
	}

	bool BeginHorizontal = false;
	void BeginH() {
		if ((scriptLayout == ScriptLayout.tall) || BeginHorizontal) {
			EditorGUILayout.BeginHorizontal ();
			BeginHorizontal = false;
		}
	}


	void EndH() {
		if (scriptLayout == ScriptLayout.tall) { 
			EditorGUILayout.EndHorizontal ();
		}
	}


	//
	//
	// Move
	//
	//


	void OnInspectorGUI_Move() {

		BeginH ();

		int selected = EditorGUILayout.Popup (line.move_gotoOrRotate, new string[] { "Go To", "Rotate" }, GUILayout.Width(60));	

		if (selected != line.move_gotoOrRotate) {
			line.move_gotoOrRotate = selected;
		}

		switch (line.move_gotoOrRotate) {
		case 0: // go tos
			
			selected = GUILayout.Toolbar (line.move_goTo_SteadyAcceleratingOrTeleporting, new string[] {
				"Steady",
				"Accel.",
				"Teleport"
			}, GUILayout.Width (175));
			EndH ();

			if (selected != line.move_goTo_SteadyAcceleratingOrTeleporting) {
				line.move_goTo_SteadyAcceleratingOrTeleporting = selected;
			}

			BeginH ();
			GUILayout.Label ("Go to:", GUILayout.ExpandWidth (false));

			int w = 47;
			numericTextField ("x", "X", GUILayout.Width (w));			
			numericTextField ("y", "Y", GUILayout.Width (w));		
			numericTextField ("z", "Z", GUILayout.Width (w));			

			EndH ();

			if (line.move_goTo_SteadyAcceleratingOrTeleporting != 2) {
				
				BeginH ();
				if (scriptLayout == ScriptLayout.tall) {
					GUILayout.Label (" ", GUILayout.Width (95));
				}

				string dontLook = (line ["KEEPORIGINALROTATION"] == "") ? "FALSE" : line ["KEEPORIGINALROTATION"];
				line ["KEEPORIGINALROTATION"] = GUILayout.Toggle (bool.Parse (dontLook.ToLower ()), "Keep original rotation", GUILayout.ExpandWidth (false)).ToString ().ToUpper ();
				EndH ();

			}

			switch (line.move_goTo_SteadyAcceleratingOrTeleporting) {
			case 0:
				
				BeginH ();
				numericTextField ("Speed", "SPEED", GUILayout.Width (51));
				line ["DESACCEL"] = GUILayout.Toggle (bool.Parse (line ["DESACCEL"].ToLower ()), "Desaccel on arrival", GUILayout.Width (140)).ToString ().ToUpper ();

				EndH ();

				break;
			case 1:
				
				BeginH ();
				numericTextField ("Speed", "SPEED", GUILayout.Width (51));
				numericTextField ("Acceleration", "ACCELERATION", GUILayout.Width (59));

				EndH ();

				break;
			}
			break;	

		case 1: // rotates
			
			selected = GUILayout.Toolbar (line.move_rotate_BydegreesToangleOrTowardsxyz, new string[] {
				"Spin By",
				"Spin To",
				"Look At"
			}, GUILayout.Width (175));
			EndH ();

			if (line.move_rotate_BydegreesToangleOrTowardsxyz != selected) {
				line.move_rotate_BydegreesToangleOrTowardsxyz = selected;
			}

			BeginH ();
			switch (line.move_rotate_BydegreesToangleOrTowardsxyz) {
			case 0:
				numericTextField ("Degrees", "ANGLE", GUILayout.Width (67));
				numericTextField ("Speed", "SPEED", GUILayout.Width (67));	
				break; 
			case 1:
				numericTextField ("Angle", "ANGLE", GUILayout.Width (75));
				numericTextField ("Speed", "SPEED", GUILayout.Width (75));	
				break;
			case 2:
				GUILayout.Label ("Look at:", GUILayout.ExpandWidth (false));
				numericTextField ("x", "X", GUILayout.Width (43));			
				numericTextField ("y", "Y", GUILayout.Width (43));		
				numericTextField ("z", "Z", GUILayout.Width (44));	

				EndH ();
				BeginH ();

				numericTextField ("Speed", "SPEED", GUILayout.Width (70));
				break;
			}
			EndH ();

			break;
		}


		if (line.actor.hasAnimations && line ["ACTION"] != "goToTeleporting") {
			
			BeginH ();

			GUILayout.Label ("Use Animation", GUILayout.ExpandWidth (false));

			bool useAnimation = (line ["ANIMATION"] != "");
			useAnimation = GUILayout.Toggle(useAnimation, "", GUILayout.ExpandWidth (false));

			if (useAnimation) {
				if (line ["ANIMATION"] == "") {
					line ["ANIMATION"] = line.actor.usedAnimationStateNames [0];
				}

				selected = line.actor.usedAnimationStateNames.IndexOf (line ["ANIMATION"].Replace ("_-_-"," "));

				// It also tests if the selected animation is still available or was unselected in the Actor. If so, it defaults to 0.
				selected = EditorGUILayout.Popup ( (selected == -1) ? 0 : selected , line.actor.usedAnimationStateNames.ToArray(), GUILayout.Width (127));

				if (selected != -1) {

					line ["ANIMATION"] = line.actor.usedAnimationStateNames [selected];

				}
			} else if (line ["ANIMATION"] != "") {
				line ["ANIMATION"] = "";
			}

			EndH ();


		} else {
			line ["ANIMATION"] = "";
		}
	}
		

	//
	//
	// Talk
	//
	//


	void OnInspectorGUI_Talk() {

		switch (line.actor.director.hasText_subtitlesBalloonsOrBoth) {
		case 0:
			if (line ["ACTION"] == "comicsTalk") {
				line.actionLastValidLine ["subtitles"] ["TEXT"] = line ["TEXT"];
				line.setLine(line.actionLastValidLine ["subtitles"]);
			}
			OnInspectorGUI_Subtitles ();
			break;
		case 1:
			if (line ["ACTION"] == "subtitles") {
				line.actionLastValidLine ["comicsTalk"] ["TEXT"] = line ["TEXT"];
				line.setLine(line.actionLastValidLine ["comicsTalk"]);
			}
			OnInspectorGUI_ComicsTalk ();
			break;
		case 2:
			BeginH ();
			GUILayout.Label ("Text Type", GUILayout.ExpandWidth (false));
			int selected = EditorGUILayout.Popup (line.talk_subtitlesOrComicsBalloon, new string[] { "Subtitles", "Comics Balloon" }, GUILayout.Width (175));	
			EndH ();
			if (selected != line.talk_subtitlesOrComicsBalloon) {
				line.talk_subtitlesOrComicsBalloon = selected;
			}

			if (line.talk_subtitlesOrComicsBalloon == 0) {
				OnInspectorGUI_Subtitles ();
			} else {
				OnInspectorGUI_ComicsTalk ();
			}
			break;
		}

	}


	void OnInspectorGUI_Subtitles()
	{
		text ();

		if (scriptLayout != ScriptLayout.tall) {
			EditorGUILayout.EndHorizontal ();
			BeginHorizontal = true;
		}

		GUILayout.Label ("Subtitles Text", GUILayout.ExpandWidth(false));
		line ["TEXT"] = GUILayout.TextArea(line ["TEXT"], GUILayout.ExpandWidth(true) );

		BeginH ();

		GUILayout.Label ("Custom Pivot", GUILayout.ExpandWidth(false));
		bool hasCustomPivot = (line ["XPIVOT"] != "");
		hasCustomPivot = EditorGUILayout.Toggle (hasCustomPivot, GUILayout.Width (50));

		subtitlesDialogueEditor ();

		if (hasCustomPivot) {
			pivotSlides ("0.5", "0");
		} else {
			line["XPIVOT"] = "";
			line["YPIVOT"] = "";
		}
	}


	void OnInspectorGUI_ComicsTalk()
	{
		text ();

		if (scriptLayout != ScriptLayout.tall) {
			EditorGUILayout.EndHorizontal ();
			BeginHorizontal = true;
		}

		GUILayout.Label ("Comics Balloon Text", GUILayout.ExpandWidth(false));
		line ["TEXT"] = GUILayout.TextArea(line ["TEXT"], GUILayout.ExpandWidth(true) );

		BeginH ();

		GUILayout.Label ("Draw", GUILayout.ExpandWidth(false));

		string[] relativeTypes = new string[] { "TOP LEFT", "TOP RIGHT", "BOTTOM LEFT", "BOTTOM RIGHT" }; 
		int relativeTypeIndex = Array.IndexOf(relativeTypes, line ["YTYPE"].ToUpper() + " " + line ["XTYPE"].ToUpper()); 

		relativeTypeIndex = EditorGUILayout.Popup (relativeTypeIndex, new string[] { "Top Left", "Top Right", "Bottom Left", "Bottom Right" }, GUILayout.Width (98));

		comicsDialogueEditor ();

		string[] yxSplit = relativeTypes [relativeTypeIndex].Split (new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
		line ["YTYPE"] = yxSplit [0];
		line ["XTYPE"] = yxSplit [1];

		int absoluteOrRelative = 0;
		BeginH ();
		if (line.actor.canUseRelativeCoordinates) {

			GUILayout.Label ("Balloon position", GUILayout.ExpandWidth (false));
			string[] positionTypes = new string[] { "Absolute", "Relative to avatar" }; 
			absoluteOrRelative = EditorGUILayout.Popup ( (line ["XPIVOT"] == "" ? 1 : 0), positionTypes, GUILayout.Width (141));

		}


		EditorGUILayout.EndHorizontal ();
		BeginHorizontal = true;

		if (absoluteOrRelative == 0) {
			pivotSlides ("0", "0");
		} else {
			line["XPIVOT"] = "";
			line["YPIVOT"] = "";
		}

	}


	void pivotSlides(string defaultX, string defaultY) {


		if (line ["XPIVOT"] == "") {
			line ["XPIVOT"] = defaultX;
			line ["YPIVOT"] = defaultY;
		}

		BeginH ();
		float oldValue = float.Parse (line ["XPIVOT"]);
		GUILayout.Label ("Horizontal Pivot", GUILayout.ExpandWidth (false));
		float newValue = EditorGUILayout.Slider (oldValue, 0, 1, GUILayout.Width (150));
		if (newValue != oldValue) {
			line.actionLastValidLine [line ["ACTION"]] ["XPIVOT"] = line ["XPIVOT"] = newValue.ToString ();
		}
		EndH ();

		BeginH ();
		oldValue = float.Parse (line ["YPIVOT"]);
		GUILayout.Label ("Vertical Pivot", GUILayout.Width (93));
		newValue = EditorGUILayout.Slider (oldValue, 0, 1, GUILayout.Width (150));
		if (newValue != oldValue) {
			line.actionLastValidLine [line ["ACTION"]] ["YPIVOT"] = line ["YPIVOT"] = newValue.ToString ();
		}
		EndH ();

	}


	void text() {
		BeginH();


		bool isSelected = (line["CLICKWAIT"] != "");
		if (isSelected) {
			isSelected = GUILayout.Toggle (isSelected, "Max wait (0 to forever)", GUILayout.Width (149));
		} else {
			isSelected = GUILayout.Toggle (isSelected, "Wait for click to proceed", GUILayout.ExpandWidth (false));

		}

		if (isSelected) {
			if (line ["CLICKWAIT"] == "") {
				line ["CLICKWAIT"] = "0";
			}
			numericTextField ("","CLICKWAIT", GUILayout.MaxWidth (22));	
			GUILayout.Label ("second(s)", GUILayout.ExpandWidth (false));
			EndH ();
			BeginH ();
		} else if (line ["CLICKWAIT"] != "") {
			line ["CLICKWAIT"] = "";
		}


		if (line ["ACTION"] == "subtitles") {
			isSelected = (line ["BLINKRATE"] != "");
			if (isSelected) {
				EndH ();
				BeginH ();
				isSelected = GUILayout.Toggle (isSelected, "Blink text each", GUILayout.ExpandWidth (false));
			} else {
				isSelected = GUILayout.Toggle (isSelected, "Blink text", GUILayout.ExpandWidth (false));
			}

			if (isSelected) {
				if (line ["BLINKRATE"] == "") {
					line ["BLINKRATE"] = "1";
				}
				numericTextField ("", "BLINKRATE", GUILayout.Width (22));	
				GUILayout.Label ("second(s)", GUILayout.ExpandWidth (false));	

			} else if (line ["BLINKRATE"] != "") {
				line ["BLINKRATE"] = "";
			}
		}

		EndH ();
	}

}
#endif


