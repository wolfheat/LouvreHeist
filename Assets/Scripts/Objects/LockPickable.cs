using System;
using System.Collections;
using UnityEngine;
using Wolfheat.StartMenu;

public class LockPickable : MonoBehaviour
{

    [SerializeField] protected bool isUnlocked = false;
    protected bool isAnimating = false;
    protected bool isOpen = false;

    [SerializeField] private Animator animator;

    public bool IsOpen => isOpen;
    public bool IsAnimating => isAnimating;
    public bool IsUnLocked => isUnlocked;

    // Have data here that tells what Type it is and what Sound to play?

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
        animator.SetBool("Open", true);
        isAnimating = true;

        isOpen = true;


    }
    public void CloseDoorAnimate()
    {
        Debug.Log("Animate closing door");

        SoundMaster.Instance.PlaySound(SoundName.CloseChest);

        animator.SetBool("Open", false);
        isAnimating = true;
        isOpen = false;
    }

    internal void TryOpenFail()
    {
        SoundMaster.Instance.PlaySound(SoundName.DoorLockedSound);
    }
}
