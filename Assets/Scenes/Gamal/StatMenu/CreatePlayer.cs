using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreatePlayer : MonoBehaviour
{
    private BasePlayerStatClass newPlayer;

    // UI elements
    public Text strengthText;
    public Text vitalityText;
    public Text dexterityText;
    public Text physicalDefText;
    public Text magicDefText;
    public Text fireDefText;
    public Text lightningDefText;


    private int pointsToSpend = 20;
    public Text pointText;

    // Start is called before the first frame update
    void Start()
    {
        newPlayer = new BasePlayerStatClass();
    }

    public void SetKnightClass()
    {
        pointsToSpend = 20;
        newPlayer.PlayerClass = new BaseKnightClass();
        newPlayer.Strength = newPlayer.PlayerClass.Strength;
        newPlayer.Vitality = newPlayer.PlayerClass.Vitality;
        newPlayer.Dexterity = newPlayer.PlayerClass.Dexterity;
        newPlayer.PhysicalDef = newPlayer.PlayerClass.PhysicalDef;
        newPlayer.MagicDef = newPlayer.PlayerClass.MagicDef;
        newPlayer.FireDef = newPlayer.PlayerClass.FireDef;
        newPlayer.LightningDef = newPlayer.PlayerClass.LightningDef;
        // update UI
        UpdateUI();
    }

    void UpdateUI()
    {
        strengthText.text = newPlayer.Strength.ToString();
        vitalityText.text = newPlayer.Vitality.ToString();
        dexterityText.text = newPlayer.Dexterity.ToString();
        physicalDefText.text = newPlayer.PhysicalDef.ToString();
        magicDefText.text = newPlayer.MagicDef.ToString();
        fireDefText.text = newPlayer.FireDef.ToString();
        lightningDefText.text = newPlayer.LightningDef.ToString();
        pointText.text = pointsToSpend.ToString();
    }

}
