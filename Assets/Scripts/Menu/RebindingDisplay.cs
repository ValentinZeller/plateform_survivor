using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlateformSurvivor.Menu
{
    public class RebindingDisplay : MonoBehaviour
    {
        [SerializeField] private InputActionReference inputAction;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private TextMeshProUGUI bindingDisplayText;
        [SerializeField] private GameObject startRebindObject;
        [SerializeField] private GameObject waitingForInputObject;

        private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

        private void Start()
        {
            string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
            if (string.IsNullOrEmpty(rebinds)) { return; }

            playerInput.actions.LoadBindingOverridesFromJson(rebinds);

            UpdateUI(0);
        }
        private void UpdateUI(int targetBinding)
        {
            bindingDisplayText.text = InputControlPath.ToHumanReadableString(
                inputAction.action.bindings[targetBinding].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        
        private void Save()
        {
            string rebinds = playerInput.actions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString("rebinds", rebinds);
        }
        
        private void RebindComplete(int targetBinding)
        {
            UpdateUI(targetBinding);

            Save();

            rebindingOperation.Dispose();
            inputAction.action.Enable();

            startRebindObject.SetActive(true);
            waitingForInputObject.SetActive(false);
        }

        public void StartRebinding(int targetBinding = 0)
        {
            inputAction.action.Disable();
            startRebindObject.SetActive(false);
            waitingForInputObject.SetActive(true);

            rebindingOperation = inputAction.action.PerformInteractiveRebinding()
                .WithTargetBinding(targetBinding)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindComplete(targetBinding))
                .Start();
        }
    }
}