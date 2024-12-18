using System;
using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class HexagonRotationControl : MonoBehaviour {
    #region Hexagon Config Settings
        private int _rotationSpeed;

        private float _minTimeForAutoHexagonRotate;
        private float _maxTimeForAutoHexagonRotate;
    #endregion

    public event Action HexagonRandomRotation;

    public IEnumerator IEHexagonRotation { get; private set; }
    public IEnumerator IERandomHexagonRotation { get; private set; }

    [Inject]
    private void Construct(HexagonConfigs hexagonConfigs) {
        _rotationSpeed = hexagonConfigs.RotationSpeed;
        _minTimeForAutoHexagonRotate = hexagonConfigs.MinTimeForAutoHexagonRotate;
        _maxTimeForAutoHexagonRotate = hexagonConfigs.MaxTimeForAutoHexagonRotate;
    }

    public void StartRandomRotation() {
        StartCoroutine(IERandomHexagonRotation = RandomHexagonRotation());
    }

    private IEnumerator RandomHexagonRotation() {
        while (true) {
            float timeToRotate = UnityEngine.Random.Range(_minTimeForAutoHexagonRotate, _maxTimeForAutoHexagonRotate);

            yield return new WaitForSeconds(timeToRotate);

            HexagonRandomRotation?.Invoke();
        }
    }

    public void StartRotation() {
        StartCoroutine(IEHexagonRotation = HexagonRotation());
    }

    private IEnumerator HexagonRotation() {
        bool rotateAroundX = UnityEngine.Random.Range(0, 2) == 0;
        int direction = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

        Vector3 rotationAxis;

        if (rotateAroundX) {
            rotationAxis = Vector3.right * direction;
        } else {
            rotationAxis = Vector3.forward * direction;
        }

        float targetAngle = 180f * direction;

        float rotatedAngle = 0f;
        
        while (Mathf.Abs(rotatedAngle) < Mathf.Abs(targetAngle)) {
            float stepAngle = _rotationSpeed * direction * Time.deltaTime;

            if (Mathf.Abs(rotatedAngle + stepAngle) > Mathf.Abs(targetAngle)) {
                stepAngle = targetAngle - rotatedAngle;
            }

            transform.Rotate(rotationAxis, stepAngle, Space.World);

            rotatedAngle += stepAngle;

            yield return null;
        }
    }
}