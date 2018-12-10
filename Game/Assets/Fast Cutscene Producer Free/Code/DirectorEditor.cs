
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEditor.SceneManagement;


[CustomEditor(typeof(Director))]
[CanEditMultipleObjects]
public partial class DirectorEditor : Editor {

	Director director;
	Producer producer;

	void Awake() {
		director = (serializedObject.targetObject as Director);
		producer = director.GetComponentInParent<Producer> ();
	}


	public static void colorBar(Line[] lines) {
		
		GUILayout.Label("", GUILayout.Width(0));
		float x = GUILayoutUtility.GetLastRect ().x;
		float y = GUILayoutUtility.GetLastRect ().y;
 
		float lineWidth = (EditorGUIUtility.currentViewWidth - 40) / lines.Length;
		int textureHeight = 8;

		for(int i=0; i<lines.Length; i++, x += lineWidth) {
 			GUI.DrawTexture(new Rect (x, y, lineWidth, textureHeight), lines[i].inspectorColorTextures[lines[i]["ACTION"]]);	
		}
		
	}


	public override void OnInspectorGUI()
	{

		serializedObject.Update ();	

		if (director.isActiveAndEnabled == false) {
			enableMessage ();

			serializedObject.ApplyModifiedProperties ();
			return;
		}

		if (director == null) {
			return;
		}

		colorBar(director.GetComponentsInChildren<Line>());

		director.hasText = EditorGUILayout.Toggle ("Cutscene Has Text", director.hasText, GUILayout.ExpandWidth (false));


		if (director.hasText) {
			

			producer.readingStartDelay = EditorGUILayout.Slider("Reading Start Delay", producer.readingStartDelay, 0, 3);
			producer.wordsPerSecond = EditorGUILayout.Slider("Words Per Second", producer.wordsPerSecond, 2, 10);

			GUILayout.Label (" Text Type");

			string[] toolbarSubtitlesBalloonsOrBoth = new string[] { "Subtitles", "Balloons", "Both" };
			director.hasText_subtitlesBalloonsOrBoth = GUILayout.Toolbar (director.hasText_subtitlesBalloonsOrBoth, toolbarSubtitlesBalloonsOrBoth);

			if (director.hasText_subtitlesBalloonsOrBoth != 0) {

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("Custom Comics Style", GUILayout.ExpandWidth (false));
				director.hasCustomComicsStyle = EditorGUILayout.Toggle (director.hasCustomComicsStyle, GUILayout.ExpandWidth (false));
				EditorGUILayout.EndHorizontal ();

				if (director.hasCustomComicsStyle) {
					
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("Comics Font", GUILayout.Width (81));
					director.comicsFont = EditorGUILayout.ObjectField (director.comicsFont, typeof(Font), false) as Font;
					GUILayout.Label ("Size");
					director.comicsFontSize = EditorGUILayout.IntField (director.comicsFontSize, GUILayout.Width (25));
					EditorGUILayout.EndHorizontal ();
				}

			}

			if (director.hasText_subtitlesBalloonsOrBoth != 1) {

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("Custom Subtitles Style", GUILayout.ExpandWidth (false));
				director.hasCustomSubtitlesStyle = EditorGUILayout.Toggle (director.hasCustomSubtitlesStyle, GUILayout.ExpandWidth (false));
				EditorGUILayout.EndHorizontal ();

				if (director.hasCustomSubtitlesStyle) {
					
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("Subtitles Font");
					director.subtitlesFont = EditorGUILayout.ObjectField (director.subtitlesFont, typeof(Font), false) as Font;
					GUILayout.Label ("Size");
					director.subtitlesFontSize = EditorGUILayout.IntField (director.subtitlesFontSize, GUILayout.Width (25));
					EditorGUILayout.EndHorizontal ();

					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("Custom Background", GUILayout.ExpandWidth (false));
					director.hasBackgroundImage = EditorGUILayout.Toggle (director.hasBackgroundImage, GUILayout.Width (20));
					if (director.hasBackgroundImage) {
						director.subtitlesBackgroundSprite = EditorGUILayout.ObjectField (director.subtitlesBackgroundSprite, typeof(Sprite), false, GUILayout.ExpandWidth (true)) as Sprite;
					} 
					EditorGUILayout.EndHorizontal ();

					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("Actor Name", GUILayout.ExpandWidth (false));
					director.hasActorNameHeader = EditorGUILayout.Toggle (director.hasActorNameHeader, GUILayout.Width (20));

					if (director.hasActorNameHeader) {
						GUILayout.Label ("Font", GUILayout.ExpandWidth (false));
						director.subtitlesHeaderFont = EditorGUILayout.ObjectField (director.subtitlesHeaderFont, typeof(Font), false) as Font;

						GUILayout.Label ("Size", GUILayout.ExpandWidth (false));
						director.subtitlesHeaderFontSize = EditorGUILayout.IntField (director.subtitlesHeaderFontSize, GUILayout.Width (25));
					}
					EditorGUILayout.EndHorizontal ();
				}
			}				
		} 

		EditorGUILayout.Separator ();

		importExportButtons ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Lock Script Layout View", GUILayout.ExpandWidth (false));
		director.isLayoutFlexible = !EditorGUILayout.Toggle (!director.isLayoutFlexible, GUILayout.ExpandWidth (false));
		EditorGUILayout.EndHorizontal ();

		if (director.isLayoutFlexible == false) {
			GUILayout.Label ("Script Layout View");
			director.scriptLayout = (LineEditor.ScriptLayout)GUILayout.Toolbar ((int)director.scriptLayout, new string[] { "Tall", "Medium", "Wide" });
		}

		if (GUILayout.Button ("Add New Chapter to Cutscene Script")) {
			Selection.activeGameObject = director.createNewCutsceneScriptPart();
		}

		if (GUILayout.Button ("Add Actor To Cast")) {
			int actorNumber = director.GetComponents<Actor> ().Length + 1;
			Actor actor = director.gameObject.AddComponent<Actor>();
			actor.actorName = "Actor" + actorNumber;
		}
			
		playerFastForward ();

		if (GUI.changed && !Application.isPlaying) {
			EditorUtility.SetDirty (director);
			EditorUtility.SetDirty (producer);
			EditorSceneManager.MarkSceneDirty (director.gameObject.scene);
		}

		serializedObject.ApplyModifiedProperties ();
	}





	public static void enableMessage() {
		enableMessage ("Enable game object to edit parameters");
	}

	public static void enableMessage(string message) {
		GUIStyle italicStyle = new GUIStyle ();
		italicStyle.fontStyle = FontStyle.Italic;
		italicStyle.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label ("\n" + message +"\n", italicStyle, GUILayout.ExpandWidth (true));
	}

}
#endif
