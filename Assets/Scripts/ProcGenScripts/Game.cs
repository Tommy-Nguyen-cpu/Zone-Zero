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

	public GameObject ExitSignPrefab;

	private GameObject ExitSignObject;

	[SerializeField]
	private SoundFXManager SFXManager;

	Vector3 win_pos = new Vector3(1f, -1f, 1f);
	//Vector3 win_pos = new Vector3(-17f, -1f, -19);


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
		player.Exit_Triggered += PlayerReachedExit;
		CreateMaze();
		AddNote();
		//Exit sign gets added immediately and only gets destroyed when the game is over.
		AddExitSign();
        player.StartNewGame(win_pos);

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
		struggleSystem.MyGameOverUI = GO_ui;
		struggleSystem.Player = player.gameObject;
		struggleSystem.QT = QTImage;
		struggleSystem.tmpText = QTText;
		struggleSystem.DecreaseHealth += DecreaseHealth;
	}

	//Handles resetting player after a moving to next level
	private void ResetPlayer()
	{
		player.StartNewGame(win_pos);
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
			note.note_txt_1.SetText("July 3rd 2022\n"+
				"The experiments have proven to be a complete success. Last week Dr. Henriksen's theory of splicing the creature's genome with exposed material from the nuclear reactor resulted in our first first sign of life. We registered 5 heart palpitations before the creature went into shock. Just today we managed to resuscitate it. It's currently sitting in the lab glaring at the ground and grunting. Our next steps are to perform a full top-to-bottom analysis of its mental and physical capacity. We don't have high hopes, but we've been surprised before");
		}
		else if (player.GetNoteLevel() == 2)
		{
			note.note_txt_1.SetText("July 4th 2022\n"+
				"Last night we realized the gravity of our mistake. During our physical examination, the creature attacked Dr. Lansing, viciously throwing him halfway across the room into the wall, killing him instantly. We didn't dare enter the test chamber, as the creature shuffled erratically, ripping up boxes and smashing filing cabinets, all to the sound of its horrifying cries. Those of us that remained convened in the mess hall to asses what could be our next steps, but our meeting was cut short by the sound of the alarm system. Somehow the creature escaped the test chamber, and we could hear it echoing through the halls of the research station. What's more it seemed to be causing the power systems to fluctuate, Dr. Henriksen believes it to be the trace amounts of radiation interfering with our generator.\n"
				+ "Currently myself and Dr.Henriksen are hiding in one of the storage closets, we believe we're the only ones left. The screams were enough to tips us off to our comrade's fate. We're planning on making a run for the comms system in the next hour. God help us all.");
		}
		else if (player.GetNoteLevel() == 3)
		{
			note.note_txt_1.SetText("July 5th 2022\n"+
				"My... No your name is Peter Strauss. If you're hearing this, it means our plan was a success. After Dr. Henriksen was killed by the thing, I made a run for the cryo chamber, I thought if we could bide our time the creature would be tired out. I guess you'll be the judge of that. You might not remember anything, that's ok, if we've lost all the knowledge of what went on here, it's for the best. Our only goal is to get out of here. The code to escape the base is 5429310. Find the closest exit and get out of here. Good luck. ");
		}

		Destroy(current_note);
		note.ShowNote();
	}

	//hides Note UI
	private void NoteDropped()
    {
		Cursor.lockState = CursorLockMode.Locked;
		note.HideNote();
    }

	//adds exit sign to exit point
	private void AddExitSign()
    {
		float x = -19;
		float z=-19;
		float y = 0.75f;
		Vector3 pos = new Vector3(x, y, z);
		ExitSignObject = Instantiate(ExitSignPrefab, pos, Quaternion.identity);
		print("Instantiated Exit Sign");
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
		if (Input.GetKeyDown("space"))
		{
			print("Level ended at: " + Time.deltaTime);
			EndLevel();
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
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
		Cursor.lockState = CursorLockMode.Locked;
		player.PlayerReset();
		this.level = 0;

		// Resets all hearts.
		foreach(var heart in Hearts)
        {
			heart.enabled = true;
        }
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

	//Delegate Method Triggered by Player
	private void PlayerReachedExit()
    {
		EndLevel();
    }

	private void EndLevel()
    {
		//Trigger Sound
		SFXManager.PlayExitSound(player.note_bool);

		//This function stops audio playback in order to avoid infinite loop. 
		//Enemy.GetComponent<EnemyAudioManager>().reset_unchanged_state();
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
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene("Main Menu");
	}

	private void DecreaseHealth()
    {
		bool LostAllHearts = true;
		foreach(var heart in Hearts)
        {
            if (heart.enabled)
            {
				heart.enabled = false;
				break;
            }
        }

		foreach(var heart in Hearts)
        {
            if (heart.enabled)
            {
				LostAllHearts = false;
				break;
            }
        }

        if (LostAllHearts)
        {
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0;
			GO_ui.ShowGameOver();
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
