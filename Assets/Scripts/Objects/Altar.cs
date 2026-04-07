using System;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] GameObject ownCrystalactivation;
    [SerializeField] GameObject mineralObject;
    [SerializeField] int acceptsDragonID;
    public bool HasMineral { get { return mineralObject.activeSelf; }}
    public int MineralAccepted { get { return acceptsDragonID;}}

    //public static Action AltarActivated;

    private void OnEnable()
    {
        Stats.DragonOwnedUpdate += UpdateOwnedStatus;
        Debug.Log("Altar Start Enable Updating from the value in Stats DragonsOwned");
        UpdateOwnedStatus();
    }


private void OnDisable()
    {
        Stats.DragonOwnedUpdate -= UpdateOwnedStatus;
        UpdateOwnedStatus();
    }


    private void Start()
    {
        //UpdateOwnedStatus();
    }

    public void RemoveItemFromPillar()
    {
        mineralObject.SetActive(false);
    }
    
    private void UpdateOwnedStatus()
    {
        Debug.Log("Setting Dragon " + name + " to owned: "+ Stats.Instance?.DragonsOwned[acceptsDragonID]);
        //ownCrystalactivation.SetActive(Stats.Instance?.DragonsOwned[acceptsDragonID] ?? false);
        mineralObject.SetActive(Stats.Instance?.DragonsOwned[acceptsDragonID] ?? false);
    }

    internal void AddItemToPillar() => mineralObject.SetActive(true);

    internal bool IsAvailable() => mineralObject.activeSelf;
}
