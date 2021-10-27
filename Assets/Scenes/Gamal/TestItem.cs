using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;
public class TestItem : PlayerBaseStat
{
    //public PlayerBaseStat modifiedStat;

    /*
    // Start is called before the first frame update
    void Start()
    {
        Equip(modifiedStats);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Equip(PlayerBaseStat item)
    {
        // define value and type
        item.Attack.AddModifier(new StatModifier(5, StatModType.Flat));
        Debug.Log(item.Attack.Value);
    }
    */


    private StatModifier flat;
    private StatModifier percent;

    public void Equip(PlayerBaseStat item)
    {
        flat = new StatModifier(10, StatModType.Flat);
        //item.Attack.AddModifier(flat);

        percent = new StatModifier(0.1f, StatModType.PercentAdd);
        //item.Defense.AddModifier(percent);
        //
        //Debug.Log(item.Attack.Value);
       // Debug.Log(item.Defense.Value);

    }


}
