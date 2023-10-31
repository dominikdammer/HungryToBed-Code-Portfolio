using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    //private static DebugHelper instance;
    public GameObject point;
    private SpriteRenderer spriteRenderer;


    public void SpawnDebugPoint(Vector3 pos,Color color)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
        Instantiate(point, pos, Quaternion.identity);
    }
}
