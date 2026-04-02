using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ToolType { Hands, LockPick, Hammer, Grinder, Skull, Skull2 }

public class ToolsController : MonoBehaviour
{
	public static ToolsController Instance { get; private set; }

    public int ActiveToolIndex => (int)ActiveTool;
    public ToolType ActiveTool { get; private set; }

    private bool[] unlockedTool = new bool[6];
    public GameObject[] Tools;



    internal void Reset()
    {
        unlockedTool = new bool[6];
        UnlockTool(ToolType.Hands);

        // Initiate with bare hands
        EquipTool(ToolType.Hands);

        UIController.Instance.ToolsUIReset();
        UIController.Instance.BriefingsReset();


    }

    internal bool ToolUnlocked(ToolType tool) => unlockedTool[(int)tool];

    internal void EquipTool(ToolType tool)
    {
        Debug.Log("Equipping Tool "+tool);
        // Check if player owns Tool, only then change to it?
        if (!unlockedTool[(int)tool]) return;

        // Set this as new Tool
        ActiveTool = tool;

        // Update Held Item
        SetPhysicalToolActive(ActiveTool);

        // Update UI
        UIController.Instance.SetToolsUIActiveTool(tool);
    }

    private void SetPhysicalToolActive(ToolType activeTool)
    {
        for (int i = 0; i < unlockedTool.Length; i++) 
            Tools[i].SetActive(i == (int)activeTool);
    }

    internal void EquipTool(int v)
    {
        // Disallow changing tool if doing action
        if(PlayerController.Instance.DoingAction) return;

        // if there is an unlocked tool to the left switch to it
        for (int i = ActiveToolIndex + v; i < 5 && i>=0; i += v) {
            if (unlockedTool[i]) {
                EquipTool((ToolType)(i));
                return;
            }
        }
    }

    private void Awake()
    {
        Debug.Log("ToolsController awake...");
        if (Instance != null) {
            Debug.Log("ToolsController alreade have an active instance, deleting this one ");
            Destroy(gameObject);
			return;
		}
		Instance = this;
    }

    private void Start()
    {
        Reset();
    }

    public void UnlockTool(ToolType tool)
    {
        unlockedTool[(int)tool] = true;
        UIController.Instance.ActivateToolsUITool(tool);
    }
}
