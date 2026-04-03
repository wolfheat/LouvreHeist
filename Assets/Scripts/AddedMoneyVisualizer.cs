using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AddedMoneyVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;

    private const float VisualTime = 1.5f;
    private const float MoveSpeed = 10f;
    private float visualTimer = 0f;
    
    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        // Move it upwards
        while (visualTimer < VisualTime) {
            visualTimer += Time.fixedDeltaTime;
            
            // Move it
            transform.position += Vector3.up * MoveSpeed * Time.fixedDeltaTime;
            
            // Tint it

            yield return null;
        }
        yield return null;

        Destroy(this.gameObject);
    }

    internal void SetValue(int value) => textField.text = "+ " + value;
}
