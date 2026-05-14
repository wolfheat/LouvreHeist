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

    public void SetDragonActivationChange(int setIndex) => OnDragonChange(setIndex);

    private void OnDragonChange(int activeIndex)
    {
        if(dragonImage == null) {
            Debug.Log("Dragon UI Panel image is null, cantr set color");
            return;
        }
        Debug.Log("Dragon UI Panel: activate "+activeIndex);
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
                if(activeIndex==-1)
                    HelpInstructions.Instance.ShowInstruction("Perk Removed", HelpButtonType.Info); // Hide this if sending in -2 ie dont alert player cause its initialization
                break;
        }

    }
}
