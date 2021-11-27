using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEntity
{
    [SerializeField] private bool grounded;
    [SerializeField] private bool isDead;
    [SerializeField] private bool isAbsorbed;
    [SerializeField] private int corruptionValue;
    public bool Grounded { get => grounded; set => grounded = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsAbsorbed { get => isAbsorbed; set => isAbsorbed = value; }
    public int CorruptionValue { get => corruptionValue; set => corruptionValue = value; }

    public void FixHitboxes(List<GameObject> hitboxes)
    {
        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.transform.localPosition = new Vector2(-hitbox.transform.localPosition.x, hitbox.transform.localPosition.y);
        }
    }
    public void FixHitboxes(GameObject hitbox)
    {
        hitbox.transform.localPosition = new Vector2(-hitbox.transform.localPosition.x, hitbox.transform.localPosition.y);
    }
    public void FixHitboxes(Flag flag)
    {
        flag.transform.GetComponent<StickToObject>().positionOffset.x = -flag.transform.GetComponent<StickToObject>().positionOffset.x;
    }
    public void disableHitboxes(List<GameObject> hitboxes)
    {
        foreach (GameObject hitbox in hitboxes)
            hitbox.SetActive(false);
    }
    public void enableHitboxes(List<GameObject> hitboxes)
    {
        foreach (GameObject hitbox in hitboxes)
            hitbox.SetActive(true);
    }
}
