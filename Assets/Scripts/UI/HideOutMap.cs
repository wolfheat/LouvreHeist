using UnityEngine;

public class HideOutMap : MonoBehaviour
{
    [SerializeField] private GameObject[] activeCircles;

    public void ActivateActiveCircle(int index)
    {
        for (int i = 0; i < activeCircles.Length; i++)
            activeCircles[i].SetActive(i==index);
    }
}
