using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TheTear.Story;

namespace TheTear.UI
{
    public class JournalController : MonoBehaviour
    {
        public GameObject panelRoot;
        public Component clusterText;
        public Component clueListText;
        public Button closeButton;
        public Button unlockButton;
        public Button deductionButton;

        private ClueManager clueManager;
        private DeductionController deduction;

        private void Awake()
        {
            if (closeButton != null) closeButton.onClick.AddListener(Hide);
            if (unlockButton != null) unlockButton.onClick.AddListener(UnlockEligible);
            if (deductionButton != null) deductionButton.onClick.AddListener(OpenDeduction);
        }

        public void Initialize(ClueManager manager, DeductionController deductionController)
        {
            clueManager = manager;
            deduction = deductionController;
            Refresh();
            Hide();
        }

        public void Toggle()
        {
            if (panelRoot == null)
            {
                return;
            }
            bool show = !panelRoot.activeSelf;
            panelRoot.SetActive(show);
            if (show)
            {
                Refresh();
            }
        }

        public void Hide()
        {
            if (panelRoot != null)
            {
                panelRoot.SetActive(false);
            }
        }

        public void Refresh()
        {
            if (clueManager == null)
            {
                return;
            }

            StringBuilder clusters = new StringBuilder();
            foreach (var cluster in clueManager.GetClusters())
            {
                int unlockedCount = clueManager.GetClusterUnlockedCount(cluster.id);
                int total = clueManager.GetClusterTotalCount(cluster.id);
                clusters.Append(cluster.name).Append(" [").Append(cluster.id).Append("] ")
                    .Append(unlockedCount).Append("/").Append(total).Append("\n");
            }
            UITextHelper.SetText(clusterText, clusters.ToString().TrimEnd());

            StringBuilder list = new StringBuilder();
            foreach (var clue in clueManager.GetAllClues())
            {
                string status = clueManager.IsUnlocked(clue.id) ? "[x]" : (clueManager.IsEligible(clue.id) ? "[>]" : "[ ]");
                list.Append(status).Append(" ").Append(clue.id).Append(" ").Append(clue.title).Append("\n");
            }
            UITextHelper.SetText(clueListText, list.ToString().TrimEnd());

            if (deductionButton != null)
            {
                deductionButton.interactable = clueManager.IsDeductionAvailable;
            }
        }

        private void UnlockEligible()
        {
            if (clueManager != null)
            {
                clueManager.UnlockFirstEligibleFromJournal();
                Refresh();
            }
        }

        private void OpenDeduction()
        {
            if (deduction != null && clueManager != null && clueManager.IsDeductionAvailable)
            {
                deduction.Show();
            }
        }
    }
}
