using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    // Detection Point
    public Transform detectionPoint;
    // Detection radius
    private const float detectionRadius = 0.6f;
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
    public GameObject interactPrompt;
    public TMP_Text interactText;
    [Header("Others")]
    // List of picked items
    public List<GameObject> pickedItems = new List<GameObject>();

    public string fullText;
    public string currentText;

    // Update is called once per frame
    void Update()
    {
        DetectObject();
        if (!DetectObject() || GetComponent<Player>().damaged || GetComponent<InventorySystem>().isOpen)
        {
            // Hide the Examine Window
            examineWindow.SetActive(false);
            // disable the bool
            isExamining = false;
        }
    }

    public bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    // Detect whether your interacting with an object
    public bool DetectObject()
    {
        
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

        // If the obj is null then there is no interactable ojbect near the player
        // If obj is true 
        if (obj == null)
        {
            interactPrompt.GetComponent<Animator>().SetBool("Visible", false);
            detectedObject = null;
            return false;
        } 
        else
        {
            detectedObject = obj.gameObject;

            if (!isExamining && !GetComponent<InventorySystem>().isOpen)
            {
                if (detectedObject.GetComponent<Item>().interactType == Item.InteractionType.PickUp)
                {
                    interactPrompt.GetComponent<Animator>().SetBool("Visible", true);
                    interactText.text = "Pick up item";
                }
                else
                {
                    interactPrompt.GetComponent<Animator>().SetBool("Visible", true);
                    interactText.text = "Examine";
                }
            }
            else
                interactPrompt.GetComponent<Animator>().SetBool("Visible", false);
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
            if (item.type == Item.ItemType.Consumables)
            {
                Destroy(item.gameObject);
                item.exitEvent.Invoke();
            }
            StopCoroutine(ShowText(item.delay));
        }
        else
        {
            // Show the item's image in the middle
            if (item.image)
            {
                examineImage.sprite = item.image;
                examineImage.rectTransform.localScale = item.imageSize;
            }
            else
                examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            // Write description text underneath the image
            fullText = item.descriptionText;
            StartCoroutine(ShowText(item.delay));
            // examineText.text = item.descriptionText;
            // Display the Examine Window
            examineWindow.SetActive(true);
            // enable the bool
            isExamining = true;
        }
    }
    IEnumerator ShowText(float delay)
    {
        for(int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            examineText.text = currentText;
            if (delay > 0)
                yield return new WaitForSeconds(delay);
        }
    }
}
