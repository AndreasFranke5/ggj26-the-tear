using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheTear.AR;
using TheTear.Characters;
using CharacterController = TheTear.Characters.CharacterController;
using TheTear.Interaction;
using TheTear.Story;
using TheTear.Telemetry;
using TheTear.UI;

namespace TheTear.Core
{
    public class GameManager : MonoBehaviour
    {
        public ARPlacementController arPlacement;
        public SceneRootController sceneRoot;
        public CharacterController characterController;
        public ClueManager clueManager;
        public TelemetryRecorder telemetry;
        public TapRaycaster tapRaycaster;
        public HUDController hud;
        public JournalController journal;
        public PauseMenuController pauseMenu;
        public DeductionController deduction;
        public ToastController toast;
        public ErrorPanelController errorPanel;
        public OverlayController overlay;

        private AppState state = AppState.Placement;
        private StoryModel story;

        private void Awake()
        {
            Time.timeScale = 1f;
        }

        private void Start()
        {
            StartCoroutine(LoadAndInit());
        }

        private IEnumerator LoadAndInit()
        {
            if (hud != null)
            {
                hud.SetObjective("Loading case...");
            }

            StoryModel loaded = null;
            string loadError = null;
            yield return StoryLoader.LoadStory((model, error) =>
            {
                loaded = model;
                loadError = error;
            });

            if (loaded == null)
            {
                ShowBlockingErrors(new List<string> { "Failed to load story: " + (loadError ?? "Unknown error") });
                yield break;
            }

            story = loaded;
            List<string> issues = StoryValidator.Validate(story);
            if (issues.Count > 0)
            {
                ShowBlockingErrors(issues);
                yield break;
            }

            if (clueManager != null)
            {
                clueManager.Initialize(story, sceneRoot);
                clueManager.OnClueUnlocked += HandleClueUnlocked;
                clueManager.OnDeductionAvailabilityChanged += HandleDeductionAvailability;
            }

            if (journal != null)
            {
                journal.Initialize(clueManager, deduction);
            }

            if (pauseMenu != null)
            {
                pauseMenu.Initialize(clueManager);
                pauseMenu.OnPauseChanged += HandlePauseChanged;
            }

            if (deduction != null)
            {
                deduction.Initialize(story, clueManager, telemetry);
                deduction.OnDeductionShown += HandleModalShown;
                deduction.OnDeductionHidden += HandleModalHidden;
            }

            if (hud != null)
            {
                hud.OnMatterPressed += () => characterController?.SetMode(CharacterMode.Matter);
                hud.OnVoidPressed += () => characterController?.SetMode(CharacterMode.Void);
                hud.OnFlowPressed += () => characterController?.SetMode(CharacterMode.Flow);
                hud.OnJournalPressed += () => journal?.Toggle();
                hud.OnRelocatePressed += BeginRelocate;
                hud.OnPausePressed += () => pauseMenu?.Show();
                hud.OnDeductionPressed += () => deduction?.Show();
            }

            if (arPlacement != null)
            {
                arPlacement.OnPlaced += HandlePlaced;
                arPlacement.OnRelocate += HandleRelocate;
                arPlacement.OnTrackingStateChanged += HandleTrackingChanged;
                arPlacement.BeginPlacement();
            }

            if (tapRaycaster != null)
            {
                tapRaycaster.enabled = false;
            }

            if (hud != null)
            {
                hud.SetObjective("Tap a detected plane to place the investigation bubble.");
            }

            CheckLayers();
        }

        private void CheckLayers()
        {
            if (characterController == null)
            {
                return;
            }

            string message;
            bool ok = characterController.ValidateLayers(out message);
            if (hud != null)
            {
                hud.SetVoidFlowInteractable(ok);
            }

            if (!ok && errorPanel != null)
            {
                errorPanel.ShowErrors(new List<string> { message }, true);
            }
        }

        private void HandlePlaced()
        {
            state = AppState.Investigating;
            if (tapRaycaster != null)
            {
                tapRaycaster.enabled = true;
            }

            if (clueManager != null)
            {
                clueManager.SpawnClues();
            }

            if (hud != null)
            {
                hud.SetObjective("Investigate the evidence. Switch modes to reveal hidden clues.");
            }

            if (toast != null)
            {
                toast.Show("Anchor placed. Tap evidence to unlock clues.");
            }
        }

        private void BeginRelocate()
        {
            if (arPlacement != null)
            {
                arPlacement.BeginPlacement();
            }
            if (tapRaycaster != null)
            {
                tapRaycaster.enabled = false;
            }
            if (telemetry != null)
            {
                telemetry.RecordEvent("relocate", "begin");
            }
            if (toast != null)
            {
                toast.Show("Relocate mode: tap a plane to move the investigation bubble.");
            }
        }

        private void HandleRelocate()
        {
            if (telemetry != null)
            {
                telemetry.RecordEvent("relocate", "placed");
            }
            if (tapRaycaster != null)
            {
                tapRaycaster.enabled = true;
            }
            if (toast != null)
            {
                toast.Show("Investigation bubble moved.");
            }
        }

        private void HandleClueUnlocked(ClueData clue)
        {
            if (toast != null)
            {
                toast.Show("Clue unlocked: " + clue.title);
            }
            if (telemetry != null)
            {
                telemetry.RecordEvent("clue_unlock", clue.id);
            }
            if (journal != null)
            {
                journal.Refresh();
            }
            if (pauseMenu != null)
            {
                pauseMenu.Refresh();
            }
        }

        private void HandleDeductionAvailability(bool available)
        {
            if (hud != null)
            {
                hud.SetDeductionInteractable(available);
            }
            if (available && toast != null)
            {
                toast.Show("Deduction ready. Review the board when ready.");
            }
        }

        private void HandlePauseChanged(bool isPaused)
        {
            if (tapRaycaster != null)
            {
                tapRaycaster.enabled = !isPaused;
            }
        }

        private void HandleModalShown()
        {
            if (tapRaycaster != null)
            {
                tapRaycaster.enabled = false;
            }
        }

        private void HandleModalHidden()
        {
            if (state == AppState.Investigating && arPlacement != null && !arPlacement.IsPlacementActive)
            {
                if (tapRaycaster != null)
                {
                    tapRaycaster.enabled = true;
                }
            }
        }

        private void HandleTrackingChanged(bool trackingOk)
        {
            if (hud != null)
            {
                hud.SetTrackingBanner(!trackingOk);
            }
        }

        private void ShowBlockingErrors(List<string> issues)
        {
            state = AppState.Error;
            if (tapRaycaster != null)
            {
                tapRaycaster.enabled = false;
            }
            if (errorPanel != null)
            {
                errorPanel.ShowErrors(issues, false);
            }
            if (hud != null)
            {
                hud.SetObjective("Fix story data errors to proceed.");
            }
        }
    }
}
