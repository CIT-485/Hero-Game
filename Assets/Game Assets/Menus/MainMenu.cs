using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private GameObject crossfade;
    private bool fade;
    private bool doOnce;
    private void Awake()
    {
        crossfade = GameObject.Find("Crossfade");
        crossfade.GetComponent<Animator>().SetTrigger("FadeOut");
    }
    private void Update()
    {
        if (fade && !doOnce)
        {
            doOnce = true;
            crossfade.GetComponent<Animator>().SetTrigger("FadeIn");
        }
        if (fade)
        {
            GameObject.Find("BGM").GetComponent<AudioSource>().volume -= GameObject.Find("BGM").GetComponent<AudioSource>().volume * Time.deltaTime;
        }
    }
    // Function to start the game
    public void PlayGame()
    {
        if (!fade)
            StartCoroutine(NewGame());
    }
    public void Continue()
    {
        if (!fade)
            StartCoroutine(ContinueGame());
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator NewGame()
    {
        fade = true;
        yield return new WaitWhile(() => crossfade.GetComponent<CanvasGroup>().alpha < 1);
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().playerData.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator ContinueGame()
    {
        fade = true;
        yield return new WaitWhile(() => crossfade.GetComponent<CanvasGroup>().alpha < 1);
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        SceneManager.LoadScene(GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().playerData.scene);
    }
}
