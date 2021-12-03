using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IEntity
{
    [HideInInspector] public Animator       animator;
    [HideInInspector] public HealthBar      healthBar;
    [HideInInspector] public Rigidbody2D    body2d;
    [HideInInspector] public int            facingDirection = 1;
    [HideInInspector] public float          inputX = 0;
    [HideInInspector] public bool           attacking = false;
    [HideInInspector] public bool           isDead = false;
    [HideInInspector] public bool           wallSliding = false;
    [HideInInspector] public bool           rolling = false;

    public CanvasGroup              crossfade;
    public CanvasGroup              deathFade;
    public CanvasGroup              aspectRatio;
    public GameObject               whiteCrossFade;
    public AudioPlayer              audioPlayer;
    public GameObject               playerCanvas;
    public GameObject               groundSensor;
    public GameObject               slideDust;
    public GameObject               damageFlash;
    public GameObject               absorbFlash;
    public GameObject               healFlash;
    public GameObject               soulPrefab;
    public GameObject               soulCollectionPrefab;
    public GameObject               absorbSoundPrefab;
    public GameObject               jumpParticlePrefab;
    public PhysicsMaterial2D        noFriction;
    public int                      corruption = 0;
    public int                      currentCorruption = 0;
    public float                    baseAcceration = 1.0f;
    public float                    baseMaxSpeed = 3.0f;
    public float                    baseDeceleration = 0.91f;
    public float                    baseRollForce = 4.0f;
    public float                    acceration;
    public float                    maxSpeed;
    public float                    deceleration;
    public float                    rollForce;
    public float                    jumpForce = 8.5f;
    public float                    fallMultiplier = 3.0f;
    public float                    lowJumpFallMultiplier = 0.1f;
    public bool                     actionAllowed = true;
    public bool                     grounded = false;
    public bool                     hasAmulet = false;
    public bool                     damaged = false;

    public Collider2D               hurtbox;
    public GameObject               rollingHurtbox;
    public GameObject               attackHitbox;
    public float                    attackLungingForce = 30;
    public float                    jumpAttackLungingMultiplier = .33f;

    private CollisionSensor         m_wallSensorR1;
    private CollisionSensor         m_wallSensorR2;
    private CollisionSensor         m_wallSensorL1;
    private CollisionSensor         m_wallSensorL2;
    private TMP_Text                corruptionCount;
    private int                     m_currentAttack = 0;
    private int                     testies = 0;
    private float                   m_delayToIdle = 0.0f;
    private float                   m_rollDuration = 8.0f / 14.0f;
    private float                   m_rollCurrentTime = 0;
    private float                   m_invulDuration = 28.0f / 60.0f;
    private float                   m_invulCurrentTime = 0;
    private float                   m_invulStartUpDuration = 2.0f / 60.0f;
    private float                   m_invulStartUpTime = 0;
    private float                   m_timeSinceAttack = 0.0f;
    private float                   differenceAlpha = 0;
    private float                   absorbWaitTime = 0;
    private float                   absorbingTime = 0;
    private float                   corruptionCountTime = 0;
    private bool                    deathAnimationDone = false;
    private bool                    playFadeOutOne = false;
    [HideInInspector]
    public bool                    playFadeOutTwo = false;
    private bool                    prevGround = false;
    private bool                    invul = false;
    private bool                    doneInvul = false;
    private bool                    guarding = false;
    private bool                    absorbing = false;
    private int                     jumpCount;
    private int                     jumpLimit = 1;

    public bool Grounded { get => grounded; set => grounded = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public int CorruptionValue { get => testies; set => testies = value; }

    private void Awake()
    {
    }
    // Use this for initialization
    void Start()
    {

        Application.targetFrameRate = 60;
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<HealthBar>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<CollisionSensor>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<CollisionSensor>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<CollisionSensor>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<CollisionSensor>();
        corruptionCount = GameObject.Find("CorruptionCount").GetComponent<TMP_Text>();

        attackHitbox.transform.localPosition = new Vector2(0.65f, 0.85f);
        DeactivateHitboxes();


        GameMaster gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

        GetComponent<InventorySystem>().nextPointThreshold = gm.playerData.nextPointThreshold;
        GetComponent<InventorySystem>().originalPointsAvailable = gm.playerData.pointsAvailable;
        corruption = gm.playerData.corruption;
        GetComponent<PlayerStat>().strength.BaseValue = gm.playerData.str;
        GetComponent<PlayerStat>().vitality.BaseValue = gm.playerData.vit;
        GetComponent<PlayerStat>().agility.BaseValue = gm.playerData.agi;

        healthBar.SetMaxHealth(healthBar.baseHealth + 15 * (int)GetComponent<PlayerStat>().vitality.Value);
        if (gm.playerData.currenthealth <= 0)
            gm.playerData.currenthealth = 1;
        StartCoroutine(StartHealth(gm.playerData.currenthealth));
    }

    IEnumerator StartHealth(float currenthealth)
    {
        yield return new WaitForSeconds(0.1f);
        float difference = Mathf.Abs(currenthealth - healthBar.currentHealth);
        if (currenthealth > healthBar.currentHealth)
            healthBar.Healing((int)difference);
        else
            healthBar.TakeDamage((int)difference);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<AttackManager>().attacks[GetComponent<AttackManager>().index].attackDamage = (int)GetComponent<PlayerStat>().strength.Value;
        acceration = baseAcceration + 0.075f * (int)GetComponent<PlayerStat>().agility.Value;
        maxSpeed = baseMaxSpeed + 0.05f * (int)GetComponent<PlayerStat>().agility.Value;
        deceleration = baseDeceleration - 0.005f * (int)GetComponent<PlayerStat>().agility.Value;
        rollForce = baseRollForce + 0.15f * (int)GetComponent<PlayerStat>().agility.Value;

        if (corruption >= 100)
        {
            jumpLimit = 2;
        }

        corruptionCount.text = currentCorruption.ToString();

        if (currentCorruption != corruption)
        {
            corruptionCountTime += Time.deltaTime;
            float distance = (float)(1 / (40 * Mathf.Abs(currentCorruption - corruption) * Time.deltaTime));
            if (distance > 2f / 60f)
                distance = 2f / 60f;
            if (corruptionCountTime > distance)
            {
                if (currentCorruption < corruption)
                    currentCorruption += (int)(corruptionCountTime / distance);
                else if (currentCorruption > corruption)
                    currentCorruption -= (int)(corruptionCountTime / distance);
                corruptionCountTime = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
            playerCanvas.GetComponent<Animator>().SetTrigger("FirstTime");
                
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;
        absorbWaitTime += Time.deltaTime;

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
            if (!invul && !doneInvul)
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
            invul = false;
            doneInvul = false;
            m_rollCurrentTime = 0;
            m_invulStartUpTime = 0;
            m_invulCurrentTime = 0;
        }

        // Dables invulnerability after invul timer is reached
        if (m_invulCurrentTime > m_invulDuration)
        {
            rollingHurtbox.SetActive(false);
            hurtbox.enabled = true;
            groundSensor.SetActive(true);
            invul = false;
            doneInvul = true;
            m_invulCurrentTime = 0;
        }

        // If the player is landing or jumping, then the hitboxes will dable
        if (prevGround != grounded)
        {
            if (!prevGround)
            {
                if (!rolling)
                    audioPlayer.PlaySound("Land");
                jumpCount = 0;
            }
            else
            {
                jumpCount = 1;
            }
            DeactivateHitboxes();
            prevGround = grounded;
        }

        // If the player is wall sliding, then the hitboxes will dable
        if (wallSliding && !isDead)
        {
            DeactivateHitboxes();
            attacking = false;
        }

        // Initalize the grounded state of the player animation
        animator.SetBool("Grounded", grounded);

        // If action is allowed
        if (actionAllowed)
        {
            Vector2 directionalForce = new Vector2(0, 0);

            // This will handle the player movement
            inputX = Input.GetAxis("Horizontal");
            if (!attacking)
            {
                if (Mathf.Abs(inputX) < 0.01f && !rolling)
                    body2d.velocity = new Vector2(body2d.velocity.x * deceleration, body2d.velocity.y);
                else if (rolling)
                    body2d.velocity = new Vector2(body2d.velocity.x * 0.97f, body2d.velocity.y);
                else
                {
                    if ((body2d.velocity.x < 0 && inputX > 0) || (body2d.velocity.x > 0 && inputX < 0))
                        directionalForce += new Vector2(inputX * acceration * 5 * Time.deltaTime * 150, 0);
                    else
                        directionalForce += new Vector2(inputX * acceration * Time.deltaTime * 150, 0);
                }

                if (body2d.velocity.x > maxSpeed && !rolling)
                    body2d.velocity = new Vector2(maxSpeed, body2d.velocity.y);
                else if (body2d.velocity.x < -maxSpeed && !rolling)
                    body2d.velocity = new Vector2(-maxSpeed, body2d.velocity.y);
            }
            else if (attacking)
            {
                if (grounded)
                    body2d.velocity = new Vector2(body2d.velocity.x * 0.9f, body2d.velocity.y);
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
                audioPlayer.PlaySound("Rolling");
                body2d.velocity = new Vector2(facingDirection * rollForce, body2d.velocity.y);
            }

            // Jump
            else if (Input.GetKeyDown("space") && jumpCount < jumpLimit && !rolling && !attacking)
            {
                jumpCount++;
                audioPlayer.PlaySound("Jump");
                DeactivateHitboxes();
                grounded = false;
                animator.SetTrigger("Jump");
                animator.SetBool("Grounded", grounded);
                if (jumpCount == 2)
                {
                    Instantiate(jumpParticlePrefab, transform.position + Vector3.up * 0.6f, jumpParticlePrefab.transform.rotation);
                    absorbFlash.SetActive(true);
                    body2d.velocity = new Vector2(body2d.velocity.x, jumpForce * 1.2f);
                }
                else
                {
                    body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
                }
            }

            // Attack
            else if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("k")) && m_timeSinceAttack > 0.35f && !rolling && !wallSliding && !damaged && !guarding && !attacking && !GetComponent<InventorySystem>().isOpen)
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
            else if (Input.GetKeyDown("e") && !attacking && !rolling && !wallSliding && !guarding && !GetComponent<InventorySystem>().isOpen)
            {
                InteractionSystem intSys = GetComponent<InteractionSystem>();
                if (intSys.DetectObject())
                {
                    intSys.detectedObject.GetComponent<Item>().Interact();
                    if (intSys.detectedObject.GetComponent<Item>().interactType == Item.InteractionType.PickUp)
                        animator.SetTrigger("Pickup");
                }
            }

            // Abosrb souls
            var flag = GameObject.Find("AbsorbCircle").GetComponent<Flag>();
            if (Input.GetKeyDown(KeyCode.V)) 
            {
                if (hasAmulet && absorbWaitTime > 1)
                {
                    audioPlayer.PlaySound("AbsorbCollection");
                    absorbWaitTime = 0;
                    Instantiate(soulCollectionPrefab, transform.position + new Vector3(0, 0.75f, 0), transform.rotation, transform);
                    absorbing = true;
                }
            }

            if (absorbing)
            {
                absorbingTime += Time.deltaTime;
                if (GameObject.Find("AbsorbCircle").GetComponent<Flag>().flagged)
                {
                    foreach (Collider2D col in flag.colliders)
                    {
                        if (col.GetComponent<Enemy>().IsDead && !col.GetComponent<Enemy>().IsAbsorbed)
                        {
                            col.GetComponent<Enemy>().IsAbsorbed = true;
                            GameObject soul = Instantiate(soulPrefab, col.transform);
                            soul.GetComponent<FindEnemy>().enemy = col.GetComponent<Enemy>();
                        }
                    }
                }
                if (absorbingTime > 1)
                {
                    absorbing = false;
                    absorbingTime = 0;
                }
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
        }
        if (healthBar.currentHealth <= 0 && !isDead)
        {
            GetComponent<InteractionSystem>().enabled = false;
            actionAllowed = false;
            isDead = true;
            StartCoroutine(Dead());
        }

        if (playFadeOutOne && deathFade.alpha < 1)
        {
            differenceAlpha = Mathf.Abs(deathFade.alpha - 1);
            if (differenceAlpha < 0.1f)
                differenceAlpha = 0.1f;
            deathFade.alpha += Time.deltaTime * differenceAlpha * 1.5f;
        }

        if (absorbFlash.activeSelf == true)
        {
            absorbFlash.GetComponent<Light2DFade>().Fade(0.5f);
        }
    }

    IEnumerator Dead()
    {
        Debug.Log("dead");
        actionAllowed = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1.25f);
        audioPlayer.PlaySound("Death");
        playFadeOutOne = true;
        yield return new WaitWhile(() => deathFade.alpha < 1);
        yield return new WaitForSeconds(2f);
        crossfade.gameObject.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitWhile(() => crossfade.alpha < 1);
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    void AE_Death_End()
    {
        deathAnimationDone = true;
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
        if (collision.tag == "EnemyHitbox" &&  !damaged && !invul && !isDead)
            DamageCalculation(collision);
        if (collision.tag == "Souls")
        {
            Instantiate(absorbSoundPrefab, transform);
            collision.GetComponent<ParticleFade>().Fade();
            collision.GetComponent<Light2DFade>().Fade(1);
            collision.GetComponent<Collider2D>().enabled = false;
            corruption += collision.GetComponent<FindEnemy>().enemy.CorruptionValue;
            healthBar.Healing(collision.GetComponent<FindEnemy>().enemy.CorruptionValue / 10);
            absorbFlash.SetActive(true);
        }
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
        Transform current = collision.transform;
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
