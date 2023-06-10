using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject loading;
    public void Play()
    {
        this.gameObject.SetActive(false);
        loading.SetActive(true);
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        op.allowSceneActivation = false;
        while (op.progress < .9f)
        {
            yield return null;
        }
        op.allowSceneActivation = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
