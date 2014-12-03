using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public enum GameState
{
    Menu, NewPlay, ResumePlay, Lose
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

    private bool inGame;
    private Text timer;
    private int seconds, minutes;
    private float counter;

    private void Start()
    {
        //gameState = GameState.Menu;
        OnPlayClick();
        inGame = false;
        seconds = minutes = 0;
    }

    private void Update()
    {
        if (gameState == GameState.NewPlay)
        {
            if (!inGame)
            {
                timer = GameObject.Find("In-game Canvas(Clone)/Timer").GetComponent<Text>();
                StartCoroutine(BeginGame());
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

            inGame = true;
        }
        else if (gameState == GameState.Lose)
        {
            Application.LoadLevel(1);
        }
    }

    private IEnumerator BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());

        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.startCoords));

        endInstance = Instantiate(endPrefab) as End;
        endInstance.SetLocation(mazeInstance.GetCell(mazeInstance.endCoords));

        trapInstance = Instantiate(trapPrefab) as Traps;
        trapInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

        cupInstance = Instantiate(cupPrefab) as Cup;
        cupInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

        for (int i = 0; i < cheeseInstance.Length; i++)
        {
            cheeseInstance[i] = Instantiate(cheesePrefab) as Cheese;
            cheeseInstance[i].SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        }

        for (int i = 0; i < bombInstance.Length; i++)
        {
            bombInstance[i] = Instantiate(bombPrefab) as Bombs;
        }
        bombInstance[0].SetLocation(mazeInstance.GetCell(new IntVector2(0, 0)));
        bombInstance[1].SetLocation(mazeInstance.GetCell(new IntVector2(0, mazeInstance.size.z - 1)));
        bombInstance[2].SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x - 1, 0)));
        bombInstance[3].SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1)));
    }

    //private void RestartGame()
    //{
    //    StopAllCoroutines();
    //    if (mazeInstance != null)
    //    {
    //        Destroy(mazeInstance.gameObject);
    //    }
    //    if (playerInstance != null)
    //    {
    //        Destroy(playerInstance.gameObject);
    //    }
    //    if (endInstance != null)
    //    {
    //        Destroy(endInstance.gameObject);
    //    }
    //    if (trapInstance != null)
    //    {
    //        Destroy(trapInstance.gameObject);
    //    }
    //    for (int i = 0; i < bombInstance.Length; i++)
    //    {
    //        if (bombInstance[i] != null)
    //        {
    //            Destroy(bombInstance[i].gameObject);
    //        }
    //    }
    //    StartCoroutine(BeginGame());
    //}

    public void OnPlayClick()
    {
        Destroy(GameObject.Find("Menu Canvas"));
        Instantiate(inGameCanvasPrefab);
        gameState = GameState.NewPlay;
    }
}