using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public int vida { get; set; }

    public void RecibirDaño(int dmg)
    {

    }
}
