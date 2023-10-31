using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellTower : MonoBehaviour
{
    [SerializeField]
    private SelectTower selection;
    private Image button;

    private Color usualColor;

    // Start is called before the first frame update
    void Start()
    {
        if(selection == null)
            selection = FindObjectOfType<SelectTower>();
        if (button == null)
            button = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ButtonColor();
    }

    private void ButtonColor()
    {
        if (selection.SelectedTower != null)
        {
            usualColor = button.color;
            usualColor.a = 1;
        }
        else
        {
            usualColor.a = 0.4f;
        }
        button.color = usualColor;
    }

    public void Sell()
    {
        var tower = selection.SelectedTower;

        if (tower == null)
        {
            return;
        }

        var cost = tower.GetComponent<Tower>().tower.cost;
        var sellValue = cost / 2;

        SaveData.IncreaseWealth(sellValue);

        Destroy(tower.gameObject);

    }

}
