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
    public GameObject camera;

    public Maze mazePrefab;
    public Player playerPrefab;
    public End endPrefab;
    public Traps trapPrefab;
    public Bombs bombPrefab;
    public Cup cupPrefab;
    public Canvas menuCanvasPrefab;
    public Canvas inGameCanvasPrefab;

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

    private bool inGame;

    private void Start()
    {
        gameState = GameState.Menu;
        inGame = false;
        Instantiate(camera);
    }

    private void Update()
    {
        if (gameState == GameState.NewPlay && !inGame)
        {
            inGame = true;
            StartCoroutine(BeginGame());
        }
        if (gameState == GameState.Lose)
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

        int directionX = 0, directionY = 1;
        trapInstance = Instantiate(trapPrefab) as Traps;
        trapInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates + new IntVector2(directionX, directionY)));

        for (int i = 0; i < bombInstance.Length; i++)
        {
            bombInstance[i] = Instantiate(bombPrefab) as Bombs;
        }
        bombInstance[0].SetLocation(mazeInstance.GetCell(new IntVector2(0, 0)));
        bombInstance[1].SetLocation(mazeInstance.GetCell(new IntVector2(0, mazeInstance.size.z - 1)));
        bombInstance[2].SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x - 1, 0)));
        bombInstance[3].SetLocation(mazeInstance.GetCell(new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1)));

        cupInstance = Instantiate(cupPrefab) as Cup;
        cupInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        if (mazeInstance != null)
        {
            Destroy(mazeInstance.gameObject);
        }
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
        }
        if (endInstance != null)
        {
            Destroy(endInstance.gameObject);
        }
        if (trapInstance != null)
        {
            Destroy(trapInstance.gameObject);
        }
        for (int i = 0; i < bombInstance.Length; i++)
        {
            if (bombInstance[i] != null)
            {
                Destroy(bombInstance[i].gameObject);
            }
        }
        StartCoroutine(BeginGame());
    }

    public void OnPlayClick()
    {
        Destroy(GameObject.Find("Camera(Clone)"));
        Destroy(GameObject.Find("Menu Canvas"));
        Instantiate(inGameCanvasPrefab);
        gameState = GameState.NewPlay;
    }
}