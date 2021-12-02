using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AbilitySlot : MonoBehaviour
{
    public Image image;
    public string abilityName;
    [TextArea]
    public string abilityDescription;
    public bool unlocked;
    public float prerequisiteCorruption;
}