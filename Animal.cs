using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Animal : MonoBehaviour, IDamageable, IGiveMoney
{
    
    [HideInInspector]
    public BaseAnimal animalSO;
    public HungerBar hungerBar;
    private Tower tower;
    private PathCreation.Examples.PathFollower follower;
    [SerializeField]
    private PathCreation.PathCreator vPath;

    private SpriteRenderer sprite;
    [SerializeField]
    private SpriteRenderer uiSprite;
    private Color normalColor;
    private int health = 5;
    private int maxHealth;
    private float speed = 5;
    private int dirt = 0;
    private float despawnDelay = 0.3f;
    private float fadeOut = 0.4f;
    private int dropMoney;
    private const int baseDamage = 1;

    private AudioSource audioSource;
    private AudioClip sound;

    private Animator animator;

    public float Speed
    {
        get { return follower.Speed; }
        set { follower.Speed = value; }
    }

    public SpriteRenderer UISpriteRenderer
    {
        get { return uiSprite; }
        set { uiSprite = value; }
    }

    private void Start()
    {
        follower = GetComponent<PathCreation.Examples.PathFollower>();
        vPath = FindObjectOfType<PathCreation.PathCreator>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        //anim = GetComponentInChildren<Animation>();
        hungerBar = GetComponentInChildren<HungerBar>();
        hungerBar.UpdateBar(animalSO.health, 0f);
        health = animalSO.health;
        maxHealth = health;
        dropMoney = animalSO.dropAmount;

        Sprite();
        Parameter();
        Sound();
        Animation();
    }

    private void Update()
    {
        hungerBar.UpdateBar(maxHealth,health - maxHealth);
    }

    private void Sound()
    {
        sound = animalSO.sound;
        audioSource.clip = sound;
        audioSource.playOnAwake = true;
        audioSource.Play();

        audioSource.pitch = Random.Range(0.95f ,1.05f );
        audioSource.volume = Random.Range(0.95f * (animalSO.volume), 1.05f * (animalSO.volume));
    }

    private void Sprite()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sprite = animalSO.sprite;
        normalColor = sprite.color;
        sprite.size = animalSO.size;

        if (uiSprite.gameObject.activeInHierarchy)
        {
            uiSprite.gameObject.SetActive(false);
        }

    }

    private void Parameter()
    {
        this.name = animalSO.name;
        health = animalSO.health;
        follower.speed = animalSO.speed;
        dirt = animalSO.dirt;
        speed = animalSO.speed;
        follower.Speed = speed;
        dropMoney = animalSO.dropAmount;
    }

    private void Hit(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            ///<summary> animal halts, spawns vfx, plays death animation, gets destroyed
            ///destroy collider to remove from list, to prevent targeting
            ///tried to reduce walking speed to 0 after duration, but need update for this.
            ///might be better with animation event afterall
            ///</summary>
            follower.speed = Mathf.Lerp(animalSO.speed,0,fadeOut);
            Instantiate(animalSO?.despawnEffect, transform);            
            Destroy(GetComponent<Collider2D>());
            DeathAnimation();

            if(sprite.color.a >= 0.1)
            {
                Die();
            }           
        }
    }

    public void Die()
    {
        DropMoney(dropMoney);
        Destroy(gameObject, 
            this.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length + despawnDelay);
    }

    private void Animation()
    {
       animator?.Play(animalSO.AnimationName(name));       
    }
    private void DeathAnimation()
    {
        animator?.Play(animalSO.DeathAnimationName(name));       

        sprite.color = new Color(1,1,1,Mathf.Lerp(1, 0, fadeOut));       
    }

    IEnumerator hitFlash()
    {
        sprite.color = new Color(1f,0.47f, 0.47f);
        yield return new WaitForSeconds(0.3f);
        sprite.color = normalColor;      
    }

    private void DealDamage(GameObject animal)
    {
        if (this.gameObject == animal)
        {
            SaveData.DecreasePlayerHP(baseDamage * animalSO.damageModifier);
        }
    }


    #region Events

    private void OnEnable()
    {
        EndZone.OnEndzoneReached += DealDamage;
    }

    private void OnDisable()
    {
        EndZone.OnEndzoneReached -= DealDamage;
    }

    public void GetDamaged(int damageAmount)
    {
        StartCoroutine(hitFlash());
        hungerBar.Fill.fillAmount += damageAmount;
        Hit(damageAmount);
    }

    public void DropMoney(int moneyAmount)
    {
        SaveData.IncreaseWealth(moneyAmount);
    }



    #endregion Events
}
