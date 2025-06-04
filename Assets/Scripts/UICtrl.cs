using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICtrl : MonoBehaviour
{
    [SerializeField] private GameObject PausePn;


    public void Play()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1.0f;
    }
    public void Pause()
    {
        PausePn.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Continue()
    {
        PausePn.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }
    public void Homer()
    {
        SceneManager.LoadScene("Home");
        Time.timeScale = 1.0f;
    }
}
