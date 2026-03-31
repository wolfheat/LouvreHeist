using UnityEngine;


public enum InfoPanelTypes{HideoutMap,OfficeExit}

public class InfoPanel : MonoBehaviour
{
    [SerializeField] InfoPanelTypes panelType;

    public void OpenInfoPanel()
    {
        if(panelType == InfoPanelTypes.HideoutMap)
            UIController.Instance.ShowHideOutMap();
        if (panelType == InfoPanelTypes.OfficeExit) {
            // Maybe first Show a Information which leads to scene change
            SceneChanger.Instance.ChangeScene("Hideout");
        }
    }
}