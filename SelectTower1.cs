//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SelectTower1 : MonoBehaviour
//{
//    private int towerLayer = 1 << 10;
//    private SceneChecks sceneChecks;
//    private GameObject selectedTower;
//    private GameObject lastSelectedTower;
//    [SerializeField]
//    private ShowTowerRange rangeIndicator;

//    public GameObject SelectedTower
//    {
//        get { return lastSelectedTower; }
//    }

//    private void Start()
//    {
//        sceneChecks = FindObjectOfType<SceneChecks>();
//    }


//    // Update is called once per frame
//    void Update()
//    {
//        ///Checks
//        if (!sceneChecks._IsOverTower())
//        {
//            return;
//        }
//        if (!Input.GetMouseButtonDown(0))
//        {
//            return;
//        }

//        RaycastSelection();

//        //Debug.Log("Selected Tower: " + selectedTower.GetInstanceID() + " " + "LastSelected Tower: " + lastSelectedTower.GetInstanceID());
//        if (!rangeIndicator.CircleIsEnabled())
//        {
//            if (selectedTower != lastSelectedTower)
//            {

//                Deselect(lastSelectedTower);
                
//                Select();
//            }
//            else
//            {
//                Select();
//            }
//        }
//        else
//        {
//            Deselect(lastSelectedTower);
//        }

        
//    }

//    private void Select()
//    {
//        rangeIndicator.EnableCircle();
  
//        lastSelectedTower = selectedTower;

//        Debug.Log("select");
//    }

//    private void Deselect(GameObject lastTower)
//    {
//        if (lastTower == null)
//               return;

//        rangeIndicator.DisableCircle();
//        lastSelectedTower = null;
//        Debug.Log("deselect");
//    }


//    private void RaycastSelection()
//    {
//        RaycastHit2D hit = Physics2D.Raycast(Helpers.GetMousePosition(),
//                            transform.TransformDirection(Vector3.forward), Mathf.Infinity, towerLayer);

//        if (hit)
//        {
//            selectedTower = hit.transform.root.gameObject;
//            rangeIndicator = selectedTower.GetComponentInChildren<ShowTowerRange>();
//        }
//        else { return; }
//    }

//}

