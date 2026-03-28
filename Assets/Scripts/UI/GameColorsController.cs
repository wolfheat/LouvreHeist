using UnityEngine;

public class GameColorsController : MonoBehaviour
{
    [SerializeField] private Color activeToolColor;
    [SerializeField] private Color inactiveToolColor;
    [SerializeField] private Color unavailableToolColor;

    public Color ActiveToolColor => activeToolColor;
    public Color InActiveToolColor => inactiveToolColor;
    public Color UnavailableToolColor => unavailableToolColor;

    public static GameColorsController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
