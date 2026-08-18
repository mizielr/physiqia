using Michsky.UI.Reach;
using Physiqia;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Physiqia.GameSync
{
    public class GameSyncController : MonoBehaviour
    {
        [Header("Panel")]
        public GameObject syncPanel;

        [Header("UI")]
        [SerializeField]
        private TMP_InputField codeInputField;

        [SerializeField]
        private ButtonManager validateButton;

        [SerializeField]
        private TMP_Text feedbackText;

        [SerializeField]
        private GameObject notLinkedButton;

        [SerializeField]
        private GameObject linkedButton;

        private readonly Serina serina = new Serina();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            HideSyncPanel();
        }

        private void OnDestroy() { }

        // Update is called once per frame
        void Update() { }

        /// <summary>
        ///
        /// </summary>
        public void ShowSyncPanel()
        {
            syncPanel.SetActive(true);
            feedbackText.text = string.Empty;
            codeInputField.text = string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        public void HideSyncPanel()
        {
            syncPanel.SetActive(false);
        }

        /// <summary>
        ///
        /// </summary>
        public void ValidateSyncCode()
        {
            Debug.Log("Game synchronization started.");

            string code = codeInputField.text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                feedbackText.text = "Please enter a code.";
                return;
            }

            validateButton.Interactable(false);
            feedbackText.text = "Checking...";

            serina.postSyncCode(code, DeviceIdentity.GetOrCreate(), HandleSyncResponse);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="response"></param>
        private void HandleSyncResponse(HSyncResponse response)
        {
            validateButton.Interactable(true);

            if (response == null)
            {
                feedbackText.text = "Network error, please try again.";
                return;
            }

            Debug.Log($"Sync response: success={response.success}, message={response.message}");

            if (!response.success)
            {
                feedbackText.text = response.message ?? "Invalid code, please try again.";
                return;
            }

            feedbackText.text = "Linked successfully!";

            HideSyncPanel();
            notLinkedButton.SetActive(false);
            linkedButton.SetActive(true);
        }
    }
}
