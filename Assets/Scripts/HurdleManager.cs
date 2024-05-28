using UnityEngine;

public class HurdleManager : MonoBehaviour
{
	public int width = 10;
	public int height = 10;

	public TileDataSO tileDataSO;

	[SerializeField] TilemapGenerator tileMapGen;

	// Called from the editor script to place the hurdles.
	public void PlaceHurdles()
	{
		tileMapGen.PlaceHurdles(tileDataSO.hurdlePlacementIndex);
	}
}
