using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace TheTear.AR
{
    public class ARPlacementController : MonoBehaviour
    {
        public ARRaycastManager raycastManager;
        public Camera arCamera;
        public SceneRootController sceneRoot;

        public bool IsPlacementActive => placementActive;

        public event Action OnPlaced;
        public event Action OnRelocate;
        public event Action<bool> OnTrackingStateChanged;

        private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();
        private bool placementActive = true;
        private bool hasPlaced = false;

        private void OnEnable()
        {
            ARSession.stateChanged += OnARSessionStateChanged;
        }

        private void OnDisable()
        {
            ARSession.stateChanged -= OnARSessionStateChanged;
        }

        private void Update()
        {
            if (!placementActive || raycastManager == null)
            {
                return;
            }

            if (!TryGetTap(out Vector2 screenPos))
            {
                return;
            }

            if (raycastManager.Raycast(screenPos, Hits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = Hits[0].pose;
                PlaceAtPose(pose);
            }
        }

        public void BeginPlacement()
        {
            placementActive = true;
        }

        private void PlaceAtPose(Pose pose)
        {
            if (sceneRoot == null)
            {
                return;
            }

            if (!sceneRoot.gameObject.activeSelf)
            {
                sceneRoot.SetActive(true);
            }

            sceneRoot.transform.SetPositionAndRotation(pose.position, pose.rotation);
            placementActive = false;

            if (!hasPlaced)
            {
                hasPlaced = true;
                OnPlaced?.Invoke();
            }
            else
            {
                OnRelocate?.Invoke();
            }
        }

        private bool TryGetTap(out Vector2 screenPos)
        {
#if ENABLE_INPUT_SYSTEM
            if (Touchscreen.current != null)
            {
                var touch = Touchscreen.current.primaryTouch;
                if (touch.press.wasPressedThisFrame)
                {
                    screenPos = touch.position.ReadValue();
                    return true;
                }
            }

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                screenPos = Mouse.current.position.ReadValue();
                return true;
            }
#endif

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == UnityEngine.TouchPhase.Began)
                {
                    screenPos = touch.position;
                    return true;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                screenPos = Input.mousePosition;
                return true;
            }

            screenPos = default;
            return false;
        }

        private void OnARSessionStateChanged(ARSessionStateChangedEventArgs args)
        {
            bool trackingOk = args.state == ARSessionState.SessionTracking;
            OnTrackingStateChanged?.Invoke(trackingOk);
        }
    }
}
