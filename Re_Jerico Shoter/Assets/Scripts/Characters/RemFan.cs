using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RemFan : Personaje
{
    private NavMeshAgent agent;
    private Animator anim;
    [SerializeField] private GameObject summonFx;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        StartCoroutine(PoisonDmg());

        Destroy(Instantiate(summonFx, transform.position, transform.rotation), 2f);
    }

    private void LateUpdate()
    {
        agent.destination = GetPlayerTr().position;
        anim.SetFloat("speed", agent.velocity.magnitude);

        float dis = Vector3.Distance(transform.position, GetPlayerTr().position);

        if (dis <= 0.8f)
        {
            agent.destination = transform.position;
        }
    }

    IEnumerator PoisonDmg()
    {
        float dis = Vector3.Distance(transform.position, GetPlayerTr().position);

        if (dis <= 2f)
        {
            GetPlayerTr().GetComponent<IDamageable>().RecibirDaño(3);
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(PoisonDmg());
    }
}
