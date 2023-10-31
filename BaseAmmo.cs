using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseAmmo : MonoBehaviour
{
    [Header("General")]
    [HideInInspector]public Tower tower;
    ///has to be a vector and no transform, because of the vertexlist of path
    [HideInInspector]public Vector3 target;
    [HideInInspector]public int damage;
    [HideInInspector]public float moveSpeed;
    [HideInInspector]public float effectAmount;
    [HideInInspector]public float effectDuration;
    [HideInInspector]public Collider2D _collider;
    public Sprite slowUIIndicator;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound;

    [Header("Effects")]
    public GameObject hitEffect;

    public bool targetPosReached;

    private bool beginColliding = true;

    public bool BeginColliding
    {
        get { return beginColliding; }
        set { beginColliding = value; }
    }

    /// <summary>
    /// for ammo with damage
    /// </summary>
    /// <param name="target">where to move</param>
    /// <param name="moveSpeed">movement speed of the ammo</param>
    /// <param name="sender">tower the ammo originated from</param>
    public virtual void InitializeDmgAmmo(Vector3 target, int damage, float moveSpeed, Tower sender)
    {
        this.target = target;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        this.tower = sender;
    }

    /// <summary>
    /// for ammo without damage, but statuseffect
    /// </summary>
    /// <param name="target">where to move</param>
    /// <param name="effectAmount">status effect</param>
    /// <param name="moveSpeed">movement speed of the ammo</param>
    /// <param name="sender">tower the ammo originated from</param>
    public virtual void InitializeEffectAmmo(Vector3 target, float effectAmount, float effectDuration, float moveSpeed, Tower sender)
    {
        this.target = target;
        this.effectAmount = effectAmount;
        this.effectDuration = effectDuration;
        this.moveSpeed = moveSpeed;
        this.tower = sender;
    }

    public virtual void Update()
    {
        if (target != null)
        {
            targetPosReached = false;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }

        if (transform.position == target)
        {
            targetPosReached = true;
        }
    }

    public virtual void PlaySound()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = Random.Range(0.95f, 1.05f);
        audioSource.Play();
        audioSource.PlayOneShot(sound);
    }

    public virtual void GetCollider()
    {
        _collider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// for dropable ammo
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!BeginColliding)
        {
            return;
        }

        if (other.CompareTag("Animal"))
        {
            ///change this
            SpecificBehaviour(other);

            ///initial behaviour
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);
           
            if (sound != null)
                PlaySound();
                       
            tower.CurrentUsagesAmount -= 1;
            ///disables components so the sound plays correctly
            GetCollider();
            _collider.enabled = false;
            GetComponentInChildren<SpriteRenderer>().enabled = false;


            DestroyAmmo();                   
        }
    }

    private void DestroyAmmo()
    {
        if(sound != null)
        {
            Destroy(gameObject, sound.length);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Animal"))
        {
            return;
        }
    }

    public virtual void SpecificBehaviour(Collider2D other)
    {
        return;
    }
}
