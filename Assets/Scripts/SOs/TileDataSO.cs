using UnityEngine;

[CreateAssetMenu(fileName = "SO/TileData", menuName = "SO", order = 0)]
public class TileDataSO : ScriptableObject
{
	// Stores the hurdle placement data of every node on the grid. TRUE means hurdle is present on the grid value.
	public bool[] hurdlePlacementIndex;

	// those who want to know when the tilemap was changed, can subscribe to this event. 
	public delegate void TileMapChanged();
	public event TileMapChanged ETileMapChanged;

	// Call this function to Invoke the tilemapChanged event. Anything that is listening to the tileChanged event will be notified.
	public void RaiseTileMapChanged()
	{
		if (ETileMapChanged != null) ETileMapChanged.Invoke();
		else
		{
			Debug.LogWarning("No one is there to listen to TileMapChange");
		}
	}
}
