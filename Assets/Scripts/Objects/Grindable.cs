using UnityEngine;
using Wolfheat.StartMenu;

public class Grindable : MonoBehaviour
{
    protected bool isOpen = false;

    [SerializeField] private Animator animator;

    public bool IsOpen => isOpen;


    public void GrindOpen()
    {
        isOpen = true;

        Debug.Log("Glass was successfully Opened ");

        SoundMaster.Instance.PlaySound(SoundName.UnlockChest);

        // Run Glass destroy animator? Show Shattered glass particles?
        animator.SetBool("Shatter", true);
    }

}
