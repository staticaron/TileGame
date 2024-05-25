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

	[ContextMenu("Generate Tilemap")]
	public void Generate()
	{
		tiles = new List<GameObject>();

		foreach (Transform t in tileContainer)
		{
			Destroy(t.gameObject);
		}

		tiles.Clear();

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

	public Vector2? HighlightTile(int? tileObjectID)
	{
		if (!tileObjectID.HasValue && wasChanged == false) return null;

		if (!tileObjectID.HasValue && wasChanged == true)
		{
			foreach (GameObject g in tiles)
			{
				g.GetComponent<Renderer>().sharedMaterial = normalMaterial;
			}

			wasChanged = false;

			return null;
		}

		Vector2 index = Vector2.zero;

		for (int x = 0; x < tiles.Count; x++)
		{
			if (tiles[x].GetInstanceID() == tileObjectID.Value)
			{
				tiles[x].GetComponent<Renderer>().sharedMaterial = highlightMaterial;
				index = new Vector2(x / 10, x % 10);
			}
			else
			{
				tiles[x].GetComponent<Renderer>().sharedMaterial = normalMaterial;
			}
		}

		wasChanged = true;

		return index;
	}

	public void PlaceHurdles(bool[] placementIndices)
	{
		DestroyImmediate(hurdleContainer.gameObject);

		Debug.Break();

		GameObject gO = new GameObject("Hurdle Container");
		gO.transform.position = Vector3.zero;
		gO.transform.rotation = Quaternion.identity;
		gO.transform.parent = transform;

		hurdleContainer = gO.transform;

		hurdles.Clear();

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
