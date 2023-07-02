using TMPro;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Random = UnityEngine.Random;

using static Unity.Mathematics.math;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	[SerializeField]
	Player player;
	public List<UnityEngine.UI.Image> Hearts;
	public GameObject Menu;
	
	[SerializeField]
	GameObject Enemy;
	public UnityEngine.UI.Image QTImage;
	public TMP_Text QTText;
	GameObject instantiatedEnemy;

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

	bool endFlag = false;
	int level = 0;
	bool gameOver = false;

	public GameObject NotePrefab;

	//TODO (IV): Make a Game Over Delegate and/or win Delegate for facilitating either good or bad ending...

	private void Awake()
    {
		player.NoteFound += NoteFound;
		CreateMaze();
        player.StartNewGame(new Vector3(1f, -1f, 1f));

		#region Randomly Places Enemy In a cell.
		ResetEnemy();
		#endregion

	}

    private void OnDestroy()
    {
        maze.Dispose();
    }

    private void Update()
    {
		if (!gameOver) //Maybe change for a GetterFunc that checks current state of the game
		{
			ReachedEnd();
			InputHandler();
			if(player.enabled)
				player.Move();
		}
	}

	private void CreateMaze()
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

		if (seed != 0)
		{
			Random.InitState(seed);
		}
		//TODO: Add statement to check if this is an even level
		AddNote();
	}


	private void ResetEnemy()
    {
		// Useful for when we reset the map.
		if (instantiatedEnemy != null)
		{
			Destroy(instantiatedEnemy);
		}

		int randomStartLocation = Random.Range(0, maze.Length);
		Vector3 startLocation = maze.IndexToWorldPosition(randomStartLocation);
		instantiatedEnemy = Instantiate(Enemy, new Vector3(startLocation.x, -1f, startLocation.z), Quaternion.identity);
		DecisionTree tree = instantiatedEnemy.GetComponent<DecisionTree>();
		tree.PCGMaze = maze;
		tree.Player = player.gameObject;

		StruggleSystem struggleSystem = instantiatedEnemy.GetComponent<StruggleSystem>();
		struggleSystem.EnemyTree = instantiatedEnemy.GetComponent<DecisionTree>();
		struggleSystem.Player = player.gameObject;
		struggleSystem.QT = QTImage;
		struggleSystem.tmpText = QTText;
		struggleSystem.DecreaseHealth += DecreaseHealth;
	}

	//Handles resetting player after a moving to next level
	private void ResetPlayer()
	{
		player.StartNewGame(new Vector3(1f, -1f, 1f));
	}

	private void ResetMaze()
	{
		if (!maze.Equals(null))
		{
			visualization.ClearEnvironment();
			print("Disposed!");
			maze.Dispose();
		}
	}

	//Handle displaying note
	private void NoteFound()
	{
		if (player.GetNoteLevel() == 1)
		{

		}
		else if (player.GetNoteLevel() == 2)
		{

		}
		else if (player.GetNoteLevel() == 3)
		{

		}
	}

	private void AddNote()
	{
		Vector3 pos;
		int x = Random.Range(-19, 19);
		int z = Random.Range(-19, 19);
		//cull evens. some even positions are out of bounds.
		if (x % 2 == 0)
			x += 1;
		if (z % 2 == 0)
			z += 1;

		//debug values
		x = 3;
		z = 1;

		pos = new Vector3(x, 0, z);
		Instantiate(NotePrefab, pos, Quaternion.identity);
		print("Instantiated a note at: " + pos.x.ToString() + " and " + pos.z.ToString());
	}

	private void InputHandler()
	{
		if (Input.GetKey("space"))
		{
			print("SPACE");
			NewLevel();
			ResetEnemy();
		}
		else if (Input.GetKey(KeyCode.Escape))
        {
			PauseGame();
        }
	}

	//Generates a new level
	private void NewLevel()
	{
		ResetMaze();
		CreateMaze();
		ResetPlayer();
		ResetEnemy();
	}

	//Handles when the player reaches the end of the map.
	private void ReachedEnd()
	{
		Vector3 pos = player.transform.position;
		if (!endFlag && pos.x <= -18 && pos.z <= -18)
		{
			if (player.GetNoteLevel() >= 3) //If you've found all the notes
			{
				print("You Escaped");
				gameOver = true;
			}
			else //Reset map
			{
				endFlag = true;
				level += 1;
				print("Reached End!");
				NewLevel();
			}
			if (this.level == 5) //If you reach the last map without finding all notes
			{
				print("You did not escape. You must try AGAIN!");
				player.PlayerReset();
				ResetEnemy();
			}
		}
		endFlag = false;
	}


	private void DecreaseHealth()
    {
		bool LostAllHearts = true;
		foreach(var heart in Hearts)
        {
            if (heart.enabled)
            {
				LostAllHearts = false;
				heart.enabled = false;
				break;
            }
        }

        if (LostAllHearts)
        {
			// TODO: Once the lose delegate is created, invoke it here.
			Debug.Log("Lost.");
        }
    }

	public void PauseGame()
    {
		Time.timeScale = 0;
		Menu.SetActive(true);
    }

    #region Pause Menu Buttons
	public void Continue()
    {
		Time.timeScale = 1;
		Menu.SetActive(false);
    }

	public void Settings()
    {

    }

    public void Quit()
    {
		Application.Quit(0);
    }
    #endregion
}
