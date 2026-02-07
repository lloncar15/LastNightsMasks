using System;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using LastNightsMasks.Interactable;
using UnityEngine.Rendering;

namespace LastNightsMasks.Player {
    /// <summary>
    /// Handles camera zoom effects by adjusting field of view.
    /// Use for focusing on objects, dialogue close-ups, etc.
    /// </summary>
    public class CameraController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Renderer playerRenderer;

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

        public async Task OnEndGameRotation() {
            Transform cam = playerCamera.transform;
            Vector3 startPos = cam.position;
            Quaternion startRot = cam.rotation;
            
            Vector3 endPos = startPos + cam.forward * 2f + cam.right * 1.5f + Vector3.up * 1f;
            Quaternion endRot = Quaternion.Euler(20f, cam.eulerAngles.y + 220f, 0);
            
            float duration = 1.5f;
            float elapsed = 0f;

            bool addedRabbitToMask = false;
            float durationHalved = duration / 2;
            
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float smoothT = t * t * (3f - 2f * t);

                if (!addedRabbitToMask && elapsed >= durationHalved) {
                    addedRabbitToMask = true;
                    MakeRabbitVisible();
                }
        
                cam.position = Vector3.Lerp(startPos, endPos, smoothT);
                cam.rotation = Quaternion.Slerp(startRot, endRot, smoothT);
        
                await Task.Yield();
            }
            
            cam.position = endPos;
            cam.rotation = endRot;
        }

        private void MakeRabbitVisible() {
            playerRenderer.shadowCastingMode = ShadowCastingMode.On;
        }
    }
}