using TMPro;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Random = UnityEngine.Random;

using static Unity.Mathematics.math;

public class Game : MonoBehaviour
{
	[SerializeField]
	Player player;
	
	[SerializeField]
	GameObject Enemy;
	[SerializeField]
	MazeVisualization visualization;
	[SerializeField, Range(0f, 1f)]
	float pickLastProbability = 0.5f;
	[SerializeField, Range(0f, 1f)]
	float openDeadEndProbability = 0.5f;
	[SerializeField]
	int2 mazeSize = int2(20, 20);
	Maze maze;
    [SerializeField, Tooltip("Use zero for random seed.")]
    int seed;

    private void Awake()
    {
		maze = new Maze(mazeSize);
		new GenerateMazeJob
		{
			maze = maze,
			seed = seed != 0 ? seed : Random.Range(1, int.MaxValue),
			pickLastProbability = pickLastProbability,
			openDeadEndProbability = openDeadEndProbability
		}.Schedule().Complete();
		visualization.Visualize(maze);

		if (seed !=0)
        {
			Random.InitState(seed);
        }
        player.StartNewGame(new Vector3(1f, -1f, 1f));

		#region Randomly Places Enemy In a cell.

        int randomStartLocation = Random.Range(0, maze.Length);
		Vector3 startLocation = maze.IndexToWorldPosition(randomStartLocation);
		GameObject instantiatedEnemy = Instantiate(Enemy, new Vector3(startLocation.x, startLocation.y, startLocation.z), Quaternion.identity);
		DecisionTree tree = instantiatedEnemy.GetComponent<DecisionTree>();
		tree.PCGMaze = maze;
		tree.Player = player.gameObject;
		#endregion

    }

    private void OnDestroy()
    {
        maze.Dispose();
    }

    private void Update()
    {
		player.Move();
    }
}
