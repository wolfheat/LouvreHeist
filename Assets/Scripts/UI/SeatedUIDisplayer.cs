using TMPro;
using UnityEngine;

public class SeatedUIDisplayer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI seatedText;
    [SerializeField] private TextMeshProUGUI PickupText;


    public static SeatedUIDisplayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowSeated(bool show) => seatedText?.gameObject.SetActive(show);
    public void ShowPickupType(string typeName) => PickupText.text = typeName;
}
