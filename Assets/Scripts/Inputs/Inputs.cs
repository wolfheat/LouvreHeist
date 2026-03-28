using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Wolfheat.StartMenu;

namespace Wolfheat.Inputs
{
    public class Inputs : MonoBehaviour
    {
        public Controls Controls { get; set; }
        public InputAction Actions { get; set; }

        private List<System.Action<InputAction.CallbackContext>> storedLinks = new();

        public static Inputs Instance { get; private set; }

        private void OnEnable()
        {
            // Enable this when you want to use the loading of a saved file
            //SavingUtility.LoadingComplete += LoadingComplete;

#if UNITY_EDITOR
    Controls.Player.T.performed += JumpToNextEntryPoint;
    Controls.Player.Y.performed += Give10Gold;
#endif
        }

        private void Start()
        {
            LoadingComplete();
            
        }

        private void LoadingComplete()
        {
            Debug.Log("Loading Complete");
            Controls.Player.M.performed += SoundMaster.Instance.ToggleAllAudio;
            Controls.Player.N.performed += SoundMaster.Instance.ToggleMusic;

            // Tools Input Loader
            Controls.Player.One.performed += One;
            Controls.Player.Two.performed += Two;
            Controls.Player.Three.performed += Three;
            Controls.Player.Four.performed += Four;
            Controls.Player.Five.performed += Five;
            Controls.Player.Six.performed += Six;
            Controls.Player.ScrollDown.performed += Prev;
            Controls.Player.ScrollUp.performed += Next;
        }

        private void One(InputAction.CallbackContext c) => ToolsController.Instance.EquipTool(ToolType.Hands);
        private void Two(InputAction.CallbackContext c) => ToolsController.Instance.EquipTool(ToolType.LockPick);
        private void Three(InputAction.CallbackContext c) => ToolsController.Instance.EquipTool(ToolType.Hammer);
        private void Four(InputAction.CallbackContext c) => ToolsController.Instance.EquipTool(ToolType.Grinder);
        private void Five(InputAction.CallbackContext c) => ToolsController.Instance.EquipTool(ToolType.Skull);
        private void Six(InputAction.CallbackContext c) => ToolsController.Instance.EquipTool(ToolType.Skull2);
        private void Prev(InputAction.CallbackContext c) => ToolsController.Instance.EquipTool(-1);
        private void Next(InputAction.CallbackContext c) => ToolsController.Instance.EquipTool(1);

        private void OnDisable()
        {
            Controls.Player.M.performed -= SoundMaster.Instance.ToggleAllAudio;
            Controls.Player.N.performed -= SoundMaster.Instance.ToggleMusic;
            Controls.Player.T.performed -= JumpToNextEntryPoint;
            Controls.Player.Y.performed -= Give10Gold;
            Controls.Disable();


            // Tools Input Loader
            Controls.Player.One.performed -= One;
            Controls.Player.Two.performed -= Two;
            Controls.Player.Three.performed -= Three;
            Controls.Player.Four.performed -= Four;
            Controls.Player.Five.performed -= Five;
            Controls.Player.Six.performed -= Six;
            Controls.Player.ScrollDown.performed -= Prev;
            Controls.Player.ScrollUp.performed -= Next;
        }

        public void Give10Gold(InputAction.CallbackContext context)
        {
            Debug.Log("Player Gets 10g");
            Inventory.Instance.AddCoins(10);
        }
        
        public void JumpToNextEntryPoint(InputAction.CallbackContext context)
        {
            Stats.Instance.HasTeleported = true;
            Debug.Log("Jumping to Next Exit point");
            PlayerController.Instance.GotoNextStartPosition();
        }

        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log("Created Inputs Controller");
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            Controls = new Controls();
            Controls.Enable();
        }
    }
}