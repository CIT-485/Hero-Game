using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantRatAI : MonoBehaviour
{
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private GameObject          player;
    private int                 facingDirection = -1;
    private HealthBar           healthBar;
    private bool                damaged;
    public bool                 attacking;
    public Flag                 m_attack0_Flag;
    public List<GameObject>     m_attack0_Hitboxes = new List<GameObject>();

    public void Start()
    {
        healthBar = GetComponent<HealthBar>();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        if (m_attack0_Flag.flagged)
        {
            StartCoroutine(Attack0());
            StopCoroutine(Attack0());
        }
        if (attacking)
        {
            m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
        }
        else
        {
            if (!m_animator.GetBool("Moving"))
                m_animator.SetBool("Moving", true);
            if (player.transform.position.x < transform.position.x)
                m_body2d.velocity = new Vector2(-1, m_body2d.velocity.y);
            else
                m_body2d.velocity = new Vector2(1, m_body2d.velocity.y);
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
    IEnumerator Attack0()
    {
        while (m_attack0_Flag.flagged)
        {
            int rand = Random.Range(0, 100);
            Debug.Log(rand);
            if (rand < 30)
            {
                m_animator.SetBool("Moving", false);
                attacking = true;
                StartCoroutine(Attack0());
                m_attack0_Flag.flagged = false;
                m_attack0_Flag.paused = true;
                m_animator.SetTrigger("Attack0_Start_Trigger");
                yield return new WaitForSeconds(1.2f);
                m_animator.SetTrigger("Attack0_Trigger");
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
        }
    }
    public void Attack0_Hitbox0()
    {
        m_attack0_Hitboxes[0].SetActive(true);
    }
    public void Attack0_Hitbox1()
    {
        m_attack0_Hitboxes[0].SetActive(false);
        m_attack0_Hitboxes[1].SetActive(true);
    }
    public void Attack0_Hitbox2()
    {
        m_attack0_Hitboxes[1].SetActive(false);
        m_attack0_Hitboxes[2].SetActive(true);
    }
    public void Attack0_Hitbox3()
    {
        m_attack0_Hitboxes[2].SetActive(false);
        m_attack0_Hitboxes[3].SetActive(true);
    }
    public void Attack0_End()
    {
        m_attack0_Hitboxes[3].SetActive(false);
        m_attack0_Flag.paused = false;
        attacking = false;
    }
}
