using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

namespace HexagonControl {
    public sealed class HexagonRotationControl : MonoBehaviour {
        public event Action HexagonRandomRotation;
        public bool IsHexagonRotation { get; private set; }

        private IEnumerator _hexagonRotationFromTimeStarted;

        private static readonly Vector3[] edgeCenters = new Vector3[] {
            new Vector3(0.5f, 0f, 0.866f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0.5f, 0f, -0.866f),
            new Vector3(-0.5f, 0f, -0.866f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(-0.5f, 0f, 0.866f)
        };

        #region DI
            private HexagonConfigs _hexagonConfigs;
        #endregion

        [Inject]
        private void Construct(HexagonConfigs hexagonConfigs) {
            // Set configurations
            _hexagonConfigs = hexagonConfigs;
        }

        public void StopAllActions() {
            StopAllCoroutines();

            IsHexagonRotation = false;
        }

        public void RotationFromTimeEnable() {
            if (_hexagonRotationFromTimeStarted != null) StopCoroutine(_hexagonRotationFromTimeStarted);

            StartCoroutine(_hexagonRotationFromTimeStarted = HexagonRotationFromTimeStarted());
        }

        private IEnumerator HexagonRotationFromTimeStarted() {
            while (true) {
                float timeToRotate = UnityEngine.Random.Range(_hexagonConfigs.MinTimeForAutoHexagonRotate, _hexagonConfigs.MaxTimeForAutoHexagonRotate);

                yield return new WaitForSeconds(timeToRotate);

                HexagonRandomRotation?.Invoke();
            }
        }

        public void HexagonRotationEnable(DirectionalRotationType directionalRotationType) {
            StartCoroutine(HexagonRotationStarted(directionalRotationType));
        }

        private IEnumerator HexagonRotationStarted(DirectionalRotationType directionalRotationType) {
            IsHexagonRotation = true;

            Vector3 edgeDirection = (directionalRotationType switch {
                DirectionalRotationType.Random => edgeCenters[UnityEngine.Random.Range(0, edgeCenters.Length)],
                DirectionalRotationType.TopRightSide => edgeCenters[3],
                DirectionalRotationType.RightSide => edgeCenters[4],
                DirectionalRotationType.BottomRightSide => edgeCenters[5],
                DirectionalRotationType.BottomLeftSide => edgeCenters[0],
                DirectionalRotationType.LeftSide => edgeCenters[1],
                DirectionalRotationType.TopLeftSife => edgeCenters[2],
                _ => throw new ArgumentOutOfRangeException(nameof(directionalRotationType), "Invalid hexagon rotation type")
            }).normalized;

            Vector3 rotationAxis = Vector3.Cross(Vector3.up, edgeDirection);

            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.AngleAxis(180f, rotationAxis) * startRotation;

            float elapsedTime = 0f;

            while (elapsedTime < _hexagonConfigs.RotationTime) {
                elapsedTime += Time.deltaTime;

                float t = elapsedTime / _hexagonConfigs.RotationTime;

                transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

                yield return null;
            }

            transform.rotation = endRotation;

            IsHexagonRotation = false;
        }
    }
}

public enum DirectionalRotationType {
    Random,
    TopRightSide,
    RightSide,
    BottomRightSide,
    BottomLeftSide,
    LeftSide,
    TopLeftSife
}