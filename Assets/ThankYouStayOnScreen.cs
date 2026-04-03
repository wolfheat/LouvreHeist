using System;
using System.Collections;
using UnityEngine;

public class ThankYouStayOnScreen : MonoBehaviour
{
    [SerializeField] private GameObject centerParent;
    [SerializeField] private GameObject thankYouOriginalParent;
    private float centerPosition = 0f;
    private bool rising = true;

    private void Start()
    {
        rising = true;
        centerPosition = Screen.height / 2;                    
    }
    public void Reset()
    {
        transform.SetParent(thankYouOriginalParent.transform);
        transform.localScale = new Vector3(1, 1, 1);
        transform.localPosition = new Vector3(0, 0, 0);
        rising = true;
    }

    void Update()
    {       
        if(transform.position.y > centerPosition && rising) {
            rising = false;
            //Center
            transform.SetParent(centerParent.transform);
            Debug.Log("Start Grow coroutine");
            StartCoroutine(Grow());
        }    
    }

    private IEnumerator Grow()
    {
        Debug.Log(" Grow ");
        yield return null;
        float currentScale = 1f;
        const float MaxScale = 2.1f;
        const float GrowSpeed = 0.003f;
        while(currentScale < MaxScale) {
            Debug.Log("currentscale is "+currentScale);
            transform.localScale = new Vector3(currentScale,currentScale,currentScale);
            currentScale += GrowSpeed;
            Debug.Log("currentscale becomes "+currentScale+" max = "+MaxScale);
            //yield return new WaitForSeconds(GrowSpeed);
            yield return null;
        }
    }
}
