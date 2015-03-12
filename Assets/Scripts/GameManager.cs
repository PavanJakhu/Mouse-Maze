using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public enum GameState
{
    Menu, Loading, NewPlay, ResumePlay, Lose, Win
}

public class GameManager : MonoBehaviour
{
    public static GameState gameState;

    public Maze mazePrefab;
    public Player playerPrefab;
    public End endPrefab;
    public Traps trapPrefab;
    public Bombs bombPrefab;
    public Cup cupPrefab;
    public Cheese cheesePrefab;
    public Canvas menuCanvasPrefab;
    public Canvas inGameCanvasPrefab;
    public Canvas loadingCanvasPrefab;

    private Maze mazeInstance;
    public Maze MazeInstance { get { return mazeInstance; } set { mazeInstance = value; } }

    private Player playerInstance;
    public Player PlayerInstance { get { return playerInstance; } set { playerInstance = value; } }

    private End endInstance;
    public End EndInstance { get { return endInstance; } set { endInstance = value; } }

    private Traps trapInstance;
    public Traps TrapsInstance { get { return trapInstance; } set { trapInstance = value; } }

    private Bombs[] bombInstance = new Bombs[4];
    public Bombs[] BombInstance { get { return bombInstance; } set { bombInstance = value; } }

    private Cup cupInstance;
    public Cup CupInstance { get { return cupInstance; } set { cupInstance = value; } }

    private Cheese[] cheeseInstance = new Cheese[5];
    public Cheese[] CheeseInstance { get { return cheeseInstance; } set { cheeseInstance = value; } }

    private Canvas inGameCanvasInstance;
    public Canvas InGameCanvasInstance { get { return inGameCanvasInstance; } set { inGameCanvasInstance = value; } }

    private Canvas menuCanvasInstance;
    public Canvas MenuCanvasInstance { get { return menuCanvasInstance; } set { menuCanvasInstance = value; } }

    public bool saved;

    private Canvas loadingCanvasInstance;

    private bool resumeActive;
    private bool loading;
    private bool loadedTimer;
    private Text timer;
    private int seconds, minutes;
    private float counter;

    private void Start()
    {
        gameState = GameState.Menu;
        menuCanvasInstance = GameObject.Find("Menu Canvas").GetComponent<Canvas>();
        //OnPlayClick();
        loading = false;
        loadedTimer = false;
        seconds = minutes = 0;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.Menu:
                if (System.IO.File.Exists("Assets/Game Data/SaveData.xml"))
                {
                    GameObject.Find("Menu Canvas/Resume Button").GetComponent<Button>().interactable = true;
                    resumeActive = true;
                }
                loading = false;
                loadedTimer = false;
                seconds = minutes = 0;
                break;
            case GameState.Loading:
                if (!loading)
                {
                    loadingCanvasInstance = Instantiate(loadingCanvasPrefab) as Canvas;
                    StartCoroutine(BeginGame());
                    loading = true;
                }
                break;
            case GameState.NewPlay:
                if (!loadedTimer)
                {
                    Destroy(loadingCanvasInstance.gameObject);
                    timer = GameObject.Find("In-game Canvas(Clone)/Timer").GetComponent<Text>();
                    loadedTimer = true;
                }

                counter += Time.deltaTime;
                if (counter >= 1.0f)
                {
                    seconds++;
                    if (seconds >= 60)
                    {
                        seconds = 0;
                        minutes++;
                    }
                    counter = 0;
                }
                if (seconds >= 0 && seconds <= 9)
                {
                    timer.text = "Time: " + minutes + ":0" + seconds;
                }
                else
                {
                    timer.text = "Time: " + minutes + ":" + seconds;
                }
                break;
            case GameState.ResumePlay:
                if (!loadedTimer)
                {
                    timer = GameObject.Find("In-game Canvas(Clone)/Timer").GetComponent<Text>();
                    loadedTimer = true;
                }

                counter += Time.deltaTime;
                if (counter >= 1.0f)
                {
                    seconds++;
                    if (seconds >= 60)
                    {
                        seconds = 0;
                        minutes++;
                    }
                    counter = 0;
                }
                if (seconds >= 0 && seconds <= 9)
                {
                    timer.text = "Time: " + minutes + ":0" + seconds;
                }
                else
                {
                    timer.text = "Time: " + minutes + ":" + seconds;
                }
                break;
            case GameState.Lose:
                Application.LoadLevel(1);
                break;
            case GameState.Win:
                Application.LoadLevel(2);
                break;
            default:
                break;
        }
    }

    private IEnumerator BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());

        inGameCanvasInstance = Instantiate(inGameCanvasPrefab) as Canvas;
        Destroy(GameObject.Find("Camera"));

        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.startCoords));
        yield return 0;

        endInstance = Instantiate(endPrefab) as End;
        endInstance.SetLocation(mazeInstance.GetCell(mazeInstance.endCoords));
        yield return 0;

        trapInstance = Instantiate(trapPrefab) as Traps;
        trapInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        yield return 0;

        cupInstance = Instantiate(cupPrefab) as Cup;
        cupInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        yield return 0;

        for (int i = 0; i < cheeseInstance.Length; i++)
        {
            cheeseInstance[i] = Instantiate(cheesePrefab) as Cheese;
            cheeseInstance[i].SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        }
        yield return 0;

        for (int i = 0; i < bombInstance.Length; i++)
        {
            bombInstance[i] = Instantiate(bombPrefab) as Bombs;
        }
        bombInstance[0].SetLocation(mazeInstance.GetCell(new IntVector2(0, 0)));
        bombInstance[1].SetLocation(mazeInstance.GetCell(new IntVector2(0, mazeInstance.size.z - 1)));
        bombInstance[2].SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x - 1, 0)));
        bombInstance[3].SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1)));
        yield return 0;

        gameState = GameState.NewPlay;
    }

    public void OnPlayClick()
    {
        Destroy(GameObject.Find("Menu Canvas"));
        gameState = GameState.Loading;
    }
}