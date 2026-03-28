using System;
using UnityEngine;


public class ToolsUIController : MonoBehaviour
{

    [SerializeField] private ToolSquare[] ToolSquares;
    private bool[] activatedTools = new bool[6];

    private int ActiveToolIndex = 0;
    
    private bool available = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowActiveTool(ActiveToolIndex);
    }

    internal void SetAsAvailable(bool a) => available = a;
    
    internal void ShowActiveTool(ToolType type) => ShowActiveTool((int)type);

    private void ShowActiveTool(int activeToolIndex)
    {
        ActiveToolIndex = activeToolIndex;

        // Disable all but the Active one
        for (int i = 0; i < ToolSquares.Length; i++) {
            ToolSquares[i].SetToolActive(i == ActiveToolIndex, activatedTools[i]);
        }
    }

    internal void ActivateTool(ToolType tool)
    {
        Debug.Log("Activating Tool "+tool);
        activatedTools[(int)tool] = true;

        // Just Update all Tools, but with one new Available
        ShowActiveTool(ActiveToolIndex);
    }
}
