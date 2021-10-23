using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public string attackName;
    public int attackDamage;
    public float stunTime;

    public Attack(string name, int damage, float stun)
    {
        attackName = name;
        attackDamage = damage;
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