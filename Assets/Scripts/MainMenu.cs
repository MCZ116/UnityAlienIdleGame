using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider slider;
    public Text progressText;
    public GameObject loadingBar;
    public GameObject[] menuButtons;
    public GameObject namePanel;
    public TextMeshProUGUI playerNameInput;
    public string playerName;
    public Text textError;
    public GameObject textErrorBox;

    private void Start()
    {            
        if (PlayerPrefs.GetString("Nick").Length > 3)
        {
            namePanel.SetActive(false);
        }
        else
        {
            namePanel.SetActive(true);
        }

    }

    public void PlayGame (int sceneIndex)
    {
        loadingBar.SetActive(true);
        menuButtons[0].SetActive(false);
        menuButtons[1].SetActive(false);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    public void NameCheckInput()
    {
        playerName = playerNameInput.text;

        if (playerName.Length > 3)
        {
            PlayerPrefs.SetString("Nick",playerName);
            textErrorBox.SetActive(false);
            namePanel.SetActive(false);
        }
        else {
            textErrorBox.SetActive(true);
            playerName = null;
            textError.text = "Not enough letters!";
        }

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
