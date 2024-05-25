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

public class PlayerMovement : PathFinder
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2Int? currentlySelectedTile = tileSelector.GetCurrentlySelected();

			if (currentlySelectedTile == null) return;

			FindPath(currentIndex, currentlySelectedTile.Value);
		}
	}

	public override IEnumerator MoveOnPath(List<Node> pathToFollow)
	{
		int indexProcessed = -1;

		while (indexProcessed < pathToFollow.Count - 1)
		{
			Vector2Int tileIndex = pathToFollow[indexProcessed + 1].index;
			Vector3 newPosition = tilemapGenerator.GetPositionFromIndex(tileIndex);
			transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);

			indexProcessed++;

			currentIndex = tileIndex;

			PlayWalkingSound();

			yield return new WaitForSeconds(moveSpeed);
		}

	}
}
