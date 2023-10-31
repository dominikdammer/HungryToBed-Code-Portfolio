using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlaceTower : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPlaceable
{
    private SceneChecks area;
    private TowerSlot slot;
    private GameObject towerPrefab;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rect;
    private Image image;

    private int slotID;
    private Vector2 slotAnchor;
    private string towerName;
    private Vector3 targetAnchorPos;
    private Vector2 originAnchorPos;
    private Color originColor;
    private float dragTransparancy = 0.6f;
    private GameObject towerToSpawn;
    private Vector3 finalPosition;

    /// <summary>
    /// Mouse event für position
    /// getrennt von tower
    /// sollte auf button liegen
    /// anzeigen wo gebaut werden darf und wo nicht
    /// tower darf nicht auf path oder in andere tower ragen (instantiate ein pivot zuerst und check distance zu path und tower?)
    /// evtl zonen einfärben, die zeigen wo gebaut werden kann
    /// </summary>


    public int TowerSlot { get { return slotID; } }

    public RectTransform TowerAnker
    {
        get { return rect; }
        set { rect = value; }
    }
    public Vector3 TargetAnchorPos { get { return targetAnchorPos; } }
    public Vector2 OriginAnchorPos { get { return originAnchorPos; } }

    public Color OriginColor
    {
        get { return originColor; }
        set { originColor = value; }
    }

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
        image = GetComponentInParent<Image>();
        area = FindObjectOfType<SceneChecks>();

        canvasGroup = GetComponent<CanvasGroup>();
        //change with actual name setter script variable
        //towerName = GetComponentInChildren<Text>().text;

        originColor = image.color;

        //GetNewTowerPosition(pos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        slot = GetComponentInParent<TowerSlot>();
        slotID = slot.GetInstanceID();
        slotAnchor = slot.GetComponent<RectTransform>().anchoredPosition;
        originAnchorPos = rect.localPosition;
        canvasGroup.alpha = dragTransparancy;
        canvasGroup.blocksRaycasts = false;      
    }

    /// <summary>
    /// makes sure image rect stays on mouse pos
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;       

        if (area._IsInsideSpawnArea())
        {
            canvasGroup.alpha = 0.9f;
            image.color = new Color(0.3f,0.8f,0.3f);
        }
        else
        {
            canvasGroup.alpha = dragTransparancy;
            image.color = new Color(0.8f, 0.3f, 0.3f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            slotID = 0;

            /// <summary>
            /// on mouse button release tower is instantiated on dragposition and ui image is resetted, if valid
            /// </summary>
            if (area._IsInsideSpawnArea() /*&& !OverlappingTower()*/)
            {
                PlaceNewTower(); 
            }

            rect.transform.localPosition = originAnchorPos;
            image.color = originColor;
        }
    }

    public void PlaceNewTower()
    {
        var cost = slot.Tower.cost;
        if (PlayerPrefs.GetInt("Wealth") >= cost)
        {
            towerPrefab = slot.TowerPrefab;
            finalPosition = Helpers.GetMousePosition() + new Vector3(0, 0, 10);
            Instantiate(towerPrefab, finalPosition, Quaternion.identity);
            SaveData.DecreaseWealth(cost);
        }
    }
}
