using UnityEngine;
using System.Collections;

public class PlayAgain : MonoBehaviour
{

    public void onPlayAgain()
    {
        //GameManager.gameState = GameState.Menu;
        Application.LoadLevel(0);
    }
}