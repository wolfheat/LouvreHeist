using TMPro;
using UnityEngine;

public class DebugMessage : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textField;
    public void SetText(string textToSet) => textField.text = textToSet;
}
