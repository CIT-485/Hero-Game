using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Event_Trigger : MonoBehaviour
{
    
    public SpriteRenderer customImage;
    public GameObject PNG;
    public bool isImageOn;
    private int count = 0;
    //private void Awake() => customImage.enabled = false;
    // Start is called before the first frame update

    void Start()
    {
        customImage.enabled = false;
        isImageOn = false;
    }
    //display tutorial box on enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(count == 0)
            {
                //Instantiate(PNG, new Vector3(0, 0, 0), Quaternion.identity);
                Debug.Log("entered");
                customImage.enabled = true;
                isImageOn = true;
                count += 1;
                
            }
        }

    }
    //hide tutorial box on exit
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Destroy(PNG);
            Debug.Log("exited");
            customImage.enabled = false;
            isImageOn = false;
        }
    }

}
