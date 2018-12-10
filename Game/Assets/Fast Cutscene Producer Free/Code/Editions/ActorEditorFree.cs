

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Animations;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System;
using UnityEditor.SceneManagement;


public partial class ActorEditor : Editor {

	bool hasSelectedInsertLineToBefore = false;

	void insertLineBefore() {

		List<LineName> lineNames = getLineNames ();
		List<string> lineNameStrings = lineNames.ConvertAll (new Converter<LineName, string> (LineName.ConvertToString));

		if (lineNames.Count > 2) {
			EditorGUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();

			int lineSelectedIndex = EditorGUILayout.Popup (0, lineNameStrings.ToArray(), GUILayout.MaxWidth (150));
			if (lineSelectedIndex > 1) {
				hasSelectedInsertLineToBefore = true;
			}
			EditorGUILayout.EndHorizontal ();


			if (hasSelectedInsertLineToBefore) {
				DirectorEditor.pointToProVersion ();
			}
		}

	}
}

#endif


