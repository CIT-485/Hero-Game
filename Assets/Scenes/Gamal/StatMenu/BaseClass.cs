using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClass
{
    private int strength;
    private int vitality;
    private int dexterity;
    private int physicalDef;
    private int magicDef;
    private int fireDef;
    private int lightningDef;

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

}
