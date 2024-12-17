using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class HexagonController : MonoBehaviour, IHexagonControl {
    [Header("Hexagon object points")]
    [SerializeField] private Transform _firstObjectPoist;
    [SerializeField] private Transform _secondObjectPoist;

    #region Hexagon Settings
        private float _rotationSpeed;

        private Material _defaultHexagonMaterial;
        private Material _shadowHexagonMaterial;
        private Material _randomHexagonMaterial;
        private Material _fragileHexagonMaterial;
        private Material _temporaryHexagonMaterial;

        private float _minTimeForAutoHexagonRotate;
        private float _maxTimeForAutoHexagonRotate;

        private int _minNumberRotationsForHexagon;
        private int _maxNumberRotationsForHexagon;

        private float _minTimeSquadForHexagon;
        private float _maxTimeSquadForHexagon;


    #endregion

    private HexagonType _hexagonType;
    private IHexagonObjectControl _currentObject;
    private IHexagonObjectControl _oldObject;
    private MeshRenderer _mhHexagon;
    
    // Hexagon type value
    private bool _isRotation;
    private bool _isCollapses;
    private bool _isRandomRotation;
    private bool _isFragile;
    private EfficiencyOfBuildingsType _efficiencyOfBuildings;
    private int _currentAvailableNumberRotations;
    private IEnumerator _hexagonRotation;
    private IEnumerator _randomRotation;
    private IEnumerator _destroyBecauseSquad;

    public int HexagonID { get; private set; }

    [Inject]
    private void Construct(HexagonConfigs hexagonConfigs) {
        _rotationSpeed = hexagonConfigs.RotationSpeed;

        _defaultHexagonMaterial = hexagonConfigs.DefaultHexagonMaterial;
        _shadowHexagonMaterial = hexagonConfigs.ShadowHexagonMaterial;
        _randomHexagonMaterial = hexagonConfigs.RandomHexagonMaterial;
        _fragileHexagonMaterial = hexagonConfigs.FragileHexagonMaterial;
        _temporaryHexagonMaterial = hexagonConfigs.TemporaryHexagonMaterial;

        _minTimeForAutoHexagonRotate = hexagonConfigs.MinTimeForAutoHexagonRotate;
        _maxTimeForAutoHexagonRotate = hexagonConfigs.MaxTimeForAutoHexagonRotate;
        _minNumberRotationsForHexagon = hexagonConfigs.MinNumberRotationsForHexagon;
        _maxNumberRotationsForHexagon = hexagonConfigs.MaxNumberRotationsForHexagon;
        _minTimeSquadForHexagon = hexagonConfigs.MinTimeSquadForHexagon;
        _maxTimeSquadForHexagon = hexagonConfigs.MaxTimeSquadForHexagon;

        _mhHexagon = gameObject.GetComponent<MeshRenderer>();
    }

    public void SetPositionAndID(Vector3 position, int id) {
        transform.position = position;
        HexagonID = id;
    }

    public void SetHexagonTypeAndEnable(HexagonType hexagonType, bool rotateShadow = false) {
        _hexagonType = hexagonType;
        
        gameObject.SetActive(true);

        switch (hexagonType) {
            case HexagonType.Default:
                _mhHexagon.material = _defaultHexagonMaterial;
                _isRotation = true;
                _isCollapses = true;
                _isRandomRotation = false;
                _isFragile = false;
                _efficiencyOfBuildings = EfficiencyOfBuildingsType.Standard;
            break;

            case HexagonType.Shadow:
                _mhHexagon.material = _shadowHexagonMaterial;
                _isRotation = rotateShadow;
                _isCollapses = false;
                _isRandomRotation = false;
                _isFragile = false;
                if (rotateShadow) _efficiencyOfBuildings = EfficiencyOfBuildingsType.Standard;
                else _efficiencyOfBuildings = EfficiencyOfBuildingsType.Low;
            break;

            case HexagonType.Random:
                _mhHexagon.material = _randomHexagonMaterial;
                _isRotation = true;
                _isCollapses = true;
                _isRandomRotation = true;
                _isFragile = false;
                _efficiencyOfBuildings = EfficiencyOfBuildingsType.High;
                StartCoroutine(_randomRotation = RandomRotation());
            break;

            case HexagonType.Fragile:
                _mhHexagon.material = _fragileHexagonMaterial;
                _isRotation = true;
                _isCollapses = true;
                _isRandomRotation = false;
                _isFragile = true;
                _efficiencyOfBuildings = EfficiencyOfBuildingsType.High;
                _currentAvailableNumberRotations = Random.Range(_minNumberRotationsForHexagon, _maxNumberRotationsForHexagon);
            break;

            case HexagonType.Temporary:
                _mhHexagon.material = _temporaryHexagonMaterial;
                _isRotation = true;
                _isCollapses = true;
                _isRandomRotation = true;
                _isFragile = true;
                _efficiencyOfBuildings = EfficiencyOfBuildingsType.VeryHigh;
                _currentAvailableNumberRotations = Random.Range(_minNumberRotationsForHexagon, _maxNumberRotationsForHexagon);
                StartCoroutine(_randomRotation = RandomRotation());
            break;
        }
    }

    private IEnumerator RandomRotation() {
        while (true) {
            float timeToRotate = Random.Range(_minTimeForAutoHexagonRotate, _maxTimeForAutoHexagonRotate);

            yield return new WaitForSeconds(timeToRotate);

            CheckingBeforeRotate();
        }
    }

    private void CheckingBeforeRotate() {
        switch (_hexagonType) {
            case HexagonType.Shadow:
                if (!_isRotation) return;
            break;

            case HexagonType.Fragile:
                if (_currentAvailableNumberRotations - 1 <= 0) {
                    DestroyHexagon();

                    return;
                }
                _currentAvailableNumberRotations--;
            break;

            case HexagonType.Temporary:
                if (_currentAvailableNumberRotations - 1 <= 0) {
                    DestroyHexagon();

                    StopCoroutine(_randomRotation);

                    return;
                }
                _currentAvailableNumberRotations--;
            break;
        }

        // Set new object

        StartCoroutine(_hexagonRotation = HexagonRotation());
    }

    private void DestroyHexagon() {
        
    }

    public void SetFirstObject(IHexagonObjectControl iHexagonObjectControl) {
        _currentObject = iHexagonObjectControl;

        //Set object parent and position
    }

    private IEnumerator HexagonRotation() {
        bool rotateAroundX = Random.Range(0, 2) == 0;
        int direction = Random.Range(0, 2) == 0 ? 1 : -1;

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

public interface IHexagonControl {
    public void SetPositionAndID(Vector3 position, int id);
    public void SetHexagonTypeAndEnable(HexagonType hexagonType, bool rotateShadow = false);
    public void SetFirstObject(IHexagonObjectControl iHexagonObjectControl);
}

public enum HexagonType {
    Default,
    Shadow,
    Random,
    Fragile,
    Temporary
}

public enum EfficiencyOfBuildingsType {
    Low,
    Standard,
    High,
    VeryHigh
}