using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
public class UIHelper : MonoBehaviour
{
    [SerializeField]
    private SceneChecks sceneChecks;

    [SerializeField]
    private TMP_Text text1;
    [SerializeField]
    private TMP_Text text2;
    [SerializeField]
    private TMP_Text text3;
    [SerializeField]
    private TMP_Text text4;

    private void Start()
    {
        sceneChecks = FindObjectOfType<SceneChecks>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Helpers.IsOverUI())
        {
            text2.color = Color.white;
            text2.text = "is over UI";
        }
        else
        {
            text2.color = Color.red;
            text2.text = "NOT over UI";
        }

        //text2.text = "MousePos " + Input.mousePosition;
        text1.text = "MousePos " + Helpers.GetMousePosition();
        //text3.text = "MousePos " + Camera.main.ScreenPointToRay(Input.mousePosition);

        if (sceneChecks._IsOverTower())
        {
            text3.color = Color.green;
            text3.text = "over Tower";
        }       
        else    
        {       
            text3.color = Color.red;
            text3.text = "NOT over Tower";
        }


        if (sceneChecks._IsInsideSpawnArea())
        {
            text4.color = Color.green;
            text4.text = "inside SpawnArea";
        }       
        else    
        {       
            text4.color = Color.red;
            text4.text = "NOT inside SpawnArea";
        }
    }
}
#endif