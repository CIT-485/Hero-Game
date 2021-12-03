using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToNextScene : MonoBehaviour
{
    private Player player;
    private GameMaster gm;
    public Vector2 nextMapStartingPosition;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gm.Save();
            StartCoroutine(NextLevel());
            player.actionAllowed = false;
        }
    }
    IEnumerator NextLevel()
    {
        player.aspectRatio.GetComponent<Animator>().SetTrigger("FadeIn");
        player.crossfade.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(4f);
        GameMaster gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        gm.playerData.lastRespawnPos = nextMapStartingPosition;
        gm.lastRespawnPos = nextMapStartingPosition;
        gm.playerData.currenthealth = 9999;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
