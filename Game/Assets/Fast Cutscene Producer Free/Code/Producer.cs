using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Producer))]
[CanEditMultipleObjects]
public class ProducerEditor : Editor {
	Producer producer;

	void Awake() {
		producer = (serializedObject.targetObject as Producer);
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update ();	

		DirectorEditor.colorBar(producer.GetComponentsInChildren<Line>());

		DrawDefaultInspector ();

		if (GUILayout.Button ("Add New Cutscene")) {
			Director[] directors = producer.GetComponentsInChildren<Director> ();
			foreach (Director  director in directors) {
				director.gameObject.SetActive (false);
			}

			int cutsceneNumber = directors.Length + 1;
			GameObject cutsceneGameObject = new GameObject ("Cutscene" + cutsceneNumber);
			cutsceneGameObject.transform.SetParent (producer.transform);

			cutsceneGameObject.AddComponent<Director> ();
			Selection.activeGameObject = cutsceneGameObject;
		}
		serializedObject.ApplyModifiedProperties ();
	}
}
#endif

public class Producer : MonoBehaviour {

	public Text subtitlesText, subtitlesHeader;
	public Image subtitlesBackgroundImage;
	public Text leftBottomText, leftTopText, rightBottomText, rightTopText;

	[HideInInspector]public float readingStartDelay = 2f, wordsPerSecond = 4f;
	
	void OnEnable()
	{
		if (Application.isPlaying) {
			Director[] directors = GetComponentsInChildren<Director> ();
			for(int i=1; i<directors.Length; i++) {
				directors[i].gameObject.SetActive (false);
			}
		}
	}

}
