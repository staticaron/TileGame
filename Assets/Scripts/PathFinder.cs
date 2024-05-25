using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	public bool isHurdle;
	public int G;
	public int H;
	public Vector2Int index;

	public Node parent;

	public Node(bool isHurdle, Vector2Int index)
	{
		this.isHurdle = isHurdle;
		this.G = 0;
		this.H = 0;
		this.index = index;
		this.parent = default;
	}

	public int FCost
	{
		get { return G + H; }
	}

	public static bool operator ==(Node n1, Node n2)
	{
		return n1.index == n2.index;
	}

	public static bool operator !=(Node n1, Node n2)
	{
		return n1.index != n2.index;
	}
}

public class PathFinder : MonoBehaviour
{
	[SerializeField] int width = 10;
	[SerializeField] int height = 10;

	[SerializeField] TileDataSO tileDataSO;

	[SerializeField] Vector2Int currentIndex;
	[SerializeField] float moveSpeed = 1.0f;

	[SerializeField] TileSelector tileSelector;
	[SerializeField] TilemapGenerator tilemapGenerator;

	private Node[,] nodes = default;
	private bool initialized = false;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2Int? currentlySelectedTile = tileSelector.GetCurrentlySelected();

			if (currentlySelectedTile == null) return;

			FindPath(currentIndex, currentlySelectedTile.Value);
		}
	}

	private void Init()
	{
		nodes = new Node[width, height];

		for (int x = 0; x < tileDataSO.hurdlePlacementIndex.Length; x++)
		{
			Vector2Int index = new Vector2Int(x / width, x % width);
			nodes[index.x, index.y] = new Node(tileDataSO.hurdlePlacementIndex[x], index);
		}

		initialized = true;
	}

	private List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();

		foreach (Node n in nodes)
		{
			if (n == node) continue;

			if (Mathf.Abs(node.index.x - n.index.x) + Mathf.Abs(node.index.y - n.index.y) == 1)
			{
				neighbours.Add(n);
			}

			if (Mathf.Abs(node.index.x - n.index.x) == 1 && Mathf.Abs(node.index.y - n.index.y) == 1)
			{
				neighbours.Add(n);
			}
		}

		return neighbours;
	}

	private void FindPath(Vector2Int startIndex, Vector2Int endIndex)
	{
		if (!initialized) Init();

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();

		Node startNode = new Node(tileDataSO.hurdlePlacementIndex[startIndex.x * width + startIndex.y], startIndex);
		Node endNode = new Node(tileDataSO.hurdlePlacementIndex[endIndex.x * width + endIndex.y], endIndex);

		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node currentNode = openSet[0];

			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].H < currentNode.H))
				{
					currentNode = openSet[i];
				}
			}

			openSet.Remove(currentNode);
			closedSet.Add(currentNode);

			if (currentNode == endNode)
			{
				RetracePath(startNode, currentNode);
				return;
			}

			List<Node> neighbours = GetNeighbours(currentNode);

			for (int x = 0; x < neighbours.Count; x++)
			{
				Node neighbour = neighbours[x];

				if (neighbour.isHurdle || closedSet.Contains(neighbour)) continue;

				int newDistanceToNeighbour = currentNode.G + GetDistance(currentNode, neighbour);

				if (newDistanceToNeighbour < neighbour.G || !openSet.Contains(neighbour))
				{
					neighbour.G = newDistanceToNeighbour;
					neighbour.H = GetDistance(neighbour, endNode);
					neighbour.parent = currentNode;

					if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
				}

				nodes[neighbour.index.x, neighbour.index.y] = neighbour;
			}
		}
	}

	private void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();

		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		path.Reverse();

		StartCoroutine(MoveOnPath(path));
	}

	private IEnumerator MoveOnPath(List<Node> pathToFollow)
	{
		int indexProcessed = -1;

		while (indexProcessed < pathToFollow.Count - 1)
		{
			Vector2Int tileIndex = pathToFollow[indexProcessed + 1].index;
			Vector3 newPosition = tilemapGenerator.GetPositionFromIndex(tileIndex);
			transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);

			indexProcessed++;

			yield return new WaitForSeconds(moveSpeed);
		}

		currentIndex = pathToFollow[pathToFollow.Count - 1].index;
	}

	private int GetDistance(Node n1, Node n2)
	{
		int dotX = Mathf.Abs(n1.index.x - n2.index.x);
		int dotY = Mathf.Abs(n1.index.y - n2.index.y);

		if (dotX > dotY) return 14 * dotY + 10 * (dotX - dotY);
		else return 14 * dotX + 10 * (dotY - dotX);
	}
}
