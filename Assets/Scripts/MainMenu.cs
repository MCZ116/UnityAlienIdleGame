﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider slider;
    public Text progressText;
    public GameObject loadingBar;
    public GameObject[] menuButtons;

    public void PlayGame (int sceneIndex)
    {
        loadingBar.SetActive(true);
        menuButtons[0].SetActive(false);
        menuButtons[1].SetActive(false);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneIndex);

        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
