using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerListForLevel : MonoBehaviour
{
    private LevelSettings _levelOptions;

    [SerializeField]
    private List<Button> towerButtons;
    
    [SerializeField]
    private List<BaseTower> towersSO;

    private List<Sprite> towerImages;
    private List<string> towerNames;
    private List<string> towerCosts;

    private Sprite image;
    private string nameUI;
    private string costUI;

    private string actualTowerName;
    private int actualTowerCost;


    /// <summary>
    /// towerButtons is manually filled... 
    /// TODO: fill towerButtons automatically....
    /// </summary>

    private void Awake()
    {
        //works
        GrabTowersFromSettings();

        //works not
        //FillComponentsLists();

        //apply towers set for level to UI
        foreach (var SO in towersSO)
        {
            for (int i = 0; i < towerButtons.Count - 1; i++)
            {
                image = SO.towerSprite;
                //nameUI = SO._name;
                //costUI = SO.cost.ToString();
            }
        }
    }

    private void GrabTowersFromSettings()
    {
        if (_levelOptions == null)
        {
            _levelOptions = FindObjectOfType<LevelSettings>();
        }

        foreach (var item in _levelOptions.availableTowers)
        {
            towersSO.Add(item);
        }
    }

    private void FillComponentsLists()
    {
        foreach (var item in towerButtons)
        {
            image = item.GetComponent<TowerSlot>().GetComponentInChildren<Image>().sprite;
            //nameUI = GetChildComponentByName<TextMeshPro>("TowerImage").text;
            //costUI = GetChildComponentByName<TextMeshPro>("Cost").text;


            towerImages.Add(image);
            //towerNames.Add(nameUI);
            //towerCosts.Add(costUI);
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
