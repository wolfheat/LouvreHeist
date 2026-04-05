using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Wolfheat.StartMenu;

public class VehicleAvailableState : MonoBehaviour
{
    public bool IsAvailable;
}
public class ItemPickUpState : MonoBehaviour
{
    public bool IsAvailable;
}
public class GrindableState : MonoBehaviour
{
    public bool IsOpen;
}

public class BreakableState : MonoBehaviour
{
    public bool IsOpen;
}

public class LockPickableState : MonoBehaviour
{
    public bool IsOpen;
    public bool IsUnLocked;
}

// Main INterface to get all SaveItems
public interface ISavable
{
    string GUID { get; }
    object GetState();
    void RestoreState(object state);

    void SetGUID(string newGUID);
}
public class LockPickable : MonoBehaviour, ISavable
{
    [SerializeField] protected bool walkableWhenOpen = false;
    [SerializeField] protected bool isUnlocked = false;
    protected bool isAnimating = false;
    protected bool isOpen = false;

    [SerializeField] private Animator animator;

    public bool IsOpen => isOpen;
    public bool IsAnimating => isAnimating;
    public bool IsUnLocked => isUnlocked;
    public bool Walkable => walkableWhenOpen && isOpen;



    
    #region Savable_Required_Region
    public string GUID => guid;
    [SerializeField] private string guid;

    public void SetGUID(string newGUID) => guid = newGUID;

#if UNITY_EDITOR
    private void OnValidate()
    {

        ISavable[] allItems = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>().ToArray();

        if (string.IsNullOrEmpty(guid) || allItems.Count(x => x.GUID == guid) > 1) {
            guid = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }

    [ContextMenu("FORCE RENEW GUID")]
    private void ForceRenewGUID()
    {
        guid = System.Guid.NewGuid().ToString();
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    public object GetState()
    {
        return new LockPickableState()
        {
            IsOpen = isOpen,
            IsUnLocked = isUnlocked
        };
    }

    public void RestoreState(object stateObject)
    {
        if (stateObject is not LockPickableState) {
            Debug.Log("Unable to Read Object as LockpickableState");
            return;
        }
        LockPickableState state = (LockPickableState)stateObject;

        isOpen = state.IsOpen;
        isUnlocked = state.IsUnLocked;
        
        if (isOpen) {
            animator.SetBool("Open", true);
            //animator.CrossFade("Open")
        }

    }

    # endregion
    

    [SerializeField] private bool data = true; 

    public void Unlock()
    {
        isUnlocked = true;

        Debug.Log("Safe was successfully unlocked ");

        if(data)
            SoundMaster.Instance.PlaySound(SoundName.UnlockChest);
        else
            SoundMaster.Instance.PlaySound(SoundName.UnlockDoor);
    }

    public void DoorAnimateComplete()
    {
        isAnimating = false;
        if (walkableWhenOpen && isOpen)
            gameObject.layer = LayerMask.NameToLayer("Door");

        // Call for updte of pickupColliders here


    }
    
    public void OpenDoorAnimate()
    {
        Debug.Log("Animate opening door");

        //GetComponent<Collider>().enabled = false;
        
        if (data)
            SoundMaster.Instance.PlaySound(SoundName.OpenChest);
        else
            SoundMaster.Instance.PlaySound(SoundName.OpenDoor);

        //StartCoroutine(OpenDoorCO());
        //doorPart.transform.Rotate(0, -90, 0);
        Debug.Log("Animate to open Door");
        animator.SetBool("Open", true);
        isAnimating = true;


        isOpen = true;
        // Maybe dont need this, can check if its open when deciding if it is walkable instead?
        //if(TryGetComponent(out Collider collider)) {
        //    Debug.Log("Found a Collider, disabling it");
        //    collider.enabled = false;
        //}
        // Changing this to door layer when opened
    }
    public void CloseDoorAnimate()
    {
        Debug.Log("Animate closing door");

        SoundMaster.Instance.PlaySound(SoundName.CloseChest);

        animator.SetBool("Open", false);
        isAnimating = true;
        isOpen = false;

        if (walkableWhenOpen && !isOpen)
            gameObject.layer = LayerMask.NameToLayer("Wall");
    }

    internal void TryOpenFail()
    {
        SoundMaster.Instance.PlaySound(SoundName.DoorLockedSound);
    }

}
