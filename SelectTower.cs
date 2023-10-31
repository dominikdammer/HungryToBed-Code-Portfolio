using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTower : MonoBehaviour
{
    private SceneChecks sceneChecks;
    private GameObject selectionCircle;
    private GameObject selectedTower;
    private GameObject lastSelectedTower;
    private ShowTowerRange circleIndicator;
    private int towerLayer = 1 << 10;

    [SerializeField]
    private GameObject Prefab;
    private GameObject lastCircle;

    public GameObject SelectedTower
    {
        get { return lastSelectedTower; }
    }

    private void Start()
    {
        sceneChecks = FindObjectOfType<SceneChecks>();
    }

    // Update is called once per frame
    void Update()
    {

        ///Checks
        if (!sceneChecks._IsOverTower())
        {
            return;
        }
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        RaycastSelection();

        if (selectionCircle == null)
        {
            if (selectedTower != lastSelectedTower)
            {
                Deselect(lastSelectedTower);
                Select();
            }
            else
            {
                Select();
            }
        }
        else
        {
            Deselect(lastSelectedTower);
        }
    }

    private void RaycastSelection()
    {
        RaycastHit2D hit = Physics2D.Raycast(Helpers.GetMousePosition(),
                            transform.TransformDirection(Vector3.forward), Mathf.Infinity, towerLayer);

        if (hit)
        {
            selectedTower = hit.transform.root.gameObject;
        }
        else { return; }
    }

    private void Select()
    {
        var root = selectedTower.transform.Find("Root");
        if (root == null)
        {
            Debug.Log("Root not found. Add one to the Tower prefab");
        }

        if (lastCircle == null)
        {
            selectionCircle = Instantiate(Prefab, root);
            //selectionCircle.transform.localScale = new Vector3(circleIndicator.CircleSize.x, circleIndicator.CircleSize.y, 0);
            lastCircle = selectionCircle;
        }

        lastSelectedTower = selectedTower;
    }

    private void Deselect(GameObject lastTower)
    {
        if (lastTower == null)
            return;

        var circle = lastTower.GetComponentInChildren<SelectionCircle>().gameObject;
        Destroy(circle);
        lastSelectedTower = null;
    }

}
