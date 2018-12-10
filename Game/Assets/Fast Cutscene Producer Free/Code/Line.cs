using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


[ExecuteInEditMode]
public class Line : MonoBehaviour {

	[SerializeField]LineType lineType;

	public string this [string key] {
		get {
			return lineType [key];
		}



		#if UNITY_EDITOR

		set {
			if (lineType [key] != value) {
				if (key == "ANIMATION") {
					value = value.Replace (" ", "_-_-");
				}

				lineType [key] = value;

				switch (key) {
					case "X":
						syncX ();
						break;
					case "Y":
						syncY ();
						break;
					case "Z":
						syncZ ();
						break;
					case "SPEED":
						syncSpeed ();
						break;
					case "ANIMATION":
						syncAnimation ();
					break;
				case "TEXT":
					syncText ();
					break;
				case "FADETIME":
					syncFadeTime ();
					break;
				default:
					if (sharedParams.Contains (key)) {
						syncSharedParam (key);
					} else {
						actionLastValidLine [lineType ["ACTION"]] [key] = lineType [key];
					}
					break;
				}
			}
		}

		#endif



	}
		



	#if UNITY_EDITOR

	public Actor actor;

	public Dictionary<string, Texture2D> inspectorColorTextures;
	public Dictionary<string, Color> actionTextColors;

	string[] sharedParams = new string[] {"ACTOR","INTERVAL","ASYNC"};

	[NonSerialized]public Dictionary<string,LineType> actionLastValidLine;

	int moveEnterExitOrTalkValue;
	int move_gotoOrRotateValue;
	int move_goTo_SteadyAcceleratingOrTeleportingValue;
	int move_rotate_BydegreesToangleOrTowardsxyzValue;
	int talk_subtitlesOrComicsBalloonValue;


	string[] actions = new string[] { "enter", "exit", "comicsTalk", "subtitles", "goTo", "goToAccelerating", "goToTeleporting", "rotateBy", "rotateTo", "rotateTowards"};


	void Awake() {
		if (lineType != null && ((string)lineType).Length > 0) {
			initializeLine ();
		} else {
			if (actor == null) {
				Actor[] actors = GetComponentsInParent<Actor> ();
				foreach (Actor validActor in actors) {
					if (validActor.actorTransform && validActor.actorName.Trim () != "") {
						actor = validActor;
						break;
					}
				}
			}

			setDefaultValidLines (actor);
			lineType = new LineType(actionLastValidLine ["enter"]);
			initMenus ();
			initColors ();
		}
	}
		

	public void setLine(string s) {
		lineType = new LineType (s);
	}


	public static implicit operator string(Line l) {
		return l.lineType;
	}


	public void importLine(string s) {

		lineType = new LineType(s);

		initializeLine ();
	}


	public void initializeLine() {
		actor = GetComponentInParent<Director>().getActorByName (lineType["ACTOR"]);

		setDefaultValidLines (actor);

		actionLastValidLine [lineType ["ACTION"]] = lineType;


		foreach (string actionsSharedParam in sharedParams) {
			syncSharedParam (actionsSharedParam);
		}

		if (lineType ["FADETIME"] != "") {
			syncFadeTime ();
		}
		if (lineType ["TEXT"] != "") {
			syncText ();
		}
		if (lineType ["X"] != "" && lineType ["Y"] != "" && lineType ["Z"] != "") {
			syncX ();
			syncY ();
			syncZ ();
		}
		if (lineType ["SPEED"] != "") {
			syncSpeed ();
		}
		if (lineType ["ANIMATION"] != "") {
			syncAnimation ();
		}


		initMenus ();
		initColors ();
	}

	void syncSharedParam(string actionsSharedParam) {
		foreach (KeyValuePair<string,LineType> anyAction in actionLastValidLine) {
			anyAction.Value [actionsSharedParam] = lineType [actionsSharedParam];
		}
	}

	public void syncFadeTime() {
		actionLastValidLine ["enter"] ["FADETIME"] = actionLastValidLine ["exit"] ["FADETIME"] = lineType ["FADETIME"];	
	}

	public void syncText() {
		actionLastValidLine ["comicsTalk"] ["TEXT"] = actionLastValidLine ["subtitles"] ["TEXT"] = lineType ["TEXT"];	
	}
	public void syncSpeed() {
		actionLastValidLine ["goTo"] ["SPEED"] = actionLastValidLine ["goToAccelerating"] ["SPEED"] = 
			actionLastValidLine ["rotateBy"] ["SPEED"] = actionLastValidLine ["rotateTo"] ["SPEED"] = actionLastValidLine ["rotateTowards"] ["SPEED"] = lineType ["SPEED"];
	}

	public void syncAnimation() {
		actionLastValidLine ["goTo"] ["ANIMATION"] = actionLastValidLine ["goToAccelerating"] ["ANIMATION"] = 
			actionLastValidLine ["rotateBy"] ["ANIMATION"] = actionLastValidLine ["rotateTo"] ["ANIMATION"] = actionLastValidLine ["rotateTowards"] ["ANIMATION"] = lineType ["ANIMATION"];

	}
		
	void syncX() {
		syncXYZ("X");
	}

	void syncY() {
		syncXYZ("Y");
	}


	void syncZ() {
		syncXYZ("Z");
	}
		
	void syncXYZ(string xyOrZ) {
		actionLastValidLine ["goTo"] [xyOrZ] = actionLastValidLine ["goToAccelerating"] [xyOrZ] = actionLastValidLine ["goToTeleporting"] [xyOrZ] = actionLastValidLine ["rotateTowards"] [xyOrZ] = lineType [xyOrZ];
	}

	public void setDefaultLine (Actor actorParam) {
		actor = actorParam;

		setDefaultValidLines (actor);

		lineType = new LineType(actionLastValidLine ["enter"]);
		initMenus ();
		initColors ();
	}


	void initColors() {

		Color orange1 = new Color (1f, 0.5f, 0f); 
		Color orange2 = new Color (0.9f, 0.45f, 0f); 
		Color orange3 = new Color (0.8f, 0.4f, 0f); 
		Color brown1 = new Color (0.5f, 0.25f, 0);
		Color brown2 = new Color (0.35f, 0.175f, 0);
		Color brown3 = new Color (0.2f, 0.1f, 0);
		Color green = new Color (0f, 0.5f, 0f);
		Color darkGreen = new Color (0f, 0.2f, 0f);
		Color darkBlue = new Color (0f, 0f, 0.3f);
		Color[] colors = new Color[] { green, darkGreen, Color.blue, darkBlue, brown1, brown2, brown3, orange1, orange2, orange3 };

		actionTextColors = new Dictionary<string, Color> ();
		inspectorColorTextures = new Dictionary<string, Texture2D> ();
		for (int i = 0; i < actions.Length; i++) {
			actionTextColors.Add (actions [i], colors [i]);
			inspectorColorTextures.Add (actions [i], getSolidTexture(colors[i], 1, 1) );
		}
	}


	public static Texture2D getSolidTexture(Color color, int width, int height) {
		Texture2D texture = new Texture2D (width, height);
		Color[] pixels = Enumerable.Repeat (color, width * height).ToArray ();
		texture.SetPixels (pixels);
		texture.Apply ();
		return texture;
	}


	void initMenus() {
		moveEnterExitOrTalkInit ();
		move_gotoOrRotateInit ();
		move_goTo_SteadyAcceleratingOrTeleportingInit ();
		move_rotate_BydegreesToangleOrTowardsxyzInit ();
		talk_subtitlesOrComicsBalloonInit ();
	}


	public int moveEnterExitOrTalk {
		get {
			return moveEnterExitOrTalkValue;
		}
		set {
			moveEnterExitOrTalkValue = value;
			switch (moveEnterExitOrTalkValue) {
			case 0:
				move_gotoOrRotate = move_gotoOrRotateValue;
				break;
			case 1:
				lineType = new LineType(actionLastValidLine ["enter"]);
				break;
			case 2:
				lineType = new LineType(actionLastValidLine ["exit"]);
				break;
			case 3:
				switch (actor.director.hasText_subtitlesBalloonsOrBoth) {
				case 0:
					lineType = new LineType(actionLastValidLine ["subtitles"]);
					break;
				case 1:
					lineType = new LineType(actionLastValidLine ["comicsTalk"]);
					break;
				case 2:
					talk_subtitlesOrComicsBalloon = talk_subtitlesOrComicsBalloonValue;
					break;
				}
				break;
			}
		}
	}
	void moveEnterExitOrTalkInit() {
		switch (lineType ["ACTION"]) {
		case "subtitles":
			moveEnterExitOrTalkValue = 3;
			break;
		case "comicsTalk":
			moveEnterExitOrTalkValue = 3;
			break;
		case "enter":
			moveEnterExitOrTalkValue = 1;
			break;
		case "exit":
			moveEnterExitOrTalkValue = 2;
			break;
		default:
			moveEnterExitOrTalkValue = 0;
			break;
		}
	}
		

	public int move_gotoOrRotate {
		get {
			return move_gotoOrRotateValue;
		}
		set {
			move_gotoOrRotateValue = value;
			switch (move_gotoOrRotateValue) {
			case 0:
				move_goTo_SteadyAcceleratingOrTeleporting = move_goTo_SteadyAcceleratingOrTeleportingValue;
				break;
			case 1:
				move_rotate_BydegreesToangleOrTowardsxyz = move_rotate_BydegreesToangleOrTowardsxyzValue;
				break;
			}
		}
	}
	void move_gotoOrRotateInit() {
		move_gotoOrRotateValue = lineType ["ACTION"].StartsWith ("rotate") ? 1 : 0;		
	}


	public string[] goTos = new string[] { "goTo", "goToAccelerating", "goToTeleporting"};
	public int move_goTo_SteadyAcceleratingOrTeleporting {
		get {
			return move_goTo_SteadyAcceleratingOrTeleportingValue;
		}
		set {
			lineType = new LineType(actionLastValidLine [goTos[value]]);
			move_goTo_SteadyAcceleratingOrTeleportingValue = value;
		}
	}
	void move_goTo_SteadyAcceleratingOrTeleportingInit() {
		switch(lineType ["ACTION"]) {
		case "goToAccelerating":
			move_goTo_SteadyAcceleratingOrTeleportingValue = 1;
			break;
		case "goToTeleporting":
			move_goTo_SteadyAcceleratingOrTeleportingValue = 2;
			break;
		default:
			move_goTo_SteadyAcceleratingOrTeleportingValue = 0;
			break;
		}
	}


	public string[] rotates = new string[] { "rotateBy", "rotateTo", "rotateTowards"};
	public int move_rotate_BydegreesToangleOrTowardsxyz {
		get {
			return move_rotate_BydegreesToangleOrTowardsxyzValue;
		}
		set {
			lineType = new LineType(actionLastValidLine [rotates[value]]);
			move_rotate_BydegreesToangleOrTowardsxyzValue = value;
		}
	}
	void move_rotate_BydegreesToangleOrTowardsxyzInit() {
		switch(lineType ["ACTION"]) {
		case "rotateTo":
			move_rotate_BydegreesToangleOrTowardsxyzValue = 1;
			break;
		case "rotateTowards":
			move_rotate_BydegreesToangleOrTowardsxyzValue = 2;
			break;
		default:
			move_rotate_BydegreesToangleOrTowardsxyzValue = 0;
			break;
		}
	}


	string[] talks = new string[] { "subtitles", "comicsTalk" };
	public int talk_subtitlesOrComicsBalloon {
		get {
			return talk_subtitlesOrComicsBalloonValue;
		}
		set {
			lineType = new LineType(actionLastValidLine [talks[value]]);
			talk_subtitlesOrComicsBalloonValue = value;
		}
	}
	void talk_subtitlesOrComicsBalloonInit() {
		talk_subtitlesOrComicsBalloonValue = (lineType ["ACTION"] == "comicsTalk") ? 1 : 0;		
	}


	void setDefaultValidLines(Actor actorParam) {
		actionLastValidLine = new Dictionary<string, LineType> ();

		foreach (string action in actions) {
			actionLastValidLine.Add(action, new LineType(getDefaultLine (actorParam, action)));
		}
	}


	string getDefaultLine (Actor actorParam, string action) {

		actor = actorParam;
		string defaultLine = "ACTOR=" + actorParam.actorName + " ACTION=" + action + " ASYNC=FALSE INTERVAL=0";

		switch (action) {
		case "enter":
			break;
		case "exit":
			break;
		case "comicsTalk":
			if (actor.canUseRelativeCoordinates) {
				defaultLine += " XTYPE=right YTYPE=top TEXT=Text";
			} else {
				defaultLine += " XTYPE=right YTYPE=top XPIVOT=0 YPIVOT=0 TEXT=Text";
			}
			break;
		case "subtitles":
			defaultLine += " TEXT=Text";
			break;
		case "rotateBy":
			defaultLine += " ANGLE=360 SPEED=1";
			break;
		case "rotateTo":
			defaultLine += " ANGLE=0 SPEED=1";
			break;
		case "rotateTowards":
			defaultLine += " X="+ actorParam.actorTransform.localPosition.x +" Y="+ actorParam.actorTransform.localPosition.y +" Z="+ actorParam.actorTransform.localPosition.z +" SPEED=1";
			break;
		case "goTo":
			defaultLine += " X=" + actorParam.actorTransform.localPosition.x + " Y=" + actorParam.actorTransform.localPosition.y + " Z=" + actorParam.actorTransform.localPosition.z + " SPEED=1 DESACCEL=FALSE";
			break;
		case "goToAccelerating":
			defaultLine += " X=" + actorParam.actorTransform.localPosition.x + " Y=" + actorParam.actorTransform.localPosition.y + " Z=" + actorParam.actorTransform.localPosition.z + " SPEED=1 ACCELERATION=1";
			break;
		case "goToTeleporting":
			defaultLine += " X=" + actorParam.actorTransform.localPosition.x + " Y=" + actorParam.actorTransform.localPosition.y + " Z=" + actorParam.actorTransform.localPosition.z;
			break;
		}
		return defaultLine;
	}


	void Update(){	} // To enable enable/disable

	#endif




}


[Serializable]
public class LineType {
	// LineType behaves as a string and as a dictionary.
	// It can be assigned to or from strings, or in each of its fields through the operator [].

	[SerializeField]string lineString;

	public string this [string key] {
		get { 
			return getValue (key); 
		}



		#if UNITY_EDITOR

		set {
			if (value != "" || key == "TEXT") {
				setValue (key, value); 
			} else {
				Remove (key);
			}
		}

		#endif



	}

	string getValue(string key) {
		int keyIndex = lineString.IndexOf (key + "=");
		int start = keyIndex + key.Length + 1;
		int end = (key != "TEXT") ? lineString.IndexOf (" ", start) : -1;
		return  (keyIndex == -1) ? "" :
				((end != -1) ? lineString.Substring (start, end - start) : lineString.Substring (start));
	}



	#if UNITY_EDITOR

	[SerializeField]List<string> booleanKeys = new List<string>(new string[] { "ASYNC", "DESACCEL", "KEEPORIGINALROTATION" });
	[SerializeField]List<string> stringKeys = new List<string>(new string[] { "ACTOR","ACTION","XTYPE", "YTYPE", "TEXT", "ANIMATION" });

	public LineType(string lineData) {
		lineString = lineData;

		if (!lineString.Contains ("PART=")) {
			if (!lineString.Contains ("ACTOR=")) {
				lineString = lineString.Insert (0, "ACTOR=");
			}

			if (!lineString.Contains ("ACTION=")) {
				lineString = lineString.Insert (lineString.IndexOf (" ") + 1, "ACTION=");
			}

			foreach (string booleanKey in booleanKeys) {
				if (!lineString.Contains (booleanKey + "=")) { // If it was stored with value in other format.
					if (lineString.Contains (" " + booleanKey)) {
						lineString = lineString.Replace (" " + booleanKey, " " + booleanKey + "=TRUE");
					} else {
						lineString = lineString.Insert (lineString.IndexOf (" ", lineString.IndexOf (" ") + 1), " " + booleanKey + "=FALSE");
					}
				}
			} 
		}
	}


	public string exportString() {
		string exportLine = lineString.Replace ("ACTOR=", "");
		exportLine = exportLine.Replace (" ACTION=", " ");

		foreach (string booleanKey in booleanKeys) {
			exportLine = exportLine.Replace (booleanKey + "=TRUE", booleanKey).Replace("  "," ").Trim();
			exportLine = exportLine.Replace (booleanKey + "=FALSE", "").Replace("  "," ").Trim();
		}

		return exportLine;
	}


	void setValue(string key, string value) {
		if (!stringKeys.Contains (key) && !booleanKeys.Contains (key)) {
			try {
				value = float.Parse (value).ToString ();
				replaceValue (key, value);
			} 
			catch (Exception e) {	}
		} 
		else {
			replaceValue (key, value);
		}
	}

	void replaceValue(string key, string newValue) {
		if (lineString.Contains(key + "=")) {
			lineString = lineString.Replace (key + "=" + getValue (key), key + "=" + newValue);
		} else {
			int insertIndex = lineString.IndexOf (" ", lineString.IndexOf(" ")+1);
			lineString = lineString.Insert (insertIndex, " " + key + "=" + newValue);
		}
	}


	void Remove(string key) {
		if (lineString.Contains (key + "=")) {
			lineString = lineString.Replace (" " + key + "=" + getValue (key), "");
		}
	}


	public static implicit operator string(LineType l) {
		return l.ToString();
	}


	public override string ToString() {
		return lineString;
	}
	#endif

}


