using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace TheTear.AR
{
    public class ARBoundaryPlacementController : MonoBehaviour
    {
        public ARRaycastManager raycastManager;
        public Camera arCamera;
        public GameObject markerPrefab;
        public Transform markerParent;
        public TrackableType trackableTypes = TrackableType.AllTypes;
        public float placementCooldown = 0.25f;
        public bool placementActive = true;
        public float extraClueRadius = 0.18f;

        public bool IsPlacementActive => placementActive;
        public int MarkerCount => markerPoses.Count;
        public IReadOnlyList<Pose> MarkerPoses => markerPoses;

        public event Action<Pose> OnMarkerPlaced;
        public event Action OnMarkersCleared;

        private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();
        private readonly List<Pose> markerPoses = new List<Pose>();
        private readonly List<GameObject> markerInstances = new List<GameObject>();
        private float nextPlacementTime;

        public void BeginPlacement()
        {
            placementActive = true;
        }

        public void EndPlacement()
        {
            placementActive = false;
        }

        public void ClearMarkers()
        {
            markerPoses.Clear();
            ClearMarkerVisuals();
            OnMarkersCleared?.Invoke();
        }

        public void ClearMarkerVisuals()
        {
            for (int i = 0; i < markerInstances.Count; i++)
            {
                if (markerInstances[i] != null)
                {
                    Destroy(markerInstances[i]);
                }
            }
            markerInstances.Clear();
        }

        public bool TryGetMarkerPose(int index, out Pose pose)
        {
            if (index < 0 || index >= markerPoses.Count)
            {
                pose = default;
                return false;
            }

            pose = markerPoses[index];
            return true;
        }

        private void Update()
        {
            if (!placementActive || raycastManager == null)
            {
                return;
            }

            if (Time.unscaledTime < nextPlacementTime)
            {
                return;
            }

            if (!TryGetTap(out Vector2 screenPos, out int pointerId))
            {
                return;
            }

            if (IsPointerOverUI(pointerId))
            {
                return;
            }

            if (raycastManager.Raycast(screenPos, Hits, trackableTypes))
            {
                Pose pose = Hits[0].pose;
                markerPoses.Add(pose);
                SpawnMarker(pose);
                nextPlacementTime = Time.unscaledTime + placementCooldown;
                OnMarkerPlaced?.Invoke(pose);
            }
        }

        private void SpawnMarker(Pose pose)
        {
            GameObject prefab = markerPrefab;
            if (prefab == null && raycastManager != null)
            {
                prefab = raycastManager.raycastPrefab;
            }

            if (prefab == null)
            {
                return;
            }

            GameObject instance = markerParent != null
                ? Instantiate(prefab, pose.position, pose.rotation, markerParent)
                : Instantiate(prefab, pose.position, pose.rotation);
            markerInstances.Add(instance);
        }

        private bool IsPointerOverUI(int pointerId)
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            if (pointerId >= 0)
            {
                return EventSystem.current.IsPointerOverGameObject(pointerId);
            }

            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool TryGetTap(out Vector2 screenPos, out int pointerId)
        {
#if ENABLE_INPUT_SYSTEM
            if (UnityEngine.InputSystem.Touchscreen.current != null)
            {
                var touch = UnityEngine.InputSystem.Touchscreen.current.primaryTouch;
                if (touch.press.wasPressedThisFrame)
                {
                    screenPos = touch.position.ReadValue();
                    pointerId = touch.touchId.ReadValue();
                    return true;
                }
            }

            if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                screenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                pointerId = -1;
                return true;
            }
#endif

            if (Input.touchCount > 0)
            {
                UnityEngine.Touch touch = Input.GetTouch(0);
                if (touch.phase == UnityEngine.TouchPhase.Began)
                {
                    screenPos = touch.position;
                    pointerId = touch.fingerId;
                    return true;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                screenPos = Input.mousePosition;
                pointerId = -1;
                return true;
            }

            screenPos = default;
            pointerId = -1;
            return false;
        }
    }
}
