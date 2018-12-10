

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;


public partial class LineEditor : Editor {

	bool hasSelectedMoveLineToBefore = false;
	bool hasClickedDialogueEditor = false;


	void moveLineToBefore() {
		EditorGUILayout.BeginHorizontal ();

		GUILayout.Label ("", GUILayout.Width (18));
		GUILayout.Label ("Move line to before ", GUILayout.ExpandWidth (false));

		int lineOrder = new List<Line> (line.GetComponents<Line> ()).IndexOf (line);
		int lineSelectedIndex = EditorGUILayout.Popup (lineOrder, getLineNames (), GUILayout.Width ((scriptLayout == ScriptLayout.wide) ? 105 : 94));

		if (lineSelectedIndex != lineOrder) {
			hasSelectedMoveLineToBefore = true;
		}
		if (hasSelectedMoveLineToBefore) {
			EndH ();
			DirectorEditor.pointToProVersion (scriptLayout == ScriptLayout.tall);

			BeginH ();
		}

		EndH ();
	}


	void subtitlesDialogueEditor() {
		if (GUILayout.Button ("Dialogue Editor", GUILayout.ExpandWidth (false))) {
			hasClickedDialogueEditor = true;
		}

		EditorGUILayout.EndHorizontal ();
		BeginHorizontal = true;

		if (hasClickedDialogueEditor) {
			DirectorEditor.pointToProVersion ();
		}

	}


	void comicsDialogueEditor() {
		if (GUILayout.Button ("Dialogue Editor", GUILayout.ExpandWidth (false))) {
			hasClickedDialogueEditor = true;
		}

		EndH ();

		if (hasClickedDialogueEditor) {
			DirectorEditor.pointToProVersion ();
		}

	}


}

#endif

