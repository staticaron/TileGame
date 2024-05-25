using TMPro;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
	[SerializeField] LayerMask tileLayer;
	[SerializeField] Material highlightMaterial;

	[SerializeField] TilemapGenerator tileMapGen;

	[SerializeField] TMP_Text indexTEXT;

	private Camera mainCam;

	private void Awake()
	{
		mainCam = Camera.main;
	}

	private void Update()
	{
		RaycastHit hitInfo = default;

		Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

		Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 10);

		Physics.Raycast(ray, out hitInfo, 1000, tileLayer);

		Vector2? index = tileMapGen.HighlightTile(hitInfo.collider == null ? null : hitInfo.collider.gameObject.GetInstanceID());

		indexTEXT.text = index.HasValue == true ? $"({index.Value.x}, {index.Value.y})" : "Not Selected";
	}
}
