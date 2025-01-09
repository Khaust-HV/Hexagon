using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace Hexagon {
    public sealed class HexagonRotationControl : MonoBehaviour {
        private HexagonConfigs _hexagonConfigs;

        public event Action HexagonRandomRotation;
        public bool IsHexagonRotation { get; private set; }

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            // Set configurations
            _hexagonConfigs = hexagonConfigs;
        }

        public void StopAllActions() {
            StopAllCoroutines();
        }

        public void StartRandomRotation() {
            StartCoroutine(RandomHexagonRotation());
        }

        private IEnumerator RandomHexagonRotation() {
            while (true) {
                float timeToRotate = UnityEngine.Random.Range(_hexagonConfigs.MinTimeForAutoHexagonRotate, _hexagonConfigs.MaxTimeForAutoHexagonRotate);

                yield return new WaitForSeconds(timeToRotate);

                HexagonRandomRotation?.Invoke();
            }
        }

        public void StartRotation() {
            StartCoroutine(HexagonRotation());
        }

        private IEnumerator HexagonRotation() {
            IsHexagonRotation = true;

            bool rotateAroundX = UnityEngine.Random.Range(0, 2) == 0;
            int direction = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

            Vector3 rotationAxis = (rotateAroundX ? Vector3.right : Vector3.forward) * direction;

            float targetAngle = 180f;
            float rotatedAngle = 0f;
            float rotationSpeed = targetAngle / _hexagonConfigs.RotationTime;

            while (Mathf.Abs(rotatedAngle) < targetAngle) {
                float stepAngle = rotationSpeed * Time.deltaTime;
                if (rotatedAngle + stepAngle > targetAngle) {
                    stepAngle = targetAngle - rotatedAngle;
                }

                transform.Rotate(rotationAxis, stepAngle, Space.World);
                rotatedAngle += stepAngle;

                yield return null;
            }

            IsHexagonRotation = false;
        }
    }
}