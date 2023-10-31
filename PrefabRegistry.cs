using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRegistry : ScriptableObject
{
    private static PrefabRegistry instance;

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    public static void LoadInstance()
    {
        string guid = "ae9a323488255d04cafed1dd8e910ec1"; // Found in .meta file of asset.
        string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
        instance = UnityEditor.AssetDatabase.LoadAssetAtPath<PrefabRegistry>(assetPath);
    }
#else
    private void OnEnable() {
        instance = this;
    }
#endif


    [SerializeField]
    private GameObject baseAnimal;
    public GameObject BaseAnimal => instance.baseAnimal;
    
    [SerializeField]
    private GameObject baseTower;
    public GameObject BaseTower => instance.baseTower;
}
