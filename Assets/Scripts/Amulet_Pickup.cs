using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Amulet_Pickup : MonoBehaviour
{
    public SpriteRenderer amuletImage;
    public SpriteRenderer tutorialBox;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        amuletImage.enabled = false;
        tutorialBox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(count == 0)
            {
                amuletImage.enabled = true;
                tutorialBox.enabled = true;
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            tutorialBox.enabled = false;
        }
    }
}
