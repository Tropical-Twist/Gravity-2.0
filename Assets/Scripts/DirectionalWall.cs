using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalWall : MonoBehaviour
{
	public enum Direction
	{
		UP,
		DOWN,
		NORTH,
		SOUTH,
		EAST,
		WEST,
	}

	public Direction direction;
}
