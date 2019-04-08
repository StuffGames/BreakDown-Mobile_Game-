using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject youDiedGameObject;
    public GameObject BackButton;
    public PlayerBallControl player;

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayerDied()
    {
        BackButton.SetActive(false);
        youDiedGameObject.SetActive(true);
        StartCoroutine("SlowToHalt");
    }

    private IEnumerator SlowToHalt()
    {
        for (int i = 10; i >= 0; i --)
        {
            Time.timeScale = i/10f;
            yield return new WaitForSecondsRealtime(0.1f);
            //Debug.Log(Time.timeScale);
        }
    }

    bool inCheckpoint = false;

    public void CheckPointReached()
    {
        inCheckpoint = true;
    }

    public void RestartLevel()
    {
        if (!inCheckpoint)
        {
            Scene something = SceneManager.GetActiveScene();
            int index = something.buildIndex;
            SceneManager.LoadScene(index);
        }
        else if (inCheckpoint)
        {
            BackButton.SetActive(true);
            youDiedGameObject.SetActive(false);
            player.SendMessage("CheckPointRestart");
        }
        Time.timeScale = 1f;
    }

    public void PauseUnpause(bool pauseGame)
    {
        if (pauseGame)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
