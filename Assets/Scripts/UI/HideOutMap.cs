using System;
using UnityEngine;
public class HideOutMap : MonoBehaviour
{
    [SerializeField] private Destination[] destinations;
    [SerializeField] private GameObject[] activeCircles;

    public void ActivateActiveCircle(int index)
    {
        // Let Player click the locations and show the active circle around them

        for (int i = 0; i < activeCircles.Length; i++)
            activeCircles[i].SetActive(i==index);
    }

    internal void UnlockDestination(int index)
    {
        destinations[index].Lock(false);
    }
    internal bool Locked(int index) => destinations[index].Locked;

    internal void Reset()
    {
        // Locks All destinations but the first one
        for (int i = 0; i < destinations.Length; i++) {
            destinations[i].Lock(i > 1);
        }
    }
}
