using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum UIAnimationTypes
{
    Move,
    Scale,
    ScaleX,
    ScaleY,
    ScaleZ,
    Fade
}

public class UITweener : MonoBehaviour
{
    [BoxGroup]
    public GameObject objectToAnimate;

    public UIAnimationTypes animationTypes;

    public LeanTweenType easeType;
    [SerializeField]
    [ShowIf("isAnimationCurve")]
    AnimationCurve animationCurve;
    public float duration;
    public float delay;

    public bool loop;
    public bool pingpong;

    public bool startPosOffset;
    public Vector3 from, to;

    private LTDescr _tweenObject;

    public bool showOnEnable;
    public bool workOnDisable;


    private bool isAnimationCurve = false;



    public void OnEnable()
    {
        if (showOnEnable)
        {
            Show();
        }
    }

    private void OnValidate()
    {
        if(easeType == LeanTweenType.animationCurve)
        {
            isAnimationCurve = true;
        }
        else { isAnimationCurve = false; }
    }

    public void Show()
    {
        HandleTween();
    }

    public void HandleTween()
    {
        if(objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        switch (animationTypes)
        {
            case UIAnimationTypes.Move:
                MoveAbsolute();
                break;
            case UIAnimationTypes.Scale:
                Scale();
                break;
            case UIAnimationTypes.ScaleX:
                Scale();
                break;
            case UIAnimationTypes.ScaleY:
                Scale();
                break;
            case UIAnimationTypes.ScaleZ:
                Scale();
                break;
            case UIAnimationTypes.Fade:
                Fade();
                break;
            default:
                break;
        }


        _tweenObject.setDelay(delay);
        _tweenObject.setEase(easeType);

        if (loop)
        {
            _tweenObject.loopCount = int.MaxValue;
        }
        if (pingpong)
        {
            _tweenObject.setLoopPingPong();
        }

    }

    public void Fade()
    {
        if(gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        if (startPosOffset)
        {
            objectToAnimate.GetComponent<CanvasGroup>().alpha = from.x;
        }
        else
        {
            _tweenObject = LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), to.x, duration);
        }
    }

    public void MoveAbsolute()
    {
        objectToAnimate.GetComponent<RectTransform>().anchoredPosition = from;

        _tweenObject = LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), to, duration);
    }

    public void Scale()
    {
        if (startPosOffset)
        {
            objectToAnimate.GetComponent <RectTransform>().localScale = from;
        }

        _tweenObject = LeanTween.scale(objectToAnimate, to, duration);
    }

    void SwapDirection()
    {
        var temp = from;
        from = to;
        to = temp;
    }

    public void OnDisable()
    {
        SwapDirection();
        HandleTween();

        _tweenObject.setOnComplete(() =>
        {
            SwapDirection();
            gameObject.SetActive(false);
        });
    }
}
