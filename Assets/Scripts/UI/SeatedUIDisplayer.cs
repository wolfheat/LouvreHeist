using TMPro;
using UnityEngine;

public class SeatedUIDisplayer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI seatedText;


    public static SeatedUIDisplayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowSeated(bool show) => seatedText.gameObject.SetActive(show);
}
