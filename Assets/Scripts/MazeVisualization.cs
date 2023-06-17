using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MazeVisualization : ScriptableObject
{
    [SerializeField]
    MazeCellObject end, straight, corner, tJunction, xJunction;

	static float x = 270;

	static Quaternion[] rotations =
	{
		Quaternion.Euler(x, 0f, 0f),
		Quaternion.Euler(x, 90f, 0f),
		Quaternion.Euler(x, 180f, 0f),
		Quaternion.Euler(x, 270f, 0f)
	};

	public void Visualize(Maze maze)
	{
		for (int i = 0; i < maze.Length; i++)
		{
			(MazeCellObject, int) prefabWithRotation = GetPrefab(maze[i]);
			MazeCellObject instance = prefabWithRotation.Item1.GetInstance();
			//instance.transform.localPosition = maze.IndexToWorldPosition(i);
			instance.transform.SetPositionAndRotation(
				maze.IndexToWorldPosition(i), rotations[prefabWithRotation.Item2]
			);
		}
	}

	(MazeCellObject, int) GetPrefab(MazeFlags flags) => flags switch
	{
		MazeFlags.PassageN => (end, 0),
		MazeFlags.PassageE => (end, 1),
		MazeFlags.PassageS => (end, 2),
		MazeFlags.PassageW => (end, 3),

		MazeFlags.PassageN | MazeFlags.PassageS => (straight, 0),
		MazeFlags.PassageE | MazeFlags.PassageW => (straight, 1),

		MazeFlags.PassageN | MazeFlags.PassageE => (corner, 0),
		MazeFlags.PassageE | MazeFlags.PassageS => (corner, 1),
		MazeFlags.PassageS | MazeFlags.PassageW => (corner, 2),
		MazeFlags.PassageW | MazeFlags.PassageN => (corner, 3),

		MazeFlags.PassageAll & ~MazeFlags.PassageW => (tJunction, 0),
		MazeFlags.PassageAll & ~MazeFlags.PassageN => (tJunction, 1),
		MazeFlags.PassageAll & ~MazeFlags.PassageE => (tJunction, 2),
		MazeFlags.PassageAll & ~MazeFlags.PassageS => (tJunction, 3),

		_ => (xJunction, 0)
	};

	/**
    public void Visualize (Maze maze)
    {
        for (int i = 0; i< maze.Length; i++)
        {
            MazeCellObject instance = xJunction.GetInstance();
            instance.transform.localPosition = maze.IndexToWorldPosition(i);
        }
    }

	MazeCellObject GetPrefab(MazeFlags flags) => flags switch
	{
		MazeFlags.PassageN => end,
		MazeFlags.PassageE => end,
		MazeFlags.PassageS => end,
		MazeFlags.PassageW => end,

		MazeFlags.PassageN | MazeFlags.PassageS => straight,
		MazeFlags.PassageE | MazeFlags.PassageW => straight,

		MazeFlags.PassageN | MazeFlags.PassageE => corner,
		MazeFlags.PassageE | MazeFlags.PassageS => corner,
		MazeFlags.PassageS | MazeFlags.PassageW => corner,
		MazeFlags.PassageW | MazeFlags.PassageN => corner,

		MazeFlags.PassageAll & ~MazeFlags.PassageW => tJunction,
		MazeFlags.PassageAll & ~MazeFlags.PassageN => tJunction,
		MazeFlags.PassageAll & ~MazeFlags.PassageE => tJunction,
		MazeFlags.PassageAll & ~MazeFlags.PassageS => tJunction,

		_ => xJunction
	**/

}
