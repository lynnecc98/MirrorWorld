#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Animations;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Actor))]
[CanEditMultipleObjects]
public partial class ActorEditor : Editor {
	
	Actor actor;

	Director director;

	void Awake() {
		actor = (serializedObject.targetObject as Actor);

		director = actor.GetComponent<Director>();

	}


	void colorBar() {

		Line[] lines = director.GetComponentsInChildren<Line>();
		
		GUILayout.Label("", GUILayout.Width(0));
		float x = GUILayoutUtility.GetLastRect ().x;
		float y = GUILayoutUtility.GetLastRect ().y;

		float lineWidth = (EditorGUIUtility.currentViewWidth - 40) / lines.Length;
		int textureHeight = 8;

		for(int i=0; i<lines.Length; i++, x += lineWidth) {
			if (lines[i].actor == actor) {
 				GUI.DrawTexture(new Rect (x, y, lineWidth, textureHeight), lines[i].inspectorColorTextures[lines[i]["ACTION"]]);
			}
			else {
 				GUI.DrawTexture(new Rect (x, y, lineWidth, textureHeight), Line.getSolidTexture(Color.white, 1, 1));
			}	
		}
		
	}

	List<LineName> getLineNames() {
		List<LineName> lineNames = new List<LineName> ();
		lineNames.Add(new LineName(0,0,0,"Insert Line Before..."));
		lineNames.Add(new LineName(0,0,0,""));

		director.initializeCutsceneScriptParts ();
		for (int partIndex = 0; partIndex < director.cutsceneScriptParts.Count; partIndex++) {
			Line[] partLines = director.cutsceneScriptParts[partIndex].GetComponents<Line> ();
			int partLength = partLines.Length;
			string lineNameHeader = "";
			if (director.cutsceneScriptParts.Count > 1) {
				lineNameHeader = "Part " + (partIndex+1) + " Line ";
			}
			for (int lineIndex = 0; lineIndex < partLength; lineIndex++) {
				lineNames.Add(new LineName(partIndex, partLength, lineIndex, lineNameHeader + (lineIndex+1) + " " +  partLines [lineIndex]["ACTOR"] + " " + partLines [lineIndex]["ACTION"]));
			}
		}

		return lineNames;
	}


	public override void OnInspectorGUI()
	{
		serializedObject.Update ();	

		if (actor.isActiveAndEnabled == false) {
			DirectorEditor.enableMessage ();
			serializedObject.ApplyModifiedProperties ();
			return;
		}

		colorBar();

		string newActorName = EditorGUILayout.TextField ("Actor Name", actor.actorName);
		if (newActorName != actor.actorName && newActorName.Trim() != "") {
			actor.actorName = newActorName;
			Line[] lines = director.GetComponentsInChildren<Line> ();
			foreach (Line line in lines) {
				if (line.actor == actor) {
					line ["ACTOR"] = actor.actorName;
					//line.line = line;
				}
			}
		}

		Transform newActorTransform = EditorGUILayout.ObjectField ("Actor Transform", actor.actorTransform, typeof(Transform), true) as Transform;
		if (newActorTransform != actor.actorTransform) {
			actor.updateActorTransform (newActorTransform);
		}


		EditorGUILayout.Separator ();

		if (actor.actorTransform) {
				
			if (director.hasText_subtitlesBalloonsOrBoth != 0 && !(actor.actorTransform is RectTransform)) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("Balloons Coordinates Are Automatic");
				actor.canUseRelativeCoordinates = EditorGUILayout.Toggle (actor.canUseRelativeCoordinates, GUILayout.ExpandWidth (false));
				EditorGUILayout.EndHorizontal ();

				if (actor.canUseRelativeCoordinates) {
					
					GUILayout.Label (" Pivots Rotation Center", GUILayout.ExpandWidth(false));
					float w = 40;
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("X", GUILayout.ExpandWidth (false));
					float x = EditorGUILayout.FloatField (actor.pivotRotationCenter.x, GUILayout.Width(w));
					GUILayout.Label ("Y", GUILayout.ExpandWidth (false));
					float y = EditorGUILayout.FloatField (actor.pivotRotationCenter.y, GUILayout.Width(w));
					GUILayout.Label ("Z", GUILayout.ExpandWidth (false));
					float z = EditorGUILayout.FloatField (actor.pivotRotationCenter.z, GUILayout.Width(w));
					actor.pivotRotationCenter = new Vector3 (x, y, z);
					EditorGUILayout.EndHorizontal ();


					w = 66;
					GUILayout.Label (" Top Balloons Distance From Center", GUILayout.ExpandWidth(false));
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("X", GUILayout.ExpandWidth (false));
					actor.topBalloonXdistance = EditorGUILayout.FloatField (actor.topBalloonXdistance, GUILayout.Width(w));
					GUILayout.Label ("  Y", GUILayout.ExpandWidth (false));
					actor.topBalloonYdistance = EditorGUILayout.FloatField (actor.topBalloonYdistance, GUILayout.Width(w));
					EditorGUILayout.EndHorizontal ();

					GUILayout.Label (" Bottom Balloons Distance From Center", GUILayout.ExpandWidth(false));
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("X", GUILayout.ExpandWidth (false));
					actor.bottomBalloonXdistance = EditorGUILayout.FloatField (actor.bottomBalloonXdistance, GUILayout.Width(w));
					GUILayout.Label ("  Y", GUILayout.ExpandWidth (false));
					actor.bottomBalloonYdistance = EditorGUILayout.FloatField (actor.bottomBalloonYdistance, GUILayout.Width(w));
					EditorGUILayout.EndHorizontal ();

				}

			}

			EditorGUILayout.Separator ();

			if (actor.animator && actor.animatorController) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Label ("Use Animations");
				actor.hasAnimations = EditorGUILayout.Toggle (actor.hasAnimations, GUILayout.ExpandWidth (false));
				EditorGUILayout.EndHorizontal ();
				if (actor.hasAnimations) {
					if (actor.actorTransform.gameObject.activeSelf) {
						AnimatorController animatorController = actor.animator.runtimeAnimatorController as AnimatorController;
				
						for (int i = 0; i < actor.animator.layerCount; i++) {

							EditorGUILayout.BeginHorizontal ();

							GUILayout.Label (" " + animatorController.layers [i].stateMachine.defaultState.name);
							GUILayout.Label ("  (Default Animation)");
							EditorGUILayout.EndHorizontal ();

							ChildAnimatorState[] states = animatorController.layers [i].stateMachine.states;

							for (int n = 0; n < states.Length; n++) {
								if (animatorController.layers [i].stateMachine.defaultState != states [n].state) {
									bool isBeingUsedNthAnimation = actor.usedAnimationStateNames.Contains (states [n].state.name);
									bool useNthAnimation = EditorGUILayout.Toggle (" " + states [n].state.name, isBeingUsedNthAnimation, GUILayout.ExpandWidth (false));
									if (useNthAnimation && !isBeingUsedNthAnimation) {
										actor.usedAnimationStateNames.Add (states [n].state.name);
									} else if (!useNthAnimation && isBeingUsedNthAnimation) {
										actor.usedAnimationStateNames.Remove (states [n].state.name);			
									}
								}
							}

						}
					} else {
						DirectorEditor.enableMessage ("ActorTransform \""+ actor.actorTransform.name +"\" must be\nenabled to select animations");
					}
				}
			}


			if (actor.actorName.Length > 0) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();

				if (GUILayout.Button ("Add Line To Script", GUILayout.ExpandWidth (false))) {
					actor.addLineToCutsceneScript ();
				}

				EditorGUILayout.EndHorizontal ();

				insertLineBefore ();
			}
		}


		if (GUI.changed && !Application.isPlaying) {
			EditorUtility.SetDirty (actor);
			EditorSceneManager.MarkSceneDirty (actor.gameObject.scene);
		}

		serializedObject.ApplyModifiedProperties ();
	}

	struct LineName {
		public int partIndex;
		public int partLength;
		public int lineIndex;
		public string name;

		public LineName(int p, int pl, int li, string n) {
			partIndex = p;
			partLength = pl;
			lineIndex = li;
			name = n;
		}

		public static string ConvertToString(LineName ln) {
			return ln.name;
		}
	}
}


#endif



