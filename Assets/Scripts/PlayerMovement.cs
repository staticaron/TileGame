using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores the node details of the A* grid.
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
		// Get the tile to move on, when primary is pressed.
		if (Input.GetMouseButtonDown(0))
		{
			Vector2Int? currentlySelectedTile = tileSelector.GetCurrentlySelected();

			// If primary was not pressed on a tile, return doing nothing.
			if (currentlySelectedTile == null) return;

			// Start the path finding process from the current position to the target position, which is the tile we get after clicking the button.
			FindPath(currentIndex, currentlySelectedTile.Value);
		}
	}

	// Overrides the default path finding logic to allow custom movement for any child.
	public override IEnumerator MoveOnPath(List<Node> pathToFollow)
	{
		int indexProcessed = -1;

		// Loop though all the nodes in the path finder path, and move the player along x-z direction in certain time intervals.
		while (indexProcessed < pathToFollow.Count - 1)
		{
			Vector2Int tileIndex = pathToFollow[indexProcessed + 1].index;
			Vector3 newPosition = tilemapGenerator.GetPositionFromIndex(tileIndex);
			transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);

			indexProcessed++;

			currentIndex = tileIndex;

			// after moving, play move sound
			PlayWalkingSound();

			// wait a bit before moving again.
			yield return new WaitForSeconds(moveSpeed);
		}

	}
}
