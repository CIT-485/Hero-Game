using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToNextScene : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(NextLevel());
            player.actionAllowed = false;
        }
    }
    IEnumerator NextLevel()
    {
        player.aspectRatio.GetComponent<Animator>().SetTrigger("FadeIn");
        player.crossfade.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(4f);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
