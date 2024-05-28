using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : PathFinder
{
	[SerializeField] PlayerMovement playerMovement;

	[SerializeField] Vector2Int target;

	public override void Awake()
	{
		base.Awake();

		target = playerMovement.GetCurrentIndex();
		FindPath(currentIndex, playerMovement.GetCurrentIndex());
	}

	private void Update()
	{
		if (playerMovement.GetCurrentIndex() == target) return;

		target = playerMovement.GetCurrentIndex();
		FindPath(currentIndex, playerMovement.GetCurrentIndex());
	}

	// Implements custom movement code.
	public override IEnumerator MoveOnPath(List<Node> pathToFollow)
	{
		int indexProcessed = 0;

		// Skip the last node to move on, which will allow enemy to move close to but not on the player itself.
		while (indexProcessed < pathToFollow.Count - 1)
		{
			yield return new WaitForSeconds(moveSpeed);

			Vector2Int tileIndex = pathToFollow[indexProcessed].index;
			Vector3 newPosition = tilemapGenerator.GetPositionFromIndex(tileIndex);
			transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);

			indexProcessed++;

			PlayWalkingSound();

			// update current position of the enemy.
			currentIndex = tileIndex;
		}
	}
}
