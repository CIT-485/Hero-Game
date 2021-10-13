using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GiantRatAI : MonoBehaviour
{
    public AttackManager        attackManager;
    public Flag                 attack0_Flag;
    public Flag                 attack2_Flag;
    public List<GameObject>     hurtboxes = new List<GameObject>();
    public List<GameObject>     downHurtboxes = new List<GameObject>();
    public List<GameObject>     attack0_Hitboxes = new List<GameObject>();
    public List<GameObject>     attack1_Hitboxes = new List<GameObject>();
    public List<GameObject>     intoDown_Hitboxes = new List<GameObject>();
    public GameObject           damageFlash;
    public GameObject           locationPoint;
    public float                acceleration = 4f;
    public float                maxSpeed = 2f;
    public bool                 attacking;

    private Animator            animator;
    private GameObject          player;
    private HealthBar           healthBar;
    private Rigidbody2D         body2d;
    private bool                damaged = false;
    private bool                forward = false;
    private bool                decided = false;
    private bool                dontMove = false;
    private bool                grounded = false;
    private bool                doneMoving = false;
    private float               attackWaitTime = 0;
    private float               moveCommitTime = 0;
    public bool                 facingRight = false;
    private bool                doOnce = false;
    private bool                cinematic = false;
    
    
    public void Start()
    {
        healthBar = GetComponent<HealthBar>();
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        Vector2 directionalForce = Vector2.zero;
        attackWaitTime += Time.deltaTime;
        if (healthBar.currentHealth <= healthBar.maxHealth / 2)
        {
            if (!doOnce && !attacking)
            {
                doOnce = true;
                cinematic = true;
            }
            if (cinematic)
            {
                if (!doneMoving)
                {
                    // Move to set location
                    if (!facingRight)
                        ChangeFacingDirection();
                    if (transform.position.x < locationPoint.transform.position.x)
                    {
                        animator.SetBool("Moving", true);
                        directionalForce += new Vector2(acceleration, body2d.velocity.y);
                    }
                    else
                    {
                        animator.SetBool("Moving", false);
                        body2d.velocity = Vector2.zero;
                        ChangeFacingDirection();
                        StartCoroutine(Attack2(0));
                        doneMoving = true;
                    }
                }
                else
                {

                    // Getting down
                    // RAWR
                    // Spawn Falling Rocks
                    // Summon Laser
                    // Shoot Laser
                    // Start Phase 2
                    //doOnce = true;
                }
            }
        }
        else
        {
            if (!grounded && !attacking && attackWaitTime > 3)
            {
                int attackChance = Random.Range(0, 100);
                if (attack0_Flag.flagged)
                {
                    if (attackChance < 40)
                    {
                        float holdTime = Random.Range(0.75f, 1.5f);
                        StartCoroutine(Attack0(holdTime));
                        attackWaitTime = 0;
                    }
                    else if (attackChance >= 40 && attackChance < 80)
                    {
                        float holdTime = Random.Range(0.75f, 1.5f);
                        StartCoroutine(Attack1(holdTime));
                        attackWaitTime = 0;
                    }
                    else
                        attackWaitTime = 2;
                }
                else if (attack2_Flag.flagged)
                {
                    if (attackChance < 70)
                    {
                        animator.SetBool("Attack2", true);
                        float holdTime = Random.Range(0.75f, 1.5f);
                        StartCoroutine(Attack2(holdTime));
                        attackWaitTime = 0;
                    }
                }
            }
        }
        if (!attacking && !cinematic)
        {
            // Movement
            float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
            if (distance > 1)
            {
                if (player.transform.position.x < transform.position.x && facingRight)
                {
                    ChangeFacingDirection();
                }
                else if (player.transform.position.x > transform.position.x && !facingRight)
                {
                    ChangeFacingDirection();
                }
                if (distance > 1 && distance < 3)
                {
                    int moveChance = Random.Range(0, 100);
                    if (decided)
                    {
                        if (!dontMove)
                        {
                            if (forward)
                            {
                                moveCommitTime += Time.deltaTime;
                                animator.SetBool("Moving", true);
                                if (player.transform.position.x < transform.position.x)
                                    directionalForce += new Vector2(-acceleration, body2d.velocity.y);
                                else
                                    directionalForce += new Vector2(acceleration, body2d.velocity.y);
                            }
                            else
                            {
                                moveCommitTime += Time.deltaTime;
                                animator.SetBool("Moving", true);
                                if (player.transform.position.x < transform.position.x)
                                    directionalForce += new Vector2(acceleration, body2d.velocity.y);
                                else
                                    directionalForce += new Vector2(-acceleration, body2d.velocity.y);
                            }
                        }
                        else
                        {
                            moveCommitTime += Time.deltaTime;
                            body2d.velocity = new Vector2(body2d.velocity.x * 0.97f, body2d.velocity.y);
                            animator.SetBool("Moving", false);
                        }
                    }
                    else
                    {
                        if (!dontMove)
                        {
                            if (moveChance < 80)
                            {
                                forward = true;
                                decided = true;
                            }
                            else if (moveChance >= 80 && moveChance < 90)
                            {
                                forward = false;
                                decided = true;
                            }
                        }
                        else
                        {
                            dontMove = true;
                            decided = true;
                        }
                    }
                }
                else if (distance >= 3 && decided)
                {
                    if (forward)
                    {
                        moveCommitTime += Time.deltaTime;
                        animator.SetBool("Moving", true);
                        if (player.transform.position.x < transform.position.x)
                            directionalForce += new Vector2(-acceleration, body2d.velocity.y);
                        else
                            directionalForce += new Vector2(acceleration, body2d.velocity.y);
                    }
                    else
                    {
                        moveCommitTime += Time.deltaTime * 1.2f;
                        animator.SetBool("Moving", true);
                        if (player.transform.position.x < transform.position.x)
                            directionalForce += new Vector2(acceleration, body2d.velocity.y);
                        else
                            directionalForce += new Vector2(-acceleration, body2d.velocity.y);
                    }
                }
                else if (distance >= 3 && !decided)
                {
                    moveCommitTime += Time.deltaTime;
                    animator.SetBool("Moving", true);
                    if (player.transform.position.x < transform.position.x)
                        directionalForce += new Vector2(-acceleration, body2d.velocity.y);
                    else
                        directionalForce += new Vector2(acceleration, body2d.velocity.y);
                }
            }
            else
            {
                decided = false;
                moveCommitTime = 0;
                body2d.velocity = new Vector2(body2d.velocity.x * 0.97f, body2d.velocity.y);
                animator.SetBool("Moving", false);
            }
            if (moveCommitTime > 1.2f)
            {
                decided = false;
                moveCommitTime = 0;
            }
        }

        body2d.AddForce(directionalForce);

        if (body2d.velocity.x > maxSpeed)
            body2d.velocity = new Vector2(maxSpeed, body2d.velocity.y);
        else if (body2d.velocity.x < -maxSpeed)
            body2d.velocity = new Vector2(-maxSpeed, body2d.velocity.y);
    }
    private void ChangeFacingDirection()
    {
        if (facingRight)
        {
            facingRight = false;
            GetComponent<SpriteRenderer>().flipX = false;
            FixHitboxes(attack0_Hitboxes);
            FixHitboxes(attack1_Hitboxes);
            FixHitboxes(intoDown_Hitboxes);
            FixHitboxes(hurtboxes);
            FixHitboxes(downHurtboxes);
            FixHitboxes(attack2_Flag);
        }
        else
        {
            facingRight = true;
            GetComponent<SpriteRenderer>().flipX = true;
            FixHitboxes(attack0_Hitboxes);
            FixHitboxes(attack1_Hitboxes);
            FixHitboxes(intoDown_Hitboxes);
            FixHitboxes(hurtboxes);
            FixHitboxes(downHurtboxes);
            FixHitboxes(attack2_Flag);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox" && !damaged)
        {
            damaged = true;
            StartCoroutine(invulActivate(player.GetComponent<AttackManager>().currentAttack.stunTime)); 
            if (!cinematic)
                healthBar.TakeDamage(player.GetComponent<AttackManager>().currentAttack.attackDamage);
            else
                healthBar.TakeDamage(player.GetComponent<AttackManager>().currentAttack.attackDamage / 10);
        }
    }
    private void FixHitboxes(List<GameObject> hitboxes)
    {
        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.transform.localPosition = new Vector2(-hitbox.transform.localPosition.x, hitbox.transform.localPosition.y);
        }
    }
    private void FixHitboxes(Flag flag)
    {
        flag.transform.GetComponent<StickToObject>().positionOffset.x = -flag.transform.GetComponent<StickToObject>().positionOffset.x;
    }
    private void disableHitboxes(List<GameObject> hitboxes)
    {
        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.SetActive(false);
        }
    }
    private void enableHitboxes(List<GameObject> hitboxes)
    {
        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.SetActive(true);
        }
    }
    IEnumerator invulActivate(float time)
    {
        damageFlash.SetActive(true);
        yield return new WaitForSeconds(time);
        damageFlash.SetActive(false);
        damaged = false;
    }
    IEnumerator Attack0(float time)
    {
        attackManager.index = 0;
        animator.SetBool("Moving", false);
        attacking = true;
        animator.SetTrigger("Attack0_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack0_Trigger");
    }
    IEnumerator Attack1(float time)
    {
        attackManager.index = 1;
        animator.SetBool("Moving", false);
        attacking = true;
        animator.SetTrigger("Attack1_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack1_0_Trigger");
        yield return new WaitForSeconds(time/2);
        animator.SetTrigger("Attack1_1_Trigger");
    }
    IEnumerator Attack2(float time)
    {
        attackManager.index = 2;
        animator.SetBool("Moving", false);
        attacking = true;
        animator.SetTrigger("IntoDown_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("IntoDown_Trigger");
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
        attacking = false;
    }
    public void Attack1_Hitbox0()
    {
        attack1_Hitboxes[0].SetActive(true);
    }
    public void Attack1_Hitbox0_End()
    {
        attack1_Hitboxes[0].SetActive(false);
    }
    public void Attack1_Hitbox1()
    {
        attack1_Hitboxes[1].SetActive(true);
    }
    public void Attack1_Hitbox1_End()
    {
        attack1_Hitboxes[1].SetActive(false);
    }
    public void Attack1_Recovery()
    {
        attacking = false;
    }
    public void Into_Down_Hitbox0()
    {
        intoDown_Hitboxes[0].SetActive(true);
    }
    public void Into_Down_Hitbox1()
    {
        intoDown_Hitboxes[0].SetActive(false);
        intoDown_Hitboxes[1].SetActive(true);
        disableHitboxes(hurtboxes);
        enableHitboxes(downHurtboxes);
    }
    public void Into_Down_Hitbox2()
    {
        intoDown_Hitboxes[1].SetActive(false);
        intoDown_Hitboxes[2].SetActive(true);
    }
    public void Into_Down_Hitbox2_End()
    {

        intoDown_Hitboxes[2].SetActive(false);
    }
    public void Down_Idle_Start()
    {
        attacking = false;
    }
    public void Into_Up_End()
    {
        animator.SetBool("Attack2", false);
        attacking = false;
        enableHitboxes(hurtboxes);
        disableHitboxes(downHurtboxes);
    }
}
