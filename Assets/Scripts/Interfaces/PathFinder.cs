using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFinder : MonoBehaviour, IPathfinder
{
	[SerializeField] int width = 10;
	[SerializeField] int height = 10;

	[SerializeField] TileDataSO tileDataSO;

	[SerializeField] protected Vector2Int currentIndex;
	[SerializeField] protected float moveSpeed = 1.0f;

	[SerializeField] protected TileSelector tileSelector;
	[SerializeField] protected TilemapGenerator tilemapGenerator;

	[SerializeField] AudioClip walkClip;
	[SerializeField] AudioClip pathfinderFailedClip;
	private AudioSource audioPlayer;

	private Node[,] nodes = default;
	private bool initialized = false;

	public virtual void Awake()
	{
		audioPlayer = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		tileDataSO.ETileMapChanged += Init;
	}

	private void OnDisable()
	{
		tileDataSO.ETileMapChanged -= Init;
	}

	public void Init()
	{
		nodes = new Node[width, height];

		for (int x = 0; x < tileDataSO.hurdlePlacementIndex.Length; x++)
		{
			Vector2Int index = new Vector2Int(x / width, x % width);
			nodes[index.x, index.y] = new Node(tileDataSO.hurdlePlacementIndex[x], index);
		}

		initialized = true;
	}

	public List<Node> GetNeighbours(Node node)
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

	// Performs the entire path finding algorithm on the nodes and start the movement coroutine. 
	public void FindPath(Vector2Int startIndex, Vector2Int endIndex)
	{
		// Initialize the nodes, read the data from the SO and load it into the memory.
		if (!initialized) Init();

		// Stop all existing movement coroutines to restart them after the path is recalculated.
		StopAllCoroutines();

		// A* Algorithm Implementation.
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

		// If the Retrace Path function was not called then this function will execute playing a 'Failed to Create Path Sound'.
		PlayPathfinderFailed();
	}

	// Creates a path in the forward direction from the nodes that are in the path.
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

	// This coroutine is implemented on the class which is going to inherit this class so that we can implement custom movement code from the singular pathfinding algorithm.
	public abstract IEnumerator MoveOnPath(List<Node> path);

	// Distance between two nodes.
	private int GetDistance(Node n1, Node n2)
	{
		int dotX = Mathf.Abs(n1.index.x - n2.index.x);
		int dotY = Mathf.Abs(n1.index.y - n2.index.y);

		if (dotX > dotY) return 14 * dotY + 10 * (dotX - dotY);
		else return 14 * dotX + 10 * (dotY - dotX);
	}

	public Vector2Int GetCurrentIndex()
	{
		return currentIndex;
	}

	protected void PlayWalkingSound()
	{
		audioPlayer.PlayOneShot(walkClip);
	}

	protected void PlayPathfinderFailed()
	{
		audioPlayer.PlayOneShot(pathfinderFailedClip);
	}
}
