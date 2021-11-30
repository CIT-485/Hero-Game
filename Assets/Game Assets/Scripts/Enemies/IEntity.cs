using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    bool Grounded { get; set; }
    bool IsDead { get; set; }
    int CorruptionValue { get; set; }
}
