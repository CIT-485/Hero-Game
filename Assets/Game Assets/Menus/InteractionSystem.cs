using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    // Detection Point
    public Transform detectionPoint;
    // Detection radius
    private const float detectionRadius = 0.2f;
    // Detection layer
    public LayerMask detectionLayer;

    // Cached Trigger Object
    public GameObject detectedObject;

    [Header("Examine Fields")]
    // Examine window object
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public bool isExamining;
    [Header("Others")]
    // List of picked items
    public List<GameObject> pickedItems = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if(DetectObject())
        {
            if(InteractInput())
            {
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }

    bool InteractInput()
    {
        //return Input.GetKeyDown(KeyCode.E);
        return (Input.GetMouseButtonDown(0) || Input.GetKeyDown("k"));
    }

    // Detect whether your interacting with an object
    bool DetectObject()
    {
        
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

        // If the obj is null then there is no interactable ojbect near the player
        // If obj is true 
        if (obj == null)
        {
            detectedObject = null;
            return false;
        } 
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }

    public void PickUpItem(GameObject item)
    {
        pickedItems.Add(item);
    }

    public void ExamineItem(Item item)
    {
        if(isExamining)
        {
            // Hide the Examine Window
            examineWindow.SetActive(false);
            // disable the bool
            isExamining = false;
        } else
        {
            // Show the item's image in the middle
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            // Write description text underneath the image
            examineText.text = item.descriptionText;
            // Display the Examine Window
            examineWindow.SetActive(true);
            // enable the bool
            isExamining = true;
        }
    }

    
}
