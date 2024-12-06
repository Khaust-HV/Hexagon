using System.Collections;
using UnityEngine;

public sealed class HexagonControl : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    public bool IsRotateActive { get; private set; }

    public void StartRandomRotation()
    {
        if (IsRotateActive) return;

        IsRotateActive = true;

        bool rotateAroundX = Random.Range(0, 2) == 0;

        int direction = Random.Range(0, 2) == 0 ? 1 : -1;

        Vector3 rotationAxis;

        if (rotateAroundX) {
            rotationAxis = Vector3.right * direction;
        } else {
            rotationAxis = Vector3.forward * direction;
        }

        float targetAngle = 180f * direction;

        StartCoroutine(RotateOverTime(rotationAxis, targetAngle, direction));
    }

    private IEnumerator RotateOverTime(Vector3 rotationAxis, float targetAngle, int direction)
    {
        float rotatedAngle = 0f;
        
        while (Mathf.Abs(rotatedAngle) < Mathf.Abs(targetAngle))
        {
            float stepAngle = rotationSpeed * direction * Time.deltaTime;

            if (Mathf.Abs(rotatedAngle + stepAngle) > Mathf.Abs(targetAngle))
            {
                stepAngle = targetAngle - rotatedAngle;
            }

            transform.Rotate(rotationAxis, stepAngle, Space.World);

            rotatedAngle += stepAngle;

            yield return null;
        }

        IsRotateActive = false;
    }
}