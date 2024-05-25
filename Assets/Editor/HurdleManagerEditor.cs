using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HurdleManager))]
public class HurdleManagerEditor : Editor
{
	private bool expandButtons = false;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		HurdleManager hurdleManager = (HurdleManager)target;

		expandButtons = EditorGUILayout.BeginFoldoutHeaderGroup(expandButtons, "Expand Buttons");

		if (expandButtons)
		{
			if (hurdleManager.tileDataSO.hurdlePlacementIndex.Length != 100) hurdleManager.tileDataSO.hurdlePlacementIndex = new bool[100];

			GUILayout.Space(5);

			for (int x = 0; x < hurdleManager.width; x++)
			{
				EditorGUILayout.BeginHorizontal();

				for (int y = 0; y < hurdleManager.height; y++)
				{
					hurdleManager.tileDataSO.hurdlePlacementIndex[x * hurdleManager.width + y] = GUILayout.Toggle(hurdleManager.tileDataSO.hurdlePlacementIndex[x * 10 + y], $"{x}.{y}", "Button", GUILayout.Width(30), GUILayout.Height(30));
				}

				EditorGUILayout.EndHorizontal();
			}
		}

		GUILayout.Space(5);

		if (GUILayout.Button("Place Hurdles", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
		{
			hurdleManager.tileDataSO.RaiseTileMapChanged();
			hurdleManager.PlaceHurdles();
		}
	}
}
