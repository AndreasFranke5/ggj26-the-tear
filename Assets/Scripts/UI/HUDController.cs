using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheTear.UI
{
    public class HUDController : MonoBehaviour
    {
        public Button matterButton;
        public Button voidButton;
        public Button flowButton;
        public Button journalButton;
        public Button relocateButton;
        public Button pauseButton;
        public Button deductionButton;
        public Component objectiveText;
        public GameObject trackingBanner;
        public Component trackingText;

        public event Action OnMatterPressed;
        public event Action OnVoidPressed;
        public event Action OnFlowPressed;
        public event Action OnJournalPressed;
        public event Action OnRelocatePressed;
        public event Action OnPausePressed;
        public event Action OnDeductionPressed;

        private void Awake()
        {
            if (matterButton != null) matterButton.onClick.AddListener(() => OnMatterPressed?.Invoke());
            if (voidButton != null) voidButton.onClick.AddListener(() => OnVoidPressed?.Invoke());
            if (flowButton != null) flowButton.onClick.AddListener(() => OnFlowPressed?.Invoke());
            if (journalButton != null) journalButton.onClick.AddListener(() => OnJournalPressed?.Invoke());
            if (relocateButton != null) relocateButton.onClick.AddListener(() => OnRelocatePressed?.Invoke());
            if (pauseButton != null) pauseButton.onClick.AddListener(() => OnPausePressed?.Invoke());
            if (deductionButton != null) deductionButton.onClick.AddListener(() => OnDeductionPressed?.Invoke());
        }

        public void SetObjective(string text)
        {
            UITextHelper.SetText(objectiveText, text);
        }

        public void SetTrackingBanner(bool show)
        {
            if (trackingBanner != null)
            {
                trackingBanner.SetActive(show);
            }
            if (show)
            {
                UITextHelper.SetText(trackingText, "Tracking limited - move device slowly.");
            }
        }

        public void SetVoidFlowInteractable(bool layersReady)
        {
            if (voidButton != null) voidButton.interactable = layersReady;
            if (flowButton != null) flowButton.interactable = layersReady;
        }

        public void SetDeductionInteractable(bool ready)
        {
            if (deductionButton != null) deductionButton.interactable = ready;
        }
    }
}
