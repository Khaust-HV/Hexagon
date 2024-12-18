using UnityEngine;

public sealed class HexagonDestroyControl : MonoBehaviour {
    [Header("HexagonLP settings")]
    [SerializeField] private GameObject _hexagonLP;
    [Header("FragileHexagon settings")]
    [SerializeField] private GameObject _fragileHexagon;
    [SerializeField] private MeshCollider[] _mcFragileHexagonParts;
    [SerializeField] private Rigidbody[] _rbFragileHexagonParts;
    [Header("FragileHexagon settings")]
    [SerializeField] private GameObject _destroyedHexagon;
    [SerializeField] private Rigidbody[] _rbDestroyedHexagonParts;

    public void DestroyPlannedHexagon() {
        _hexagonLP.SetActive(false);

        for (int i = 0; i < _mcFragileHexagonParts.Length; i++) {
            _mcFragileHexagonParts[i].enabled = true;
            _rbFragileHexagonParts[i].isKinematic = false;
            _rbFragileHexagonParts[i].AddExplosionForce(50f, transform.position + Random.onUnitSphere * 5f, 5f, 1f, ForceMode.Impulse);
        }
    }

    public void DestroyForceHexagon() {
        _hexagonLP.SetActive(false);
        _fragileHexagon.SetActive(false);
        _destroyedHexagon.SetActive(true);

        for (int i = 0; i < _rbDestroyedHexagonParts.Length; i++) {
            _rbDestroyedHexagonParts[i].AddExplosionForce(50f, transform.position + Random.onUnitSphere * 5f, 5f, 1f, ForceMode.Impulse);
        }
    }
}