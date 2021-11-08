using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// makes sure that whenever a script is added to a specific object it's going to add a box collider by default and it's going to take the 
// shape of the object
[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    // Different types of item interaction
    // 
    public enum InteractionType
    {
        NONE, PickUp, Examine
    }

    public InteractionType type;
    [Header("Examine")]
    public string descriptionText;
    public Sprite image;
    public UnityEvent customEvent;

    // gets called in the editor only to set the default values of the component of the object
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 8;
    }

    
    public void Interact()
    {
        switch(type) 
        {
            case InteractionType.PickUp:
                //Debug.Log("PICK UP");
                // Add the object to the PickedUpItems list
                FindObjectOfType<InventorySystem>().PickUp(gameObject);
                // Disable
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                //Debug.Log("Examine");
                // Call the Examine item in the interaction system
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                break;
            default:
               // Debug.Log("NULL ITEM");
                break;
        }

        // Invoke (call) the custom event(s)
        customEvent.Invoke();
    }
    


}
