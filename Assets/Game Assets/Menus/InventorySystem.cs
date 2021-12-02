using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventorySystem : MonoBehaviour
{

    //private static bool gameIsPaused = false;

    [Header("General Fields")]
    // List of items picked up
    public List<GameObject> items = new List<GameObject>();
    public List<AbilitySlot> abilities = new List<AbilitySlot>();
    public List<StatNumber> stats = new List<StatNumber>();
    private List<int> unmodifiedStats = new List<int>() { 0, 0, 0 };
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
    public TMP_Text descriptionTitle;
    public TMP_Text descriptionText;
    public Image abilityImage;
    public TMP_Text abilityTitle;
    public TMP_Text abilityText;
    private int pointCount = 0;
    private int originalPointsAvailable;
    private int pointsAvailable;
    public TMP_Text points;
    private int nextPointBase = 100;
    private int nextPointThreshold;
    public TMP_Text nextPoint;
    private PlayerStat stat;

    public Player player;
    public CameraFollowObject cam;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && player.actionAllowed)
        {
            ToggleInventory();
        }
        if (isOpen)
        {
            cam.targetZoom = 3;
            cam.positionOffset = new Vector2(3.5f, 1);
        }
        else
        {
            cam.targetZoom = 5;
            cam.positionOffset = new Vector2(0, 2);
        }
        if (player.IsDead || !player.actionAllowed)
        {
            if (isOpen)
                ToggleInventory();
        }
        foreach (AbilitySlot ab in abilities)
        {
            if (ab.name.Contains("Absorb"))
            {
                if (player.hasAmulet)
                    ab.image.color = new Color(255, 255, 255, 255);
            }
            else if (player.corruption >= ab.prerequisiteCorruption)
                ab.image.color = new Color(255, 255, 255, 255);
        }

        points.text = pointsAvailable.ToString();
        if (player.corruption > nextPointThreshold)
        {
            pointCount++;
            originalPointsAvailable++;
            Debug.Log(nextPointThreshold);
            nextPointThreshold += (int)(nextPointThreshold * 0.15f);
        }

        nextPoint.text = player.corruption + "/" + nextPointThreshold;
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        stat = GetComponent<PlayerStat>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollowObject>();
        unmodifiedStats[0] = (int)stat.strength.Value;
        unmodifiedStats[1] = (int)stat.vitality.Value;
        unmodifiedStats[2] = (int)stat.agility.Value;
        pointsAvailable = originalPointsAvailable;
        stats[0].number = unmodifiedStats[0];
        stats[1].number = unmodifiedStats[1];
        stats[2].number = unmodifiedStats[2];
        nextPointThreshold = nextPointBase;
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        uiWindow.SetActive(isOpen);
        unmodifiedStats[0] = (int)stat.strength.Value;
        unmodifiedStats[1] = (int)stat.vitality.Value;
        unmodifiedStats[2] = (int)stat.agility.Value;
        pointsAvailable = originalPointsAvailable;
        stats[0].number = unmodifiedStats[0];
        stats[1].number = unmodifiedStats[1];
        stats[2].number = unmodifiedStats[2];
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

    public void IncreaseStat(int id)
    {
        if (pointsAvailable > 0)
        {
            stats[id].number++;
            pointsAvailable--;
        }
    }
    public void DecreaseStat(int id)
    {
        if (stats[id].number > unmodifiedStats[id])
        {
            stats[id].number--;
            pointsAvailable++;
        }
    }
    public void SaveStats()
    {
        if (pointsAvailable != originalPointsAvailable)
        {
            stat.strength.BaseValue += Mathf.Abs(stats[0].number - unmodifiedStats[0]);
            stat.vitality.BaseValue += Mathf.Abs(stats[1].number - unmodifiedStats[1]);
            stat.agility.BaseValue += Mathf.Abs(stats[2].number - unmodifiedStats[2]);
            originalPointsAvailable = pointsAvailable;
            player.healthBar.SetMaxHealth(player.healthBar.baseHealth + 15 * (int)GetComponent<PlayerStat>().vitality.Value);

            unmodifiedStats[0] = (int)stat.strength.Value;
            unmodifiedStats[1] = (int)stat.vitality.Value;
            unmodifiedStats[2] = (int)stat.agility.Value;
            pointsAvailable = originalPointsAvailable;
            stats[0].number = unmodifiedStats[0];
            stats[1].number = unmodifiedStats[1];
            stats[2].number = unmodifiedStats[2];
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
    public void ShowAbility(int id)
    {
        // Set the image
        abilityImage.sprite = abilities[id].image.sprite;
        abilityImage.color = abilities[id].image.color;
        // Set the title
        abilityTitle.text = abilities[id].abilityName;
        // Show the description
        abilityText.text = abilities[id].abilityDescription;
        // Show the elements
        abilityImage.gameObject.SetActive(true);
        abilityTitle.gameObject.SetActive(true);
        abilityText.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        descriptionImage.gameObject.SetActive(false);
        descriptionTitle.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }
    public void HideAbility()
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
            // Invoke the consume custome event
            items[id].GetComponent<Item>().consumeEvent.Invoke();
            // Clear the item from the list
            items.RemoveAt(id);
            // Update the UI
            UpdateUI();
        }
    }
}
