using UnityEngine;

public class Stair : MonoBehaviour
{

    public bool IsActive { get; set; } = true;

    //[SerializeField] private Transform origin;
    [SerializeField] private Transform destination; 

    public bool TryUseStair(out Transform dest)
    {
        Debug.Log("Trying To use stair at " + transform.position + " to reach target at " + destination.position);
        dest = destination;
        return IsActive;
    }


}
