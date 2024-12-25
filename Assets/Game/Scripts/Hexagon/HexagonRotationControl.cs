using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace Hexagon {
    public sealed class HexagonRotationControl : MonoBehaviour {
        #region Hexagon Configs Settings
            private float _rotationTime;

            private float _minTimeForAutoHexagonRotate;
            private float _maxTimeForAutoHexagonRotate;
        #endregion

        public event Action HexagonRandomRotation;

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            // Set configurations
            _rotationTime = hexagonConfigs.RotationTime;
            _minTimeForAutoHexagonRotate = hexagonConfigs.MinTimeForAutoHexagonRotate;
            _maxTimeForAutoHexagonRotate = hexagonConfigs.MaxTimeForAutoHexagonRotate;
        }

        public void StopAllActions() {
            StopAllCoroutines();
        }

        public void StartRandomRotation() {
            StartCoroutine(RandomHexagonRotation());
        }

        private IEnumerator RandomHexagonRotation() {
            while (true) {
                float timeToRotate = UnityEngine.Random.Range(_minTimeForAutoHexagonRotate, _maxTimeForAutoHexagonRotate);

                yield return new WaitForSeconds(timeToRotate);

                HexagonRandomRotation?.Invoke();
            }
        }

        public void StartRotation() {
            StartCoroutine(HexagonRotation());
        }

        private IEnumerator HexagonRotation() {
            bool rotateAroundX = UnityEngine.Random.Range(0, 2) == 0;
            int direction = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

            Vector3 rotationAxis = (rotateAroundX ? Vector3.right : Vector3.forward) * direction;

            float targetAngle = 180f;
            float rotatedAngle = 0f;
            float rotationSpeed = targetAngle / _rotationTime;

            while (Mathf.Abs(rotatedAngle) < targetAngle) {
                float stepAngle = rotationSpeed * Time.deltaTime;
                if (rotatedAngle + stepAngle > targetAngle) {
                    stepAngle = targetAngle - rotatedAngle;
                }

                transform.Rotate(rotationAxis, stepAngle, Space.World);
                rotatedAngle += stepAngle;

                yield return null;
            }
        }
    }
}