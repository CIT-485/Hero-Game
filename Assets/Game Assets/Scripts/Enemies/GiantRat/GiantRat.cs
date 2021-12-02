using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GiantRat : MonoBehaviour
{
    public BehaviourTree tree;
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
    public GameObject intoDownPosition;
    public GameObject runningLeftLimit;
    public GameObject runningRightLimit;
    public GameObject laserPosition;
    public GameObject damageFlash;
    public GameObject laserBeamKnockback;
    public GameObject DeadRat;
    public ParticleSystem LaserBeam;
    public Light2D LaserBeamLight;
    public Light2D LaserBeamBodyLight;

    public Flag closeFlag;
    public Flag farFlag;

    public bool damaged;
    public bool reached;
    public bool running;
    public bool poweredUp;
    public bool doneOnce;
    public ValueWrapper<bool> laserReady = new ValueWrapper<bool>(false);
    public bool laserRotate;
    public bool laserShrink;
    public bool doOnce;
    public Vector3 originalPosition;

    public float laserRotation = 1;
    public float laserShake = 0.015f;
    private void Awake()
    {
        BehaviourTree clone = tree.Clone();
        tree = clone;
        tree.Bind();
    }
    void Start()
    {
        healthbar = GetComponent<HealthBar>();
        player = GameObject.FindGameObjectWithTag("Player");
        am = transform.parent.GetComponent<AttackManager>();
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();

        tree.blackboard.delegates.GetValue("CloseRangeAttacks") = CloseRangeAttacks;
        tree.blackboard.delegates.GetValue("FarRangeAttacks") = FarRangeAttacks;
        tree.blackboard.delegates.GetValue("ReduceVelocity") = ReduceVelocity;
        tree.blackboard.delegates.GetValue("ChangeFacingDirection") = ChangeFacingDirection;
        tree.blackboard.delegates.GetValue("Move") = Move;
        tree.blackboard.delegates.GetValue("RunningAttack") = RunningAttack;
        tree.blackboard.delegates.GetValue("UndoCinematicChanges") = UndoCinematicChanges;
        tree.blackboard.delegates.GetValue("LaserAttack") = LaserAttack;
        tree.blackboard.integers.GetValue("MaxHP") = healthbar.maxHealth;
        tree.blackboard.integers.GetValue("CurrentHP") = healthbar.currentHealth;
        tree.blackboard.vector2s.GetValue("PlayerPosition") = player.transform.position;
        tree.blackboard.vector2s.GetValue("CurrentPosition") = transform.position;
        tree.blackboard.vector2s.GetValue("IntoDownPosition") = intoDownPosition.transform.position;
        tree.blackboard.vector2s.GetValue("RunningLeftLimit") = runningLeftLimit.transform.position;
        tree.blackboard.vector2s.GetValue("RunningRightLimit") = runningRightLimit.transform.position;
        tree.blackboard.vector2s.GetValue("LaserPosition") = laserPosition.transform.position;
        tree.blackboard.gameObjects.GetValue("This") = gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        tree.blackboard.integers.GetValue("CurrentHP") = healthbar.currentHealth;
        tree.blackboard.vector2s.GetValue("PlayerPosition") = player.transform.position;
        tree.blackboard.vector2s.GetValue("CurrentPosition") = transform.position;

        tree.blackboard.booleans.GetValue("InCloseRange") = closeFlag.flagged;
        tree.blackboard.booleans.GetValue("InFarRange") = farFlag.flagged;

        if (running)
        {
            if (!tree.blackboard.booleans.GetValue("FacingRight"))
            {
                if (transform.position.x < runningLeftLimit.transform.position.x)
                    reached = true;
                else
                {
                    tree.blackboard.vector2s.GetValue("DirectionalForce").x -= tree.blackboard.floats.GetValue("Acceleration");
                    Move();
                }
            }
            else
            {
                if (transform.position.x > runningRightLimit.transform.position.x)
                    reached = true;
                else
                {
                    tree.blackboard.vector2s.GetValue("DirectionalForce").x += tree.blackboard.floats.GetValue("Acceleration");
                    Move();
                }
            }
        }

        LaserBeamBodyLight.intensity = LaserBeam.main.startSize.constant / 2;
        LaserBeamLight.transform.localScale = new Vector3(LaserBeam.main.startSize.constant * 1, LaserBeam.main.startSize.constant * 10, 1);
        LaserBeamLight.transform.localPosition = new Vector3(0, LaserBeam.main.startSize.constant * 13, 0);
        laserShake = LaserBeam.main.startSize.constant / 20;

        if (LaserBeam.gameObject.activeSelf && !laserShrink)
        {
            if (LaserBeam.startSize < 2)
                LaserBeam.startSize *= 1.05f;
        }
        else if (laserShrink || healthbar.currentHealth == 0)
        {
            if (LaserBeam.startSize > 0.1f)
                LaserBeam.startSize *= 0.95f;
            else
            {
                LaserBeam.Stop();
                LaserBeamLight.gameObject.SetActive(false);
                LaserBeamBodyLight.gameObject.SetActive(false);
                laserReady.Value = false;
                tree.blackboard.booleans.GetValue("Laser") = false;
                animator.SetTrigger("Attack1_End_Trigger");
            }
        }

        if (laserRotate)
        {
            Debug.Log(LaserBeam.transform.localRotation.eulerAngles.z);
            if (LaserBeam.transform.localRotation.eulerAngles.z < 250)
            {
                laserRotation *= 1.025f;
                LaserBeam.transform.Rotate(new Vector3(0, 0, laserRotation));
            }
            else if (LaserBeam.transform.localRotation.eulerAngles.z < 360)
            {
                laserRotation *= 0.975f;
                LaserBeam.transform.Rotate(new Vector3(0, 0, laserRotation));
            }
        }

        if (healthbar.currentHealth > 0)
            tree.Update();
        else
        {
            DeadRat.GetComponent<SpriteRenderer>().enabled = true;
            DeadRat.GetComponent<StickToObject>().enabled = false;
            DeadRat.GetComponent<GiantRatDead>().enabled = true;
            DeadRat.GetComponent<GiantRatDead>().amuletLight.gameObject.SetActive(true);
            if (tree.blackboard.booleans.GetValue("FacingRight"))
                DeadRat.GetComponent<SpriteRenderer>().flipX = true;
            Destroy(transform.parent.gameObject);
        }
    }
    private Node.State RunningAttack()
    {
        ChangeFacingDirection();
        body2d.velocity = Vector2.zero;
        StartCoroutine(Attack2(0));
        return Node.State.SUCCESS;
    }
    private Node.State LaserAttack()
    {
        ChangeFacingDirection();
        body2d.velocity = Vector2.zero;
        StartCoroutine(Attack1(0));
        return Node.State.SUCCESS;
    }
    Node.State CloseRangeAttacks()
    {
        float holdTime = 0;
        if (poweredUp)
            holdTime = Random.Range(0.45f, 0.85f);
        else
            holdTime = Random.Range(0.75f, 1.5f);
        if (tree.blackboard.integers.GetValue("AttackChance") < 40)
        {
            holdTime = Random.Range(0.5f, 2f);
            tree.blackboard.booleans.GetValue("IsAttacking") = true;
            StartCoroutine(Attack0(holdTime));
            tree.blackboard.floats.GetValue("AttackWaitTime") = 0;
        }
        else if (tree.blackboard.integers.GetValue("AttackChance") < 80)
        {
            tree.blackboard.booleans.GetValue("IsAttacking") = true;
            if (poweredUp)
            {
                float variant = Random.Range(0, 60);
                Debug.Log(variant);
                if (variant < 20)
                    StartCoroutine(Attack1_Var2(holdTime));
                else if (variant < 40)
                    StartCoroutine(Attack1_Var3(holdTime));
                else if (variant < 60)
                    StartCoroutine(Attack1_Var4(holdTime));
            }
            else
                StartCoroutine(Attack1(holdTime));
            tree.blackboard.floats.GetValue("AttackWaitTime") = 0;
        }
        else
            tree.blackboard.floats.GetValue("AttackWaitTime") = 2;
        return Node.State.SUCCESS;
    }
    Node.State FarRangeAttacks()
    {
        float holdTime = 0;
        if (poweredUp)
            holdTime = Random.Range(0.45f, 0.85f);
        else
            holdTime = Random.Range(0.75f, 1.5f);
        if (tree.blackboard.integers.GetValue("AttackChance") < 50)
        {
            tree.blackboard.booleans.GetValue("IsAttacking") = true;
            float runChance = Random.Range(0, 100);
            if (runChance < 50 && poweredUp)
            {
                holdTime = Random.Range(1.25f, 1.5f);
                animator.SetBool("Attack2", false);
            }
            else
                animator.SetBool("Attack2", true);
            StartCoroutine(Attack2(holdTime));
            tree.blackboard.floats.GetValue("AttackWaitTime") = 0;
        }
        else
            tree.blackboard.floats.GetValue("AttackWaitTime") = 2;
        return Node.State.SUCCESS;
    }
    Node.State ReduceVelocity()
    {
        body2d.velocity = new Vector2(body2d.velocity.x * 0.97f, body2d.velocity.y);
        return Node.State.SUCCESS;
    }
    Node.State ChangeFacingDirection()
    {
        if (tree.blackboard.booleans.GetValue("FacingRight"))
        {
            tree.blackboard.booleans.GetValue("FacingRight") = false;
            GetComponent<SpriteRenderer>().flipX = false;
            FixHitboxes(hitboxesAttack0);
            FixHitboxes(hitboxesAttack1);
            FixHitboxes(hitboxesIntoDown);
            FixHitboxes(hitboxesRunning);
            FixHitboxes(hurtboxesUp);
            FixHitboxes(hurtboxesDown);
            FixHitboxes(farFlag);
        }
        else
        {
            tree.blackboard.booleans.GetValue("FacingRight") = true;
            GetComponent<SpriteRenderer>().flipX = true;
            FixHitboxes(hitboxesAttack0);
            FixHitboxes(hitboxesAttack1);
            FixHitboxes(hitboxesIntoDown);
            FixHitboxes(hitboxesRunning);
            FixHitboxes(hurtboxesUp);
            FixHitboxes(hurtboxesDown);
            FixHitboxes(farFlag);
        }
        return Node.State.SUCCESS;
    }
    Node.State UndoCinematicChanges()
    {
        tree.blackboard.floats.GetValue("Acceleration") = 7f;
        tree.blackboard.booleans.GetValue("CinematicDoneMoving") = false;
        poweredUp = true;
        hitboxesRunning[0].SetActive(false);
        maxSpeed = 3f;
        animator.SetTrigger("IntoUp_Trigger");
        return Node.State.SUCCESS;
    }
    Node.State Move()
    {
        body2d.AddForce(tree.blackboard.vector2s.GetValue("DirectionalForce"));
        if (body2d.velocity.x > maxSpeed)
            body2d.velocity = new Vector2(maxSpeed, body2d.velocity.y);
        else if (body2d.velocity.x < -maxSpeed)
            body2d.velocity = new Vector2(-maxSpeed, body2d.velocity.y);
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
    IEnumerator Attack0(float time)
    {
        if (poweredUp)
            am.attacks[0].attackDamage = 75 + (int)(time * 50);
        else
            am.attacks[0].attackDamage = 100;
        am.index = 0;
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack0_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack0_Trigger");
    }
    IEnumerator LaserReady()
    {
        yield return new WaitForSeconds(2);
        laserReady.Value = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollowObject>().Shake(0.1f, laserReady);
    }
    IEnumerator Laser()
    {
        StartCoroutine(LaserReady());
        yield return new WaitUntil(() => laserReady.Value);
        am.index = 5;
        LaserBeam.gameObject.SetActive(true);
        LaserBeamBodyLight.gameObject.SetActive(true);
        laserBeamKnockback.SetActive(false);
        yield return new WaitForSeconds(2);
        laserRotate = true;
        yield return new WaitUntil(() => LaserBeam.transform.localRotation.eulerAngles.z > 290);
        laserRotate = false;
        laserShrink = true;
    }
    IEnumerator Attack1(float time)
    {
        if (tree.blackboard.booleans.GetValue("Laser"))
        {
            am.index = 6;
            laserBeamKnockback.SetActive(true);
        }
        else
            am.index = 1;
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack1_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack1_0_Trigger");
        yield return new WaitForSeconds(time / 2);
        animator.SetTrigger("Attack1_1_Trigger");
        if (tree.blackboard.booleans.GetValue("Laser"))
        {
            StartCoroutine(Laser());
        }
        else
            animator.SetTrigger("Attack1_End_Trigger");
    }
    IEnumerator Attack1_Var2(float time)
    {
        am.index = 1;
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack1_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack1_1_Trigger");
        yield return new WaitForSeconds(time / 2);
        animator.SetTrigger("Attack1_1_Trigger");
        animator.SetTrigger("Attack1_End_Trigger");
    }
    IEnumerator Attack1_Var3(float time)
    {
        am.index = 1;
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack1_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack1_0_Trigger");
        yield return new WaitForSeconds(time / 2);
        animator.SetTrigger("Attack1_1_Trigger");
        yield return new WaitForSeconds(time / 2);
        animator.SetTrigger("Attack1_0_Trigger");
        yield return new WaitForSeconds(time / 2);
        animator.SetTrigger("Attack1_1_Trigger");
        animator.SetTrigger("Attack1_End_Trigger");
    }
    IEnumerator Attack1_Var4(float time)
    {
        am.index = 1;
        animator.SetBool("Moving", false);
        animator.SetTrigger("Attack1_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Attack1_0_Trigger");
        yield return new WaitForSeconds(time / 2);
        animator.SetTrigger("Attack1_0_Trigger");
        yield return new WaitForSeconds(time / 2);
        animator.SetTrigger("Attack1_0_Trigger");
        animator.SetTrigger("Attack1_End_Trigger");
    }
    IEnumerator Attack2(float time)
    {
        am.index = 2;
        animator.SetBool("Moving", false);
        animator.SetTrigger("IntoDown_Start_Trigger");
        yield return new WaitForSeconds(time);
        animator.SetTrigger("IntoDown_Trigger");
    }
    IEnumerator RunningRight()
    {
        am.index = 4;
        yield return new WaitUntil(() => reached);
        reached = false;
        tree.blackboard.floats.GetValue("Acceleration") = 7f;
        running = false;
        hitboxesRunning[0].SetActive(false);
        maxSpeed = 3f;
        animator.SetTrigger("IntoUp_Trigger");
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
        tree.blackboard.booleans.GetValue("IsAttacking") = false;
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
        tree.blackboard.booleans.GetValue("IsAttacking") = false;
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
        if (tree.blackboard.booleans.GetValue("Cinematic"))
        {
            ChangeFacingDirection();
            animator.SetTrigger("Down_Move_Trigger");
            tree.blackboard.booleans.GetValue("CinematicRunningLeft") = true;
            tree.blackboard.floats.GetValue("Acceleration") = 12f;
            maxSpeed = 10f;
            tree.blackboard.booleans.GetValue("CinematicRunning") = true;
            hitboxesRunning[0].SetActive(true);
            am.index = 3;
        }
        else
        {
            if (poweredUp && !animator.GetBool("Attack2"))
            {
                running = true;
                animator.SetTrigger("Down_Move_Trigger");
                tree.blackboard.floats.GetValue("Acceleration") = 12f;
                maxSpeed = 10f;
                hitboxesRunning[0].SetActive(true);
                StartCoroutine(RunningRight());
            }
        }
    }
    public void Down_Idle_Start()
    {
        //tree.blackboard.booleans.GetValue("IsAttacking") = false;
    }
    public void Into_Up_End()
    {
        animator.SetBool("Attack2", false);
        tree.blackboard.booleans.GetValue("IsAttacking") = false;
        enableHitboxes(hurtboxesUp);
        disableHitboxes(hurtboxesDown);
        if (tree.blackboard.booleans.GetValue("Cinematic"))
            tree.blackboard.booleans.GetValue("Cinematic") = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox" && !damaged)
        {
            damaged = true;
            StartCoroutine(invulActivate(player.GetComponent<AttackManager>().currentAttack.stunTime));
            if (!tree.blackboard.booleans.GetValue("Cinematic") && !running && !tree.blackboard.booleans.GetValue("Laser"))
                healthbar.TakeDamage(player.GetComponent<AttackManager>().currentAttack.attackDamage);
        }
    }
    IEnumerator invulActivate(float time)
    {
        damageFlash.SetActive(true);
        yield return new WaitForSeconds(time);
        damageFlash.SetActive(false);
        damaged = false;
    }
}
