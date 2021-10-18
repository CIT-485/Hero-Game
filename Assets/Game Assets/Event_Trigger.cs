using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Event_Trigger : MonoBehaviour
{
    
    public Image customImage;
    private int count = 0;
    //display tutorial box on enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(count == 0)
            {
                
                customImage.enabled = true;
                count += 1;
                
            }
        }

    }
    //hide tutorial box on exit
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            customImage.enabled = false;
        }
    }

}
