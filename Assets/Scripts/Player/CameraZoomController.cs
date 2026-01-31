using System;
using UnityEngine;
using DG.Tweening;
using LastNightsMasks.Interactable;

namespace LastNightsMasks.Player {
    /// <summary>
    /// Handles camera zoom effects by adjusting field of view.
    /// Use for focusing on objects, dialogue close-ups, etc.
    /// </summary>
    public class CameraZoomController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Camera playerCamera;

        [Header("Settings")]
        [SerializeField] private float defaultFOV = 60f;
        [SerializeField] private float zoomedFOV = 40f;
        [SerializeField] private float zoomDuration = 0.3f;
        [SerializeField] private Ease zoomEase = Ease.InOutQuad;

        private Tweener _fovTween;
        private Tweener _rotationTween;
        private Quaternion _originalLocalRotation;

        private void Awake()
        {
            if (playerCamera == null) {
                playerCamera = Camera.main;
            }
            
            playerCamera.fieldOfView = defaultFOV;
        }

        private void OnEnable() {
            InteractableObject.InteractedWithObject += InteractedWithObject;
            InteractableObject.FinishedInteractingWithObject += FinishedInteractingWithObject;
        }
        
        private void OnDisable() {
            InteractableObject.InteractedWithObject -= InteractedWithObject;
            InteractableObject.FinishedInteractingWithObject -= FinishedInteractingWithObject;
        }

        private void InteractedWithObject(Transform trans) {
            ZoomInToward(trans.position);
        }

        private void FinishedInteractingWithObject() {
            ZoomOut();
        }

        private void OnDestroy() {
            // Clean up tweens when destroyed
            _fovTween?.Kill();
            _rotationTween?.Kill();
        }

        /// <summary>
        /// Zooms in while rotating to look at a target point.
        /// </summary>
        public void ZoomInToward(Vector3 targetPoint, Action onComplete = null)
        {
            _fovTween?.Kill();
            _rotationTween?.Kill();

            // Store current rotation so we can restore it on zoom out
            _originalLocalRotation = playerCamera.transform.localRotation;

            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - playerCamera.transform.position);

            _fovTween = playerCamera
                .DOFieldOfView(zoomedFOV, zoomDuration)
                .SetEase(zoomEase)
                .OnComplete(() => onComplete?.Invoke());

            _rotationTween = playerCamera.transform
                .DORotateQuaternion(targetRotation, zoomDuration)
                .SetEase(zoomEase);
        }

        /// <summary>
        /// Zooms back out to default FOV and restores original rotation.
        /// </summary>
        public void ZoomOut(Action onComplete = null)
        {
            _fovTween?.Kill();
            _rotationTween?.Kill();

            _fovTween = playerCamera
                .DOFieldOfView(defaultFOV, zoomDuration)
                .SetEase(zoomEase)
                .OnComplete(() => onComplete?.Invoke());

            _rotationTween = playerCamera.transform
                .DOLocalRotateQuaternion(_originalLocalRotation, zoomDuration)
                .SetEase(zoomEase);
        }
    }
}