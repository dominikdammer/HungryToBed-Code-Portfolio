using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnAnimationEnd : MonoBehaviour
{
    private Animal animal;

    private void Die()
    {
        animal.Die();
    }
}
