using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rem : Personaje
{
    private NavMeshAgent agent;
    private Animator anim;
    [SerializeField] private Animator anim2;
    [SerializeField] private GameObject remFan;
    private int state = 0;
    private bool isEnter;

    [Header("Patrol")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public LayerMask whatIsGround;

    [SerializeField] private float summonTime = 2.4f;
    private float resetPos = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.Move(Vector3.forward);
    }

    private void LateUpdate()
    {
        float dis = Vector3.Distance(transform.position, GetPlayerTr().position);

        if (state == 0)
        {
            if (dis <= 10f && !isEnter)
            {
                StartCoroutine(SummonSimps());

                isEnter = true;
            }
        }
        else if (state == 1)
        {
            Patroling();

            anim.SetFloat("speed", agent.velocity.magnitude);
            anim2.SetFloat("speed", agent.velocity.magnitude);
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        resetPos += 1 * Time.deltaTime;

        if(resetPos >= 4)
        {
            resetPos = 0;
            if (walkPointSet) walkPointSet = false;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 2.5f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, 1f, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround) && Vector3.Distance(walkPoint, GetPlayerTr().position) >= 8)
        {
            walkPointSet = true;
        }
    }

    IEnumerator SummonSimps()
    {
        anim.SetBool("isCast", true);
        anim2.SetBool("isCast", true);

        StartCoroutine(Summon());

        yield return new WaitForSeconds(0.5f);

        anim.SetBool("isCast", false);
        anim2.SetBool("isCast", false);

        yield return new WaitForSeconds(1.2f);

        state = 1;
    }

    public override void Morir()
    {
        if (vida > 0) return;

        GetComponent<CapsuleCollider>().enabled = false;
        isDeath = true;
        agent.enabled = false;
        GetComponent<Rem>().enabled = false;
        anim.applyRootMotion = true;
        anim.SetBool("isDeath", true);
        anim2.SetBool("isDeath", true);

        Destroy(gameObject, 6f);
    }

    IEnumerator Summon()
    {
        Vector3 pos = transform.position;

        yield return new WaitForSeconds(summonTime);

        summonTime += 1.5f;

        Instantiate(remFan, pos, transform.rotation);

        StartCoroutine(Summon());
    }
}
