// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityToolbarExtender;

namespace Manul.Toolbar
{
	#region ============== Main Scriptable Object & Classes ==============

	[CreateAssetMenu(fileName = "Manul Toolbar Settings", menuName = "Manul Tools/Manul Toolbar Settings")]
	public class ManulToolbarSettings : ScriptableObject
	{
		public bool settingsExpanded;
		public string currentVersion;
		public ManulToolbarPreferences settings;
		public List<ManulToolbarEntry> leftSide;
		public List<ManulToolbarEntry> rightSide;
	}

	[System.Serializable]
	public class ManulToolbarPreferences
	{
		public float leftBeginOffset;
		public float rightBeginOffset;
		public float betweenOffset;

		public EditorStylesEnum defaultButtonStyle;
		public EditorStylesEnum defaultToggleStyle;
		public EditorStylesEnum defaultLabelStyle;
		public EditorStylesEnum defaultPopupStyle;

		public bool showConsoleMessages;
		public bool disableToolbar;
		public Color mutedColor;

		public OverrideSideType overrideLeftType;
		public ManulToolbarButtonList SOForLeftSide;
		public ManulToolbarButtonListSet SOSetForLeftSide;

		public OverrideSideType overrideRightType;
		public ManulToolbarButtonList SOForRightSide;
		public ManulToolbarButtonListSet SOSetForRightSide;
	}

	[System.Serializable]
	public class ManulToolbarEntry
	{
		public bool isActive = true;
		public bool isExpanded = true;
		public ToolbarEntryType type;

		public ToolbarEntryLabelType labelType;
		public string labelText = "Name";
		public Texture2D labelIcon;

		public bool useStyle;
		public EditorStylesEnum editorStyle;
		public string styleName;
		public GUISkin skin;

		public bool useWidth;
		public float width;

		public bool useColors;
		public Color globalColor = Color.white;
		public Color contentColor = Color.white;
		public Color backgroundColor = Color.white;

		public bool useTooltip;
		public int linesCount;
		public string labelTooltip;

		public string togglePrefName;
		public List<ManulToolbarAction> actions;

		public bool showNameField;

		public ManulToolbarPopupEntry[] intNamesList;

		public ManulToolbarEntry() { }

		public ManulToolbarEntry(string newName, ToolbarButtonType newType, UnityEngine.Object buttonObject, string path, bool useCombo)
		{
			isActive = true;
			isExpanded = true;
			type = ToolbarEntryType.Button;
			labelText = newName;

			actions = new List<ManulToolbarAction>();
			actions.Add(new ManulToolbarAction(ToolbarMouseButton.LMB, newType, buttonObject, path));

			if (useCombo)
			{
				actions.Add(new ManulToolbarAction(ToolbarMouseButton.RMB, ToolbarButtonType.SelectAsset, buttonObject, path));
			}
		}

	}

	[System.Serializable]
	public class ManulToolbarButtonListSetEntry
	{
		public string setName;
		public ManulToolbarButtonList setSO;
	}

	[System.Serializable]
	public class ManulToolbarPopupEntry
	{
		public string itemName;
		public int itemIndex;
	}

	[System.Serializable]
	public class ManulToolbarAction
	{
		public ToolbarMouseButton mouseButton;
		public ToolbarKeyboardButton keyboardButton;
		public ToolbarButtonType buttonType;
		public UnityEngine.Object buttonObject;
		public string objectName;
		public string className;
		public string methodName;

		public ManulToolbarAction() { }

		public ManulToolbarAction(ToolbarMouseButton mouseType, ToolbarButtonType newButtonType, UnityEngine.Object newButtonObject, string newPath)
		{
			mouseButton = ToolbarMouseButton.LMB;
			keyboardButton = ToolbarKeyboardButton.None;
			buttonType = newButtonType;
			buttonObject = newButtonObject;
			className = newPath;
		}
	}

	#region Enums

	public enum ToolbarMouseButton
	{
		LMB,
		RMB,
		MMB
	}

	public enum ToolbarKeyboardButton
	{
		None,
		Ctrl,
		Shift,
		Alt
	}

	public enum ToolbarButtonType
	{
		None,
		OpenAsset,
		SelectAsset,
		ShowAssetInExplorer,
		PropertiesWindow,
		StaticMethod,
		ObjectOfTypeMethod,
		ComponentMethod,
		OpenFolder,
		FindGameObjectInScene,
		ExecuteMenuItem
	}

	public enum ToolbarEntryType
	{
		None,
		Button,
		Toggle,
		Label,
		Popup
	}

	public enum ToolbarEntryLabelType
	{
		Text,
		Icon,
		Both,
		None
	}

	public enum ToolbarEntrySideType
	{
		None,
		Left,
		Right
	}

	public enum OverrideSideType
	{
		None,
		List,
		Set
	}

	#endregion

	#endregion

	#region ===================== Editors & Drawers ======================

	[CustomEditor(typeof(ManulToolbarSettings))]

	public class ManulToolbarSettings_Editor : Editor
	{
		ManulToolbarSettings obj;
		SerializedProperty settings;
		SerializedProperty settingsExpanded;
		SerializedProperty leftSide;
		SerializedProperty rightSide;

		protected void OnEnable()
		{
			obj = (ManulToolbarSettings)target;
			settings = serializedObject.FindProperty("settings");
			leftSide = serializedObject.FindProperty("leftSide");
			rightSide = serializedObject.FindProperty("rightSide");
			settingsExpanded = serializedObject.FindProperty("settingsExpanded");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.BeginHorizontal();
			GUI.Box(new Rect(0, 0, Screen.width, 38), "Manul Toolbar", ManulToolbarStyles.HeaderStyle);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("");
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("");
			EditorGUILayout.EndHorizontal();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();
			settingsExpanded.boolValue = EditorGUILayout.Foldout(settingsExpanded.boolValue, new GUIContent("Settings"), true);
			EditorGUILayout.EndHorizontal();

			if (settingsExpanded.boolValue)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(settings);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(leftSide);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(rightSide);
			EditorGUILayout.EndHorizontal();

			if (EditorGUI.EndChangeCheck())
			{
				ManulToolbar.RefreshToolbar();
				EditorUtility.SetDirty(obj);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}

	[CustomPropertyDrawer(typeof(ManulToolbarPreferences))]
	public class ManulToolbarPreferences_Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;

			/// Offsets

			EditorGUI.LabelField(new Rect(x, y, w, h), "Offsets", EditorStyles.centeredGreyMiniLabel);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			float currentLabelWidth = EditorGUIUtility.labelWidth;

			EditorGUIUtility.labelWidth = 75;

			property.FindPropertyRelative("leftBeginOffset").floatValue = EditorGUI.FloatField(new Rect(x, y, w, h), new GUIContent("Left Begin"), property.FindPropertyRelative("leftBeginOffset").floatValue);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			property.FindPropertyRelative("rightBeginOffset").floatValue = EditorGUI.FloatField(new Rect(x, y, w, h), new GUIContent("Right Begin"), property.FindPropertyRelative("rightBeginOffset").floatValue);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			property.FindPropertyRelative("betweenOffset").floatValue = EditorGUI.FloatField(new Rect(x, y, w, h), new GUIContent("Between"), property.FindPropertyRelative("betweenOffset").floatValue);

			/// Default Styles

			y += 10;

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(new Rect(x, y, w, h), "Default Styles", EditorStyles.centeredGreyMiniLabel);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUIUtility.labelWidth = 55;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultButtonStyle"), new GUIContent("Button"));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultToggleStyle"), new GUIContent("Toggle"));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultLabelStyle"), new GUIContent("Label"));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("defaultPopupStyle"), new GUIContent("Popup"));

			/// Other

			y += 10;

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(new Rect(x, y, w, h), "Other", EditorStyles.centeredGreyMiniLabel);

			EditorGUIUtility.labelWidth = 120;

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("showConsoleMessages"), new GUIContent("Console Messages"));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("disableToolbar"), new GUIContent("Disable Toolbar"));

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("mutedColor"), new GUIContent("Disabled Color"));

			/// Override

			y += 10;

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(new Rect(x, y, w, h), "Override", EditorStyles.centeredGreyMiniLabel);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(new Rect(x, y, w, h), "Use external assets to override button lists:", EditorStyles.helpBox);

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUIUtility.labelWidth = 115;

			float fieldWidth = 190;
			float buttonWidth = 25;

			EditorGUI.PropertyField(new Rect(x, y, fieldWidth - 10, h), property.FindPropertyRelative("overrideLeftType"), new GUIContent("Override Left Side"));

			switch (property.FindPropertyRelative("overrideLeftType").intValue)
			{
				case 0:
					break;

				case 1:
					EditorGUI.PropertyField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), property.FindPropertyRelative("SOForLeftSide"), GUIContent.none);

					if (GUI.Button(new Rect(x + fieldWidth + (w - fieldWidth - buttonWidth) + 7, y + 1, buttonWidth - 7, h), EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, EditorStyles.iconButton))
					{
						if (property.FindPropertyRelative("SOForLeftSide").objectReferenceValue != null)
						{
#if UNITY_2021_1_OR_NEWER
							EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("SOForLeftSide").objectReferenceValue);
#else
							EditorGUIUtility.PingObject(property.FindPropertyRelative("SOForLeftSide").objectReferenceValue);
#endif
						}
					}

					break;

				case 2:
					EditorGUI.PropertyField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), property.FindPropertyRelative("SOSetForLeftSide"), GUIContent.none);

					if (GUI.Button(new Rect(x + fieldWidth + (w - fieldWidth - buttonWidth) + 7, y + 1, buttonWidth - 7, h), EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, EditorStyles.iconButton))
					{
						if (property.FindPropertyRelative("SOSetForLeftSide").objectReferenceValue != null)
						{
#if UNITY_2021_1_OR_NEWER
							EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("SOSetForLeftSide").objectReferenceValue);
#else
							EditorGUIUtility.PingObject(property.FindPropertyRelative("SOSetForLeftSide").objectReferenceValue);
#endif
						}
					}

					break;
			}

			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(new Rect(x, y, 180, h), property.FindPropertyRelative("overrideRightType"), new GUIContent("Override Right Side"));

			switch (property.FindPropertyRelative("overrideRightType").intValue)
			{
				case 0:
					break;

				case 1:
					EditorGUI.PropertyField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), property.FindPropertyRelative("SOForRightSide"), GUIContent.none);

					if (GUI.Button(new Rect(x + fieldWidth + (w - fieldWidth - buttonWidth) + 7, y + 1, buttonWidth - 7, h), EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, EditorStyles.iconButton))
					{
						if (property.FindPropertyRelative("SOForRightSide").objectReferenceValue != null)
						{
#if UNITY_2021_1_OR_NEWER
							EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("SOForRightSide").objectReferenceValue);
#else
							EditorGUIUtility.PingObject(property.FindPropertyRelative("SOForRightSide").objectReferenceValue);
#endif
						}
					}

					break;

				case 2:
					EditorGUI.PropertyField(new Rect(x + fieldWidth, y, w - fieldWidth - buttonWidth, h), property.FindPropertyRelative("SOSetForRightSide"), GUIContent.none);

					if (GUI.Button(new Rect(x + fieldWidth + (w - fieldWidth - buttonWidth) + 7, y + 1, buttonWidth - 7, h), EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, EditorStyles.iconButton))
					{
						if (property.FindPropertyRelative("SOSetForRightSide").objectReferenceValue != null)
						{
#if UNITY_2021_1_OR_NEWER
							EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("SOSetForRightSide").objectReferenceValue);
#else
							EditorGUIUtility.PingObject(property.FindPropertyRelative("SOSetForRightSide").objectReferenceValue);
#endif
						}
					}

					break;
			}

			EditorGUIUtility.labelWidth = currentLabelWidth;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 17 + 40;
		}
	}

	[CustomPropertyDrawer(typeof(ManulToolbarEntry))]
	public class ManulToolbarEntry_Drawer : PropertyDrawer
	{
		Color tempMutedColor;

		SerializedProperty isActive_SP;
		SerializedProperty isExpanded_SP;
		SerializedProperty type_SP;
		SerializedProperty labelType_SP;
		SerializedProperty labelText_SP;
		SerializedProperty style_SP;
		SerializedProperty useStyle_SP;
		SerializedProperty useWidth_SP;
		SerializedProperty useColors_SP;
		SerializedProperty useTooltip_SP;
		SerializedProperty lineCount_SP;
		SerializedProperty actions_SP;
		SerializedProperty intListNames_SP;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;

			isActive_SP = property.FindPropertyRelative("isActive");
			isExpanded_SP = property.FindPropertyRelative("isExpanded");
			type_SP = property.FindPropertyRelative("type");
			labelType_SP = property.FindPropertyRelative("labelType");
			labelText_SP = property.FindPropertyRelative("labelText");
			style_SP = property.FindPropertyRelative("editorStyle");
			useStyle_SP = property.FindPropertyRelative("useStyle");
			useWidth_SP = property.FindPropertyRelative("useWidth");
			useColors_SP = property.FindPropertyRelative("useColors");
			useTooltip_SP = property.FindPropertyRelative("useTooltip");

			EditorGUI.BeginProperty(position, label, property);

			if (!isActive_SP.boolValue)
			{
				tempMutedColor = GUI.color;
				GUI.color = ManulToolbar.settings.settings.mutedColor;
			}

			#region Row 1 - Foldout & Is Active

			isExpanded_SP.boolValue = EditorGUI.Foldout(new Rect(x, y, w - 70 - 15 - 10, h), isExpanded_SP.boolValue, labelText_SP.stringValue, true);
			EditorGUI.PropertyField(new Rect(x += (w - 70 - 15 - 5), y, 70, h), type_SP, GUIContent.none);
			isActive_SP.boolValue = EditorGUI.Toggle(new Rect(x += 75, y, 15, h), isActive_SP.boolValue);

			if (!isExpanded_SP.boolValue)
			{
				if (!isActive_SP.boolValue)
				{
					GUI.color = tempMutedColor;
				}

				EditorGUI.EndProperty();
				return;
			}

			#endregion

			#region Row 2a - Bool Editor Pref	 

			if (type_SP.intValue == 2)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				EditorGUI.LabelField(new Rect(x, y, 135, h), " Editor Pref Bool Name:");
				EditorGUI.PropertyField(new Rect(x += 135, y, w - 135, h), property.FindPropertyRelative("togglePrefName"), new GUIContent(""));
			}

			#endregion

			#region Row 2b - Int Editor Pref	 

			if (type_SP.intValue == 4)
			{
				intListNames_SP = property.FindPropertyRelative("intNamesList");

				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				EditorGUI.LabelField(new Rect(x, y, 125, h), " Editor Pref Int Name:");
				EditorGUI.PropertyField(new Rect(x += 125, y, w - 125 - 50, h), property.FindPropertyRelative("togglePrefName"), new GUIContent(""));

				if (GUI.Button(new Rect(x += (w - 125 - 45), y, 45, h), "Fill"))
				{
					for (int i = 0; i < intListNames_SP.arraySize; i++)
					{
						intListNames_SP.GetArrayElementAtIndex(i).FindPropertyRelative("itemIndex").intValue = i;
					}

					EditorUtility.SetDirty(intListNames_SP.serializedObject.targetObject);
				}

				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				EditorGUI.PropertyField(new Rect(x + 15, y, w - 15, h), intListNames_SP, new GUIContent("Names List: "));

				if (intListNames_SP.isExpanded)
				{
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					y += 5;

					if (intListNames_SP.arraySize > 1)
					{
						for (int i = 1; i < intListNames_SP.arraySize; i++)
						{
							y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
						}
					}
				}
			}

			#endregion

			#region Row 3 - Label Type & Label Text			 

			x = position.x;
			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

			EditorGUI.PropertyField(new Rect(x, y, 55, h), labelType_SP, GUIContent.none);
			EditorGUI.PropertyField(new Rect(x += 60, y, w - 60, h), labelText_SP, GUIContent.none);

			#endregion

			#region Row 4 - Label Icon

			if (labelType_SP.intValue == 1 || labelType_SP.intValue == 2)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("labelIcon"), GUIContent.none);
			}

			#endregion

			#region Row 5 - Toggle Buttons

			x = position.x + 2;
			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
			w = (position.width - 210 - 2) / 4;

			useStyle_SP.boolValue = GUI.Toggle(new Rect(x, y, 45 + w, h), useStyle_SP.boolValue, "Style", EditorStyles.miniButtonMid);
			useWidth_SP.boolValue = GUI.Toggle(new Rect(x += 50 + w, y, 45 + w, h), useWidth_SP.boolValue, "Width", EditorStyles.miniButtonMid);


			EditorGUI.BeginChangeCheck();

			useColors_SP.boolValue = GUI.Toggle(new Rect(x += 50 + w, y, 50 + w, h), useColors_SP.boolValue, "Colors", EditorStyles.miniButtonMid);

			if (EditorGUI.EndChangeCheck())
			{
				if (useColors_SP.boolValue)
				{
					if (property.FindPropertyRelative("globalColor").colorValue == Color.clear &&
						property.FindPropertyRelative("contentColor").colorValue == Color.clear &&
						property.FindPropertyRelative("backgroundColor").colorValue == Color.clear)
					{
						property.FindPropertyRelative("globalColor").colorValue = Color.white;
						property.FindPropertyRelative("contentColor").colorValue = Color.white;
						property.FindPropertyRelative("backgroundColor").colorValue = Color.white;

						property.serializedObject.ApplyModifiedProperties();
					}
				}
			}

			useTooltip_SP.boolValue = GUI.Toggle(new Rect(x += 55 + w, y, 55 + w, h), useTooltip_SP.boolValue, "Tooltip", EditorStyles.miniButtonMid);

			w = position.width;

			#endregion

			#region Row 6, 7 - Style Rows

			if (useStyle_SP.boolValue)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				EditorGUI.LabelField(new Rect(x, y, 38, h), " Style:");

				switch (style_SP.intValue)
				{
					case 1:

						EditorGUI.PropertyField(new Rect(x += 55, y, 100, h), style_SP, GUIContent.none);
						EditorGUI.PropertyField(new Rect(x += 105, y, w - 55 - 105, h), property.FindPropertyRelative("skin"), GUIContent.none);

						x = position.x;
						y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
						w = position.width;

						EditorGUI.LabelField(new Rect(x, y, 38, h), "");
						EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), property.FindPropertyRelative("styleName"), GUIContent.none);

						break;

					default:

						EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), style_SP, GUIContent.none);
						break;
				}
			}

			#endregion

			#region Row 8 - Width Row

			if (useWidth_SP.boolValue)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				float currentLabelWidth = EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth = 54;
				EditorGUI.PropertyField(new Rect(x, y, w, h), property.FindPropertyRelative("width"), new GUIContent(" Width:"));
				EditorGUIUtility.labelWidth = currentLabelWidth;

				x = position.x;

				EditorGUI.LabelField(new Rect(x, y, w, h), new GUIContent(" ", "Fixed width."));
			}

			#endregion

			#region Row 9 - Colors Row

			if (useColors_SP.boolValue)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				w = (position.width - 65) / 3;

				EditorGUI.LabelField(new Rect(x, y, 55, h), " Colors:");

				EditorGUI.PropertyField(new Rect(x += 55, y, w, h), property.FindPropertyRelative("globalColor"), new GUIContent(""));
				EditorGUI.PropertyField(new Rect(x += w + 5, y, w, h), property.FindPropertyRelative("contentColor"), new GUIContent(""));
				EditorGUI.PropertyField(new Rect(x += w + 5, y, w, h), property.FindPropertyRelative("backgroundColor"), new GUIContent(""));

				x = position.x;

				EditorGUI.LabelField(new Rect(x += 55, y, w, h), new GUIContent(" ", "Global GUI color (GUI.color)."));
				EditorGUI.LabelField(new Rect(x += w + 5, y, w, h), new GUIContent(" ", "Content GUI color (GUI.contentColor)."));
				EditorGUI.LabelField(new Rect(x += w + 5, y, w, h), new GUIContent(" ", "Background GUI color (GUI.backgroundColor)."));
			}

			#endregion

			#region Row 10 - Tooltip Row

			if (useTooltip_SP.boolValue)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				w = position.width;

				EditorGUI.LabelField(new Rect(x, y, 50, h), " Tooltip:");

				lineCount_SP = property.FindPropertyRelative("linesCount");

				float tooltipHeight = 0f;

				if (lineCount_SP.intValue > 1)
				{
					tooltipHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * lineCount_SP.intValue +
						(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
				}
				else
				{
					tooltipHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
				}

				property.FindPropertyRelative("labelTooltip").stringValue
					= EditorGUI.TextArea(new Rect(x += 55, y, w - 55, tooltipHeight), property.FindPropertyRelative("labelTooltip").stringValue, EditorStyles.textArea);



				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				h = EditorGUIUtility.singleLineHeight;

				EditorGUI.LabelField(new Rect(x, y, 5, h), " ");
				lineCount_SP.intValue = EditorGUI.IntField(new Rect(x += 5, y, 40, h), lineCount_SP.intValue);

				y += tooltipHeight - 2 * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

			}

			#endregion

			#region Row 11 - Button Actions

			if (type_SP.intValue == 1)
			{
				x = position.x;
				y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				w = position.width;


				EditorGUI.PropertyField(new Rect(x += 15, y, w - 15, h), property.FindPropertyRelative("actions"), new GUIContent("Button Actions"), true);
			}

			#endregion

			if (!isActive_SP.boolValue)
			{
				GUI.color = tempMutedColor;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			isExpanded_SP = property.FindPropertyRelative("isExpanded");
			type_SP = property.FindPropertyRelative("type");
			labelType_SP = property.FindPropertyRelative("labelType");
			style_SP = property.FindPropertyRelative("editorStyle");

			useStyle_SP = property.FindPropertyRelative("useStyle");
			useWidth_SP = property.FindPropertyRelative("useWidth");
			useColors_SP = property.FindPropertyRelative("useColors");
			useTooltip_SP = property.FindPropertyRelative("useTooltip");

			/// Row 1 - Folout & Is Active

			float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 5;

			if (!isExpanded_SP.boolValue) return height;

			/// Row 2a - Toggle Editor Pref

			if (type_SP.intValue == 2)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
			}

			/// Row 2b - Int Editor Pref

			if (type_SP.intValue == 4)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				intListNames_SP = property.FindPropertyRelative("intNamesList");

				if (intListNames_SP.isExpanded)
				{
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += 5;

					if (intListNames_SP.arraySize > 1)
					{
						for (int i = 1; i < intListNames_SP.arraySize; i++)
						{
							height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
						}
					}
				}
			}

			/// Row 3 - Label Type & Label Text

			height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

			/// Row 4 - Label Icon

			if (labelType_SP.intValue == 1 || labelType_SP.intValue == 2)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
			}

			/// Row 5 - Toggle Buttons

			height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

			/// Row 6, 7 - Style Rows

			if (useStyle_SP.boolValue)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

				if (style_SP.intValue == 1)
				{
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				}
			}

			/// Row 8 - Width Row

			if (useWidth_SP.boolValue)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
			}

			/// Row 9 - Colors Row

			if (useColors_SP.boolValue)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
			}

			/// Row 10 - Tooltip Row

			if (useTooltip_SP.boolValue)
			{
				lineCount_SP = property.FindPropertyRelative("linesCount");

				if (lineCount_SP.intValue > 1)
				{
					height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * lineCount_SP.intValue +
								(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
				}
				else
				{
					height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
				}
			}

			/// Row 11 - Button Actions

			if (type_SP.intValue == 1)
			{
				actions_SP = property.FindPropertyRelative("actions");

				if (actions_SP.isExpanded)
				{
					if (actions_SP.arraySize < 1)
					{
						height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3 + 10;
					}
					else
					{
						height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2 + 10;

						for (int i = 0; i < actions_SP.arraySize; i++)
						{
							height += EditorGUI.GetPropertyHeight(actions_SP.GetArrayElementAtIndex(i)) + EditorGUIUtility.standardVerticalSpacing;
						}
					}
				}
				else
				{
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
				}
			}

			return height;
		}
	}

	[CustomPropertyDrawer(typeof(ManulToolbarAction))]
	public class ManulToolbarAction_Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;

			EditorGUI.PropertyField(new Rect(x, y, w - 120, h), property.FindPropertyRelative("buttonType"), GUIContent.none);
			EditorGUI.PropertyField(new Rect(x += (w - 120) + 5, y, 50, h), property.FindPropertyRelative("mouseButton"), GUIContent.none);
			EditorGUI.PropertyField(new Rect(x += 50 + 5, y, 60, h), property.FindPropertyRelative("keyboardButton"), GUIContent.none);

			int buttonOption = property.FindPropertyRelative("buttonType").intValue;

			x = position.x;
			y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

			switch (buttonOption)
			{
				case 1:
				case 2:
				case 3:
				case 4:
					EditorGUI.PropertyField(new Rect(x + 1, y, w - 1, h), property.FindPropertyRelative("buttonObject"), GUIContent.none);
					break;

				case 5:

					EditorGUI.LabelField(new Rect(x, y, 43, h), " Class");
					EditorGUI.PropertyField(new Rect(x += 43, y, w - 43, h), property.FindPropertyRelative("className"), GUIContent.none);

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 55, h), " Method");
					EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), property.FindPropertyRelative("methodName"), GUIContent.none);

					break;

				case 6:

					EditorGUI.LabelField(new Rect(x, y, 40, h), " Type");
					EditorGUI.PropertyField(new Rect(x += 40, y, w - 40, h), property.FindPropertyRelative("className"), GUIContent.none);

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 55, h), " Method");
					EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), property.FindPropertyRelative("methodName"), GUIContent.none);

					break;

				case 7:

					EditorGUI.LabelField(new Rect(x, y, 82, h), " GameObject");
					EditorGUI.PropertyField(new Rect(x += 82, y, w - 82, h), property.FindPropertyRelative("objectName"), GUIContent.none);

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 77, h), " Component");
					EditorGUI.PropertyField(new Rect(x += 77, y, w - 77, h), property.FindPropertyRelative("className"), GUIContent.none);

					x = position.x;
					y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;

					EditorGUI.LabelField(new Rect(x, y, 55, h), " Method");
					EditorGUI.PropertyField(new Rect(x += 55, y, w - 55, h), property.FindPropertyRelative("methodName"), GUIContent.none);

					break;

				case 8:
				case 9:

					EditorGUI.PropertyField(new Rect(x + 1, y, w - 1, h), property.FindPropertyRelative("className"), GUIContent.none);

					break;

				case 10:

					EditorGUI.PropertyField(new Rect(x + 1, y, w - 1 - 28, h), property.FindPropertyRelative("className"), GUIContent.none);

					if (GUI.Button(new Rect(x + 1 + (w - 1 - 23), y, 23, h), EditorGUIUtility.isProSkin ? ManulToolbar.searchIconWhite : ManulToolbar.searchIconBlack))
					{
						ManulToolbarBrowser.OpenWindow(position, property);
					}

					//	property.FindPropertyRelative("className").stringValue = EditorGUI.TextField(new Rect(x + 1, y, w - 1 - 28, h), GUIContent.none, property.FindPropertyRelative("className").stringValue);

					break;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 5;

			int buttonOption = property.FindPropertyRelative("buttonType").intValue;

			switch (buttonOption)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 8:
				case 9:
				case 10:
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					break;

				case 5:
				case 6:
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					break;

				case 7:
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + 2;
					break;
			}

			return height;
		}
	}

	[CustomPropertyDrawer(typeof(ManulToolbarPopupEntry))]
	public class ManulToolbarPopupEntry_Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;

			EditorGUI.PropertyField(new Rect(x, y, w - 50, h), property.FindPropertyRelative("itemName"), GUIContent.none);
			EditorGUI.PropertyField(new Rect(x + (w - 45), y, 45, h), property.FindPropertyRelative("itemIndex"), GUIContent.none);
		}
	}

	[CustomPropertyDrawer(typeof(ManulToolbarButtonListSetEntry))]
	public class ManulToolbarButtonListSetEntry_Drawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			float x = position.x;
			float y = position.y;
			float w = position.width;
			float h = EditorGUIUtility.singleLineHeight;
			float buttonWidth = 25;
			float width = (w / 2) - buttonWidth / 2;

			EditorGUI.PropertyField(new Rect(x, y, width, h), property.FindPropertyRelative("setName"), GUIContent.none);

			EditorGUI.PropertyField(new Rect(x + width + 5f, y, width, h), property.FindPropertyRelative("setSO"), GUIContent.none);

			if (GUI.Button(new Rect(x + width + 5f + width + 5f, y + 1, buttonWidth - 7, h), EditorGUIUtility.isProSkin ? ManulToolbar.openIconWhite : ManulToolbar.openIconBlack, EditorStyles.iconButton))
			{
				if (property.FindPropertyRelative("setSO").objectReferenceValue != null)
				{
#if UNITY_2021_1_OR_NEWER
					EditorUtility.OpenPropertyEditor(property.FindPropertyRelative("setSO").objectReferenceValue);
#else
					EditorGUIUtility.PingObject(property.FindPropertyRelative("setSO").objectReferenceValue);
#endif
				}
			}

		}
	}

	public static class ManulToolbarStyles
	{
		public static GUIStyle HeaderStyle { get; private set; }

		static ManulToolbarStyles()
		{
			HeaderStyle = new GUIStyle(GUI.skin.box);
			HeaderStyle.fontSize = 13;
			HeaderStyle.alignment = TextAnchor.MiddleCenter;
			HeaderStyle.fontStyle = FontStyle.Bold;
			HeaderStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.75f) : new Color(0.15f, 0.15f, 0.15f, 0.75f);
		}
	}

	#endregion

	#region ==================== Manul Toolbar Class =====================

	[InitializeOnLoad]
	static class ManulToolbar
	{
		const string currentVersion = "1.3.3";

		#region --------------- Getters ---------------

		static Color tempGlobalColor;
		static Color tempContentColor;
		static Color tempBackgroundColor;

		static ManulToolbarSettings _settings;
		public static ManulToolbarSettings settings
		{
			get
			{
				if (_settings == null)
				{
					VersionCheck();
				}

				if (_settings == null)
				{
					ManulToolbarMessages.ShowMessage(Message.NoSettingsAsset, MessageType.Warning, null);
					return null;
				}

				return _settings;
			}
		}

		static Texture2D _openIconWhite;
		public static Texture2D openIconWhite
		{
			get
			{
				if (_openIconWhite == null)
				{
					_openIconWhite = Resources.Load("open_icon_white") as Texture2D;
				}

				return _openIconWhite;
			}
		}

		static Texture2D _openIconBlack;
		public static Texture2D openIconBlack
		{
			get
			{
				if (_openIconBlack == null)
				{
					_openIconBlack = Resources.Load("open_icon_black") as Texture2D;
				}

				return _openIconBlack;
			}
		}

		static Texture2D _searchIconWhite;
		public static Texture2D searchIconWhite
		{
			get
			{
				if (_searchIconWhite == null)
				{
					_searchIconWhite = Resources.Load("search_icon_white") as Texture2D;
				}

				return _searchIconWhite;
			}
		}

		static Texture2D _searchIconBlack;
		public static Texture2D searchIconBlack
		{
			get
			{
				if (_searchIconBlack == null)
				{
					_searchIconBlack = Resources.Load("search_icon_black") as Texture2D;
				}

				return _searchIconBlack;
			}
		}

		static Texture2D _browseBorder;
		public static Texture2D browseBorder
		{
			get
			{
				if (_browseBorder == null)
				{
					_browseBorder = Resources.Load("browse_border") as Texture2D;
				}

				return _browseBorder;
			}
		}

		#endregion

		#region -------- Main Handling Methods --------

		public static void RefreshToolbar()
		{
			if (settings == null) return;

			ToolbarCallback.RefreshContainers();
		}

		static ManulToolbar()
		{
			EditorApplication.delayCall -= InitializeToolbar;
			EditorApplication.delayCall += InitializeToolbar;
		}

		static void InitializeToolbar()
		{
			ToolbarExtender.LeftToolbarGUI.Add(HandleToolbarLeftSide);
			ToolbarExtender.RightToolbarGUI.Add(HandleToolbarRightSide);
		}

		static void HandleToolbarLeftSide()
		{
			if (settings == null) return;

			HandleToolbarSide(settings.settings.overrideLeftType, settings.settings.leftBeginOffset, settings.settings.SOSetForLeftSide,
					GetEntries(settings.settings.overrideLeftType, settings.leftSide, settings.settings.SOForLeftSide, settings.settings.SOSetForLeftSide, "left", "Left"));
		}

		static void HandleToolbarRightSide()
		{
			if (settings == null) return;

			HandleToolbarSide(settings.settings.overrideRightType, settings.settings.rightBeginOffset, settings.settings.SOSetForRightSide,
				GetEntries(settings.settings.overrideRightType, settings.rightSide, settings.settings.SOForRightSide, settings.settings.SOSetForRightSide, "right", "Right"));
		}

		static List<ManulToolbarEntry> GetEntries(OverrideSideType overrideType, List<ManulToolbarEntry> defaultSide, ManulToolbarButtonList list, ManulToolbarButtonListSet set, string sideName, string sideNameUpper)
		{
			if (settings == null) return null;

			switch (overrideType)
			{
				case OverrideSideType.None:
					return defaultSide;

				case OverrideSideType.List:

					if (list == null)
					{
						ManulToolbarMessages.ShowMessage(Message.NoButtonListAsset, MessageType.Warning, new string[2] { sideName, sideNameUpper });
						return null;
					}

					if (list.buttons == null)
					{
						list.buttons = new List<ManulToolbarEntry>();
					}

					return list.buttons;


				case OverrideSideType.Set:

					if (set == null)
					{
						ManulToolbarMessages.ShowMessage(Message.NoButtonListSetAsset, MessageType.Warning, new string[2] { sideName, sideNameUpper });
						return null;
					}

					if (string.IsNullOrEmpty(set.intPrefName) || set.intPrefName == "")
					{
						ManulToolbarMessages.ShowMessage(Message.EmptyPrefFieldForSet, MessageType.Warning, new string[1] { sideNameUpper });
						return null;
					}

					int currentIndex = EditorPrefs.GetInt(set.intPrefName, 0);

					if (set.listEntries == null)
					{
						set.listEntries = new List<ManulToolbarButtonListSetEntry>();
					}

					if (currentIndex >= set.listEntries.Count)
					{
						ManulToolbarMessages.ShowMessage(Message.NoIndexInButtonListSetAsset, MessageType.Warning, new string[2] { sideNameUpper, currentIndex.ToString() });
						return null;
					}

					if (set.listEntries[currentIndex].setSO == null)
					{
						ManulToolbarMessages.ShowMessage(Message.NoButtonListAssetInButtonListSetAsset, MessageType.Warning, new string[2] { sideNameUpper, currentIndex.ToString() });
						return null;
					}

					if (set.listEntries[currentIndex].setSO.buttons == null)
					{
						set.listEntries[currentIndex].setSO.buttons = new List<ManulToolbarEntry>();
					}

					return set.listEntries[currentIndex].setSO.buttons;
			}

			return null;
		}

		static void HandleToolbarSide(OverrideSideType overrideType, float beginOffset, ManulToolbarButtonListSet set, List<ManulToolbarEntry> entries)
		{
			if (settings == null) return;

			if (settings.settings.disableToolbar) return;

			if (entries == null) return;

			GUILayout.BeginHorizontal();

			GUILayout.Label(new GUIContent(""), EditorStyles.inspectorDefaultMargins, GUILayout.Width(0));

			GUILayout.Space(beginOffset);

			switch (overrideType)
			{
				case OverrideSideType.None:
				case OverrideSideType.List:
					break;

				case OverrideSideType.Set:

					int popup = EditorPrefs.GetInt(set.intPrefName);

					GUI.changed = false;

					GUIContent[] contentNamesList = new GUIContent[set.listEntries.Count];

					for (int i = 0; i < set.listEntries.Count; i++)
					{
						contentNamesList[i] = new GUIContent(set.listEntries[i].setName);
					}

					popup = EditorGUILayout.Popup(GUIContent.none, popup, contentNamesList, EditorStyles.popup, GetGUILayoutOptions(true, set.popupWidth));

					if (GUI.changed) EditorPrefs.SetInt(set.intPrefName, popup);

					break;
			}

			CreateToolbarSide(entries);

			GUILayout.EndHorizontal();
		}

		#endregion

		#region --------- Create Toolbar Side ---------
		static GUIContent GetGUIContent(ManulToolbarEntry entry)
		{
			switch (entry.labelType)
			{
				case ToolbarEntryLabelType.Text:
					return entry.useTooltip ? new GUIContent(entry.labelText, entry.labelTooltip) : new GUIContent(entry.labelText);

				case ToolbarEntryLabelType.Icon:
					return entry.useTooltip ? new GUIContent(entry.labelIcon, entry.labelTooltip) : new GUIContent(entry.labelIcon);

				case ToolbarEntryLabelType.Both:
					return entry.useTooltip ? new GUIContent(entry.labelText, entry.labelIcon, entry.labelTooltip) : new GUIContent(entry.labelText, entry.labelIcon);
			}

			return GUIContent.none;
		}

		static GUILayoutOption[] GetGUILayoutOptions(bool useWidth, float fixedWidth)
		{
			List<GUILayoutOption> options = new List<GUILayoutOption>();

			options.Add(GUILayout.ExpandWidth(false));

			if (useWidth) options.Add(GUILayout.Width(fixedWidth));

			return options.ToArray();
		}

		static void UseColorStart(ManulToolbarEntry entry)
		{
			if (entry.useColors)
			{
				tempGlobalColor = GUI.color;
				tempContentColor = GUI.contentColor;
				tempBackgroundColor = GUI.backgroundColor;

				GUI.color = entry.globalColor;
				GUI.contentColor = entry.contentColor;
				GUI.backgroundColor = entry.backgroundColor;
			}
		}

		static void UseColorEnd(ManulToolbarEntry entry)
		{
			if (entry.useColors)
			{
				GUI.color = tempGlobalColor;
				GUI.contentColor = tempContentColor;
				GUI.backgroundColor = tempBackgroundColor;
			}
		}

		static void CreateButton(ManulToolbarEntry entry)
		{
			GUIContent guiContent = GetGUIContent(entry);

			GUILayoutOption[] layoutOptions = GetGUILayoutOptions(entry.useWidth, entry.width);

			switch (entry.type)
			{
				#region Toggle

				case ToolbarEntryType.Toggle:

					bool toggle = EditorPrefs.GetBool(entry.togglePrefName);

					GUI.changed = false;

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:

								GUILayout.Toggle(toggle, guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultToggleStyle), layoutOptions);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									GUILayout.Toggle(toggle, guiContent, entry.styleName, layoutOptions);

									GUI.skin = currentSkin;
								}
								else
								{
									GUILayout.Toggle(toggle, guiContent, entry.styleName, layoutOptions);
								}

								break;

							default:

								GUILayout.Toggle(toggle, guiContent, GetEditorStyle.GetStyle(entry.editorStyle), layoutOptions);

								break;
						}
					}
					else
					{
						GUILayout.Toggle(toggle, guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultToggleStyle), layoutOptions);
					}

					if (GUI.changed) EditorPrefs.SetBool(entry.togglePrefName, !toggle);

					break;

				#endregion

				#region Button

				case ToolbarEntryType.Button:

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:

								if (GUILayout.Button(guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultButtonStyle), layoutOptions)) PerformButtonAction(entry);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									if (GUILayout.Button(guiContent, entry.styleName, layoutOptions)) PerformButtonAction(entry);

									GUI.skin = currentSkin;
								}
								else
								{
									if (GUILayout.Button(guiContent, entry.styleName, layoutOptions)) PerformButtonAction(entry);
								}

								break;

							default:

								if (GUILayout.Button(guiContent, GetEditorStyle.GetStyle(entry.editorStyle), layoutOptions)) PerformButtonAction(entry);

								break;
						}
					}
					else
					{
						if (GUILayout.Button(guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultButtonStyle), layoutOptions)) PerformButtonAction(entry);
					}

					break;

				#endregion

				#region Label

				case ToolbarEntryType.Label:

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:

								GUILayout.Label(guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultLabelStyle), layoutOptions);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									GUILayout.Label(guiContent, entry.styleName, layoutOptions);

									GUI.skin = currentSkin;
								}
								else
								{
									GUILayout.Label(guiContent, entry.styleName, layoutOptions);
								}

								break;

							default:

								GUILayout.Label(guiContent, GetEditorStyle.GetStyle(entry.editorStyle), layoutOptions);

								break;
						}
					}
					else
					{
						GUILayout.Label(guiContent, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultLabelStyle), layoutOptions);
					}

					break;

				#endregion

				#region Popup

				case ToolbarEntryType.Popup:

					int popup = EditorPrefs.GetInt(entry.togglePrefName);

					GUI.changed = false;

					GUIContent[] contentNamesList = new GUIContent[entry.intNamesList.Length];
					int[] intList = new int[entry.intNamesList.Length];

					for (int i = 0; i < entry.intNamesList.Length; i++)
					{
						contentNamesList[i] = new GUIContent(entry.intNamesList[i].itemName);
						intList[i] = entry.intNamesList[i].itemIndex;
					}

					if (entry.useStyle)
					{
						switch (entry.editorStyle)
						{
							case EditorStylesEnum.Default:

								popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultPopupStyle), layoutOptions);

								break;

							case EditorStylesEnum.FindByName:

								if (entry.useStyle && entry.skin != null)
								{
									GUISkin currentSkin = GUI.skin;
									GUI.skin = entry.skin;

									popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, entry.styleName, layoutOptions);

									GUI.skin = currentSkin;
								}
								else
								{
									popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, entry.styleName, layoutOptions);
								}

								break;

							default:

								popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, GetEditorStyle.GetStyle(entry.editorStyle), layoutOptions);

								break;
						}
					}
					else
					{
						popup = EditorGUILayout.IntPopup(GUIContent.none, popup, contentNamesList, intList, GetEditorStyle.GetStyle(ManulToolbar.settings.settings.defaultPopupStyle), layoutOptions);
					}

					if (GUI.changed) EditorPrefs.SetInt(entry.togglePrefName, popup);

					break;

					#endregion
			}
		}

		static void CreateToolbarSide(List<ManulToolbarEntry> entries)
		{
			foreach (var entry in entries)
			{
				if (!entry.isActive) continue;

				if (entry.type == ToolbarEntryType.None) continue;

				UseColorStart(entry);

				if (entry.showNameField)
				{
					entry.labelText = GUILayout.TextField(entry.labelText, GUILayout.MaxWidth(150));

					if (GUILayout.Button("OK", GUILayout.Width(27), GUILayout.MinWidth(27), GUILayout.MaxWidth(27)))
					{
						entry.showNameField = false;

						EditorUtility.SetDirty(settings);

						switch (settings.settings.overrideLeftType)
						{
							case OverrideSideType.List:
								if (settings.settings.SOForLeftSide != null) EditorUtility.SetDirty(settings.settings.SOForLeftSide);
								break;

							case OverrideSideType.Set:
								if (settings.settings.SOSetForLeftSide != null) EditorUtility.SetDirty(settings.settings.SOSetForLeftSide);
								break;
						}

						switch (settings.settings.overrideRightType)
						{
							case OverrideSideType.List:
								if (settings.settings.SOForRightSide != null) EditorUtility.SetDirty(settings.settings.SOForRightSide);
								break;

							case OverrideSideType.Set:
								if (settings.settings.SOSetForRightSide != null) EditorUtility.SetDirty(settings.settings.SOSetForRightSide);
								break;
						}
					}
				}
				else
				{
					CreateButton(entry);
				}

				GUILayout.Space(settings.settings.betweenOffset);

				UseColorEnd(entry);
			}
		}

		#endregion

		#region ------- Create & Perform Action -------

		static void PerformButtonAction(ManulToolbarEntry entry)
		{
			Event E = Event.current;

			if (E.button == 0 && E.control && E.shift && !E.alt)
			{
				entry.showNameField = true;
			}
			else if (E.button == 0 && E.control && E.shift && E.alt)
			{
				entry.isActive = false;
			}
			else
			{
				foreach (var a in entry.actions)
				{
					System.Action action = CreateAction(a, entry.labelText);

					if ((E.button == 0 && a.mouseButton == ToolbarMouseButton.LMB) ||
						(E.button == 1 && a.mouseButton == ToolbarMouseButton.RMB) ||
						(E.button == 2 && a.mouseButton == ToolbarMouseButton.MMB))
					{
						if (a.keyboardButton == ToolbarKeyboardButton.None)
						{
							if (!E.control && !E.shift && !E.alt)
							{
								action?.Invoke();
							}
						}
						else
						{
							if ((E.control && a.keyboardButton == ToolbarKeyboardButton.Ctrl) ||
								(E.shift && a.keyboardButton == ToolbarKeyboardButton.Shift) ||
								(E.alt && a.keyboardButton == ToolbarKeyboardButton.Alt))
							{
								action?.Invoke();
							}
						}
					}
				}
			}

			E.Use();
		}

		static System.Action CreateAction(ManulToolbarAction toolbarAction, string buttonName)
		{
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

			switch (toolbarAction.buttonType)
			{
				#region Open Asset

				case ToolbarButtonType.OpenAsset:

					return () =>
					{
						if (toolbarAction.buttonObject == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoObjectForButton, MessageType.Error, new string[] { "Open Asset", buttonName });
							return;
						}

						AssetDatabase.OpenAsset(toolbarAction.buttonObject);
						SceneView.FrameLastActiveSceneView();
					};

				#endregion

				#region Select Asset

				case ToolbarButtonType.SelectAsset:

					return () =>
					{
						if (toolbarAction.buttonObject == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoObjectForButton, MessageType.Error, new string[] { "Select Asset", buttonName });
							return;
						}

						EditorGUIUtility.PingObject(toolbarAction.buttonObject);
					};

				#endregion

				#region Show Asset In Explorer

				case ToolbarButtonType.ShowAssetInExplorer:

					return () =>
					{
						if (toolbarAction.buttonObject == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoObjectForButton, MessageType.Error, new string[] { "Show Asset In Explorer", buttonName });
							return;
						}

						EditorUtility.RevealInFinder(AssetDatabase.GetAssetPath(toolbarAction.buttonObject));
					};

				#endregion

				#region Open Properties Window

				case ToolbarButtonType.PropertiesWindow:

					return () =>
					{
						if (toolbarAction.buttonObject == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoObjectForButton, MessageType.Error, new string[] { "Properties Window", buttonName });
							return;
						}

#if UNITY_2021_1_OR_NEWER
						EditorUtility.OpenPropertyEditor(toolbarAction.buttonObject);
#else
						EditorGUIUtility.PingObject(toolbarAction.buttonObject);
#endif
					};

				#endregion

				#region Open Folder

				case ToolbarButtonType.OpenFolder:

					return () =>
					{
						UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(toolbarAction.className);

						if (obj == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindFolder, MessageType.Error, new string[] { toolbarAction.className, buttonName });
							return;
						}

						ShowFolder(obj.GetInstanceID());
					};

				#endregion

				#region Find and Select GameObject In Scene

				case ToolbarButtonType.FindGameObjectInScene:

					return () =>
					{
						GameObject GO = GameObject.Find(toolbarAction.className);

						if (GO == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindGameObject, MessageType.Error, new string[] { toolbarAction.className, buttonName });
							return;
						}

						Selection.activeObject = GO;
					};

				#endregion

				#region Invoke a Static Method

				case ToolbarButtonType.StaticMethod:

					return () =>
					{
						var actionClass = System.AppDomain.CurrentDomain.GetClass(toolbarAction.className);	 

						if (actionClass == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindClass, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Static Method" });
							return;
						}

						var method = actionClass.GetMethod(toolbarAction.methodName, bindingFlags);

						if (method == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindMethod, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Static Method", toolbarAction.methodName });
							return;
						} 

						method.Invoke(null, null);
					};

				#endregion

				#region Invoke a Method from Component attached to a GameObject

				case ToolbarButtonType.ComponentMethod:

					return () =>
					{
						GameObject GO = GameObject.Find(toolbarAction.objectName);

						if (GO == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindGameObject, MessageType.Error, new string[] { toolbarAction.objectName, buttonName, "Component Method" });
							return;
						}

						var actionClass = System.AppDomain.CurrentDomain.GetClass(toolbarAction.className);

						if (actionClass == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindClass, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Component Method" });
							return;
						}

						UnityEngine.Object obj = GO.GetComponent(actionClass);

						if (obj == null)
						{
							ManulToolbarMessages.ShowMessage(Message.NoComponent, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Component Method" });
							return;
						}

						var method = actionClass.GetMethod(toolbarAction.methodName, bindingFlags);

						if (method == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindMethod, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Component Method", toolbarAction.methodName });
							return;
						}

						obj.GetType().GetMethod(toolbarAction.methodName, bindingFlags).Invoke(obj, null);
					};

				#endregion

				#region Invoke a Method from Object found by Type

				case ToolbarButtonType.ObjectOfTypeMethod:

					return () =>
					{
						var actionClass = System.AppDomain.CurrentDomain.GetClass(toolbarAction.className);

						if (actionClass == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindClass, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Object Of Type Method" });
							return;
						}

						UnityEngine.Object[] objects = UnityEngine.Object.FindObjectsOfType(actionClass, true);

						if (objects.Length < 1)
						{
							ManulToolbarMessages.ShowMessage(Message.NoTypeOnScene, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Object Of Type Method", toolbarAction.methodName });
							return;
						}

						if (objects.Length > 1)
						{
							ManulToolbarMessages.ShowMessage(Message.MoreThanOneTypeOnScene, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Object Of Type Method", toolbarAction.methodName });
							return;
						}

						var method = objects[0].GetType().GetMethod(toolbarAction.methodName, bindingFlags);

						if (method == null)
						{
							ManulToolbarMessages.ShowMessage(Message.CantFindMethod, MessageType.Error, new string[] { toolbarAction.className, buttonName, "Object Of Type Method", toolbarAction.methodName });
							return;
						}

						method.Invoke(objects[0], null);
					};

				#endregion

				#region Execute Menu Item

				case ToolbarButtonType.ExecuteMenuItem:

					return () =>
					{ 
						EditorApplication.ExecuteMenuItem(toolbarAction.className);
					};
 
				#endregion

				default:

					return null;
			}
		}

		#endregion

		#region ----------- Helper Methods ------------

		static System.Type GetClass(this System.AppDomain aAppDomain, string name)
		{
			Assembly[] assemblies = aAppDomain.GetAssemblies();
			foreach (var A in assemblies)
			{
				Type[] types = A.GetTypes();
				foreach (var T in types)
				{
					if (T.FullName == name) return T;
				}
			}
			return null;
		}

		static void ShowFolder(int folderID)
		{
			System.Type projectWindowType = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");

			MethodInfo showFolderContents = projectWindowType.GetMethod("ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic);

			UnityEngine.Object[] projectWindows = UnityEngine.Resources.FindObjectsOfTypeAll(projectWindowType);

			if (projectWindows.Length > 0)
			{
				for (int i = 0; i < projectWindows.Length; i++)
				{
					ShowFolderInternal(projectWindows[i], showFolderContents, folderID);
				}
			}
			else
			{
				EditorWindow projectWindow = EditorWindow.GetWindow(projectWindowType);

				projectWindow.Show();

				MethodInfo initMethod = projectWindowType.GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);

				initMethod.Invoke(projectWindow, null);

				ShowFolderInternal(projectWindow, showFolderContents, folderID);
			}
		}

		static void ShowFolderInternal(UnityEngine.Object projectWindow, MethodInfo showContents, int folderID)
		{
			bool isTwoColumns = new SerializedObject(projectWindow).FindProperty("m_ViewMode").enumValueIndex == 1;

			if (!isTwoColumns)
			{
				MethodInfo setTwoColumns = projectWindow.GetType().GetMethod("SetTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);

				setTwoColumns.Invoke(projectWindow, null);
			}

			showContents.Invoke(projectWindow, new object[] { folderID, true });
		}

		#endregion

		#region ------------ Create Button ------------

		public static void CreateButton(ToolbarEntrySideType side, ToolbarButtonType type, bool useCombo = false)
		{
			if (Selection.objects == null) return;

			if (Selection.objects.Length < 1) return;

			UnityEngine.Object selectedObject = Selection.objects[0];

			string assetPath = AssetDatabase.GetAssetPath(selectedObject);

			string[] splitPath = assetPath.Split("/");

			string folderPath = "";

			for (int i = 0; i < splitPath.Length - 1; i++)
			{
				folderPath += splitPath[i] + "/";
			}

			folderPath = folderPath.Substring(0, folderPath.Length - 1);

			string[] fullAssetName = splitPath[splitPath.Length - 1].Split(".");

			string assetName = fullAssetName[0];

			if (type == ToolbarButtonType.OpenFolder)
			{
				assetName = splitPath[splitPath.Length - 2];
			}

			ManulToolbarEntry entry = new ManulToolbarEntry(assetName, type, selectedObject, folderPath, useCombo);

			List<ManulToolbarEntry> entryList = null;

			switch (side)
			{
				case ToolbarEntrySideType.Left:
					entryList = GetEntries(settings.settings.overrideLeftType, settings.leftSide, settings.settings.SOForLeftSide, settings.settings.SOSetForLeftSide, "left", "Left");
					break;

				case ToolbarEntrySideType.Right:
					entryList = GetEntries(settings.settings.overrideRightType, settings.rightSide, settings.settings.SOForRightSide, settings.settings.SOSetForRightSide, "right", "Right");
					break;
			}

			if (entryList != null)
			{
				entryList.Add(entry);

				switch (side)
				{
					case ToolbarEntrySideType.Left:

						switch (settings.settings.overrideLeftType)
						{
							case OverrideSideType.None:
								EditorUtility.SetDirty(settings);
								break;

							case OverrideSideType.List:
								EditorUtility.SetDirty(settings.settings.SOForLeftSide);
								break;

							case OverrideSideType.Set:
								int currentIndex = EditorPrefs.GetInt(settings.settings.SOSetForLeftSide.intPrefName, 0);
								EditorUtility.SetDirty(settings.settings.SOSetForLeftSide.listEntries[currentIndex].setSO);
								break;
						}

						break;

					case ToolbarEntrySideType.Right:

						switch (settings.settings.overrideLeftType)
						{
							case OverrideSideType.None:
								EditorUtility.SetDirty(settings);
								break;

							case OverrideSideType.List:
								EditorUtility.SetDirty(settings.settings.SOForRightSide);
								break;

							case OverrideSideType.Set:
								int currentIndex = EditorPrefs.GetInt(settings.settings.SOSetForRightSide.intPrefName, 0);
								EditorUtility.SetDirty(settings.settings.SOSetForRightSide.listEntries[currentIndex].setSO);
								break;
						}

						break;
				}
			}
		}

		#endregion

		#region ------------ Version Check ------------

		static void VersionCheck()
		{
			/// Check if new version exists
			
			_settings = Resources.Load("Manul Toolbar") as ManulToolbarSettings; 

			if (_settings != null)
			{
				if (string.IsNullOrEmpty(_settings.currentVersion) || _settings.currentVersion != currentVersion)
				{
					if (EditorUtility.DisplayDialog("Welcome to the MANUL Toolbar!", "The MANUL Toolbar was successfully upgraded to version: " + currentVersion + "\n\n" +
						"Important information: you can move the Manul Toolbar settings asset into any of the 'Resources' folder but remember not to change the name of this asset ('Manul Toolbar').", "OK"))
					{ }

					_settings.currentVersion = currentVersion;
					EditorUtility.SetDirty(settings);
				}
			}

			/// There is no 'new' Manul Toolbar Settings

			else
			{
				ManulToolbarSettings newSettings = ScriptableObject.CreateInstance<ManulToolbarSettings>();

				newSettings.currentVersion = currentVersion;

				ManulToolbarSettings settingsOldVersion = Resources.Load("ManulToolbar") as ManulToolbarSettings;

				/// Installed version 1.3.1 or above

				if (settingsOldVersion != null)
				{
					EditorUtility.CopySerialized(settingsOldVersion, newSettings);
				}

				/// Installed first time

				else
				{
					ManulToolbarSettings settingsDefault = Resources.Load("Manul Toolbar Settings (Default)") as ManulToolbarSettings;

					if (settingsDefault == null)
					{
						if (!EditorPrefs.GetBool("PleaseReimportMessage"))
						{
							EditorPrefs.SetBool("PleaseReimportMessage", true);

							if (EditorUtility.DisplayDialog("Please re-import MANUL Toolbar", "Manul Toolbar Settings (Default) file was not found. Please re-import  package.", "OK")) { }
						} 

						return;
					} 

					EditorUtility.CopySerialized(settingsDefault, newSettings);
				}

				if (!AssetDatabase.IsValidFolder("Assets/Resources"))
				{
					AssetDatabase.CreateFolder("Assets", "Resources");
				}

				AssetDatabase.CreateAsset(newSettings, "Assets/Resources/Manul Toolbar.asset");
				AssetDatabase.SaveAssetIfDirty(newSettings);

				if (settingsOldVersion != null)
				{
					if (EditorUtility.DisplayDialog("Welcome to the MANUL Toolbar!", "Your settings from the previous version were copied to the new 'Manul Toolbar' settings asset located in the Assets/Resources folder. \n\n Would you like to delete the old settings asset (recommended)?", "Yes", "No"))
					{
						string oldSettingsPath = AssetDatabase.GetAssetPath(settingsOldVersion);
						AssetDatabase.DeleteAsset(oldSettingsPath);
					}

					if (EditorUtility.DisplayDialog("Important information", "You can move the new Manul Toolbar settings asset into any of the 'Resources' folder but remember not to change the name of this asset ('Manul Toolbar').", "OK")) { }
				}
				else
				{
					if (EditorUtility.DisplayDialog("Welcome to the MANUL Toolbar!", "Important information: you can move the Manul Toolbar settings asset into any of the 'Resources' folder but remember not to change its name ('Manul Toolbar').", "OK")) { }
				}

				_settings = Resources.Load("Manul Toolbar") as ManulToolbarSettings;
			}
		}

		public static void FindManulToolbar()
		{
			if (settings == null) return;

			GetManulToolbar().actions[1].buttonObject = settings;
		}

		public static ManulToolbarEntry GetManulToolbar()
		{
			if (settings == null) return null;

			int toolbarIndex = -1;

			ToolbarEntrySideType side = ToolbarEntrySideType.None;

			if (settings.rightSide != null)
			{
				for (int i = 0; i < settings.rightSide.Count; i++)
				{
					ManulToolbarEntry entry = settings.rightSide[i];

					if (entry.type == ToolbarEntryType.Button && entry.labelText == "Toolbar" && entry.actions != null)
					{
						if (entry.actions.Count == 2)
						{
							if (entry.actions[0].buttonType == ToolbarButtonType.StaticMethod &&
								(entry.actions[0].className == "ManulToolbar" || entry.actions[0].className == "Manul.Toolbar.ManulToolbar") &&
								entry.actions[0].methodName == "FindManulToolbar" &&
								entry.actions[1].buttonType == ToolbarButtonType.PropertiesWindow)
							{
								side = ToolbarEntrySideType.Right;
								toolbarIndex = i;
							}
						}
					}
				}
			}

			if (toolbarIndex < 0 && settings.leftSide != null)
			{
				for (int i = 0; i < settings.leftSide.Count; i++)
				{
					ManulToolbarEntry entry = settings.leftSide[i];

					if (entry.type == ToolbarEntryType.Button && entry.labelText == "Toolbar" && entry.actions != null)
					{
						if (entry.actions.Count == 2)
						{
							if (entry.actions[0].buttonType == ToolbarButtonType.StaticMethod &&
								(entry.actions[0].className == "ManulToolbar" || entry.actions[0].className == "Manul.Toolbar.ManulToolbar") &&
								entry.actions[0].methodName == "FindManulToolbar" &&
								entry.actions[1].buttonType == ToolbarButtonType.PropertiesWindow)
							{
								side = ToolbarEntrySideType.Left;
								toolbarIndex = i;
							}
						}
					}
				}
			}

			if (toolbarIndex > -1)
			{ 
				switch (side)
				{
					case ToolbarEntrySideType.Left:
						settings.leftSide[toolbarIndex].actions[0].className = "Manul.Toolbar.ManulToolbar";
						return settings.leftSide[toolbarIndex];

					case ToolbarEntrySideType.Right:
						settings.rightSide[toolbarIndex].actions[0].className = "Manul.Toolbar.ManulToolbar";
						return settings.rightSide[toolbarIndex];
				}
			}

			return null;
		}

		#endregion
	}

	#endregion

	#region ======================= Editor Styles ========================

	public enum EditorStylesEnum
	{
		Default,
		FindByName,
		boldLabel,
		centeredGreyMiniLabel,
		colorField,
		foldout,
		foldoutHeader,
		foldoutHeaderIcon,
		foldoutPreDrop,
		helpBox,
		iconButton,
		inspectorDefaultMargins,
		inspectorFullWidthMargins,
		label,
		largeLabel,
		layerMaskField,
		linkLabel,
		miniBoldLabel,
		miniButton,
		miniButtonLeft,
		miniButtonMid,
		miniButtonRight,
		miniLabel,
		miniPullDown,
		miniTextField,
		numberField,
		objectField,
		objectFieldMiniThumb,
		objectFieldThumb,
		popup,
		radioButton,
		selectionRect,
		textArea,
		textField,
		toggle,
		toggleGroup,
		toolbar,
		toolbarButton,
		toolbarDropDown,
		toolbarPopup,
		toolbarSearchField,
		toolbarTextField,
		whiteBoldLabel,
		whiteLabel,
		whiteLargeLabel,
		whiteMiniLabel,
		wordWrappedLabel,
		wordWrappedMiniLabel,
	}
	public static class GetEditorStyle
	{
		public static GUIStyle GetStyle(EditorStylesEnum style)
		{
			switch (style)
			{
				case EditorStylesEnum.boldLabel: return EditorStyles.boldLabel;
				case EditorStylesEnum.centeredGreyMiniLabel: return EditorStyles.centeredGreyMiniLabel;
				case EditorStylesEnum.colorField: return EditorStyles.colorField;
				case EditorStylesEnum.foldout: return EditorStyles.foldout;
				case EditorStylesEnum.foldoutHeader: return EditorStyles.foldoutHeader;
				case EditorStylesEnum.foldoutHeaderIcon: return EditorStyles.foldoutHeaderIcon;
				case EditorStylesEnum.foldoutPreDrop: return EditorStyles.foldoutPreDrop;
				case EditorStylesEnum.helpBox: return EditorStyles.helpBox;

#if UNITY_2021_1_OR_NEWER
				case EditorStylesEnum.iconButton: return EditorStyles.iconButton;
#endif

				case EditorStylesEnum.inspectorDefaultMargins: return EditorStyles.inspectorDefaultMargins;
				case EditorStylesEnum.inspectorFullWidthMargins: return EditorStyles.inspectorFullWidthMargins;
				case EditorStylesEnum.label: return EditorStyles.label;
				case EditorStylesEnum.largeLabel: return EditorStyles.largeLabel;
				case EditorStylesEnum.layerMaskField: return EditorStyles.layerMaskField;

#if UNITY_2020_1_OR_NEWER
				case EditorStylesEnum.linkLabel: return EditorStyles.linkLabel;
#endif
				case EditorStylesEnum.miniBoldLabel: return EditorStyles.miniBoldLabel;
				case EditorStylesEnum.miniButton: return EditorStyles.miniButton;
				case EditorStylesEnum.miniButtonLeft: return EditorStyles.miniButtonLeft;
				case EditorStylesEnum.miniButtonMid: return EditorStyles.miniButtonMid;
				case EditorStylesEnum.miniButtonRight: return EditorStyles.miniButtonRight;
				case EditorStylesEnum.miniLabel: return EditorStyles.miniLabel;
				case EditorStylesEnum.miniPullDown: return EditorStyles.miniPullDown;
				case EditorStylesEnum.miniTextField: return EditorStyles.miniTextField;
				case EditorStylesEnum.numberField: return EditorStyles.numberField;
				case EditorStylesEnum.objectField: return EditorStyles.objectField;
				case EditorStylesEnum.objectFieldMiniThumb: return EditorStyles.objectFieldMiniThumb;
				case EditorStylesEnum.objectFieldThumb: return EditorStyles.objectFieldThumb;
				case EditorStylesEnum.popup: return EditorStyles.popup;
				case EditorStylesEnum.radioButton: return EditorStyles.radioButton;

#if UNITY_2021_1_OR_NEWER
				case EditorStylesEnum.selectionRect: return EditorStyles.selectionRect;
#endif
				case EditorStylesEnum.textArea: return EditorStyles.textArea;
				case EditorStylesEnum.textField: return EditorStyles.textField;
				case EditorStylesEnum.toggle: return EditorStyles.toggle;
				case EditorStylesEnum.toggleGroup: return EditorStyles.toggleGroup;
				case EditorStylesEnum.toolbar: return EditorStyles.toolbar;
				case EditorStylesEnum.toolbarButton: return EditorStyles.toolbarButton;
				case EditorStylesEnum.toolbarDropDown: return EditorStyles.toolbarDropDown;
				case EditorStylesEnum.toolbarPopup: return EditorStyles.toolbarPopup;
				case EditorStylesEnum.toolbarSearchField: return EditorStyles.toolbarSearchField;
				case EditorStylesEnum.toolbarTextField: return EditorStyles.toolbarTextField;
				case EditorStylesEnum.whiteBoldLabel: return EditorStyles.whiteBoldLabel;
				case EditorStylesEnum.whiteLabel: return EditorStyles.whiteLabel;
				case EditorStylesEnum.whiteLargeLabel: return EditorStyles.whiteLargeLabel;
				case EditorStylesEnum.whiteMiniLabel: return EditorStyles.whiteMiniLabel;
				case EditorStylesEnum.wordWrappedLabel: return EditorStyles.wordWrappedLabel;
				case EditorStylesEnum.wordWrappedMiniLabel: return EditorStyles.wordWrappedMiniLabel;

				default: return EditorStyles.label;
			}
		}
	}

	#endregion
}

#endif