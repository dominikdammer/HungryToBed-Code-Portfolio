using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTowerRange : MonoBehaviour
{
    [SerializeField]
    private Tower tower;
    [SerializeField]
    private Transform aimer;
    private SpriteRenderer visualCircle;

    private Vector2 size;
    [SerializeField, Range(1f, 10f)]
    private float sizeChange;

    /// range.radius of the circle collider
    /// base tower gives initial value
    /// should be readonly
    [ReadOnly]
    private float getRange;
    private float setRange;

    public Vector2 CircleSize { get { return size; } }

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();


        //if (CircleIsEnabled())
        //{
        //    DisableCircle();
        //}

        CircleIsEnabled();

        if (sizeChange <= 1)
        {
            sizeChange = 1;
        }
    }

    private void GetComponents()
    {
        tower = GetComponentInParent<Tower>();
        visualCircle = GetComponent<SpriteRenderer>();
        getRange = tower.Range.radius;
        transform.position = tower.transform.position;
        size = transform.localScale;
    }

    private void Update()
    {
        UpdateRange();
    }

    private void UpdateRange()
    {
        //has to be double the size, to calcuate the diameter instead of radius
        size = new Vector2((getRange * 2) * sizeChange, (getRange * 2) * sizeChange);
        transform.localScale = size;
        setRange = size.x / 2;

        tower.Range.radius = setRange;
    }

    public void EnableCircle()
    {
        visualCircle.enabled = true;
        CircleIsEnabled();
    }
    public void DisableCircle()
    {
        visualCircle.enabled = false;
        CircleIsEnabled();
    }

    public bool CircleIsEnabled()
    {
        if (visualCircle.enabled)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
