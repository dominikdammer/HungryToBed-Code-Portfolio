using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Tower : MonoBehaviour
{
    #region Variables
    public BaseTower tower;
    private Animal curAnimal;
    private PathCreation.PathCreator vPath;

    #region enums
    public enum TowerTargetPriority
    {
        First,
        Close,
        Strong,
        None
    }

    public enum TowerType
    {
        SmallDispenser,
        LargeDispenser,
        SlowVine
    }

    public enum AttackType
    {
        Shoot,
        DropAmmo,
        None
    }
    #endregion enums

    [Header("Info")]
    public AudioClip defaultTowerSound;
    public AudioClip towerSound;
    public AudioSource audioSource;
    public bool rotateTowardsTarget;
    public ReloadBar reloadBar;
    public TowerTargetPriority targetPriority;
    public TowerType towertype;
    [SerializeField, ReadOnly, ExecuteInEditMode, Tooltip("To change firing speed use SO")]
    private float _timeToShoot;

    [Header("Attacking")]
    public GameObject ammo;
    public Transform ammoSpawnPos;
    public float ammoSpeed = 1f;

    [SerializeField]
    public List<Animal> curAnimalsInRange = new List<Animal>();

    //PRIVATES

    private Animator animator;
    private BoxCollider2D towerBound;
    private CircleCollider2D range;
    private Dictionary<Vector3, float> vertexValues = new Dictionary<Vector3, float>();
    private float effectAmount;
    private float effectDuration;
    private float lastAttackTime;
    private float nextTimeToShoot;
    private float value;
    private int ammoDamage;
    private int currentUsagesAmount;
    //private int targetUsagesAmount = 3;
    [SerializeField]
    [BoxGroup]
    private List<Vector3> validPoints = new List<Vector3>();
    private SpriteRenderer sprite;
    private Transform ammoSortingObject;
    private Vector3 key;
    private Vector3[] points;
    private Vector3 usedPos;

    SquashAndStretch squash;


    #endregion

    #region Accessibles

    public Animal CurAnimal 
    {
        get{return curAnimal;}
    }
    public Animal _GetAnimal
    {
        get{return GetAnimal();}
    }
    public PathCreation.PathCreator VPath
    {
        get{return vPath;}
    }
    public float FireRate 
    { 
        get { return tower.shootInterval; } 
    }
    public CircleCollider2D Range 
    { 
        get { return range; } 
    }
    public int AmmoDamage
    {
        get{return ammoDamage;}
        set{ammoDamage = value;}
    }
    public float EffectAmount
    {
        get { return effectAmount; }
        set { effectAmount = value; }
    }    
    public float EffectDuration
    {
        get { return effectDuration; }
        set { effectDuration = value; }
    }
    //public int TargetUsagesAmount
    //{
    //    get{return targetUsagesAmount;}
    //    set{targetUsagesAmount = value;}
    //}
    public int CurrentUsagesAmount
    {
        get { return currentUsagesAmount; }
        set { currentUsagesAmount = value; }
    }
    public float LastAttackTime
    {
        get{return lastAttackTime;}
        set{lastAttackTime = value; }
    }
    public float NextTimeToShoot
    {
        get{return nextTimeToShoot;}
        set{nextTimeToShoot = value;}
    }
    public SpriteRenderer Sprite
    {
        get{return sprite;}
        set{sprite = new SpriteRenderer();}
    }
    public BoxCollider2D TowerBound
    {
        get{return towerBound;}
        set{towerBound = new BoxCollider2D();}
    }
    public Animator _Animator
    {
        get {return animator;}
        set { animator = new Animator();}
    }

    #endregion Accessibles

    #region AWAKE
    private void Awake()
    {
        GetComponents();
        UpdateSprite();
        FillVertexDict();
        //DEBUGREDDOTS();
        //OnDrawGizmos();

        _timeToShoot = tower.shootInterval;
        ammo = tower.ammo;
        ammoDamage = tower.dmg;
        ammoSortingObject = transform;
        effectAmount = tower.effectAmount;
        effectDuration = tower.effectDuration;
        reloadBar.UpdateBar(FireRate, LastAttackTime);    
        squash = GetComponent<SquashAndStretch>();
        usedPos = Vector3.zero;
    }

    private void GetComponents()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        range = GetComponentInChildren<CircleCollider2D>();
        towerBound = GetComponentInChildren<BoxCollider2D>();
        vPath = FindObjectOfType<PathCreation.PathCreator>();
    }

[ExecuteInEditMode]
    private void UpdateSprite()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sprite = tower.towerSprite;
        sprite.gameObject.transform.localScale = tower.towerSize;
    }

    private void FillVertexDict()
    {
        points = VPath.path.localPoints;
        float[] distances = new float[points.Length];
        Vector3[] pointPos = new Vector3[points.Length];
       
        ///<summary> save pos and distance into dictionary </summary>
        /// Calculate the distance between each point and the current point.
        /// save each points vector3

        for (int i = 0; i < points.Length; i++)
        {
            distances[i] = Vector3.Distance(points[i], transform.position);

            pointPos[i] = points[i];
                
            if (vertexValues.Count != points.Length - 1)
            {                          
                vertexValues.Add(pointPos[i], distances[i]);
            }        
        }
    }

    #endregion AWAKE

    public void PlaySound()
    {
        if (audioSource == null)
            return;

        //% chance to play sound
        float soundRandomizer = UnityEngine.Random.value;

        if (soundRandomizer < 0.8f)
        {
            towerSound = tower.sound;
            audioSource.clip = towerSound;
        }
        else
        {
            audioSource.clip = defaultTowerSound;
        }

        audioSource.Play();
        audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        audioSource.volume = UnityEngine.Random.Range(0.85f, 1.05f);
    }




    #region Attack
    //------------------------------ ATTACK LOGIC ------------------------------//

    // returns the current enemy for the tower to attack
    Animal GetAnimal()
    {
        curAnimalsInRange.RemoveAll(x => x == null);

        if (curAnimalsInRange.Count == 0)
            return null;
        if (curAnimalsInRange.Count == 1)
            return curAnimalsInRange[0];

        switch (targetPriority)
        {
            case TowerTargetPriority.First:
                {
                    return curAnimalsInRange[0];
                }
            case TowerTargetPriority.Close:
                {
                    Animal closest = null;
                    float dist = 99;
                    for (int x = 0; x < curAnimalsInRange.Count; x++)
                    {
                        float d = (transform.position - curAnimalsInRange[x].transform.position).sqrMagnitude;
                        if (d < dist)
                        {
                            closest = curAnimalsInRange[x];
                            dist = d;
                        }
                    }
                    return closest;
                }
            case TowerTargetPriority.Strong:
                {
                    Animal strongest = null;
                    int strongestHealth = 0;
                    foreach (Animal animal in curAnimalsInRange)
                    {
                        if (animal.animalSO.health > strongestHealth)
                        {
                            strongest = animal;
                            strongestHealth = animal.animalSO.health;
                        }
                    }
                    return strongest;
                }
            case TowerTargetPriority.None:
                {
                    return null;
                }
        }
        return null;
    }


    private void TowerChoice(TowerType towerType)
    {
        switch (towerType)
        {
            case TowerType.SmallDispenser:
                towerType = TowerType.SmallDispenser;
                break;
            case TowerType.LargeDispenser:
                towerType = TowerType.LargeDispenser;
                break;
            case TowerType.SlowVine:
                towerType = TowerType.SlowVine;
                break;
            default:
                break;
        }
    }

    private void HandleAttackType(AttackType _attackType, Animal targetAnimal)
    {
        switch (_attackType)
        {
            case AttackType.Shoot:
                AttackType_Shoot(targetAnimal);
                break;
            case AttackType.DropAmmo:
                AttackType_DropAmmo();
                break;
            case AttackType.None:
                break;
            default:
                break;
        }
    }
    //TODO: Beschränk anzahl an pellets....

    // attacks the targetAnimal if any
    public virtual void Attack(AttackType attackType, Animal targetAnimal, int targetUsage)
    {
        // Debug.Log(gameObject.name + " has " + attackType + " with " + targetUsage + " shoots and a speed of " +ammoSpeed);

        // attack every "attackRate" seconds
        //TOWERS WITH ONLY ONE SHOT HAVE NO LASTATTACKTIME!!
        //TODO:change reset time on spawn. not possible with time.time?


        //ja also.. attack wird nun von einem event und nicht alle frames gecallt,
        //damit hat der timer der reloadbar aber ein problem...

        PlaySound();
        HandleAttackType(attackType, targetAnimal);
        squash.PlaySquashAndStretch();
        CurrentUsagesAmount++;
    }


    #region AttackTypes
    public virtual void AttackType_Shoot(Animal _curAnimal)
    {
        Debug.Log("WHY IS THIS ACTIVE?");
        range.radius = tower.range;
        nextTimeToShoot = Time.time + _timeToShoot;

        var sortingObject = new GameObject("Ammo").transform;
        if (sortingObject != null)
        {
            ammoSortingObject = sortingObject;
        }

        if (rotateTowardsTarget)
        {
            transform.LookAt(curAnimal.transform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

        //TODO: check all possibilities for ammo behaviour. either shoot or drop....
        GameObject proj = Instantiate(ammo, ammoSpawnPos.position, Quaternion.identity, ammoSortingObject);
        proj.GetComponent<Projectile>().Initialize(_curAnimal, ammoDamage, ammoSpeed);

    }

    public virtual void AttackType_DropAmmo()
    {
        range.radius = tower.range;
        nextTimeToShoot = Time.time + _timeToShoot;
        
        //// currently no rotation planned
        //if (rotateTowardsTarget)
        //{
        //    transform.LookAt(curAnimal.transform);
        //    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        //}

        int randomIndex = CheckForValidVertexPositions();

        GameObject ammo = Instantiate(this.ammo, ammoSpawnPos.position, Quaternion.identity, ammoSortingObject);
        var sendingTower = gameObject.GetComponentInParent<Tower>();
        float offset = 0.2f;

        if (towertype == TowerType.SmallDispenser)
        {
            ammo.GetComponent<Pellets>().InitializeDmgAmmo(validPoints[randomIndex]
                    + new Vector3(UnityEngine.Random.Range(-offset, offset), UnityEngine.Random.Range(-offset, offset)),
                    ammoDamage, ammoSpeed, sendingTower);
        }
        else if (towertype == TowerType.SlowVine)
        {
            ///has no random range, because it would stack to shoot more than one...
            ammo.GetComponent<SproutBomb>().InitializeEffectAmmo(validPoints[randomIndex]
                    , effectAmount, effectDuration, ammoSpeed, sendingTower);
        }

    }

    private int CheckForValidVertexPositions()
    {
        ///checks if list of valid points in range is empty or not
        //ATTENTION: REMEMBER TO CLEAR(UPDATE) LIST ONCE RANGE.RADIUS CHANGES ON PLAY
        if (validPoints.Count == 0)
        {
            validPoints = IfPathPointIsValid();
        }

        var randomIndex = UnityEngine.Random.Range(0, validPoints.Count);

        ///compares vector3 pos of point with previously used pos
        //TODO: this means every other can still stack

        if (validPoints[randomIndex] != usedPos)
        {
            usedPos = validPoints[randomIndex];
        }
        else
        {
            if (randomIndex != validPoints.Count)
            {
                randomIndex++;
            }
            else
            {
                randomIndex--;
            }
        }
        return randomIndex;
    }
    #endregion AttackTypes


    //------------------------------ ATTACK LOGIC END ------------------------------//
    #endregion attack



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Animal"))
        {
            curAnimalsInRange.Add(other.GetComponent<Animal>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Animal"))
        {
            curAnimalsInRange.Remove(other.GetComponent<Animal>());
        }
    }

    private List<Vector3> IfPathPointIsValid()
    {
        validPoints = new List<Vector3>();

        foreach (KeyValuePair<Vector3, float> keyValue in vertexValues)
        {
            key = keyValue.Key;
            value = keyValue.Value;

            ///checks if distance to tower is smaller than tower range and adds valid point to list
            if (value < range.radius)
            {
                validPoints.Add(key);
            }
        }

        return validPoints;
    }


    //DEBUG
    private void OnDrawGizmos()
    {
        foreach (KeyValuePair<Vector3, float> keyValue in vertexValues)
        {
            key = keyValue.Key;
            value = keyValue.Value;
            Gizmos.DrawIcon(key, null, true, Color.blue);

            ///checks if distance to tower is smaller than tower range and adds valid point to list
            if (value < range.radius)
            {
                Gizmos.DrawIcon(key, null, true, Color.red);
            }
        }
    }




}
