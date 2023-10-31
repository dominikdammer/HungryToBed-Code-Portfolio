using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChecks : MonoBehaviour
{
    /// ---- Variables --- ///

    // Bit shift the index of the layers to get a bit mask
    int towerLayer = 1 << 10;
    int spawnAreaLayer = 1 << 7;

    /// ---- Methods --- ///

    #region OverlappingTower

    /// <summary>
    /// Check for other Towers in the area
    /// </summary>
    /// <returns></returns>
    public bool OverlappingTower()
    {
        // Bit shift the index of the layer (10) to get a bit mask

        Collider2D[] collider2DArray = 
            Physics2D.OverlapAreaAll(Helpers.GetMousePosition(), 
            Helpers.GetMousePosition() + new Vector3(0.3f, 0.3f, 0.3f), towerLayer);

        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D != null)
            {
                return true;
            }
        }

        return false;
    }
    #endregion

    #region IsInsideSpawnArea

    /// <summary>
    /// Checks if selection overlaps with spawnarea
    /// </summary>
    /// <returns></returns>
    public bool _IsInsideSpawnArea()
    {
        if (Physics2D.Raycast(Helpers.GetMousePosition(),
            transform.TransformDirection(Vector3.forward), Mathf.Infinity, spawnAreaLayer))
        {
            return true;
        }

        return false;
    }
    #endregion

    #region IsOverTower

    /// <summary>
    /// Checks if Mouse is over a Tower
    /// </summary>
    public bool _IsOverTower()
    {
        RaycastHit2D hit = Physics2D.Raycast(Helpers.GetMousePosition(),
            transform.TransformDirection(Vector3.forward), Mathf.Infinity, towerLayer);

        if (hit)
        {
            return true;
        }

        return false;
    }

    #endregion
}
