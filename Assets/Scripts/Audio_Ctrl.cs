using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Ctrl : MonoBehaviour
{
    [SerializeField] public AudioSource m_Source;
    [SerializeField] public AudioSource b_Source;
    [SerializeField] public AudioSource shoot_Source;
    [SerializeField] public AudioSource bot_Source;

    public GameObject buttonP_On;
    public GameObject buttonP_Off;

    public AudioClip background;
    public AudioClip move;
    public AudioClip shoot;
    public AudioClip bot;

    private int isPlay;
    void Start()
    {
        isPlay = PlayerPrefs.GetInt("isplay", 1);
        b_Source.clip = move;
        shoot_Source.clip = shoot;
        bot_Source.clip = bot;
        if (m_Source != null)
        {
            m_Source.clip = background;

            if (isPlay == 1)
                m_Source.Play();
        }

        UpdateButtonUI();
    }
    public void ExploiSound()
    {
        if (isPlay == 1) 
        {
            b_Source.Play();
        }
        
    }
    public void ShootSound()
    {
        if (isPlay == 1)
        {
            shoot_Source.Play();
        }

    }
    public void StopP()
    {
        if (m_Source.isPlaying)
        {
            m_Source.Stop();
        }

        isPlay = 0;
        PlayerPrefs.SetInt("isplay", isPlay);
        PlayerPrefs.Save();

        UpdateButtonUI();
    }
    public void PlayP()
    {
        if (!m_Source.isPlaying)
        {
            m_Source.Play();
        }

        isPlay = 1;
        PlayerPrefs.SetInt("isplay", isPlay);
        PlayerPrefs.Save();

        UpdateButtonUI();
    }

    private void UpdateButtonUI()
    {
        buttonP_On.SetActive(isPlay == 1);
        buttonP_Off.SetActive(isPlay == 0);
    }
}
