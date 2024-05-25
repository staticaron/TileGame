using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilemapGenerator))]
public class TilemapGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		TilemapGenerator tilemapGenerator = (TilemapGenerator)target;

		GUILayout.Space(5);

		if (GUILayout.Button("Generate Tile Map", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
		{
			tilemapGenerator.Generate();
		}
	}
}
