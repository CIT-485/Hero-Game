using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditAI : Enemy
{
    [HideInInspector] public Animator           animator;
    [HideInInspector] public AttackManager      am;
    [HideInInspector] public HealthBar          healthBar;
    [HideInInspector] public Rigidbody2D        rb;
    [HideInInspector] public SpriteRenderer     render;
    [HideInInspector] public bool               isAttacking = false;
    [HideInInspector] public bool               isDamaged = false;
    [HideInInspector] public bool               isAbsorbed = false;
    [HideInInspector] public bool               waypointReached = false;
    [HideInInspector] public float              waitTime = 0;
    [HideInInspector] public float              attackWaitTime = 0;
    private GameObject                          currentStop;
    private float                               jumpWaitTime = 0;

    public BehaviourTree                        tree;
    public GameObject                           player;
    public GameObject                           attackHitboxes;
    public GameObject                           damageFlash;
    public GameObject                           healtBarCanvas;

    public float                                acceleration = 5;
    public float                                maxSpeed = 2.5f;
    public float                                jumpForce = 2.5f;
    public int                                  waypointIndex = 0;

    public List<Waypoint>                       waypoints = new List<Waypoint>();

    // Start is called before the first frame update
    void Awake()
    {
        tree = tree.Clone();
        tree.Bind();
    }

    private void Start()
    {
        am = GetComponent<AttackManager>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<HealthBar>();
        player = GameObject.FindGameObjectWithTag("Player");
        tree.blackboard.delegates.GetValue("Patrol") = Patrol;
        tree.blackboard.delegates.GetValue("Chase") = Chase;
        tree.blackboard.delegates.GetValue("Combat") = Combat;
        tree.blackboard.delegates.GetValue("Dead") = Dead;
        tree.blackboard.integers.GetValue("MaxHP") = healthBar.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        tree.blackboard.integers.GetValue("CurrentHP") = healthBar.currentHealth;
        tree.blackboard.vector2s.GetValue("CurrentPosition") = transform.position;
        tree.blackboard.vector2s.GetValue("PlayerPosition") = player.transform.position;
        tree.Update();
        animator.SetBool("Grounded", Grounded);
    }
    Node.State Dead()
    {
        healtBarCanvas.SetActive(false);
        tree.blackboard.booleans.GetValue("IsActive") = false;
        ReduceVelocity();
        animator.SetTrigger("Death");
        if (isAbsorbed)
            return Node.State.SUCCESS;
        return Node.State.RUNNING;
    }
    Node.State Combat()
    {
        if (IsDead())
            return Node.State.FAILURE;
        // Increase wait time by the amount of seconds that has elapsed since the last frame
        attackWaitTime += Time.deltaTime;
        tree.blackboard.floats.GetValue("Distance") = Mathf.Abs(player.transform.position.x - transform.position.x);

        // If an attack is not in motion
        if (!isAttacking)
        {
            animator.SetInteger("AnimState", 1);
            // When the player is out of range, then this action will fail
            if (tree.blackboard.floats.GetValue("Distance") > 1 || tree.blackboard.floats.GetValue("DistanceY") > 2)
                return Node.State.FAILURE;

            // This will correct the facing direction of the bandit
            if (transform.position.x < player.transform.position.x)
            {
                if (!render.flipX)
                    ChangeFacingDirection();
            }
            else
            {
                if (render.flipX)
                    ChangeFacingDirection();
            }
        }

        // This is to reduce the bandit's velocity so it's standing still
        ReduceVelocity();

        // When the attack wait time is over 3 seconds, then it will decide on an attack
        if (attackWaitTime > 3)
        {
            int attackChance = Random.Range(0, 100);
            float holdTime = Random.Range(0.5f, 0.75f);
            if (attackChance < 75)
            {
                // it will have a 75% chance of performing an attack after which the attack wait time is refreshed back to zero 
                isAttacking = true;
                attackWaitTime = 0;
                StartCoroutine(Attack0(holdTime));
            }
            else
                // if it chooses not to attack, then the attack wait time is refreshed, but at a lower cooldown
                attackWaitTime = 2f;
        }
        return Node.State.RUNNING;
    }
    Node.State Chase()
    {
        if (IsDead())
            return Node.State.FAILURE;
        // The attack wait time is also built up during the chase.
        attackWaitTime += Time.deltaTime;
        tree.blackboard.floats.GetValue("Distance") = Mathf.Abs(player.transform.position.x - transform.position.x);

        // if the player is within attacking distance, then it will return success
        if (tree.blackboard.floats.GetValue("Distance") < 1)
        {
            animator.SetInteger("AnimState", 1);
            return Node.State.SUCCESS;
        }

        // if the player is outside agro range, then it will return failure
        else if (tree.blackboard.floats.GetValue("Distance") > 7.5f)
        {
            animator.SetInteger("AnimState", 0);
            tree.blackboard.booleans.GetValue("InCombat") = false;
            return Node.State.FAILURE;

        }

        // if the bandit hits a wall that reduces it's horizontal velocity to zero, then it will attempt to jump
        if (rb.velocity.x == 0)
            jumpWaitTime += Time.deltaTime;
        else
            jumpWaitTime = 0;
        if (jumpWaitTime > 0.1f)
            Jump();

        // This will correct the facing direction of the bandit and allows it to move
        Vector2 directionalForce = Vector2.zero;
        animator.SetInteger("AnimState", 2);
        if (transform.position.x < player.transform.position.x)
        {
            if (!render.flipX)
                ChangeFacingDirection();
            directionalForce += new Vector2(acceleration, 0);
        }
        else
        {
            if (render.flipX)
                ChangeFacingDirection();
            directionalForce += new Vector2(-acceleration, 0);
        }
        if (currentStop != null)
        {
            if ((player.transform.position.x > currentStop.transform.position.x && directionalForce.x <= 0) ||
                (player.transform.position.x < currentStop.transform.position.x && directionalForce.x >= 0))
                Move(directionalForce);
            else
                ReduceVelocity();
        }
        else
            Move(directionalForce);
        return Node.State.RUNNING;
    }
    Node.State Patrol()
    {
        if (IsDead())
            return Node.State.FAILURE;
        Vector2 directionalForce = Vector2.zero;

        // if the player enters agro range, then it will return a failure to the patrol delegate
        tree.blackboard.floats.GetValue("Distance") = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (tree.blackboard.floats.GetValue("Distance") < 5)
            tree.blackboard.booleans.GetValue("InCombat") = true;

        if (tree.blackboard.booleans.GetValue("InCombat"))
            return Node.State.FAILURE;

        // if the bandit has reached the waypoint, then it will wait the designated amount of time, then set the new waypoint to the next one on the list and return successful
        if (transform.position.x > waypoints[waypointIndex].transform.position.x - 0.1f &&
            transform.position.x < waypoints[waypointIndex].transform.position.x + 0.1f)
        {
            animator.SetInteger("AnimState", 0);
            waypointReached = true;
        }
        if (waypointReached)
        {
            ReduceVelocity();
            waitTime += Time.deltaTime;
            if (waitTime > waypoints[waypointIndex].stayTime)
            {
                waitTime = 0;
                waypointReached = false;
                waypointIndex++;
                if (waypointIndex >= waypoints.Count)
                    waypointIndex = 0;
                return Node.State.SUCCESS;
            }
        }
        // if the waypoint has not been reached yet, then it will move towards it.
        else
        {

            // if the bandit hits a wall that reduces it's horizontal velocity to zero, then it will attempt to jump
            if (rb.velocity.x == 0)
                jumpWaitTime += Time.deltaTime;
            else
                jumpWaitTime = 0;
            if (jumpWaitTime > 0.1f)
                Jump();

            animator.SetInteger("AnimState", 2);
            if (transform.position.x < waypoints[waypointIndex].transform.position.x)
            {
                if (!render.flipX)
                    ChangeFacingDirection();
                directionalForce += new Vector2(acceleration, 0);
            }
            else
            {
                if (render.flipX)
                    ChangeFacingDirection();
                directionalForce += new Vector2(-acceleration, 0);
            }
        }

        Move(directionalForce);
        return Node.State.RUNNING;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox" && !isDamaged && !IsDead())
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
        else if (collision.tag == "Stop")
        {
            if (collision.transform.position.x > transform.position.x)
            {
                transform.position += new Vector3(-0.05f, 0f);
            }
            if (collision.transform.position.x < transform.position.x)
            {
                transform.position += new Vector3(0.05f, 0f);
            }
            currentStop = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Stop")
        {
            currentStop = null;
        }
    }
    private void Move(Vector2 directionalForce)
    {
        // This makes it so it can move, but only at the maxSpeed designated in the inspector
        rb.AddForce(directionalForce);
        if (rb.velocity.x > maxSpeed)
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        else if (rb.velocity.x < -2.5f)
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
    }
    bool IsDead()
    {
        if (healthBar.currentHealth <= 0)
            return true;
        return false;
    }
    void Jump()
    {
        if (Grounded)
        {
            jumpWaitTime = 0;
            Grounded = false;
            animator.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
    void ReduceVelocity(float reducePercentage = 0.9f)
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
