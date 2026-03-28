using System;
using UnityEngine;
using UnityEngine.UI;

public class ToolSquare : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image borderImage;
    [SerializeField] private GameObject visual;

    internal void SetToolActive(bool setActive, bool available = false)
    {
        if (!available) {
            visual.SetActive(false);
            //borderImage.color = GameColorsController.Instance.UnavailableToolColor;
            //backgroundImage.color = GameColorsController.Instance.UnavailableToolColor;
            return;
        }
        visual.SetActive(true);
        borderImage.color = setActive ? GameColorsController.Instance.ActiveToolColor : GameColorsController.Instance.InActiveToolColor;
        backgroundImage.color = setActive ? GameColorsController.Instance.ActiveToolColor : GameColorsController.Instance.InActiveToolColor;
    }
}
