using UnityEngine;

[CreateAssetMenu(fileName = "SO/TileData", menuName = "SO", order = 0)]
public class TileDataSO : ScriptableObject
{
	public bool[] hurdlePlacementIndex;

	public delegate void TileMapChanged();
	public event TileMapChanged ETileMapChanged;

	public void RaiseTileMapChanged()
	{
		if (ETileMapChanged != null) ETileMapChanged.Invoke();
		else
		{
			Debug.LogWarning("No one is there to listen to TileMapChange");
		}
	}
}
