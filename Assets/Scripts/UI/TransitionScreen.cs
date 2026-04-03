using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] Image image;

    [SerializeField] private Color lightColor = new Color(0,0,0,0);
    [SerializeField] private Color darkColor = new Color(0, 0, 0, 1);
    private Color currentColor;

    private float animationTimer = 0;
    private float AnimationTime = 4f;
    private const float AnimationStayDarkTime = 0.5f;
    private const float AnimationTimeDefault = 4f;

    public static Action AnimationComplete;
    Action callbackMethod;

    public static TransitionScreen Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //Darken();
        //AnimationComplete += Lighten;
    }

    public void Reset()
    {
        screen.SetActive(false);
    }

    public void Lighten()
    {
        Debug.Log("Transition: Enter here Lighten" ); 
        StartCoroutine(Animate(darkColor, lightColor));
    }

    public void Darken(Action callback, float transitionTime)
    {
        Debug.Log("Transition: Enter here Darken");

        AnimationTime = transitionTime;
        callbackMethod = callback;
        StartCoroutine(Animate(lightColor,darkColor));
    }

    public void Darken()    
    {
        Debug.Log("Transition: Enter here Darken (no Callbak set)");
        AnimationTime = AnimationTimeDefault;
        StartCoroutine(Animate(lightColor,darkColor));
    }

    private IEnumerator Animate(Color fromColor, Color toColor)
    {
        screen.SetActive(true);
        animationTimer = 0;
        image.color = fromColor;
        while (animationTimer < AnimationTime)
        {
            image.color = Color.Lerp(fromColor, toColor, animationTimer/AnimationTime);
            animationTimer += Time.unscaledDeltaTime;
            yield return null;
        }
        image.color = toColor;

        animationTimer = 0;


        if(callbackMethod != null) {
            callbackMethod();
            callbackMethod = null;

            // Added Dark Part
            while (animationTimer < AnimationStayDarkTime) {
                animationTimer += Time.unscaledDeltaTime;
                yield return null;
            }
            Lighten();
        }
        else {
            screen.SetActive(false);
            AnimationComplete?.Invoke();
        }


    }



}
