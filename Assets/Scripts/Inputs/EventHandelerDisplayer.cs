
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventHandelerDisplayer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] private TextMeshProUGUI extraInfo;


    private void Update()
    {
        // Find Eventhandeler
        EventSystem[] handelers = FindObjectsByType<EventSystem>(FindObjectsSortMode.None); 

        bool currentNull = EventSystem.current == null;

        //Debug.Log("EventSystem: "+EventSystem.current != null);
        //Debug.Log("EventSystem: "+ EventSystem.current.currentSelectedGameObject != null);

        string currentSelected = EventSystem.current?.currentSelectedGameObject?.name ?? "None";

        eventText.text = "Handelers: "+handelers.Length + " [0] = "+handelers[0].name+" Selected: "+currentSelected;

    }

    public void ShowExtraText(string text) => extraInfo.text = text;

}
