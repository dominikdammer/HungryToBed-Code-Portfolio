using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tower", menuName = "SO/Tower")]
public class BaseTower : ScriptableObject
{   
    public string _name;
    public Sprite towerSprite;
    public Vector2 towerSize;
    public GameObject ammo;  
    public Vector2 ammoSize;
    public int cost;

    public float range;
    public float shootInterval;
    public int dmg;
    [Tooltip("This is a percent value")]
    public float effectAmount;
    public float effectDuration;


    public AnimationClip animation;
    private string animationName;
    public AudioClip sound;

    public string AnimationName(string name)
    {
        animationName = animation.name;
        name = animationName;

        return name;
    }
}
