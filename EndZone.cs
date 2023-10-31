using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndZone : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider2D endzoneCollider;

    public delegate void EndZoneAction(GameObject animal);
    public static event EndZoneAction OnEndzoneReached;

    private void Awake()
    {
        if (endzoneCollider == null)
        {
            endzoneCollider = GetComponent<BoxCollider2D>();
        }

    }


    /// <summary>
    /// sends an event, when animal reach endzone
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (OnEndzoneReached != null)
        {
            if (collision.gameObject.tag == "Animal")
            {
                var animal = collision.gameObject;
                OnEndzoneReached(animal);

            }
        }       
    }
}
