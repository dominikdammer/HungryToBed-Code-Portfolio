using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName= "new Animal", menuName = "SO/animal")]
public class BaseAnimal : ScriptableObject 
{

    public string _name;
    public Sprite sprite;
    public Vector2 size;

    public int health;
    public int dropAmount;
    public int damageModifier = 1;

    public int dirt;
    public float speed;


    public AnimationClip walkAnimation;
    public AnimationClip deathAnimation;
    public VisualEffect despawnEffect;

    private string animationName;

    public AudioClip sound;
    [Tooltip("adds volume %")]
    public float volume = 0.1f;

    public string AnimationName(string name)
    {
        animationName = walkAnimation.name;
        name = animationName;

        return name;
    }

    public string DeathAnimationName(string name)
    {
        animationName = deathAnimation.name;
        name = animationName;

        return name;
    }
}
