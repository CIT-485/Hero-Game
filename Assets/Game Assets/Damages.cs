using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damages : MonoBehaviour
{
    public int activeDamage;
    public List<int> list = new List<int>();
    private void Start()
    {
        if (list.Count > 0)
            activeDamage = list[0];
    }
}