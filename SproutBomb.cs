using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SproutBomb : BaseAmmo
{
    [Header("General")]
    [Range(0f, 1f)]
    private float maxGrowth = 0.97f;
    private float minGrowth = 0.2f;
    private bool fullyGrown = false;
    [SerializeField]
    private float timeToGrow = 4;
    private float refreshRate = 0.05f;
    private Material material;
    [SerializeField]
    private CircleCollider2D circleCollider;
    private Animal animal;
    [SerializeField, ReadOnly]
    private float slowDuration;
    private float _effectAmount;

    private AddStatusEffect addStatusEffect;

    public override void InitializeEffectAmmo(Vector3 target, float slowAmount, float duration, float moveSpeed, Tower sender)
    {
        this.target = target;
        this._effectAmount = slowAmount;
        this.moveSpeed = moveSpeed;
        this.tower = sender;
        this.slowDuration = duration;

        material = GetComponent<SpriteRenderer>().material;
        material.SetFloat("_grow", minGrowth);
        StartCoroutine(Growing());
    }

    public override void SpecificBehaviour(Collider2D other)
    {

        animal = other.GetComponent<Animal>();
        if(animal.GetComponent<AddStatusEffect>() == null)
        {
            animal.gameObject.AddComponent<AddStatusEffect>().
                ApplyEffect(animal, slowDuration, _effectAmount, this, AddStatusEffect.EffectType.Slow);
        }
        else
        {
            addStatusEffect.ApplyEffect(animal, slowDuration, _effectAmount, this, AddStatusEffect.EffectType.Slow);
        }              
    }

    public override void GetCollider()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    IEnumerator Growing()
    {
        float growValue = material.GetFloat("_grow");

        yield return new WaitUntil(() => targetPosReached);

        if (fullyGrown)
        {
            yield return null;
        }
        else
        {
            while (growValue < maxGrowth)
            {
                growValue += 1 / (timeToGrow / refreshRate);
                material.SetFloat("_grow", growValue);

                yield return new WaitForSeconds(refreshRate);
            }
        }

        if(growValue <= maxGrowth / 2)
        {
            BeginColliding = false;
        }
        else { BeginColliding = true; }

        if (growValue >= maxGrowth)
        {
            fullyGrown = true;            
        }
        else
        {
            fullyGrown = false;
        }
    }
}
