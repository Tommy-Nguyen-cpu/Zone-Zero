using TMPro;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Random = UnityEngine.Random;

using static Unity.Mathematics.math;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	[SerializeField]
	Player player;
	public List<UnityEngine.UI.Image> Hearts;
	public GameObject Menu;
	
	[SerializeField]
	GameObject Enemy;
	GameObject instantiatedEnemy;
	public UnityEngine.UI.Image QTImage;
	public TMP_Text QTText;

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

	int level = 0;

	public GameObject NotePrefab;

	public NoteAppear note;
	public GameOverUI GO_ui;

	private GameObject current_note;




	/* Helper Functions:
	 *	-GameRestart() - Resets Flags
	 *  -RespawnEntities() - Places Player, Enemy and Note at default positions
	 *  -EndLevel() - Checks flags and performs relevant actions
	 *  -GameReset() - Performed at GameOver, performs all three of the above
	 *  
	 * 
	 * 
	 * 
	 * 
	 */

	private void Awake()
    {
		// Locks the cursor so we don't see the cursor while playing the game.
		Cursor.lockState = CursorLockMode.Locked;

		player.NoteFound += NoteFound;
		player.NoteDropped += NoteDropped;
		CreateMaze();
		AddNote();
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
		ReachedEnd();
		InputHandler();
		if(player.enabled)
			player.Move();

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
		struggleSystem.EnemyTree = tree;
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
			note.note_txt_1.SetText("Note1");
		}
		else if (player.GetNoteLevel() == 2)
		{
			note.note_txt_1.SetText("Note2");
		}
		else if (player.GetNoteLevel() == 3)
		{
			note.note_txt_1.SetText("Note3");
		}
		note.ShowNote();
	}

	//hides Note UI
	private void NoteDropped()
    {
		note.HideNote();
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
		GameObject go = Instantiate(NotePrefab, pos, Quaternion.identity);
		go.transform.Rotate(new Vector3(0, 0, 90));
		current_note = go;

		print("Instantiated a note at: " + pos.x.ToString() + " and " + pos.z.ToString());
	}

	private void InputHandler()
	{
		if (Input.GetKey("space"))
		{
			EndLevel();
		}
		else if (Input.GetKey(KeyCode.Escape))
        {
			Cursor.lockState = CursorLockMode.None;
			PauseGame();
        }
	}

	//Places player, enemy and notes at default positions.
	private void ResetEntities()
    {
		ResetPlayer();
		ResetEnemy();
	}

	//Resets game flags. Used after GameOver to restart game
	private void RestartGame()
    {
		player.PlayerReset();
		this.level = 0;
    }

	//Generates a new level
	private void NewLevel()
	{
		if (!current_note.Equals(null)) 
			Destroy(current_note.gameObject);
		ResetMaze();
		CreateMaze();
		AddNote(); 
	}

	private void EndLevel()
    {
		level += 1;
		print("The current level is: " + level.ToString());
		if (player.GetNoteLevel()==3) //If you've found all the notes
		{
			Time.timeScale = 0;
			Cursor.lockState = CursorLockMode.None;
			GO_ui.ShowWinScreen();
			//gameOver = true;
		} else if (this.level > 2) //If you reach the last map without finding all notes
		{
			Time.timeScale = 0;
			Cursor.lockState = CursorLockMode.None;
			GO_ui.ShowGameOver();
		}
		else //Reset map and entities, increase level by 1
		{
			NewLevel();
			ResetEntities();
		}

	}

	//Handles when the player reaches the end of the map.
	private void ReachedEnd()
	{
		Vector3 pos = player.transform.position;
		if (pos.x <= -18 && pos.z <= -18)
		{
			EndLevel();
		}
	}

	//resets all flags and values
	public void GameReset()
    {
		Time.timeScale = 1;
		RestartGame();
		NewLevel();
		ResetEntities();
		GO_ui.clear();
    }

	//UI METHODS

	public void ResetButton()
    {
		///This is run when the reset button is pressed
		GameReset();
    }

	public void QuitButton()
    {
		GameReset();
		SceneManager.LoadScene("Main Menu");
	}

	private void DecreaseHealth()
    {
		Debug.Log("Got to decrease heart method");
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
			Debug.Log("Lost.");
        }
    }


	#region Pause Menu Methods
	private void PauseGame()
	{
		Time.timeScale = 0;
		Menu.SetActive(true);
	}

	public void Continue()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Menu.SetActive(false);
		Time.timeScale = 1;
    }

	public void Settings()
    {
		
    }

	public void Quit()
    {
		Application.Quit();
    }
	#endregion
}
