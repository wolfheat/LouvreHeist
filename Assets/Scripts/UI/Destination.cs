using TMPro;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lockedText; 

    public bool Locked = true;

    public void Lock(bool doLock)
    {
        Debug.Log((doLock?"Locking ":"Unlocking ")+name);

        Locked = doLock;
        lockedText.gameObject.SetActive(Locked);
    }
}
