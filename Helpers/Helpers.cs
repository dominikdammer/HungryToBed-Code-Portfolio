using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

/// <summary>
/// https://www.youtube.com/watch?v=JOABOQMurZo
/// </summary>
public static class Helpers
{
    /*----------- GENERAL HELPERS -----------*/


    private static Camera _camera;
    /// <summary>
    /// reference to camera
    /// </summary>
    public static Camera Camera
    {
        get
        {
            if( _camera == null ) _camera = Camera.main;
            return _camera;
        }
    }


    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    /// <summary>
    /// non-allocating waitforseconds
    /// </summary>
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    /// <summary>
    /// detects if mouse if over ui at all times
    /// </summary>
    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    /// <summary>
    /// helps spawning following objects and effect on ui elements in world space
    /// </summary>
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    /// <summary>
    /// destroys all children!
    /// </summary>
    public static void DeleteChildren(this Transform t)
    {
        foreach(Transform child in t) Object.Destroy(child.gameObject);
    }


    private static PointerEventData hoverPos;
    private static BaseEventData data;
    private static Vector3 clickedPos;
    private static Vector3 mouseAtWorldPosition;
    /// <summary>
    /// Get Mouse pos at world pos
    /// </summary>
    public static Vector3 GetMousePosition()
    {
        data = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        hoverPos = (PointerEventData)data;

        //hoverPos.z = 10f;
        mouseAtWorldPosition = Camera.ScreenToWorldPoint(hoverPos.position);

        return mouseAtWorldPosition;
    }
    /// <summary>
    /// Get mouse click pos at world pos
    /// </summary>
    public static Vector3 GetClickedPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedPos = mouseAtWorldPosition;
        }
        return clickedPos;
    }


    /// <summary>
    /// returns a pooled object to the pool after a certain time
    /// </summary>
    public static IEnumerator<GameObject> ReturnToPoolAfterTimer(ObjectPool<GameObject> _pool, GameObject obj, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _pool.Release(obj);
    }

    /*----------- LOCAL HELPERS -----------*/

    ///helps getting components by name, but needs monobehaviour

    //private T GetChildComponentByName<T>(string name) where T : Component
    //{
    //    foreach (T component in GetComponentsInChildren<T>(true))
    //    {
    //        if (component.gameObject.name == name)
    //        {
    //            return component;
    //        }
    //    }
    //    return null;
    //}



}
