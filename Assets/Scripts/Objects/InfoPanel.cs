using UnityEngine;


public enum InfoPanelTypes{HideoutMap}

public class InfoPanel : MonoBehaviour
{
    [SerializeField] InfoPanelTypes panelType;

    public void OpenInfoPanel()
    {

        if(panelType == InfoPanelTypes.HideoutMap)
            UIController.Instance.ShowHideOutMap();
    }
}