using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{
	[SerializeField] int width = 10;
	[SerializeField] int height = 10;
	[SerializeField] Vector2 spacing = default;

	[SerializeField] GameObject tilePrefab;
	[SerializeField] Transform tileContainer;
	[SerializeField] Material normalMaterial;
	[SerializeField] Material highlightMaterial;

	[SerializeField] GameObject hurdlePrefab;
	[SerializeField] Transform hurdleContainer;

	[SerializeField] List<GameObject> tiles = default;
	[SerializeField] List<GameObject> hurdles = default;

	private bool wasChanged = false;

	private void Start()
	{
		Generate();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return)) Generate();
	}

	// Generate the tilemap by placing w*h objects in x-z fashion.
	[ContextMenu("Generate Tilemap")]
	public void Generate()
	{
		tiles = new List<GameObject>();

		#region Clear Existing Tiles

		if (Application.isEditor) DestroyImmediate(tileContainer.gameObject);
		else Destroy(tileContainer.gameObject);

		GameObject gO = new GameObject("Tile Container");
		gO.transform.position = Vector3.zero;
		gO.transform.rotation = Quaternion.identity;
		gO.transform.parent = transform;

		tileContainer = gO.transform;

		tiles.Clear();

		#endregion

		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				Transform tile = Instantiate(tilePrefab, transform.position, Quaternion.identity).transform;
				tile.name = $"Tile.{x * width + z}";

				tile.transform.position += new Vector3(x * spacing.x, 0, z * spacing.y);
				tile.transform.parent = tileContainer;

				tiles.Add(tile.gameObject);
			}
		}
	}

	public Vector3 GetPositionFromIndex(Vector2Int index)
	{
		return transform.position + new Vector3(index.x * spacing.x, 0, index.y * spacing.y);
	}

	// Tile under the move cusor is highlighted. 
	public Vector2Int? HighlightTile(int? tileObjectID)
	{
		// if the current objID is null, and the tile were not modified in the previous frame, do nothing.
		if (!tileObjectID.HasValue && wasChanged == false) return null;

		// if the tiles were modified in the previous frame and the current objID is null, we have to set all the tiles to normal.
		if (!tileObjectID.HasValue && wasChanged == true)
		{
			foreach (GameObject g in tiles)
			{
				g.GetComponent<Renderer>().sharedMaterial = normalMaterial;
			}

			wasChanged = false;

			return null;
		}

		// Highlight the underlying tile.
		Vector2Int index = Vector2Int.zero;

		for (int x = 0; x < tiles.Count; x++)
		{
			if (tiles[x].GetInstanceID() == tileObjectID.Value)
			{
				tiles[x].GetComponent<Renderer>().sharedMaterial = highlightMaterial;
				index = new Vector2Int(x / 10, x % 10);
			}
			else
			{
				tiles[x].GetComponent<Renderer>().sharedMaterial = normalMaterial;
			}
		}

		wasChanged = true;

		return index;
	}

	// Read the SO to accurately place the hurdles on the map. 
	public void PlaceHurdles(bool[] placementIndices)
	{
		#region Clear Existing Hurdles

		if (Application.isEditor) DestroyImmediate(hurdleContainer.gameObject);
		else Destroy(hurdleContainer.gameObject);

		GameObject gO = new GameObject("Hurdle Container");
		gO.transform.position = Vector3.zero;
		gO.transform.rotation = Quaternion.identity;
		gO.transform.parent = transform;

		hurdleContainer = gO.transform;

		hurdles.Clear();

		#endregion

		for (int x = 0; x < placementIndices.Length; x++)
		{
			if (placementIndices[x] == false) continue;

			GameObject instance = Instantiate(hurdlePrefab, transform.position, Quaternion.identity);
			instance.transform.position += new Vector3((x / width) * spacing.x, 1, (x % width) * spacing.y);
			instance.transform.parent = hurdleContainer;
			instance.name = $"Hurdle.{x}";

			hurdles.Add(instance);
		}
	}
}
