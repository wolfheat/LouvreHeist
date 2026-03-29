using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Wolfheat.Inputs;
public class BriefingsManager : MonoBehaviour
{
    [SerializeField] private HideOutMap hideOutMap; 
    [SerializeField] private GameObject[] briefingsPages; 

    private int activeBriefingIndex = 0;

    public void SetNextBriefingStage() => activeBriefingIndex++;
    public bool ManagerIsActive => hideOutMap.gameObject.activeSelf;

    public void OpenHideOutMap()
    {
        if (hideOutMap.gameObject.activeSelf) return; // allready open exit

        Debug.Log("Opening the HideOut Map");
        // Opens the map and sets the current step as active
        hideOutMap.gameObject.SetActive(true);
        hideOutMap.ActivateActiveCircle(activeBriefingIndex);
    }
    
    public void HideOutMapShowActiveBriefing()
    {
        // Opens the map and sets the current step as active
        OpenBriefingPage(activeBriefingIndex);
    }
    
    public void OpenBriefingPage(int index)
    {
        briefingsPages[index].SetActive(true);
    }


    private void OnEnable()
    {
        Inputs.Instance.Controls.Player.Esc.performed += OnEscape;
    }
    private void OnDisable()
    {
        Inputs.Instance.Controls.Player.Esc.performed -= OnEscape;
    }

    private void OnEscape(InputAction.CallbackContext context)
    {
        // Exits the briefing if its open, else closes the map
        if(briefingsPages[activeBriefingIndex].activeSelf)
            briefingsPages[activeBriefingIndex].SetActive(false);
        else if(hideOutMap.gameObject.activeSelf)
            hideOutMap.gameObject.SetActive(false);
    }
    public void StartMission()
    {
        Debug.Log("Starting Mission "+(activeBriefingIndex+1));

        if(briefingsPages[activeBriefingIndex].activeSelf)
            briefingsPages[activeBriefingIndex].SetActive(false);
        if (hideOutMap.gameObject.activeSelf)
            hideOutMap.gameObject.SetActive(false);

    }
}
