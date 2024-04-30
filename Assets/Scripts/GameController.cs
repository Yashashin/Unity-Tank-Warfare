using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject gameOverSprite;
    public GameObject missionCompleteSprite;
    public GameObject restartButton;
    public Camera gameOverCam;
    public Transform player;
    public GameObject minimapCam;
    public int enemyCount;
    public GameObject bgm;
 public void GameOver()
    {
        gameOverCam.targetDisplay = 0;
        gameOverCam.gameObject.GetComponent<AudioListener>().enabled = true;
        gameOverCam.gameObject.transform.position = player.position-player.forward+player.up*5;
        minimapCam.GetComponent<MinimapScript>().enabled = false; 
        gameOverSprite.SetActive(true);
        restartButton.SetActive(true);
        Cursor.visible = true;  
    }  
    public void DestroyEnemy()
    {
        enemyCount--;
        if(enemyCount==0)
        {
            MissionComplete();
        }
    }

    public void  MissionComplete()
    {
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        missionCompleteSprite.SetActive(true);
        restartButton.SetActive(true);
        Cursor.visible = true;
        bgm.GetComponent<AudioSource>().Play();

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
