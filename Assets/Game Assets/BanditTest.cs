using UnityEngine;
using System.Collections;

public class BanditTest : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      agroRange;
    [SerializeField] float      attackRange;

    [SerializeField] GameObject m_attackHitbox;
    [SerializeField] Transform  player;

    private Damages             m_damages;
    public bool                 damaged = false;
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    public HealthBar            healthBar;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;
    private bool                m_attacking = false;




    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_damages = GetComponent<Damages>();
        healthBar = GetComponent<HealthBar>();
        m_damages.activeDamage = m_damages.list[0];
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // -- Handle Animations --
        /*//Death
        if (Input.GetKeyDown("e")) {
            if(!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }*/

        //Attack
        if(Input.GetKeyDown("l")) {
            m_animator.SetTrigger("Attack");
        }

        //distance to hero knight
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer < agroRange && distToPlayer>= attackRange)
        {
            prepAttack();
            m_animator.SetInteger("AnimState", 1);
        }
        else if (distToPlayer < attackRange)
        {
            m_animator.SetTrigger("Attack");
        }
        else
        {
            m_animator.SetInteger("AnimState", 0);
        }
    }


    void prepAttack()
    {
        if(transform.position.x < player.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if(transform.position.x > player.position.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox" && !damaged)
        {
            damaged = true;
            StartCoroutine("invul");
            if (!m_attacking)
                m_animator.SetTrigger("Hurt");
            healthBar.TakeDamage(collision.transform.parent.GetComponent<Damages>().activeDamage);
            if (collision.transform.parent.position.x < transform.position.x)
                m_body2d.AddForce(new Vector2(30, 10));
            else
                m_body2d.AddForce(new Vector2(-30, 10));
        }
    }

    void Attack_Start()
    {
        m_attacking = true;
    }
    void Attack_Active()
    {
        m_attackHitbox.SetActive(true);
    }

    void Attack_End()
    {
        m_attackHitbox.SetActive(false);
        m_attacking = false;
    }

    IEnumerator invul()
    {
        yield return new WaitForSeconds(0.1f);
        damaged = false;
    }
}
