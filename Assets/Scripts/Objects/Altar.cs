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
    }


private void OnDisable()
    {
        Stats.DragonOwnedUpdate -= UpdateOwnedStatus;
    }


    private void Start()
    {
        UpdateOwnedStatus();
    }

    public void RemoveItemFromPillar()
    {
        mineralObject.SetActive(false);
    }
    
    private void UpdateOwnedStatus()
    {
        //ownCrystalactivation.SetActive(Stats.Instance?.DragonsOwned[acceptsDragonID] ?? false);
        mineralObject.SetActive(Stats.Instance?.DragonsOwned[acceptsDragonID] ?? false);
    }

    internal void AddItemToPillar() => mineralObject.SetActive(true);

    internal bool IsAvailable() => mineralObject.activeSelf;
}
