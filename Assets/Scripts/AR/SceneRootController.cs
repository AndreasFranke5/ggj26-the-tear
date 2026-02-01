using UnityEngine;

namespace TheTear.AR
{
    /// <summary>
    /// Controller for the SceneRoot anchor that holds all evidence objects.
    /// SceneRoot must be parented to XROrigin (NOT ARCamera/CameraOffset).
    /// Evidence objects spawn as children of SceneRoot and must remain world-space 3D objects.
    /// </summary>
    public class SceneRootController : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        /// <summary>
        /// Validates that this SceneRoot is properly configured for world-space anchoring.
        /// Returns true if valid, false otherwise. Issues are logged as errors.
        /// </summary>
        public bool ValidateHierarchy()
        {
            bool valid = true;

            // SceneRoot must not be parented under a Canvas
            if (GetComponentInParent<Canvas>() != null)
            {
                Debug.LogError("[SceneRootController] SceneRoot is parented under a Canvas. Evidence will not be world-anchored.");
                valid = false;
            }

            // SceneRoot must not have RectTransform
            if (GetComponent<RectTransform>() != null)
            {
                Debug.LogError("[SceneRootController] SceneRoot has RectTransform. It must be a world-space Transform.");
                valid = false;
            }

            // Check that SceneRoot is not parented under ARCamera or CameraOffset
            Transform current = transform.parent;
            while (current != null)
            {
                string lowerName = current.name.ToLowerInvariant();
                if (lowerName.Contains("camera") && !lowerName.Contains("manager"))
                {
                    Debug.LogError("[SceneRootController] SceneRoot is parented under camera hierarchy (" + current.name + "). Move it under XROrigin instead.");
                    valid = false;
                    break;
                }
                current = current.parent;
            }

            return valid;
        }

        /// <summary>
        /// Validates that a child evidence object is properly configured as a 3D world object.
        /// Returns true if valid, false otherwise. Issues are logged as errors.
        /// </summary>
        public bool ValidateEvidenceChild(Transform child)
        {
            if (child == null)
            {
                return true;
            }

            bool valid = true;

            // Evidence must not have RectTransform anywhere in its hierarchy
            if (child.GetComponentInChildren<RectTransform>(true) != null)
            {
                Debug.LogError("[SceneRootController] Evidence '" + child.name + "' contains RectTransform. Evidence must be 3D world objects only.");
                valid = false;
            }

            // Evidence must have a Collider for tap interaction
            if (child.GetComponentInChildren<Collider>(true) == null)
            {
                Debug.LogError("[SceneRootController] Evidence '" + child.name + "' has no Collider. Tap interaction will not work.");
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Validates all current children as evidence objects.
        /// </summary>
        public bool ValidateAllChildren()
        {
            bool valid = ValidateHierarchy();

            foreach (Transform child in transform)
            {
                if (!ValidateEvidenceChild(child))
                {
                    valid = false;
                }
            }

            return valid;
        }
    }
}
