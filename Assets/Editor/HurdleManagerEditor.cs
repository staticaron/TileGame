using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HurdleManager))]
public class HurdleManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		HurdleManager hurdleManager = (HurdleManager)target;

		if (hurdleManager.tileDataSO.hurdlePlacementIndex.Length != 100) hurdleManager.tileDataSO.hurdlePlacementIndex = new bool[100];

		for (int x = 0; x < hurdleManager.width; x++)
		{
			EditorGUILayout.BeginHorizontal();

			for (int y = 0; y < hurdleManager.height; y++)
			{
				hurdleManager.tileDataSO.hurdlePlacementIndex[x * hurdleManager.width + y] = GUILayout.Toggle(hurdleManager.tileDataSO.hurdlePlacementIndex[x * 10 + y], $"{x}, {y}", "Button", GUILayout.Width(40), GUILayout.Height(40));
			}

			EditorGUILayout.EndHorizontal();
		}

		GUILayout.Space(10);

		if (GUILayout.Button("Place Hurdles", GUILayout.ExpandWidth(true), GUILayout.Height(40)))
		{
			hurdleManager.PlaceHurdles();
		}
	}
}
