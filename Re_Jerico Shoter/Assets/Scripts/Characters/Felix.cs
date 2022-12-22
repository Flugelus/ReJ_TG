using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Felix : Personaje
{
    [SerializeField] private GameObject sword;
    private NavMeshAgent agent;
    private Animator anim;
    [SerializeField]private Animator anim2;
    private bool isHurt = false;
    private int state = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        sword.GetComponent<FelixSword>().SetDmg(daño);
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        float dis = Vector3.Distance(transform.position, GetPlayerTr().position);

        if (state == 0)
        {
            agent.destination = GetPlayerTr().position;
            anim.SetFloat("speed", agent.velocity.magnitude);
            anim2.SetFloat("speed", agent.velocity.magnitude);

            if (dis <= 1.2f)
            {
                state = 1;
                StartCoroutine(AtkTime());
            }
        }
        else if(state == 1)
        {
            agent.destination = transform.position;
        }
        else if(state == 2)
        {

        }
    }

    IEnumerator AtkTime()
    {
        Vector3 direction = GetPlayerTr().transform.position - transform.position;
        direction.y += 1;

        Quaternion rotacion = Quaternion.LookRotation(direction);

        transform.rotation = rotacion;

        float dis = Vector3.Distance(transform.position, GetPlayerTr().position);
        anim.SetBool("atk02", true);
        anim2.SetBool("atk02", true);
        anim.applyRootMotion = true;
        //anim2.applyRootMotion = true;
        yield return new WaitForSeconds(0.3f);
        anim.SetFloat("speed", 0);
        anim2.SetFloat("speed", 0);

        yield return new WaitForSeconds(0.3f);

        sword.GetComponent<BoxCollider>().enabled = true;
        anim.SetBool("atk02", false);
        anim2.SetBool("atk02", false);

        yield return new WaitForSeconds(1.1f);
        sword.GetComponent<BoxCollider>().enabled = false;
        anim.applyRootMotion = false;
        //anim2.applyRootMotion = false;

        if (dis <= 2.1f)
        {
            state = 1;
            StartCoroutine(AtkTime());
        }
        else
        {
            state = 0;
        }
    }
    public override void Morir()
    {
        if (vida > 0) return;

        GetComponent<CapsuleCollider>().enabled = false;
        isDeath = true;
        agent.enabled = false;
        GetComponent<Felix>().enabled = false;
        anim.SetBool("isDeath", true);
        anim2.SetBool("isDeath", true);

        Destroy(gameObject, 6f);
    }
}
