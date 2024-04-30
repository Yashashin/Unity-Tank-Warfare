using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonHandler : MonoBehaviour
{
    public GameController gameController;
    public void RestartGame()
    {
        GetComponent<AudioSource>().Play();
        gameController.RestartGame();
    }
}
