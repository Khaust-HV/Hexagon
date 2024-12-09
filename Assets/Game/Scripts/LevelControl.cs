using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class LevelControl : MonoBehaviour, IHexagonTarget
{
    [Header("Level generate settings")]
    [SerializeField] private GameObject _hexagonPrefab;
    [SerializeField] private float _numberOfRings;

    private List<HexagonControl> _hexagonList;

    [Inject]
    private void Construct() {
        _hexagonList = new List<HexagonControl>();

        GenerateLevel();
    }

    private void Start() {
        StartCoroutine(TestRotate()); // FIX IT !
    }

    public bool SetHexagonTarget(int hexagonID) {
        return true; // !
    }

    private void GenerateLevel()
    {
        Transform trHexagons = new GameObject("Hexagons").transform;
        trHexagons.SetParent(transform);

        float hexagonRadius = _hexagonPrefab.transform.localScale.x * 1.2f;

        _hexagonList.Add(Instantiate (
            _hexagonPrefab,
            Vector3.zero, 
            Quaternion.identity, 
            trHexagons
        ).GetComponent<HexagonControl>());

        float xOffset = hexagonRadius * 1.5f;
        float zOffset = hexagonRadius * Mathf.Sqrt(3) * 0.86f;

        for (int ring = 1; ring <= _numberOfRings; ring++)
        {
            for (int side = 0; side < 6; side++)
            {
                for (int step = 0; step < ring; step++)
                {
                    float x = (ring - step) * xOffset * Mathf.Cos(Mathf.PI / 3 * side) + step * xOffset * Mathf.Cos(Mathf.PI / 3 * (side + 1));
                    float z = (ring - step) * zOffset * Mathf.Sin(Mathf.PI / 3 * side) + step * zOffset * Mathf.Sin(Mathf.PI / 3 * (side + 1));

                    Vector3 offset = new Vector3(x, 0, z);

                    _hexagonList.Add(Instantiate (
                        _hexagonPrefab, 
                        offset, 
                        Quaternion.identity, 
                        trHexagons
                    ).GetComponent<HexagonControl>());
                }
            }
        }
    }

    private IEnumerator TestRotate() { // FIX IT !
        while (true) {
            int randomIndex = Random.Range(0, _hexagonList.Count);

            _hexagonList[randomIndex].StartRandomRotation();

            yield return new WaitForSeconds(0.25f);
        }
    }
}

public interface IHexagonTarget {
    public bool SetHexagonTarget(int hexagonID);
}