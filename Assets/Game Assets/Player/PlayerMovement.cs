using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float                            speed = 4.0f;
    public float                            jumpForce = 7.5f;
    public float                            fallMultiplier = 2.0f;
    public float                            lowJumpFallMultiplier = 2.0f;
    public float                            rollForce = 6.0f;
    public GameObject                       hurtbox;
    public GameObject                       rollingHurtbox;
    public GameObject                       groundSensor;
    public bool                             jumpButtonPressed = false;

    [SerializeField] GameObject             m_slideDust;

    [HideInInspector] public bool           isWallSliding = false;
    public bool                             grounded = false;
    public bool                             rolling = false;
    public bool                             actionAllowed = true;
    [HideInInspector] public int            facingDirection = 1;
    [HideInInspector] public float          inputX = 0;
    public bool                             isInvul = false;

    private Animator                        m_animator;
    public Rigidbody2D                      m_body2d;
    private CollisionSensor                 m_wallSensorR1;
    private CollisionSensor                 m_wallSensorR2;
    private CollisionSensor                 m_wallSensorL1;
    private CollisionSensor                 m_wallSensorL2;
    private float                           m_delayToIdle = 0.0f;
    private float                           m_rollDuration = 8.0f / 14.0f;
    private float                           m_rollCurrentTime = 0;
    private float                           m_invulDuration = 30.0f / 60.0f;
    private float                           m_invulCurrentTime = 0;
    private float                           m_invulStartUpDuration = 8.0f / 60.0f;
    private float                           m_invulStartUpTime = 0;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<CollisionSensor>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<CollisionSensor>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<CollisionSensor>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<CollisionSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove() == false) {
            return;
        }

        if (Input.GetKeyDown("space"))
            jumpButtonPressed = true;
        if (Input.GetKeyUp("space"))
            jumpButtonPressed = false;
        // else, if the velocity is more than 0, as in we are rising into the air, and we are holding the jump button, then
        // the player object is less effected by gravity making it seem like we are floating
        else if (m_body2d.velocity.y > 0 && jumpButtonPressed)
        {
            m_body2d.velocity += Vector2.up * Physics.gravity.y * lowJumpFallMultiplier * Time.deltaTime;
        }
        // this makes it so we are short jumping when we quickly press the jump button
        else if (m_body2d.velocity.y > 0 && !jumpButtonPressed)
        {
            m_body2d.velocity += Vector2.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }

        // Increase timer that checks roll duration
        if (rolling)
        {
            m_rollCurrentTime += Time.deltaTime;
            if (!isInvul)
            {
                m_invulStartUpTime += Time.deltaTime;
            }
        }

        if (m_invulStartUpTime > m_invulStartUpDuration)
        {
            isInvul = true;
            m_invulStartUpTime = 0;
        }

        if (isInvul)
        {
            hurtbox.SetActive(false);
            groundSensor.SetActive(false);
            rollingHurtbox.SetActive(true);
            m_invulCurrentTime += Time.deltaTime;
        }

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
        {
            rolling = false;
            m_rollCurrentTime = 0;
        }

        if (m_invulCurrentTime > m_invulDuration)
        {
            hurtbox.SetActive(true);
            groundSensor.SetActive(true);
            rollingHurtbox.SetActive(false);
            isInvul = false;
            m_invulCurrentTime = 0;
        }

        // if action is allowed
        if (actionAllowed)
        {
            m_animator.SetBool("Grounded", grounded);

            // -- Handle input and movement --
            inputX = Input.GetAxis("Horizontal");

            // Swap direction of sprite depending on walk direction
            if (inputX > 0 && !rolling)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                facingDirection = 1;
            }

            else if (inputX < 0 && !rolling)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                facingDirection = -1;
            }

            // Move
            if (!rolling)
                m_body2d.velocity = new Vector2(inputX * speed, m_body2d.velocity.y);
            //else
            //    m_body2d.velocity = new Vector2(inputX * speed * 0.8f, m_body2d.velocity.y);
            
            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

            // -- Handle Animations --
            //Wall Slide
            isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
            m_animator.SetBool("WallSlide", isWallSliding);

            // Roll
            if (Input.GetKeyDown("v") && grounded && !rolling)
            {
                rolling = true;
                m_animator.SetTrigger("Roll");
                m_body2d.velocity = new Vector2(facingDirection * rollForce, m_body2d.velocity.y);
            }

            //Jump
            if (Input.GetKeyDown("space") && grounded && !rolling)
            {
                grounded = false;
                m_animator.SetTrigger("Jump");
                m_animator.SetBool("Grounded", grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, jumpForce);
            }

            //Run
            if (Mathf.Abs(inputX) > Mathf.Epsilon && !rolling)
            {
                // Reset timer
                m_delayToIdle = 0.05f;
                m_animator.SetInteger("AnimState", 1);
            }

            //Idle
            else
            {
                // Prevents flickering transitions to idle
                m_delayToIdle -= Time.deltaTime;
                if (m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
            }
        }
    }

    // Can the player move
    bool canMove()
    {
        bool can = true;
        if(FindObjectOfType<InteractionSystem>().isExamining)
        {
            can = false;
        }
        return can;
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

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }
}
