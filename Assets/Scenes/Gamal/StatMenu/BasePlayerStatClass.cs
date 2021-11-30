using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerStatClass
{
    private BaseClass playerClass;

    private int strength;
    private int vitality;
    private int dexterity;
    private int physicalDef;
    private int magicDef;
    private int fireDef;
    private int lightningDef;

    public BaseClass PlayerClass
    {
        get
        {
            return playerClass;
        }
        set
        {
            playerClass = value;
        }
    }
    public int Strength
    {
        get
        {
            return strength;
        }
        set
        {
            strength = value;
        }
    }

    public int Vitality
    {
        get
        {
            return vitality;
        }
        set
        {
            vitality = value;
        }
    }

    public int Dexterity
    {
        get
        {
            return dexterity;
        }
        set
        {
            dexterity = value;
        }
    }

    public int PhysicalDef
    {
        get
        {
            return physicalDef;
        }
        set
        {
            physicalDef = value;
        }
    }

    public int MagicDef
    {
        get
        {
            return magicDef;
        }
        set
        {
            magicDef = value;
        }
    }

    public int FireDef
    {
        get
        {
            return fireDef;
        }
        set
        {
            fireDef = value;
        }
    }
    public int LightningDef
    {
        get
        {
            return lightningDef;
        }
        set
        {
            lightningDef = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
