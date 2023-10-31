using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Animal target;
    private int damage;
    private float moveSpeed;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip nomSound;
    public GameObject hitEffect;

    public void Initialize(Animal target, int damage, float moveSpeed)
    {
        this.target = target;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
    }
    private void PlaySound()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = nomSound;
        audioSource.Play();
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = Random.Range(0.95f, 1.05f);
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            //transform.LookAt(target.transform);
            if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
            {
                PlaySound();
                target.GetDamaged(damage);
                if (hitEffect != null)
                    Instantiate(hitEffect, transform.position, Quaternion.identity);
                //disables components so the sound plays correctly
                GetComponent<CapsuleCollider2D>().enabled = false;
                GetComponentInChildren<SpriteRenderer>().enabled = false;
                Destroy(gameObject,nomSound.length);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
