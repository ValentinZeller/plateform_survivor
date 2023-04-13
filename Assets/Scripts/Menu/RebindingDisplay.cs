using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlateformSurvivor.Assets.Scripts.Menu
{
    public class RebindingDisplay : MonoBehaviour
    {
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private TextMeshProUGUI bindingDisplayText;
        [SerializeField] private GameObject startRebindObject;
        [SerializeField] private GameObject waitingForInputObject;

        private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

        private void UpdateUI()
        {
            int bindingIndex = jumpAction.action.GetBindingIndexForControl(jumpAction.action.controls[0]);

            bindingDisplayText.text = InputControlPath.ToHumanReadableString(
                jumpAction.action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        private void RebindComplete()
        {
            UpdateUI();

            rebindingOperation.Dispose();
            jumpAction.action.Enable();

            startRebindObject.SetActive(true);
            waitingForInputObject.SetActive(false);
        }
        public void StartRebinding()
        {
            jumpAction.action.Disable();
            startRebindObject.SetActive(false);
            waitingForInputObject.SetActive(true);

            rebindingOperation = jumpAction.action.PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindComplete())
                .Start();
        }

    }
}