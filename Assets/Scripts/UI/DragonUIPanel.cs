using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragonUIPanel : MonoBehaviour
{

    [SerializeField] private Image dragonImage;

    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color unavailableColor;


    private void Start()
    {
        // Deafult the values

        dragonImage.color = unavailableColor;
        textField.text = "";
    }

    private void OnEnable()
    {
        Stats.OnDragonActivationChange += OnDragonChange;        
    }
    

    private void OnDisable()
    {
        Stats.OnDragonActivationChange -= OnDragonChange;        
    }

    private void OnDragonChange(int activeIndex)
    {
        // Update the UI visuals
        switch (activeIndex) {
            case 0:
                dragonImage.color = blueColor;
                textField.text = "Power";
                HelpInstructions.Instance.ShowInstruction("Perk: Power Aquired - You destroy stuff faster.", HelpButtonType.Info);
                break;
            case 1:
                dragonImage.color = redColor;
                textField.text = "Stealth";
                HelpInstructions.Instance.ShowInstruction("Perk: Stealth Aquired - You become stealthier.", HelpButtonType.Info);
                break;
            case 2:
                dragonImage.color = greenColor;
                textField.text = "Speed";
                HelpInstructions.Instance.ShowInstruction("Perk: Speed Aquired - You move faster.", HelpButtonType.Info);
                break;
            default:
                dragonImage.color = unavailableColor;
                textField.text = "";
                HelpInstructions.Instance.ShowInstruction("Perk Removed", HelpButtonType.Info);
                break;
        }

    }
}
