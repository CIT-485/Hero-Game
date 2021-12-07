using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditDemoAI : Enemy
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public AttackManager am;
    [HideInInspector] public HealthBar healthBar;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer render;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isDamaged = false;
    private Player player;

    public GameObject attackHitboxes;
    public GameObject damageFlash;
    public GameObject healtBarCanvas;

    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<AttackManager>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<HealthBar>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //healthBar.TakeDamage(2000);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (distance < 4)
            animator.SetInteger("AnimState", 1);
        else
            animator.SetInteger("AnimState", 0);

        if (healthBar.currentHealth <= 0)
        {
            healtBarCanvas.SetActive(false);
            animator.SetTrigger("Death");
            IsDead = true;
        }
        else if (Input.GetKeyDown(KeyCode.T) && !isAttacking && !IsDead)
            StartCoroutine(Attack0(0.75f));

        ReduceVelocity();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox" && !isDamaged && !IsDead)
        {
            isDamaged = true;
            StartCoroutine(invul(player.GetComponent<AttackManager>().currentAttack.stunTime));
            if (!isAttacking)
                animator.SetTrigger("Hurt");
            healthBar.TakeDamage(player.GetComponent<AttackManager>().currentAttack.attackDamage);
            if (collision.transform.parent.position.x < transform.position.x)
                rb.AddForce(new Vector2(30, 10));
            else
                rb.AddForce(new Vector2(-30, 10));
        }
    }
    void ChangeFacingDirection()
    {
        if (render.flipX)
        {
            render.flipX = false;
            FixHitboxes(attackHitboxes);
        }
        else
        {
            render.flipX = true;
            FixHitboxes(attackHitboxes);
        }
    }
    void ReduceVelocity(float reducePercentage = 0.6f)
    {
        rb.velocity = new Vector2(rb.velocity.x * reducePercentage, rb.velocity.y);
    }
    IEnumerator Attack0(float time)
    {
        am.SetAttack("Attack0");
        animator.SetTrigger("StartAttackTrigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("AttackTrigger");
    }
    IEnumerator invul(float time)
    {
        damageFlash.SetActive(true);
        yield return new WaitForSeconds(time);
        damageFlash.SetActive(false);
        isDamaged = false;
    }
    void Attack_Start()
    {
        isAttacking = true;
    }
    void Attack_Active()
    {
        attackHitboxes.SetActive(true);
    }

    void Attack_End()
    {
        attackHitboxes.SetActive(false);
        isAttacking = false;
    }
}
