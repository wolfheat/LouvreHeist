using UnityEngine;

public class ConditionalExit : MonoBehaviour
{
    [SerializeField] private GameObject activateIfConditionIsFullfilled; 
    [SerializeField] private GameObject itemThatNeedsToBeDisabled; 

    
    void Update()
    {
        if(!itemThatNeedsToBeDisabled.activeSelf)
            activateIfConditionIsFullfilled.SetActive(true);
    }
}
