using System.Collections;
using UnityEngine;
using Wolfheat.StartMenu;

public class LockPickable : MonoBehaviour
{
    
    protected bool isUnlocked = true;
    protected bool isOpen = false;

    [SerializeField] private Animator animator;

    public bool IsOpen => isOpen;


    public void Unlock()
    {
        isUnlocked = true;

        Debug.Log("Safe was successfully unlocked ");

        SoundMaster.Instance.PlaySound(SoundName.OpenDoor);
    }

    public void OpenDoorAnimate()
    {
        Debug.Log("Animate opening door");

        //GetComponent<Collider>().enabled = false;
        SoundMaster.Instance.PlaySound(SoundName.OpenDoor);
        //StartCoroutine(OpenDoorCO());
        //doorPart.transform.Rotate(0, -90, 0);
        animator.SetBool("Open", true);

        isOpen = true;


    }
    public void CloseDoorAnimate()
    {
        Debug.Log("Animate closing door");

        //GetComponent<Collider>().enabled = false;
        SoundMaster.Instance.PlaySound(SoundName.OpenDoor);
        //StartCoroutine(OpenDoorCO());
        //doorPart.transform.Rotate(0, -90, 0);
        animator.SetBool("Open", false);

        isOpen = false;
    }
}
