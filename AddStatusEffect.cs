using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStatusEffect : MonoBehaviour
{
    public EffectType effectType;
    private Animal animal;
    private float duration;
    private float effectImpact;

    private float slowAmount;
    private float initialAnimalSpeed;
    private bool effectIsActive = false;
    private SpriteRenderer uiSprite;
    private Sprite slowSprite;
    private BaseAmmo sendingAmmo;

    public enum EffectType
    {
        Slow,
        Speed,
        Freeze,
        Default
    }

    public virtual void ApplyEffect(Animal animal, float duration, float effectImpact, BaseAmmo sendingAmmo,
        EffectType effectType = EffectType.Default)
    {
        this.animal = animal;
        this.duration = duration;
        this.effectImpact = effectImpact;
        this.effectType = effectType;
        this.sendingAmmo = sendingAmmo;

        slowSprite = sendingAmmo.slowUIIndicator;
        uiSprite = animal.UISpriteRenderer;
        if (this != null)
        {
            StartCoroutine(EffectDuration(animal, duration, effectImpact, effectType));
        }
    }

    private void ChooseEffect(EffectType effectType, float effectImpact)
    {
        switch (effectType)
        {
            case EffectType.Slow:
                SlowEffect(effectImpact);
                if (slowSprite != null)
                uiSprite.sprite = slowSprite;
                break;
            case EffectType.Speed:
                break;
            case EffectType.Freeze:
                break;
            case EffectType.Default:
                break;
            default:
                break;
        }
    }

    private void SlowEffect(float effectImpact) 
    { 
        if (animal != null)
        {
            if (effectIsActive)
            {
                float reduction;
                initialAnimalSpeed = animal.Speed;
                reduction = (animal.Speed *  effectImpact) / 100;
                animal.Speed -= reduction;
                //Debug.Log("Speed reduced by " + reduction + " from "  + initialAnimalSpeed + " to " + animal.Speed);
            }
            else
            {
                animal.Speed = initialAnimalSpeed;
                //Debug.Log("Speed back to " + animal.Speed);
            }
        }
    }
    private void SpeedEffect() { return; }
    private void FreezeEffect() { return; }


    private void SetUIIndicator()
    {
        if (animal != null)
        {
            uiSprite = animal.UISpriteRenderer;

            if (effectIsActive)
            {
                uiSprite.gameObject.SetActive(true);
            }
            else
            {
                uiSprite.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator EffectDuration(Animal animal, float duration, float effectImpact, EffectType effectType)
    {
        effectIsActive = true;
        SetUIIndicator();
        ChooseEffect(effectType, effectImpact);

        yield return new WaitForSeconds(duration);

        effectIsActive = false;
        SetUIIndicator();
        ChooseEffect(effectType, effectImpact);

        //TODO: maybe add a resistance for a new effect to add?
        yield return null;
    }


    private void AddScript()
    {
        animal.gameObject.AddComponent<AddStatusEffect>();
    }

    private void RemoveScript()
    {
        Destroy(this);
    }

}
