using System.Collections;
using UnityEngine;

namespace HexagonObjectControl {
    public sealed class ShieldAuraMoveControl : MonoBehaviour {
        [SerializeField] private float _orbitSpeed;
        [SerializeField] private float _orbitRadius;
        [SerializeField] private float _verticalSpeed;
        [SerializeField] private float _minHeight;
        [SerializeField] private float _maxHeight;

        private bool _isRises;

        private void Start() {
            SetMoveActive(true);
        }

        public void SetMoveActive(bool isActive) {
            if (isActive) {
                StartCoroutine(MoveStarted());
            } else {
                StopAllCoroutines();
            }
        }

        private IEnumerator MoveStarted() {
            Vector3 currentPos = transform.localPosition;
            float currentAngle = Mathf.Atan2(currentPos.z, currentPos.x) * Mathf.Rad2Deg;

            while (true) {
                //Orbital movement

                float deltaTime = Time.deltaTime;

                currentAngle -= _orbitSpeed * deltaTime;

                float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * _orbitRadius;

                float y = transform.localPosition.y;

                if (_isRises) {
                    y = Mathf.Lerp(y, _maxHeight, _verticalSpeed * deltaTime);

                    if (_maxHeight - y <= 0.01f) _isRises = false;
                } else {
                    y = Mathf.Lerp(y, _minHeight, _verticalSpeed * deltaTime);

                    if (_minHeight - y <= 0.01f) _isRises = true;
                }

                float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * _orbitRadius;

                Vector3 newPosition = new Vector3(x, y, z);

                transform.localPosition = newPosition;

                float angleY = Mathf.Atan2(newPosition.x, newPosition.z) * Mathf.Rad2Deg;

                transform.localRotation = Quaternion.Euler(0, angleY + 90f, 0);

                yield return null;
            }
        }
    }
}