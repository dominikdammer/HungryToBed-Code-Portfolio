using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private BaseTower towerSO;

    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI nameUI;
    [SerializeField]
    private TextMeshProUGUI costUI;

    private string actualTowerName;
    private int actualTowerCost;

    private PlaceTower placeTower;
    private Vector2 originalSlotPos;

    public GameObject TowerPrefab { get { return towerPrefab; } }
    public BaseTower Tower { get { return towerSO; } }

    private void Awake()
    {
        placeTower = GetComponentInChildren<PlaceTower>();


        if(towerSO != null)
        {
            image = GetChildComponentByName<Image>("TowerImage");
            nameUI = GetChildComponentByName<TextMeshProUGUI>("Name");
            costUI = GetChildComponentByName<TextMeshProUGUI>("Cost");

            image.sprite = towerSO.towerSprite;
            nameUI.text = towerSO._name;
            costUI.text = towerSO.cost.ToString();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            if (Helpers.IsOverUI())
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            }
        }
    }

    private T GetChildComponentByName<T>(string name) where T : Component
    {
        foreach (T component in GetComponentsInChildren<T>(true))
        {
            if (component.gameObject.name == name)
            {
                return component;
            }
        }
        return null;
    }

}
