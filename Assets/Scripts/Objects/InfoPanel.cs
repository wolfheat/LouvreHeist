using UnityEngine;


public enum InfoPanelTypes{HideoutMap,OfficeExit,MarketExit,BuildSiteExit,LouvreExit}

public class InfoPanel : MonoBehaviour
{
    [SerializeField] InfoPanelTypes panelType;

    public void OpenInfoPanel()
    {
        if(panelType == InfoPanelTypes.HideoutMap)
            UIController.Instance.ShowHideOutMap();
        else if (panelType != InfoPanelTypes.LouvreExit) {
            Debug.Log("Exiting any but the Louvre Scene");
            // Maybe first Show a Information which leads to scene change
            SceneChanger.Instance.ChangeScene("Hideout");
        }
        else{
            // Exiting Louvre - maybe have end scene here
            Debug.Log("Exiting Louvre Scene");
            // Maybe first Show a Information which leads to scene change
            SceneChanger.Instance.ChangeScene("Hideout");
        }
    }
}