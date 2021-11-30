using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAI : Enemy
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public AttackManager am;
    [HideInInspector] public HealthBar health;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer render;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isDamaged = false;
    [HideInInspector] public bool isAbsorbed = false;
    [HideInInspector] public bool waypointReached = false;
    [HideInInspector] public float waitTime = 0;
    [HideInInspector] public float attackWaitTime = 0;
    private GameObject currentStop;
    private float jumpWaitTime = 0;

    public BehaviourTree tree;
    public GameObject player;
    public GameObject attackHitboxes;
    public GameObject damageFlash;
    public GameObject healtBarCanvas;

    public float acceleration = 5;
    public float maxSpeed = 2.5f;
    public float jumpForce = 2.5f;
    public int waypointIndex = 0;
    public bool facingRight;
    public bool doOnce;

    public List<Waypoint> waypoints = new List<Waypoint>();



    private void Awake()
    {
        tree = tree.Clone();
        tree.Bind();
    }
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<HealthBar>();
        player = GameObject.FindGameObjectWithTag("Player");
        tree.blackboard.delegates.GetValue("Patrol") = Patrol;
        tree.blackboard.delegates.GetValue("Aggro") = Aggro;
        tree.blackboard.delegates.GetValue("Attack") = Attack;
        tree.blackboard.delegates.GetValue("Dead") = Dead;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        am = GetComponent<AttackManager>();
    }

    // Update is called once per frame
    void Update()
    {
        tree.blackboard.integers.GetValue("currentHP") = health.currentHealth;
        tree.blackboard.floats.GetValue("distance") = Mathf.Abs(player.transform.position.x - transform.position.x);
        tree.Update();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitbox"))
        {
            health.TakeDamage(player.GetComponent<AttackManager>().currentAttack.attackDamage);
        }
    }

    Node.State Dead()
    {
        attackHitboxes.SetActive(false);
        healtBarCanvas.SetActive(false);
        tree.blackboard.booleans.GetValue("IsActive") = false;
        ReduceVelocity();
        if (!doOnce)
        {
            animator.SetTrigger("Death");
            doOnce = true;
        }
        if (isAbsorbed)
            return Node.State.SUCCESS;
        return Node.State.RUNNING;
    }

    Node.State Patrol()
    {
        if (IsDead())
            return Node.State.FAILURE;
        Vector2 directionalForce = Vector2.zero;
        if (tree.blackboard.floats.GetValue("distance") < 5)
        {
            return Node.State.FAILURE;
        }
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
                if (facingRight)
                    ChangeFacingDirection();
                directionalForce += new Vector2(acceleration, 0);
            }
            else
            {
                if (!facingRight)
                    ChangeFacingDirection();
                directionalForce += new Vector2(-acceleration, 0);
            }
        }

        Move(directionalForce);
        return Node.State.RUNNING;
    }

    Node.State Aggro()
    {
        if (IsDead())
            return Node.State.FAILURE;
        // The attack wait time is also built up during the chase.
        attackWaitTime += Time.deltaTime;
        tree.blackboard.floats.GetValue("distance") = Mathf.Abs(player.transform.position.x - transform.position.x);

        // if the player is within attacking distance, then it will return success
        if (tree.blackboard.floats.GetValue("distance") < 1)
        {
            animator.SetInteger("AnimState", 0);
            return Node.State.SUCCESS;
        }

        // if the player is outside agro range, then it will return failure
        else if (tree.blackboard.floats.GetValue("distance") > 5f)
        {
            animator.SetInteger("AnimState", 0);
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
            if (facingRight)
                ChangeFacingDirection();
            directionalForce += new Vector2(acceleration, 0);
        }
        else
        {
            if (!facingRight)
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

    Node.State Attack()
    {
        if (IsDead())
            return Node.State.FAILURE;
        // Increase wait time by the amount of seconds that has elapsed since the last frame
        attackWaitTime += Time.deltaTime;
        tree.blackboard.floats.GetValue("distance") = Mathf.Abs(player.transform.position.x - transform.position.x);

        // If an attack is not in motion
        if (!isAttacking)
        {
            animator.SetInteger("AnimState", 0);
            // When the player is out of range, then this action will fail
            if (tree.blackboard.floats.GetValue("distance") > 1)
                return Node.State.FAILURE;

            // This will correct the facing direction of the bandit
            if (transform.position.x < player.transform.position.x)
            {
                if (facingRight)
                    ChangeFacingDirection();
            }
            else
            {
                if (!facingRight)
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

    private void Move(Vector2 directionalForce)
    {
        // This makes it so it can move, but only at the maxSpeed designated in the inspector
        rb.AddForce(directionalForce);
        if (rb.velocity.x > maxSpeed)
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        else if (rb.velocity.x < -2.5f)
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
    }

    void ReduceVelocity(float reducePercentage = 0.9f)
    {
        rb.velocity = new Vector2(rb.velocity.x * reducePercentage, rb.velocity.y);
    }

    void Jump()
    {
        if (Grounded)
        {
            jumpWaitTime = 0;
            Grounded = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    void ChangeFacingDirection()
    {
        if (facingRight)
        {
            facingRight = false;
            render.flipX = false;
            FixHitboxes(attackHitboxes);
        }
        else
        {
            facingRight = true;
            render.flipX = true;
            FixHitboxes(attackHitboxes);
        }
    }

    bool IsDead()
    {
        if (health.currentHealth <= 0)
            return true;
        return false;
    }
    IEnumerator Attack0(float time)
    {
        am.SetAttack("Attack0");
        animator.SetTrigger("isAttacking");
        yield return new WaitForSeconds(time);
    }

    void AttackEnd()
    {
        isAttacking = false;
        attackHitboxes.SetActive(false);
    }

    void AttackHitboxStart()
    {
        attackHitboxes.SetActive(true);
    }
}
