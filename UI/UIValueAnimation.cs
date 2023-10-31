using UnityEngine;
using Sirenix.OdinInspector;
using System;
using TMPro;
using System.Collections;
using UnityEngine.Pool;

public class UIValueAnimation : MonoBehaviour
{
    PlayerData playerData;
    [SerializeField]
    [BoxGroup("Buttons")]
    RectTransform waveButton, hpButton, moneyButton;

    [SerializeField]
    [BoxGroup("Images")]
    RectTransform waveImage, hpImage, moneyImage;

    [SerializeField]
    [BoxGroup("ChangeText")]
    private GameObject uIPopUP;

    private TMP_Text popUpText;

    private Vector3 hpImageScale;
    private Vector3 moneyImageScale;
    private Canvas canvas;

    private ObjectPool<GameObject> _pool;

    #region Pooling
    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(CreateUIPopUp, Reuse, OnPutBackInPool, defaultCapacity: 5, maxSize:20);
    }

    private GameObject CreateUIPopUp()
    {
        var popUp = Instantiate(uIPopUP, transform.position, Quaternion.identity, canvas.transform);
        return popUp;
    }

    private void Reuse(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnPutBackInPool(GameObject obj)
    {
        obj.SetActive(false);
    }
    #endregion Pooling

    private void Start()
    {
        GetComponents();

        playerData = GetComponent<PlayerData>();
        hpImageScale = hpImage.localScale;
        moneyImageScale = moneyImage.localScale;
        canvas = FindObjectOfType<Canvas>();

        ConstantAnimations();
    }

    private void GetComponents()
    {
        if (waveButton == null)
        {
            waveButton = GetChildComponentByName<RectTransform>("Wave");
        }
        if (hpButton == null)
        {
            hpButton = GetChildComponentByName<RectTransform>("HP");
        }
        if (moneyButton == null)
        {
            moneyButton = GetChildComponentByName<RectTransform>("Money");
        }
        if (waveImage == null)
        {
            waveImage = GetChildComponentByName<RectTransform>("WaveImage");
        }
        if (hpImage == null)
        {
            hpImage = GetChildComponentByName<RectTransform>("HPImage");
        }
        if (moneyImage == null)
        {
            moneyImage = GetChildComponentByName<RectTransform>("MoneyImage");
        }
    }

    private void ConstantAnimations()
    {
        var time = 3;
        var delay = 1;
        #region HEART
        LeanTween.scale(hpImage, hpImageScale + new Vector3(0.1f, 0.1f, 0.1f), time).setDelay(delay).setEase(LeanTweenType.easeOutElastic).setLoopPingPong(-1);
        LeanTween.alpha(hpImage,0.5f, time).setDelay(delay).setLoopPingPong(-1);
        #endregion
    }

    private void OnHPChange()
    {
        var time = 2;

        LeanTween.alpha(hpImage,1,0.4f).setLoopPingPong(3);

        //------------- POP UP BEHAVIOUR -------------

        ///instantiate and set it to canvas (important)
        //var hpChangeText = Instantiate(uIPopUP, transform.position, Quaternion.identity, canvas.transform);
        var hpChangeText = _pool.Get();

        //calculate local spawnposition (this is crucked but needed)
        Vector3 resourceInfoPos = hpImage.transform.parent.parent.localPosition;
        Vector3 moneyImagePos = hpImage.transform.parent.localPosition;
        Vector3 spawnPos = resourceInfoPos + moneyImagePos;
        Vector3 offset = new Vector3(0, -10f, 0);
        Vector3 targetPos = new Vector3(spawnPos.x, spawnPos.y - 100, spawnPos.z);

        //set position of text
        RectTransform hpChangeTextTransform = hpChangeText.GetComponent<RectTransform>();
        Vector3 hpChangeTextPosition = spawnPos;
        hpChangeTextTransform.localPosition = hpChangeTextPosition;

        ///animation and behaviour
        var canvasGroup = hpChangeText.GetComponent<CanvasGroup>();

        LeanTween.move(hpChangeTextTransform, targetPos, time).setEase(LeanTweenType.easeOutSine);
        LeanTween.alphaCanvas(canvasGroup, 0.3f, time);

        ///returns object to pool
        StartCoroutine(Helpers.ReturnToPoolAfterTimer(_pool,hpChangeText,time));  
    }

    private void HpIncreased(int Amount)
    {
        var hpChangeText = uIPopUP;
        /// apply amount to text
        popUpText = hpChangeText.GetComponent<TMP_Text>();
        popUpText.text = "+" + Amount.ToString();
        popUpText.color = Color.yellow;

        OnHPChange();
    }

    private void HpDecreased(int Amount)
    {
        var hpChangeText = uIPopUP;
        /// apply amount to text
        popUpText = hpChangeText.GetComponent<TMP_Text>();
        popUpText.text = "-" + Amount.ToString();

        popUpText.color = Color.red;

        OnHPChange();
    }

    private void OnMoneyChange()
    {
        var time = 2;

        ///behaviour of image
        LeanTween.alpha(moneyImage, 1, 0.4f).setLoopPingPong(3);
        LeanTween.scale(moneyImage, moneyImageScale + new Vector3(0.3f, 0.3f, 0.3f), 2f);

        //------------- POP UP BEHAVIOUR -------------

        ///instantiate and set it to canvas (important)
        var moneyChangeText = Instantiate(uIPopUP, transform.position, Quaternion.identity, canvas.transform);

        //calculate local spawnposition (this is crucked but needed)
        Vector3 resourceInfoPos = moneyImage.transform.parent.parent.localPosition;
        Vector3 moneyImagePos = moneyImage.transform.parent.localPosition;
        Vector3 spawnPos = resourceInfoPos + moneyImagePos;
        Vector3 offset = new Vector3(0, -10f, 0);
        Vector3 targetPos = new Vector3(spawnPos.x, spawnPos.y - 100, spawnPos.z);

        //set position of text
        RectTransform moneyChangeTextTransform = moneyChangeText.GetComponent<RectTransform>();
        Vector3 moneyChangeTextPosition = spawnPos;
        moneyChangeTextTransform.localPosition = moneyChangeTextPosition;

        ///animation and behaviour
        var canvasGroup = moneyChangeText.GetComponent<CanvasGroup>();

        LeanTween.move(moneyChangeTextTransform, targetPos, time).setEase(LeanTweenType.easeOutSine);
        LeanTween.alphaCanvas(canvasGroup, 0.3f, time);

        Destroy(moneyChangeText, time);
    }

    private void MoneyIncreased(int Amount)
    {
        var moneyChangeText = uIPopUP;
        /// apply amount to text
        popUpText = moneyChangeText.GetComponent<TMP_Text>();
        popUpText.text = "+" + Amount.ToString();
        popUpText.color = Color.yellow;

        OnMoneyChange();
    }

    private void MoneyDecreased(int Amount)
    {
        var moneyChangeText = uIPopUP;
        /// apply amount to text
        popUpText = moneyChangeText.GetComponent<TMP_Text>();
        popUpText.text = "-" + Amount.ToString();
        popUpText.color = Color.red;

        OnMoneyChange();
    }

    private void OnNextWave()
    {
        LeanTween.rotate(waveImage, -360, 2).setLoopPingPong(1);
    }

    /// <summary>
    /// Events
    /// </summary>

    private void OnEnable()
    {
        SaveData.OnHpIncreased += HpIncreased;
        SaveData.OnHpDecreased += HpDecreased;

        SaveData.OnWealthIncreased += MoneyIncreased;
        SaveData.OnWealthDecreased += MoneyDecreased;

        Spawner.OnGroupSpawned += OnNextWave;
    }

    private void OnDisable()
    {
        SaveData.OnHpIncreased -= HpIncreased;
        SaveData.OnHpDecreased -= HpDecreased;

        SaveData.OnWealthIncreased -= MoneyIncreased;
        SaveData.OnWealthDecreased -= MoneyDecreased;

        Spawner.OnGroupSpawned -= OnNextWave;
    }

    ///helps getting components by name, but needs monobehaviour
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
