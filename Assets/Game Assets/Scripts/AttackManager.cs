using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public string attackName;
    public int attackDamage;
    public int poiseDamage;
    public float poiseMultiplier = 1;
    public float stunTime;

    public Attack(string name, int damage, int poise, float poiseMult, float stun)
    {
        attackName = name;
        attackDamage = damage;
        poiseDamage = poise;
        poiseMultiplier = poiseMult;
        stunTime = stun;
    }
}

public class AttackManager : MonoBehaviour, ISerializationCallbackReceiver
{
    [HideInInspector] public int index;
    [SerializeField] public Attack[] attacks = new Attack[] { };
    [HideInInspector] public List<string> attackList = new List<string>();
    [HideInInspector] public Attack currentAttack;
    public void Update()
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            if (index == i)
            {
                currentAttack = attacks[index];
            }
        }
    }
    public void SetAttack(string name)
    {
        int get = 0;
        for (int i = 0; i < attacks.Length; i++)
        {
            if (attacks[i].attackName == name)
                get = i;
        }
        index = get;
    }
    public void OnBeforeSerialize()
    {
        attackList.Clear();
        foreach (Attack a in attacks)
        {
            attackList.Add(a.attackName);
        }
    }
    public void OnAfterDeserialize() { }
}