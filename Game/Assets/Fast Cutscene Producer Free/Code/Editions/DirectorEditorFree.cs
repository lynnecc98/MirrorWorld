


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


public partial class DirectorEditor : Editor {
	bool hasClickedImportExport = false;
	int selected;


	void importExportButtons() {

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Import Script") || GUILayout.Button ("Export Script")) {
			hasClickedImportExport = true;
		}
		EditorGUILayout.EndHorizontal ();

		if (hasClickedImportExport) {
			pointToProVersion ();
		}
	}


	void playerFastForward() {
		if (Application.isPlaying) {
			EditorGUILayout.Separator ();

			EditorGUILayout.BeginHorizontal ();

			GUILayout.Label ("Play Speed", GUILayout.ExpandWidth (false));

			string[] timeScaleStrings = new string[] { "1x", "2x", "4x", "8x", "16x" };
			selected = GUILayout.Toolbar( selected , timeScaleStrings, GUILayout.Width(175));

			EditorGUILayout.EndHorizontal ();

			if (selected != 0) {
				pointToProVersion ();
			}

		}
	}

	public static void pointToProVersion() {
		EditorGUILayout.Separator ();
		pointToProVersion(true); 
	}

	public static void pointToProVersion(bool isMultiline) {
		

		GUIStyle guiStyle = new GUIStyle ();
		guiStyle.fontStyle = FontStyle.Italic;

		Rect linkRect;

		if (isMultiline) {
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space (55);
			GUILayout.Label ("Feature only available in ", guiStyle, GUILayout.ExpandWidth (false));
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Space (45);
			guiStyle.normal.textColor = Color.blue;
			GUILayout.Label("Fast Cutscene Producer Pro", guiStyle, GUILayout.ExpandWidth (false));
			linkRect = GUILayoutUtility.GetLastRect ();
			EditorGUILayout.EndHorizontal ();
		} else {
			GUILayout.Label ("Feature only available in ", guiStyle, GUILayout.ExpandWidth (false));
			guiStyle.normal.textColor = Color.blue;
			GUILayout.Label("Fast Cutscene Producer Pro", guiStyle, GUILayout.ExpandWidth (false));
			linkRect = GUILayoutUtility.GetLastRect ();
		}

		float xCenter = linkRect.x + linkRect.width/2;
		GUI.DrawTexture(new Rect (xCenter - 80, linkRect.y + linkRect.height, 155, 1), Line.getSolidTexture(Color.blue, 1, 1));

		if (Event.current.type == EventType.MouseUp && linkRect.Contains (Event.current.mousePosition)) {
			Application.OpenURL ("http://u3d.as/1cZW");
		}

		EditorGUILayout.Separator ();
	}

}
#endif

