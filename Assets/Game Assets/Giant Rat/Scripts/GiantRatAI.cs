using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantRatAI : MonoBehaviour
{
    private Animator            animator;
    private GameObject          player;
    private HealthBar           healthBar;
    private Rigidbody2D         body2d;
    private bool                damaged;
    private float               attackWaitTime;
    //private int                 facingDirection = -1;
    
    public Flag                 attack0_Flag;
    public List<GameObject>     attack0_Hitboxes = new List<GameObject>();
    public bool                 attacking;

    public void Start()
    {
        healthBar = GetComponent<HealthBar>();
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        attackWaitTime += Time.deltaTime;
        if (attackWaitTime > 3)
        {
            if (attack0_Flag.flagged)
            {
                Debug.Log("attack ready");
                int attackChance = Random.Range(0, 100);
                Debug.Log(attackChance);
                if (attackChance < 30)
                {
                    float holdTime = Random.Range(1f, 2f);
                    StartCoroutine(Attack0(holdTime));
                    attackWaitTime = 0;
                }
                else
                {
                    Debug.Log("still thinking");
                    attackWaitTime = 2;
                }
            }
            else
            {
                Debug.Log("not in range");
            }
        }
        if (attacking)
        {
            body2d.velocity = new Vector2(0, body2d.velocity.y);
        }
        else
        {
            if (!animator.GetBool("Moving"))
                animator.SetBool("Moving", true);
            if (player.transform.position.x < transform.position.x)
                body2d.velocity = new Vector2(-1, body2d.velocity.y);
            else
                body2d.velocity = new Vector2(1, body2d.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox" && !damaged)
        {
            damaged = true;
            StartCoroutine(invul(collision.transform.parent.GetComponent<AttackManager>().currentAttack.stunTime));
            healthBar.TakeDamage(collision.transform.parent.GetComponent<AttackManager>().currentAttack.attackDamage);
        }
    }
    IEnumerator invul(float time)
    {
        yield return new WaitForSeconds(time);
        damaged = false;
    }
    IEnumerator Attack0(float time)
    {
        animator.SetBool("Moving", false);
        attacking = true;
        attack0_Flag.flagged = false;
        attack0_Flag.paused = true;
        animator.SetTrigger("Attack0_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack0_Trigger");
    }
    public void Attack0_Hitbox0()
    {
        attack0_Hitboxes[0].SetActive(true);
    }
    public void Attack0_Hitbox1()
    {
        attack0_Hitboxes[0].SetActive(false);
        attack0_Hitboxes[1].SetActive(true);
    }
    public void Attack0_Hitbox2()
    {
        attack0_Hitboxes[1].SetActive(false);
        attack0_Hitboxes[2].SetActive(true);
    }
    public void Attack0_Hitbox3()
    {
        attack0_Hitboxes[2].SetActive(false);
        attack0_Hitboxes[3].SetActive(true);
    }
    public void Attack0_End()
    {
        attack0_Hitboxes[3].SetActive(false);
    }
    public void Attack0_Cancellable()
    {
        attack0_Flag.paused = false;
        attacking = false;
    }
}
