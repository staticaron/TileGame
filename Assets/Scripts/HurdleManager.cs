using UnityEngine;

public class HurdleManager : MonoBehaviour
{
	public int width = 10;
	public int height = 10;

	public TileDataSO tileDataSO;

	[SerializeField] TilemapGenerator tileMapGen;

	public void PlaceHurdles()
	{
		tileMapGen.PlaceHurdles(tileDataSO.hurdlePlacementIndex);
	}
}
