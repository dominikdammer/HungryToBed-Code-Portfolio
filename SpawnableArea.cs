using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// should someday automade spawnarea creation....
/// </summary>
[ExecuteInEditMode]
public class SpawnableArea : MonoBehaviour
{
    [SerializeField]
    private GameObject map;
    [SerializeField]
    private GameObject pathObject;
    [SerializeField]
    private GameObject pathMesh;

    public bool isValidPos = false;
    public bool isValidNeg = false;
    public float targetSize = 5;

    private BoxCollider2D mapCollider;

    PathCreation.Examples.RoadMeshCreator mesh;

    private List<Vector3> validPoints = new List<Vector3>();
    private Mesh validArea;


    private void Awake()
    {
        mapCollider = map.GetComponent<BoxCollider2D>();
        CalculateSpawnArea();
    }


    //berechne distanz zwischen pathvertex und ziel und zwischen pathbreite (beide seiten) und ziel
    //das ergebnis muss größer als die erste und kleiner als die zweite rechnung sein.
    private void CalculateSpawnArea()
    {
        var path = pathObject.GetComponent<PathCreation.PathCreator>();
        mesh = pathObject.GetComponent<PathCreation.Examples.RoadMeshCreator>();

        Vector3[] vertecies = path.path.localPoints;
        Vector3 sideA = mesh.GetVertSideA;
        Vector3 sideB = mesh.GetVertSideB;
        float roadEdgeToTargetPositive;
        float roadEdgeToTargetNegative;
        float roadWidthPositive;
        float roadWidthNegative;
        float totalPos;
        float totalNeg;
       

        ///<summary>
        /// F < Cp && F >= Bp || F > Cn && F <= Bn
        /// </summary>

        for (int i = 0; i < vertecies.Length; i++)
        {
            //Ap
            roadWidthPositive = Vector3.Distance(vertecies[i], sideA);
            //An
            roadWidthNegative = roadWidthPositive * -1;       

            //Bp
            roadEdgeToTargetPositive = roadWidthPositive + targetSize;
            //Bn
            roadEdgeToTargetNegative = roadWidthPositive * -1;

            //Cp
            totalPos = roadWidthPositive + roadEdgeToTargetPositive;
            //Cn
            totalNeg = totalPos * -1;


            if(targetSize < totalPos && targetSize >= roadEdgeToTargetPositive)
            {
                validPoints.Add(new Vector3(vertecies[i].x,totalPos,0));
                Debug.Log("added pos points");
                isValidPos = true;        
            }

            if (targetSize > totalNeg && targetSize <= roadEdgeToTargetNegative)
            {
                validPoints.Add(new Vector3(vertecies[i].x, totalNeg, 0));
                Debug.Log("added neg points");
                isValidNeg = true;
            }

            validArea = new Mesh();
            validArea.SetVertices(validPoints);
            validArea.RecalculateBounds();
            validArea.RecalculateNormals();
            validArea.RecalculateTangents();
        }
    }

    //private void OnMouseOver()
    //{
    //    isOverSpawnArea = true;
    //}



    private void OnDrawGizmos()
    {
        var path = pathObject.GetComponent<PathCreation.PathCreator>();
        Vector3 mapSize = map.GetComponent<SpriteRenderer>().bounds.size;

        var mapBounds = map.GetComponent<SpriteRenderer>().bounds;
        var pathBounds = path.path.bounds;


        for (int i = 0; i < validPoints.Count; i++)
        {
            Gizmos.color = new Color(0, 1, 1, 0.3f);
            Gizmos.DrawCube(validPoints[i], new Vector3(1,1,0));
        }

        if (isValidPos && isValidNeg)
        {           
            Gizmos.color = new Color(1, 0, 1, 0.3f);
            Gizmos.DrawMesh(validArea, pathObject.transform.position);
        }

        //Gizmos.color = new Color(0, 0, 1, 0.7f);
        //if(validArea != null)
        //{
        //    Debug.Log("valid are is not null and has :" + validArea.vertexCount + " of vertecies");
        //   Gizmos.DrawMesh(validArea,Vector3.zero);
        //}
    }
}
