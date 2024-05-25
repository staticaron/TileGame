using System.Collections.Generic;
using UnityEngine;

public interface IPathfinder
{
	public void Init();
	public List<Node> GetNeighbours(Node node);
	public void FindPath(Vector2Int startNode, Vector2Int endNode);
}
