using TMPro;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
	[SerializeField] LayerMask tileLayer;
	[SerializeField] Material highlightMaterial;

	[SerializeField] TilemapGenerator tileMapGen;

	[SerializeField] TMP_Text indexTEXT;

	private Camera mainCam;

	[SerializeField] Vector2Int? currentlySelected;

	private void Awake()
	{
		mainCam = Camera.main;
		currentlySelected = null;
	}

	private void Update()
	{
		RaycastHit hitInfo = default;

		Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

		Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 10);

		Physics.Raycast(ray, out hitInfo, 1000, tileLayer);

		Vector2Int? index = tileMapGen.HighlightTile(hitInfo.collider == null ? null : hitInfo.collider.gameObject.GetInstanceID());

		indexTEXT.text = index.HasValue == true ? $"({index.Value.x}, {index.Value.y})" : "Not Selected";

		currentlySelected = index;
	}

	public Vector2Int? GetCurrentlySelected()
	{
		return currentlySelected;
	}
}
