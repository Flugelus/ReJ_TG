using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBook : MonoBehaviour
{
    [SerializeField] int healPoints;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<ThirdPersonShoterController>().Heal(healPoints);
            Destroy(gameObject);
        }
    }
}
