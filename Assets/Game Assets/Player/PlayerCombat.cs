using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour {

    [SerializeField] GameObject                 m_attack1Hitbox;
    [SerializeField] GameObject                 m_attack2Hitbox;
    [SerializeField] GameObject                 m_attack3Hitbox;

    [HideInInspector] public Animator           animator;
    [HideInInspector] public Rigidbody2D        body2d;
    [HideInInspector] public HealthBar          healthBar;

    private int                                 m_currentAttack = 0;
    private float                               m_timeSinceAttack = 0.0f;
    private bool                                m_damaged = false;
    private bool                                m_prevGround = false;
    private bool                                m_guarding = false;
    private PlayerMovement                      m_movement;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        healthBar = GetComponent<HealthBar>();
        m_movement = GetComponent<PlayerMovement>();

        m_attack1Hitbox = GameObject.Instantiate(m_attack1Hitbox);
        m_attack2Hitbox = GameObject.Instantiate(m_attack2Hitbox);
        m_attack3Hitbox = GameObject.Instantiate(m_attack3Hitbox);

        m_attack1Hitbox.transform.parent = transform;
        m_attack2Hitbox.transform.parent = transform;
        m_attack3Hitbox.transform.parent = transform;

        //m_attack1Hitbox.transform.localPosition = new Vector2(0.6f, 0.95f);
        //m_attack2Hitbox.transform.localPosition = new Vector2(0.225f, 0.85f);
        m_attack1Hitbox.transform.localPosition = new Vector2(0.65f, 0.85f);
        m_attack2Hitbox.transform.localPosition = new Vector2(0.65f, 0.85f);
        m_attack3Hitbox.transform.localPosition = new Vector2(0.65f, 0.85f);

        DeactivateHitboxes();
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        if (m_movement.rolling)
        {
            DeactivateHitboxes();
        }

        if (Input.GetKeyDown("q") && !m_movement.rolling && !m_damaged)
        {
            DeactivateHitboxes();
            animator.SetTrigger("Hurt");
        }

        if (m_prevGround != m_movement.grounded)
        {
            DeactivateHitboxes();
            m_prevGround = m_movement.grounded;
        }

        if (m_movement.isWallSliding)
        {
            DeactivateHitboxes();
        }

        //Attack
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("k")) && m_timeSinceAttack > 0.25f && !m_movement.rolling && !m_movement.isWallSliding && !m_damaged && !m_guarding)
        {
            DeactivateHitboxes();
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            animator.SetTrigger("Attack" + m_currentAttack);

            // Makes sure the orientation of the hitbox is properly placed for each of the 3 attacks
            if (m_currentAttack == 1)
            {
                m_attack1Hitbox.SetActive(true);
                if (m_movement.facingDirection == 1 && m_attack1Hitbox.transform.localPosition.x < 0)
                {
                    m_attack1Hitbox.transform.localPosition = new Vector3(-m_attack1Hitbox.transform.localPosition.x, m_attack1Hitbox.transform.localPosition.y);
                }
                else if (m_movement.facingDirection == -1 && m_attack1Hitbox.transform.localPosition.x > 0)
                {
                    m_attack1Hitbox.transform.localPosition = new Vector3(-m_attack1Hitbox.transform.localPosition.x, m_attack1Hitbox.transform.localPosition.y);
                }
            }
            else if (m_currentAttack == 2)
            {
                m_attack2Hitbox.SetActive(true);
                if (m_movement.facingDirection == 1 && m_attack2Hitbox.transform.localPosition.x < 0)
                {
                    m_attack2Hitbox.transform.localPosition = new Vector3(-m_attack2Hitbox.transform.localPosition.x, m_attack2Hitbox.transform.localPosition.y);
                }
                else if (m_movement.facingDirection == -1 && m_attack2Hitbox.transform.localPosition.x > 0)
                {
                    m_attack2Hitbox.transform.localPosition = new Vector3(-m_attack2Hitbox.transform.localPosition.x, m_attack2Hitbox.transform.localPosition.y);
                }
            }
            else
            {
                m_attack3Hitbox.SetActive(true);
                if (m_movement.facingDirection == 1 && m_attack3Hitbox.transform.localPosition.x < 0)
                {
                    m_attack3Hitbox.transform.localPosition = new Vector3(-m_attack3Hitbox.transform.localPosition.x, m_attack3Hitbox.transform.localPosition.y);
                }
                else if (m_movement.facingDirection == -1 && m_attack3Hitbox.transform.localPosition.x > 0)
                {
                    m_attack3Hitbox.transform.localPosition = new Vector3(-m_attack3Hitbox.transform.localPosition.x, m_attack3Hitbox.transform.localPosition.y);
                }
            }

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_movement.rolling && !m_damaged)
        {
            animator.SetBool("Guarding", true);
            m_guarding = true;
        }

        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("Guarding", false);
            m_guarding = false;
        }
        //m_animator.SetTrigger("Block");
    }

    public void DeactivateHitboxes()
    {
        m_attack1Hitbox.SetActive(false);
        m_attack2Hitbox.SetActive(false);
        m_attack3Hitbox.SetActive(false);
    }

    void AE_Attack1_End()
    {
        m_attack1Hitbox.SetActive(false);
    }
    void AE_Attack2_End()
    {
        m_attack2Hitbox.SetActive(false);
    }
    void AE_Attack3_End()
    {
        m_attack3Hitbox.SetActive(false);
    }
    void AE_Damaged_End()
    {
        m_movement.actionAllowed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyHitbox" && !m_damaged && !m_movement.isInvul)
        {
            DeactivateHitboxes();
            body2d.velocity = Vector2.zero;
            m_movement.actionAllowed = false;
            m_movement.rolling = false;
            m_movement.isWallSliding = false;
            m_damaged = true;
            
                animator.SetTrigger("Hurt");
                healthBar.TakeDamage(collision.transform.parent.GetComponent<Damages>().activeDamage);

            if (collision.transform.parent.position.x < transform.position.x)
                body2d.AddForce(new Vector2(75, 30));
            else
                body2d.AddForce(new Vector2(-75, 30));
            StartCoroutine("invul");
        }
    }
    IEnumerator invul()
    {
        yield return new WaitForSeconds(0.1f);
        m_damaged = false;
    }
}
