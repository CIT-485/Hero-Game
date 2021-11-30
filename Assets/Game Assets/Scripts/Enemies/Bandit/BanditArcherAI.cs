using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditArcherAI : Enemy
{
    private Animator animator;
    private AttackManager am;
    private HealthBar healthBar;
    private Rigidbody2D rb;
    private SpriteRenderer render;
    private GameObject player;
    private GameObject currentStop;
    private bool isAttacking = false;
    private bool isDamaged = false;
    private bool waypointReached = false;
    private bool doOnce = false;
    private float waitTime = 0;
    private float attackWaitTime = 0;
    private float jumpWaitTime = 0;

    public BehaviourTree tree;
    public GameObject arrow;
    public GameObject arrowSpawnPoint;
    public GameObject damageFlash;
    public GameObject healtBarCanvas;

    public float xReposRange = 3;
    public float xStartAggroRange = 8;
    public float xEndAggroRange = 10;
    public float yStartAggroRange = 4;
    public float yEndAggroRange = 6;
    public float acceleration = 5;
    public float maxSpeed = 2.5f;
    public float jumpForce = 2.5f;
    public float arrowTime = 1;
    public int waypointIndex = 0;

    public List<Waypoint> waypoints = new List<Waypoint>();

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
        tree.blackboard.delegates.GetValue("Repos") = Repos;
        tree.blackboard.delegates.GetValue("Combat") = Combat;
        tree.blackboard.delegates.GetValue("Dead") = Dead;
        tree.blackboard.integers.GetValue("MaxHP") = healthBar.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        tree.blackboard.integers.GetValue("CurrentHP") = healthBar.currentHealth;
        tree.blackboard.floats.GetValue("XDelta") = Mathf.Abs(transform.position.x - player.transform.position.x);
        tree.blackboard.floats.GetValue("YDelta") = Mathf.Abs(transform.position.y - player.transform.position.y);
        tree.blackboard.floats.GetValue("XStartAggroRange") = xStartAggroRange;
        tree.blackboard.floats.GetValue("XEndAggroRange") = xEndAggroRange;
        tree.blackboard.floats.GetValue("XReposRange") = xReposRange;
        tree.blackboard.floats.GetValue("YStartAggroRange") = yStartAggroRange;
        tree.blackboard.floats.GetValue("YEndAggroRange") = yEndAggroRange;
        tree.Update();
        animator.SetBool("Grounded", Grounded);
    }
    Node.State Dead()
    {
        healtBarCanvas.SetActive(false);
        tree.blackboard.booleans.GetValue("IsActive") = false;
        ReduceVelocity();
        if (!doOnce)
        {
            doOnce = true;
            animator.SetTrigger("Death");
        }
        IsDead = true;
        if (IsAbsorbed)
            return Node.State.SUCCESS;
        return Node.State.RUNNING;
    }
    Node.State Combat()
    {
        if (healthBar.currentHealth <= 0)
            return Node.State.FAILURE;

        // Increase wait time by the amount of seconds that has elapsed since the last frame
        attackWaitTime += Time.deltaTime;
        tree.blackboard.floats.GetValue("XDelta") = Mathf.Abs(transform.position.x - player.transform.position.x);
        tree.blackboard.floats.GetValue("YDelta") = Mathf.Abs(transform.position.y - player.transform.position.y);

        // If an attack is not in motion
        if (!isAttacking)
        {
            animator.SetInteger("AnimState", 1);
            // When the player is out of range, then this action will fail
            if (tree.blackboard.floats.GetValue("XDelta") < xReposRange || tree.blackboard.floats.GetValue("XDelta") >= xEndAggroRange || tree.blackboard.floats.GetValue("YDelta") > yEndAggroRange)
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
        if (attackWaitTime > 2f)
        {
            int attackChance = Random.Range(0, 100);
            float holdTime = Random.Range(0.5f, 0.75f);
            if (attackChance < 85)
            {
                // it will have a 85% chance of performing an attack after which the attack wait time is refreshed back to zero 
                isAttacking = true;
                attackWaitTime = 0;
                StartCoroutine(Attack0(holdTime));
            }
            else
                // if it chooses not to attack, then the attack wait time is refreshed, but at a lower cooldown
                attackWaitTime = 1f;
        }
        return Node.State.RUNNING;
    }
    Node.State Repos()
    {
        if (healthBar.currentHealth <= 0)
            return Node.State.FAILURE;

        // The attack wait time is also built up during the chase.
        attackWaitTime += Time.deltaTime;
        tree.blackboard.floats.GetValue("XDelta") = Mathf.Abs(transform.position.x - player.transform.position.x);

        // if the player enters combat range, then it will return failure
        if (tree.blackboard.floats.GetValue("XDelta") > 3 || tree.blackboard.floats.GetValue("YDelta") > 6)
        {
            animator.SetInteger("AnimState", 1);
            return Node.State.FAILURE;
        }

        // if the bandit hits a wall that reduces it's horizontal velocity to zero, then it will attempt to jump
        if (Mathf.Abs(rb.velocity.x) < 0.0001f)
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
            if (render.flipX)
                ChangeFacingDirection();
            directionalForce += new Vector2(-acceleration, 0);
        }
        else
        {
            if (!render.flipX)
                ChangeFacingDirection();
            directionalForce += new Vector2(acceleration, 0);
        }

        // IF the bandit hits a stop trigger
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
        if (healthBar.currentHealth <= 0)
            return Node.State.FAILURE;
        Vector2 directionalForce = Vector2.zero;

        // if the player enters agro range, then it will return a failure to the patrol delegate
        tree.blackboard.floats.GetValue("XDelta") = Mathf.Abs(transform.position.x - player.transform.position.x);
        if (tree.blackboard.floats.GetValue("XDelta") < 7 && tree.blackboard.floats.GetValue("YDelta") < 4)
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
            if (Mathf.Abs(rb.velocity.x) < 0.0001f)
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
        if (render.flipX)
        {
            render.flipX = false;
            FixHitboxes(arrowSpawnPoint);
        }
        else
        {
            render.flipX = true;
            FixHitboxes(arrowSpawnPoint);
        }
    }
    void ReduceVelocity(float reducePercentage = 0.9f)
    {
        rb.velocity = new Vector2(rb.velocity.x * reducePercentage, rb.velocity.y);
    }
    IEnumerator Attack0(float time)
    {
        am.SetAttack("Attack0");
        animator.SetTrigger("Attack0_Start");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack0");
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
        Vector2 targetLoc = player.transform.position + new Vector3(0, 0.75f, 0);
        ProjectileFindVelocityAndAngle(targetLoc, arrowTime);
    }
    void Attack_End()
    {
        isAttacking = false;
    }
    void ProjectileFindVelocityAndAngle(Vector2 targetLoc, float time = 1)
    {
        Vector2 startLoc = arrowSpawnPoint.transform.position;
        float xDelta = Mathf.Abs(startLoc.x - targetLoc.x);
        float yDelta = startLoc.y - targetLoc.y;
        float gravity = Mathf.Abs(arrow.GetComponent<Rigidbody2D>().gravityScale * Physics.gravity.y);
        float oppVelocity = (yDelta - (gravity * time * time / 2)) / time;
        float adjVelocity = xDelta / time;
        float angle = Mathf.Abs(Mathf.Atan2(oppVelocity, adjVelocity)) * Mathf.Rad2Deg;
        float initialVelocity = Mathf.Sqrt(Mathf.Pow(oppVelocity, 2) + Mathf.Pow(adjVelocity, 2));

        GameObject projectile = Instantiate(arrow, arrowSpawnPoint.transform);
        projectile.transform.parent = null;

        Vector3 targetPos = new Vector3(targetLoc.x, projectile.transform.position.y);
        projectile.transform.LookAt(targetPos);
        projectile.transform.Rotate(new Vector3(-angle, projectile.transform.localRotation.y, projectile.transform.localRotation.z), Space.Self);

        projectile.GetComponent<Rigidbody2D>().AddForce(initialVelocity * projectile.transform.forward, ForceMode2D.Impulse);
        projectile.GetComponent<Arrow>().enabled = true;
        projectile.GetComponent<AttackManager>().attacks[projectile.GetComponent<AttackManager>().index].attackDamage = am.currentAttack.attackDamage;
    }
    void ProjectileFindVelocity(Vector2 targetLoc)
    {
        Vector2 startLoc = arrowSpawnPoint.transform.position;
        float xDelta = Mathf.Abs(startLoc.x - targetLoc.x);
        float gravity = Mathf.Abs(arrow.GetComponent<Rigidbody2D>().gravityScale * Physics.gravity.y);
        float yDelta = startLoc.y - targetLoc.y;
        float phaseAngle = Mathf.Atan2(xDelta, yDelta) * Mathf.Rad2Deg;
        float angle = 30 + Mathf.Abs(yDelta) * 10;
        if (angle > 85)
        {
            angle = 85;
        }

        Debug.Log(angle);

        float e1 = angle * 2 - phaseAngle;

        float top = gravity * xDelta * xDelta;
        float bottom = (Mathf.Sqrt((xDelta * xDelta) + (yDelta * yDelta)) * Mathf.Cos(e1 * Mathf.Deg2Rad)) + yDelta;
        float initialVelocity = Mathf.Sqrt(top / bottom);

        GameObject projectile = Instantiate(arrow, arrowSpawnPoint.transform);
        projectile.transform.parent = null;

        Vector3 targetPos = new Vector3(targetLoc.x, projectile.transform.position.y);
        projectile.transform.LookAt(targetPos);
        projectile.transform.Rotate(new Vector3(-angle, projectile.transform.localRotation.y, projectile.transform.localRotation.z), Space.Self);

        projectile.GetComponent<Rigidbody2D>().AddForce(initialVelocity * projectile.transform.forward, ForceMode2D.Impulse);
        projectile.GetComponent<Arrow>().enabled = true;
        projectile.GetComponent<AttackManager>().attacks[projectile.GetComponent<AttackManager>().index].attackDamage = am.currentAttack.attackDamage;
    }
    void ProjectileFindAngle()
    {
        Vector2 startLoc = arrowSpawnPoint.transform.position;
        float xDelta = Mathf.Abs(startLoc.x - player.transform.position.x);
        float gravity = Mathf.Abs(arrow.GetComponent<Rigidbody2D>().gravityScale * Physics.gravity.y);
        float yDelta = startLoc.y - player.transform.position.y;
        float phaseAngle = Mathf.Atan2(xDelta, yDelta) * Mathf.Rad2Deg;
        float initialVelocity = 1;

        float e1 = ((gravity * xDelta * xDelta) / (initialVelocity * initialVelocity)) - yDelta;
        e1 /= Mathf.Sqrt((yDelta * yDelta) + (xDelta * xDelta));

        if (e1 > 1)
            return;

        float e2 = Mathf.Acos(e1) * Mathf.Rad2Deg;
        e2 += phaseAngle;
        e2 /= 2;

        GameObject projectile = Instantiate(arrow, arrowSpawnPoint.transform);
        projectile.transform.parent = null;

        Vector3 targetPos = new Vector3(player.transform.position.x, projectile.transform.position.y);
        projectile.transform.LookAt(targetPos);
        projectile.transform.Rotate(new Vector3(-e2, projectile.transform.localRotation.y, projectile.transform.localRotation.z), Space.Self);

        projectile.GetComponent<Rigidbody2D>().AddForce(initialVelocity * projectile.transform.forward, ForceMode2D.Impulse);
        projectile.GetComponent<Arrow>().enabled = true;
        projectile.GetComponent<AttackManager>().attacks[projectile.GetComponent<AttackManager>().index].attackDamage = am.currentAttack.attackDamage;
    }
}
