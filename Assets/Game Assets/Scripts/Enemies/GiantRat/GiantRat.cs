using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantRat : MonoBehaviour
{
    BehaviourTreeController btc;
    HealthBar healthbar;
    GameObject player;
    AttackManager am;
    Animator animator;
    Rigidbody2D body2d;
    public float maxSpeed = 2;

    public List<GameObject> hurtboxesUp = new List<GameObject>();
    public List<GameObject> hurtboxesDown = new List<GameObject>();
    public List<GameObject> hitboxesAttack0 = new List<GameObject>();
    public List<GameObject> hitboxesAttack1 = new List<GameObject>();
    public List<GameObject> hitboxesIntoDown = new List<GameObject>();
    public List<GameObject> hitboxesRunning = new List<GameObject>();

    public Flag closeFlag;
    public Flag farFlag;

    void Start()
    {
        healthbar = GetComponent<HealthBar>();
        player = GameObject.FindGameObjectWithTag("Player");
        am = GetComponent<AttackManager>();
        animator = GetComponent<Animator>();
        body2d = GetComponent < Rigidbody2D>();
        

        btc = GetComponent<BehaviourTreeController>();
        btc.tree.blackboard.delegates.GetValue("CloseRangeAttacks") = CloseRangeAttacks;
        btc.tree.blackboard.delegates.GetValue("FarRangeAttacks") = FarRangeAttacks;
        btc.tree.blackboard.delegates.GetValue("ReduceVelocity") = ReduceVelocity;
        btc.tree.blackboard.delegates.GetValue("ChangeFacingDirection") = ChangeFacingDirection;
        btc.tree.blackboard.delegates.GetValue("Move") = Move;
        btc.tree.blackboard.integers.GetValue("MaxHP") = healthbar.maxHealth;
        btc.tree.blackboard.integers.GetValue("CurrentHP") = healthbar.currentHealth;
        btc.tree.blackboard.vector2s.GetValue("PlayerPosition") = player.transform.position;
        btc.tree.blackboard.vector2s.GetValue("CurrentPosition") = transform.position;
        btc.tree.blackboard.gameObjects.GetValue("This") = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        btc.tree.blackboard.integers.GetValue("CurrentHP") = healthbar.currentHealth;
        btc.tree.blackboard.vector2s.GetValue("PlayerPosition") = player.transform.position;
        btc.tree.blackboard.vector2s.GetValue("CurrentPosition") = transform.position;

        btc.tree.blackboard.booleans.GetValue("InCloseRange") = closeFlag.flagged;
        btc.tree.blackboard.booleans.GetValue("InFarRange") = farFlag.flagged;
    }

    Node.State CloseRangeAttacks()
    {
        if (btc.tree.blackboard.integers.GetValue("AttackChance") < 40)
        {
            btc.tree.blackboard.booleans.GetValue("IsAttacking") = true;
            float holdTime = Random.Range(0.75f, 1.5f);
            StartCoroutine(Attack0(holdTime));
            btc.tree.blackboard.floats.GetValue("AttackWaitTime") = 0;
        }
        else if (btc.tree.blackboard.integers.GetValue("AttackChance") < 80)
        {
            btc.tree.blackboard.booleans.GetValue("IsAttacking") = true;
            float holdTime = Random.Range(0.75f, 1.5f);
            StartCoroutine(Attack1(holdTime));
            btc.tree.blackboard.floats.GetValue("AttackWaitTime") = 0;
        }
        else
            btc.tree.blackboard.floats.GetValue("AttackWaitTime") = 2;
        return Node.State.SUCCESS;
    }
    Node.State FarRangeAttacks()
    {
        if (btc.tree.blackboard.integers.GetValue("AttackChance") < 70)
        {
            btc.tree.blackboard.booleans.GetValue("IsAttacking") = true;
            animator.SetBool("Attack2", true);
            float holdTime = Random.Range(0.75f, 1.5f);
            StartCoroutine(Attack2(holdTime));
            btc.tree.blackboard.floats.GetValue("AttackWaitTime") = 0;
        }
        return Node.State.SUCCESS;
    }
    Node.State ReduceVelocity()
    {
        body2d.velocity = new Vector2(body2d.velocity.x * 0.97f, body2d.velocity.y);
        return Node.State.SUCCESS;
    }
    Node.State ChangeFacingDirection()
    {
        if (btc.tree.blackboard.booleans.GetValue("FacingRight"))
        {
            btc.tree.blackboard.booleans.GetValue("FacingRight") = false;
            GetComponent<SpriteRenderer>().flipX = false;
            FixHitboxes(hitboxesAttack0);
            FixHitboxes(hitboxesAttack1);
            FixHitboxes(hitboxesIntoDown);
            FixHitboxes(hurtboxesUp);
            FixHitboxes(hurtboxesDown);
            FixHitboxes(farFlag);
        }
        else
        {
            btc.tree.blackboard.booleans.GetValue("FacingRight") = true;
            GetComponent<SpriteRenderer>().flipX = true;
            FixHitboxes(hitboxesAttack0);
            FixHitboxes(hitboxesAttack1);
            FixHitboxes(hitboxesIntoDown);
            FixHitboxes(hurtboxesUp);
            FixHitboxes(hurtboxesDown);
            FixHitboxes(farFlag);
        }
        return Node.State.SUCCESS;
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
            hitbox.SetActive(false);
    }
    private void enableHitboxes(List<GameObject> hitboxes)
    {
        foreach (GameObject hitbox in hitboxes)
            hitbox.SetActive(true);
    }
    Node.State Move()
    {
        body2d.AddForce(btc.tree.blackboard.vector2s.GetValue("DirectionalForce"));
        if (body2d.velocity.x > maxSpeed)
            body2d.velocity = new Vector2(maxSpeed, body2d.velocity.y);
        else if (body2d.velocity.x < -maxSpeed)
            body2d.velocity = new Vector2(-maxSpeed, body2d.velocity.y);
        return Node.State.SUCCESS;
    }
    IEnumerator Attack0(float time)
    {
        am.index = 0;
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack0_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack0_Trigger");
    }
    IEnumerator Attack1(float time)
    {
        am.index = 1;
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack1_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack1_0_Trigger");
        yield return new WaitForSeconds(time / 2);
        animator.SetTrigger("Attack1_1_Trigger");
    }
    IEnumerator Attack2(float time)
    {
        am.index = 2;
        animator.SetBool("Moving", false);
        animator.SetTrigger("IntoDown_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("IntoDown_Trigger");
    }
    public void Attack0_Hitbox0()
    {
        hitboxesAttack0[0].SetActive(true);
    }
    public void Attack0_Hitbox1()
    {
        hitboxesAttack0[0].SetActive(false);
        hitboxesAttack0[1].SetActive(true);
    }
    public void Attack0_Hitbox2()
    {
        hitboxesAttack0[1].SetActive(false);
        hitboxesAttack0[2].SetActive(true);
    }
    public void Attack0_Hitbox3()
    {
        hitboxesAttack0[2].SetActive(false);
        hitboxesAttack0[3].SetActive(true);
    }
    public void Attack0_End()
    {
        hitboxesAttack0[3].SetActive(false);
    }
    public void Attack0_Cancellable()
    {
        btc.tree.blackboard.booleans.GetValue("IsAttacking") = false;
    }
    public void Attack1_Hitbox0()
    {
        hitboxesAttack1[0].SetActive(true);
    }
    public void Attack1_Hitbox0_End()
    {
        hitboxesAttack1[0].SetActive(false);
    }
    public void Attack1_Hitbox1()
    {
        hitboxesAttack1[1].SetActive(true);
    }
    public void Attack1_Hitbox1_End()
    {
        hitboxesAttack1[1].SetActive(false);
    }
    public void Attack1_Recovery()
    {
        btc.tree.blackboard.booleans.GetValue("IsAttacking") = false;
    }
    public void Into_Down_Hitbox0()
    {
        hitboxesIntoDown[0].SetActive(true);
    }
    public void Into_Down_Hitbox1()
    {
        hitboxesIntoDown[0].SetActive(false);
        hitboxesIntoDown[1].SetActive(true);
        disableHitboxes(hurtboxesUp);
        enableHitboxes(hurtboxesDown);
    }
    public void Into_Down_Hitbox2()
    {
        hitboxesIntoDown[1].SetActive(false);
        hitboxesIntoDown[2].SetActive(true);
    }
    public void Into_Down_Hitbox2_End()
    {

        hitboxesIntoDown[2].SetActive(false);
    }
    public void Into_Down_End()
    {
        /*
        if (cinematic)
        {
            moveLeft = true;
            acceleration = 12f;
            maxSpeed = 10f;
            running = true;
            running_Hitboxes[0].SetActive(true);
            attackManager.index = 3;
            animator.SetTrigger("Down_Move_Trigger");
        }
        */
    }
    public void Down_Idle_Start()
    {
        btc.tree.blackboard.booleans.GetValue("IsAttacking") = false;
    }
    public void Into_Up_End()
    {
        animator.SetBool("Attack2", false);
        btc.tree.blackboard.booleans.GetValue("IsAttacking") = false;
        enableHitboxes(hurtboxesUp);
        disableHitboxes(hurtboxesDown);
    }
}
