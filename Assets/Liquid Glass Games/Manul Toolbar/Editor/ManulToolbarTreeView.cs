// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;

namespace Manul.Toolbar
{ 
	public class ManulToolbarTreeView : UnityEditor.IMGUI.Controls.TreeView
	{
		private Dictionary<TreeViewItem, TreeViewItemType> treeViewItemDictionary;
		private List<TreeViewItem> allItems;
		private int index;

		public string SelectedString { get; set; }
		public string DoubleClickedString { get; set; }

		public enum TreeViewItemType
		{
			ShowGroups,
			Group,
			String,
			Root
		}

		public ManulToolbarTreeView(TreeViewState treeViewState) : base(treeViewState)
		{
			Reload();
		}

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return false;
		}

		protected override TreeViewItem BuildRoot()
		{
			var root = new TreeViewItem { id = 0, depth = -1, displayName = "Default" };
			treeViewItemDictionary = new Dictionary<TreeViewItem, TreeViewItemType>();
			treeViewItemDictionary.Add(root, TreeViewItemType.Root);
			SetupParentsAndChildrenFromDepths(root, CreteTreeViewList(root));
			return root;
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			base.RowGUI(args);

			TreeViewItem item = args.item;

			if (Event.current.type == EventType.MouseUp && treeViewItemDictionary[item] == TreeViewItemType.Group)
			{
				Rect rect = args.rowRect;
				rect.xMin = GetContentIndent(item);

				if (rect.Contains(Event.current.mousePosition))
				{
					SetExpanded(item.id, !IsExpanded(item.id));
					Event.current.Use();
				}
			}
		}

		protected override void SelectionChanged(IList<int> selectedIDs)
		{
			SelectedString = "";

			if (selectedIDs.Count > 0)
			{
				TreeViewItem item = FindItem(selectedIDs[0], rootItem);

				switch (treeViewItemDictionary[item])
				{
					case TreeViewItemType.String:
						SelectedString = item.displayName;
						break;
				}
			}
		}
		protected override void DoubleClickedItem(int id)
		{
			DoubleClickedString = "";

			TreeViewItem item = FindItem(id, rootItem);

			switch (treeViewItemDictionary[item])
			{
				case TreeViewItemType.String:
					DoubleClickedString = item.displayName;
					break;
			}
		}
		void CreateItem(string name, TreeViewItemType type)
		{
			int depth = type == TreeViewItemType.String ? 1 : 0;
			TreeViewItem item = new TreeViewItem { id = index, depth = depth, displayName = name };
			allItems.Add(item);
			treeViewItemDictionary.Add(item, type);
			index++;
		}
		void CreateItemList(List<string> list)
		{
			List<string> newList = new List<string>();

			for (int j = 0; j < list.Count; j++)
			{
				if (!newList.Contains(list[j]))
				{
					newList.Add(list[j]);
				}
			}

			newList.Sort();

			for (int j = 0; j < newList.Count; j++)
			{
				CreateItem(newList[j], TreeViewItemType.String);
			}
		}
		List<TreeViewItem> CreteTreeViewList(TreeViewItem root)
		{
			index = 1;
			allItems = new List<TreeViewItem>();
			root.displayName = "Search for Menu Item"; 
			CreateItemList(GetInspectorsListForBrowser());
			return allItems;
		}

		static List<string> GetInspectorsListForBrowser()
		{
			List<string> newList = new List<string>();
			IEnumerable<string> shortucuts = UnityEditor.ShortcutManagement.ShortcutManager.instance.GetAvailableShortcutIds();

			foreach (var shortcut in shortucuts)
			{
				if (shortcut.Contains("Main Menu"))
				{
					newList.Add(shortcut.Replace("Main Menu/", ""));
				}		
			} 

			return newList;
		}
	}

	public class ManulToolbarBrowser : EditorWindow
	{
		[NonSerialized] public ManulToolbarTreeView treeView;
		[NonSerialized] public SearchField searchField;

		private Texture2D borderIcon;
		private GUIStyle borderStyle;
		private SerializedProperty textProperty;


		public static void OpenWindow(Rect originalRect, SerializedProperty property)
		{
			ManulToolbarBrowser eventBrowser = ScriptableObject.CreateInstance<ManulToolbarBrowser>();
			eventBrowser.textProperty = property;
			eventBrowser.searchField.SetFocus();

			var windowRect = originalRect;
			windowRect.position = GUIUtility.GUIToScreenPoint(windowRect.position);
			windowRect.y -= 29f;

			eventBrowser.ShowAsDropDown(windowRect, new Vector2(originalRect.width - 28f, 300f));
		}

		public void OnEnable()
		{
			searchField = new SearchField();
			treeView = new ManulToolbarTreeView(new TreeViewState());
			treeView.DoubleClickedString = "";
			treeView.SelectedString = "";
			treeView.Reload();
			searchField.downOrUpArrowKeyPressed += treeView.SetFocus;
		}

		private void AffirmResources()
		{
			if (borderIcon == null)
			{
				borderIcon = ManulToolbar.browseBorder;
				borderStyle = new GUIStyle(GUI.skin.box);
				borderStyle.normal.background = borderIcon;
				borderStyle.margin = new RectOffset();
			}
		}

		void OnGUI()
		{
			AffirmResources();

			GUILayout.BeginVertical(borderStyle, GUILayout.ExpandWidth(true));
			{
				if (string.IsNullOrEmpty(treeView.searchString)) treeView.searchString = "";

				treeView.searchString = searchField.OnGUI(treeView.searchString);

				Rect treeRect = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
				treeRect.y += 2;
				treeRect.height -= 2;

				treeView.OnGUI(treeRect);
			}

			GUILayout.EndVertical();

			HandleChooserModeEvents();
		}

		void HandleChooserModeEvents()
		{
			if (Event.current.isKey)
			{
				KeyCode keyCode = Event.current.keyCode;

				if ((keyCode == KeyCode.Return || keyCode == KeyCode.KeypadEnter) && treeView.SelectedString != "")
				{
					SetOutputProperty(treeView.SelectedString);
					Event.current.Use();
					Close();
				}
				else if (keyCode == KeyCode.Escape)
				{
					Event.current.Use();
					Close();
				}
			}
			else if (treeView.DoubleClickedString != "")
			{
				SetOutputProperty(treeView.DoubleClickedString);
				Close();
			}
		}

		private void SetOutputProperty(string data)
		{ 
			textProperty.FindPropertyRelative("className").stringValue = data;
			textProperty.FindPropertyRelative("className").serializedObject.ApplyModifiedProperties();
		} 
	}
}

#endif