using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    [SerializeField]
    private Image fill;
    [SerializeField]
    private Image background;

    public Image Fill 
    { 
        get { return fill; }
        set { fill = value; }   
    }

    private void Awake()
    {
        fill.fillAmount = Random.Range(0, 0.1f);
    }

    public void UpdateBar(float maxFill, float currentFill)
    {
        fill.fillAmount = Mathf.Abs(currentFill) / maxFill;
    }
}
