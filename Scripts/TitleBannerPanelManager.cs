using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBannerPanelManager : MonoBehaviour
{
    public Button beginButton;

    public AudioSource audioSource;
    public AudioClip selectSound;
    public AudioClip mainTheme;
    public float volume = 1f;

    private void Start()
    {
        beginButton.onClick.AddListener(GameStart);

        audioSource.clip = mainTheme;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void GameStart()
    {
        SceneLoader.Instance.LoadScene("Home");
        audioSource.PlayOneShot(selectSound, volume);
    }
}
