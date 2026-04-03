using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Help : MonoBehaviour
{

    [SerializeField] PickUpController pickUpController;
    [SerializeField] PlayerController player;
    [SerializeField] TextMeshProUGUI item;
    [SerializeField] TextMeshProUGUI wall;
    [SerializeField] TextMeshProUGUI enemy;
    [SerializeField] TextMeshProUGUI mock;
    [SerializeField] TextMeshProUGUI playerPos;
    [SerializeField] TextMeshProUGUI lockPickable;
    [SerializeField] TextMeshProUGUI grindable;

    // Update is called once per frame
    void Update()
    {
        item.text = pickUpController.ActiveInteractable?.name;
        wall.text = pickUpController.Wall?.name;
        enemy.text = pickUpController.Enemy?.name;
        mock.text = pickUpController.Mockup?.name;
        lockPickable.text = pickUpController.LockPickable?.name;
        grindable.text = pickUpController.Grindable?.name;
        playerPos.text = player?.transform.position.ToString() ?? "";
    }
}
