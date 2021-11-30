using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IEntity
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public HealthBar healthBar;
    [HideInInspector] public Rigidbody2D body2d;

    public GameObject               groundSensor;
    public GameObject               slideDust;
    public GameObject               damageFlash;
    public int                      facingDirection = 1;
    public float                    inputX = 0;
    public float                    acceration = 1.0f;
    public float                    maxSpeed = 4.0f;
    public float                    jumpForce = 8.5f;
    public float                    fallMultiplier = 3.0f;
    public float                    lowJumpFallMultiplier = 0.1f;
    public float                    rollForce = 6.0f;
    public bool                     actionAllowed = true;
    public bool                     prevGround = false;
    public bool                     grounded = false;
    public bool                     rolling = false;
    public bool                     wallSliding = false;
    
    public Collider2D               hurtbox;
    public GameObject               rollingHurtbox;
    public GameObject               attackHitbox;
    public float                    attackLungingForce = 200;
    public float                    jumpAttackLungingMultiplier = .33f;
    public bool                     invul = false;
    public bool                     attackConnected = false;
    public bool                     attacking = false;
    public bool                     damaged = false;
    public bool                     guarding = false;

    private CollisionSensor         m_wallSensorR1;
    private CollisionSensor         m_wallSensorR2;
    private CollisionSensor         m_wallSensorL1;
    private CollisionSensor         m_wallSensorL2;
    private int                     m_currentAttack = 0;
    private float                   m_delayToIdle = 0.0f;
    private float                   m_rollDuration = 8.0f / 14.0f;
    private float                   m_rollCurrentTime = 0;
    private float                   m_invulDuration = 30.0f / 60.0f;
    private float                   m_invulCurrentTime = 0;
    private float                   m_invulStartUpDuration = 8.0f / 60.0f;
    private float                   m_invulStartUpTime = 0;
    private float                   m_timeSinceAttack = 0.0f;

    public bool Grounded { get => grounded; set => grounded = value; }

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<HealthBar>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<CollisionSensor>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<CollisionSensor>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<CollisionSensor>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<CollisionSensor>();

        attackHitbox.transform.localPosition = new Vector2(0.65f, 0.85f);
        DeactivateHitboxes();
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // If the jump button is held, then the player will slower so they will re higher
        if (body2d.velocity.y > 0 && Input.GetKey("space"))
        {
            body2d.velocity += Vector2.up * Physics.gravity.y * lowJumpFallMultiplier * Time.deltaTime;
        }
        // If the jump button is released, then the player will fall at a faster speed than usual
        else if (body2d.velocity.y > 0 && !Input.GetKey("space"))
        {
            body2d.velocity += Vector2.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }

        // Increase timer that checks roll duration
        if (rolling)
        {
            m_rollCurrentTime += Time.deltaTime;
            if (!invul)
            {
                m_invulStartUpTime += Time.deltaTime;
            }
            DeactivateHitboxes();
        }

        // Increase timer that checks invulnerability duration
        if (m_invulStartUpTime > m_invulStartUpDuration)
        {
            invul = true;
            m_invulStartUpTime = 0;
        }

        // If the player is flagged as Invulnerable, then we start increasing the invul timer
        if (invul)
        {
            hurtbox.enabled = false;
            groundSensor.SetActive(false);
            rollingHurtbox.SetActive(true);
            m_invulCurrentTime += Time.deltaTime;
        }

        // Enables rolling after rolling timer is reached
        if (m_rollCurrentTime > m_rollDuration)
        {
            rolling = false;
            m_rollCurrentTime = 0;
        }

        // Dables invulnerability after invul timer is reached
        if (m_invulCurrentTime > m_invulDuration)
        {
            hurtbox.enabled = true;
            groundSensor.SetActive(true);
            rollingHurtbox.SetActive(false);
            invul = false;
            m_invulCurrentTime = 0;
        }

        // If the player is landing or jumping, then the hitboxes will dable
        if (prevGround != grounded)
        {
            DeactivateHitboxes();
            prevGround = grounded;
        }

        // If the player is wall sliding, then the hitboxes will dable
        if (wallSliding)
            DeactivateHitboxes();

        // If action is allowed
        if (actionAllowed)
        {
            Vector2 directionalForce = new Vector2(0, 0);

            // Initalize the grounded state of the player animation
            animator.SetBool("Grounded", grounded);

            // This will handle the player movement
            inputX = Input.GetAxis("Horizontal");
            if (!rolling && !attacking)
            {
                if (Mathf.Abs(inputX) < 0.01f)
                    body2d.velocity = new Vector2(body2d.velocity.x * 0.94f, body2d.velocity.y);
                else
                {
                    if ((body2d.velocity.x < 0 && inputX > 0) || (body2d.velocity.x > 0 && inputX < 0))
                        directionalForce += new Vector2(inputX * acceration * 5, 0);
                    else
                        directionalForce += new Vector2(inputX * acceration, 0);
                }

                if (body2d.velocity.x > maxSpeed)
                    body2d.velocity = new Vector2(maxSpeed, body2d.velocity.y);
                else if (body2d.velocity.x < -maxSpeed)
                    body2d.velocity = new Vector2(-maxSpeed, body2d.velocity.y);
            }
            else if (attacking)
            {
                if (grounded)
                    body2d.velocity = new Vector2(body2d.velocity.x * 0.94f, body2d.velocity.y);
                else
                    body2d.velocity = new Vector2(body2d.velocity.x * 0.99f, body2d.velocity.y);
            }

            // Swap direction of sprite depending on walk direction
            if (inputX > 0 && !rolling)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                attackHitbox.GetComponent<StickToObject>().positionOffset = new Vector2(0.65f, 0.85f);
                facingDirection = 1;
            }
            else if (inputX < 0 && !rolling)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                attackHitbox.GetComponent<StickToObject>().positionOffset = new Vector2(-0.65f, 0.85f);
                facingDirection = -1;
            }

            // Rising and falling animation handling
            // If Y-velocity  greater than 0, then the rising animation will play, if it is less than then the falling animation will play instead.
            animator.SetFloat("AirSpeedY", body2d.velocity.y);

            // Wall Slide
            wallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
            animator.SetBool("WallSlide", wallSliding);

            // Roll
            if (Input.GetKeyDown("c") && grounded && !rolling && !attacking)
            {
                rolling = true;
                animator.SetTrigger("Roll");
                body2d.velocity = new Vector2(facingDirection * rollForce, body2d.velocity.y);
            }

            // Jump
            else if (Input.GetKeyDown("space") && grounded && !rolling && !attacking)
            {
                DeactivateHitboxes();
                grounded = false;
                animator.SetTrigger("Jump");
                animator.SetBool("Grounded", grounded);
                body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            }

            // Attack
            else if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("k")) && m_timeSinceAttack > 0.35f && !rolling && !wallSliding && !damaged && !guarding && !attacking)
            {
                // Flag that the player is attacking
                attacking = true;
                DeactivateHitboxes();
                m_currentAttack++;

                // This will replicate a lunge for the attack
                float currentLungingForce = attackLungingForce;
                if (!grounded)
                    currentLungingForce = attackLungingForce * jumpAttackLungingMultiplier;
                if (facingDirection == 1)
                    directionalForce +=(new Vector2(currentLungingForce, 0));
                else
                    directionalForce +=(new Vector2(-currentLungingForce, 0));

                // Loop back to one after third attack
                if (m_currentAttack > 3)
                    m_currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if (m_timeSinceAttack > 1.0f)
                    m_currentAttack = 1;

                // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                animator.SetTrigger("Attack" + m_currentAttack);

                attackHitbox.SetActive(true);
                attackHitbox.GetComponent<StickToObject>().Move();
                if (facingDirection == 1 && attackHitbox.transform.localPosition.x < 0)
                    attackHitbox.transform.localPosition = new Vector3(-attackHitbox.transform.localPosition.x, attackHitbox.transform.localPosition.y);
                else if (facingDirection == -1 && attackHitbox.transform.localPosition.x > 0)
                    attackHitbox.transform.localPosition = new Vector3(-attackHitbox.transform.localPosition.x, attackHitbox.transform.localPosition.y);

                // Reset timer
                m_timeSinceAttack = 0.0f;
            }

            // Run
            if (Mathf.Abs(body2d.velocity.x) > 0.1f && !rolling)
            {
                m_delayToIdle = 0.05f;
                animator.SetInteger("AnimState", 1);
            }

            // Idle
            else
            {
                // Prevents flickering transitions to idle
                m_delayToIdle -= Time.deltaTime;
                if (m_delayToIdle < 0)
                    animator.SetInteger("AnimState", 0);
            }

            // Block
            if (Input.GetMouseButtonDown(1) && !rolling && !damaged)
            {
                guarding = true;
                animator.SetBool("Guarding", guarding);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                guarding = false;
                animator.SetBool("Guarding", guarding);
            }
            //m_animator.SetTrigger("Block");

            body2d.AddForce(directionalForce);
            if (healthBar.currentHealth < 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void DeactivateHitboxes()
    {
        attackHitbox.SetActive(false);
    }

    void AE_Attack1_End()
    {
        attackHitbox.SetActive(false);
        StartCoroutine(AttackRecovery(0.2f));
    }
    void AE_Attack2_End()
    {
        attackHitbox.SetActive(false);
        StartCoroutine(AttackRecovery(0.2f));
    }
    void AE_Attack3_End()
    {
        attackHitbox.SetActive(false);
        StartCoroutine(AttackRecovery(0.2f));
    }
    void AE_Damaged_End()
    {
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyHitbox" && !damaged && !invul)
            DamageCalculation(collision);
    }

    void DamageCalculation(Collider2D collision)
    {
        DeactivateHitboxes();
        body2d.velocity = Vector2.zero;
        actionAllowed = false;
        rolling = false;
        wallSliding = false;
        attacking = false;
        damaged = true;

        animator.SetTrigger("Hurt");
        
        // we will search for an Attack Manager from the hitboxs that hit us
        int damage = -1;
        float stun = 0.1f;
        Vector2 knockback = Vector2.zero;
        Transform current = collision.transform.parent;
        while (current != null && damage < 0)
        {
            if (current.GetComponent<AttackManager>())
            {
                damage = current.GetComponent<AttackManager>().currentAttack.attackDamage;
                stun = current.GetComponent<AttackManager>().currentAttack.stunTime;
                knockback = current.GetComponent<AttackManager>().currentAttack.knockback;
            }
            else
            {
                current = current.parent;
            }
        }
        if (damage < 0)
            damage = 0;
        healthBar.TakeDamage(damage);
        if (collision.transform.position.x > transform.position.x)
            knockback = new Vector2(-knockback.x, knockback.y);
        body2d.AddForce(knockback);
        StartCoroutine(InvulActivate(stun));
    }

    IEnumerator InvulActivate(float time)
    {
        damageFlash.SetActive(true);
        yield return new WaitForSeconds(time);
        damageFlash.SetActive(false);
        animator.SetTrigger("HurtDone");
        actionAllowed = true;
        damaged = false;
    }
    IEnumerator AttackRecovery(float time)
    {
        yield return new WaitForSeconds(time);
        attacking = false;
    }

    /*
    // Retrict player movement if player is examining an object or if the inventory system is open
    bool CanMove()
    {
        actionAllowed = true;

        if (FindObjectOfType<InteractionSystem>().isExamining)
        {
            actionAllowed = false;
        }
        if(FindObjectOfType<InventorySystem>().isOpen)
        {
            actionAllowed = false;
        }

        return actionAllowed;
    }
    */
}
