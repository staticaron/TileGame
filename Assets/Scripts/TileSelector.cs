using TMPro;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
	[SerializeField] LayerMask tileLayer;
	[SerializeField] Material highlightMaterial;

	[SerializeField] TilemapGenerator tileMapGen;

	[SerializeField] TMP_Text indexTEXT;

	private Camera mainCam;
	private AudioSource audioSource;

	[SerializeField] Vector2Int? currentlySelected;

	private void Awake()
	{
		mainCam = Camera.main;
		currentlySelected = null;
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		RaycastHit hitInfo = default;

		Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

		Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 10);

		Physics.Raycast(ray, out hitInfo, 1000, tileLayer);

		Vector2Int? index = tileMapGen.HighlightTile(hitInfo.collider == null ? null : hitInfo.collider.gameObject.GetInstanceID());

		if (index != currentlySelected) audioSource.Play();

		indexTEXT.text = index.HasValue == true ? $"({index.Value.x}, {index.Value.y})" : "Not Selected";

		currentlySelected = index;
	}

	public Vector2Int? GetCurrentlySelected()
	{
		return currentlySelected;
	}
}
