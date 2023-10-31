//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Ammo : MonoBehaviour
//{
//    public enum AmmoBehaviour
//    {
//        Projectile,
//        Drop
//    }

//    //[Tooltip("Dont change the order of textures")]
//    //public enum AmmoType
//    //{
//    //    Carrot,
//    //    Pellet
//    //}

//    public AmmoBehaviour ammoBehaviour;
//    //public AmmoType type;
//    public GameObject hitEffect;
//    public List<Texture2D> image = new List<Texture2D>();


 
//    private Tower tower;
//    //has to be a vector and no transform, because of the vertexlist of path
//    private Vector3 target;
//    private Animal animal;
//    private bool animalDetected = false;
//    private int damage;
//    private float moveSpeed;
//    private Texture2D ammoSprite;
//    private SpriteRenderer spriteRenderer;
//    private Rect rect;
  
//    public void Initialize(Vector3 target, int damage, float moveSpeed, Tower sender, AmmoBehaviour ammoBehaviour)
//    {
//        this.target = target;
//        this.damage = damage;
//        this.moveSpeed = moveSpeed;
//        this.tower = sender;
//        this.ammoBehaviour = ammoBehaviour;
//    }

//    private void Start()
//    {       
//        GetImage();
//    }

//    void Update()
//    {
//        if (target != null)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
//            GetAmmoType();

//            DestroyAmmo();
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void GetAmmoType()
//    {
//        switch (ammoBehaviour)
//        {
//            case AmmoBehaviour.Projectile:
//                {
//                    if (animalDetected)
//                    {
//                        ammoSprite = image[0];
//                        spriteRenderer.sprite = Sprite.Create(ammoSprite, rect, new Vector2(0.5f, 0.5f));

//                        Projectile();
//                    }                  
//                    break;
//                }
//            case AmmoBehaviour.Drop:
//                {
//                    if (animalDetected)
//                    {
//                        ammoSprite = image[1];
//                        spriteRenderer.sprite = Sprite.Create(ammoSprite, rect, new Vector2(0.5f, 0.5f));
//                        Drop();
//                    }                                   
//                    break;
//                }
//        }
//    }

//    private void GetImage()
//    {
//        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
//        rect = new Rect(0, 0, ammoSprite.width, ammoSprite.height);
//    }

//    private void Projectile()
//    {          
//        animal.GetDamaged(damage);         
//    }

//    private void Drop()
//    {
//        animal.GetDamaged(damage);
//        //tower.CurrentUsagesAmount -= 1;
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Animal"))
//        {
//            animal = other.GetComponent<Animal>();
//            animalDetected = true;
//        }
//    }

//    private void DestroyAmmo()
//    {
//        if (hitEffect != null)
//            Instantiate(hitEffect, transform.position, Quaternion.identity);
//        Destroy(gameObject);
//    }


//}
