using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySystem : MonoBehaviour
{
    
    //private static bool gameIsPaused = false;

    [Header("General Fields")]
    // List of items picked up
    public List<GameObject> items = new List<GameObject>();
    // flag indicates if the inventory is open or not
    public bool isOpen;
    [Header("UI Items Section")]
    // Inventory System Window
    public GameObject uiWindow;
    // Images of each item
    public Image[] itemsImages;
    // Elements of the description window
    [Header("UI Items Description")]
    public GameObject uiDescriptionWindow;
    public Image descriptionImage;
    public Text descriptionTitle;
    public Text descriptionText;

    // 
    public HealthBar healthBar;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }
    
    void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBar>();
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        uiWindow.SetActive(isOpen);
        UpdateUI();
    }

    // Add the item to the items list
    public void PickUp(GameObject item)
    {
        items.Add(item);
        UpdateUI();
    }

    // Refresh the UI elements in the inventory window
    // If the player doesn't have any items in the inventory system
    // then the containers are hidden
    void UpdateUI()
    {
        HideAll();
        // for each item in the "items" list
        // show it in the respective slot in the itemsImages
        for (int i = 0; i < items.Count; i++) {
            itemsImages[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            itemsImages[i].gameObject.SetActive(true);
        }
    }

    // Hide all of the items ui images 
    void HideAll()
    {
        foreach(var i in itemsImages)
        {
            i.gameObject.SetActive(false);
            HideDescription();
        }
    }

    public void ShowDescription(int id)
    {
        // Set the image
        descriptionImage.sprite = itemsImages[id].sprite;
        // Set the title
        descriptionTitle.text = items[id].name;
        // Show the description
        descriptionText.text = items[id].GetComponent<Item>().descriptionText;
        // Show the elements
        descriptionImage.gameObject.SetActive(true);
        descriptionTitle.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        descriptionImage.gameObject.SetActive(false);
        descriptionTitle.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }

    // Player has consumed a healing item
    public void Consume(int id)
    {
        // Get the a consumable item
        if(items[id].GetComponent<Item>().type == Item.ItemType.Consumables) 
        {
            Debug.Log($"CONSUMED {items[id].name}");
            // Invoke the consume custome event
            items[id].GetComponent<Item>().consumeEvent.Invoke();
            // destroy the item in a short period of time
            Destroy(items[id], 0.1f);
            // Clear the item from the list
            items.RemoveAt(id);
            // Update the UI
            UpdateUI();

            healthBar.Healing(25);
        }
    }
}
