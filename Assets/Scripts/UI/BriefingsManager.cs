using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Wolfheat.Inputs;
public class BriefingsManager : MonoBehaviour
{
    [SerializeField] private HideOutMap hideOutMap; 
    [SerializeField] private GameObject[] briefingsPages; 
    [SerializeField] private GameObject StartMissionButton; 
    private string[] SceneNames = {"Office","BuildSite","Louvre"}; 
    private int[] moneyNeededToUnlockDestination = {0,10000,15000,20000}; 

    private int activeBriefingIndex = 0;

    public void SetNextBriefingStage() => activeBriefingIndex++;
    
    public void SetMissionAsActive(int index)
    {
        activeBriefingIndex = index;
        Debug.Log("Setting Mission "+index+" as active.");
        hideOutMap.ActivateActiveCircle(activeBriefingIndex);

        bool isLocked = hideOutMap.Locked(activeBriefingIndex);
        Debug.Log("Mission "+index+" is "+(isLocked?"Locked":"Not Locked"));

        // Show the start Button if its not locked
        StartMissionButton.SetActive(!isLocked);
    }

    public bool ManagerIsActive => hideOutMap.gameObject.activeSelf;

    public void UnlockDestination(int index)
    {
        hideOutMap.UnlockDestination(index);
    }

    public void Reset()
    {
        activeBriefingIndex = 0;
        hideOutMap.Reset();
    }

    public void OpenHideOutMap()
    {
        if (hideOutMap.gameObject.activeSelf) return; // already open exit

        Debug.Log("Opening the HideOut Map");
        // Opens the map and sets the current step as active
        hideOutMap.gameObject.SetActive(true);

        SetMissionAsActive(activeBriefingIndex);

        UpdateUnlockedDestinations();
    }

    private void UpdateUnlockedDestinations()
    {
        for (int i = 0; i < moneyNeededToUnlockDestination.Length; i++) {
            if(Inventory.Instance.MoneyHeld > moneyNeededToUnlockDestination[i]) {
                //Debug.Log("UNLOCK: index:"+i+" cause player has "+ Inventory.Instance.MoneyHeld+" and need only " + moneyNeededToUnlockDestination[i]+" to unlokc.");
                UnlockDestination(i);
            }
        }
    }

    public void ShowBriefing()
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
        else if (hideOutMap.gameObject.activeSelf) {
            PlayerController.Instance.DoingAction = false;
            hideOutMap.gameObject.SetActive(false);
        }
    }
    public void StartMission()
    {
        Debug.Log("Starting Mission "+(activeBriefingIndex+1));

        if(briefingsPages[activeBriefingIndex].activeSelf)
            briefingsPages[activeBriefingIndex].SetActive(false);
        if (hideOutMap.gameObject.activeSelf)
            hideOutMap.gameObject.SetActive(false);


        // Start The Mission
        SceneChanger.Instance.ChangeScene(SceneNames[activeBriefingIndex]);

    }
}
