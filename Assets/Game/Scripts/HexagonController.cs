using System.Collections;
using GameConfigs;
using UnityEngine;
using Zenject;

public sealed class HexagonController : MonoBehaviour, IHexagonControl {
    [Header("Hexagon object points")]
    [SerializeField] private Transform _firstObjectPoint;
    [SerializeField] private Transform _secondObjectPoint;

    #region Hexagon Config Settings
        private int _minNumberRotationsForHexagon;
        private int _maxNumberRotationsForHexagon;

        private float _minTimeSquadForHexagon;
        private float _maxTimeSquadForHexagon;
    #endregion

    private HexagonType _hexagonType;
    private int _currentAvailableNumberRotations;

    private IHexagonObjectControl _currentObject;
    private IHexagonObjectControl _oldObject;

    private IEnumerator _destroyBecauseSquad;

    public int HexagonID { get; private set; }

    private HexagonTypeControl _hexagonTypeControl;
    private HexagonRotationControl _hexagonRotationControl;
    private HexagonDestroyControl _hexagonDestroyControl;

    [Inject]
    private void Construct(HexagonConfigs hexagonConfigs) {
        _minNumberRotationsForHexagon = hexagonConfigs.MinNumberRotationsForHexagon;
        _maxNumberRotationsForHexagon = hexagonConfigs.MaxNumberRotationsForHexagon;
        _minTimeSquadForHexagon = hexagonConfigs.MinTimeSquadForHexagon;
        _maxTimeSquadForHexagon = hexagonConfigs.MaxTimeSquadForHexagon;

        _hexagonTypeControl = GetComponent<HexagonTypeControl>();
        _hexagonRotationControl = GetComponent<HexagonRotationControl>();
        _hexagonDestroyControl = GetComponent<HexagonDestroyControl>();

        _hexagonRotationControl.HexagonRandomRotation += CheckingBeforeRotate;
    }

    public void SetPositionAndID(Vector3 position, int id) {
        transform.position = position;
        HexagonID = id;
    }

    public void SetHexagonTypeAndEnable(HexagonType hexagonType, bool rotateShadow = false) {
        _hexagonType = hexagonType;
        
        gameObject.SetActive(true);

        _hexagonTypeControl.SetHexagonType(hexagonType, rotateShadow);

        _currentAvailableNumberRotations = Random.Range(_minNumberRotationsForHexagon, _maxNumberRotationsForHexagon);

        switch (hexagonType) {
            case HexagonType.Random:
            case HexagonType.Temporary:
                _hexagonRotationControl.StartRandomRotation();
            break;
        }
    }

    private IEnumerator DestroyBecauseSquad() {
        float timeToDestroy = Random.Range(_minTimeSquadForHexagon, _maxTimeSquadForHexagon);

        yield return new WaitForSeconds(timeToDestroy);

        _hexagonDestroyControl.DestroyPlannedHexagon();
    }

    private void CheckingBeforeRotate() {
        switch (_hexagonType) {
            case HexagonType.Shadow:
                if (!_hexagonTypeControl.IsRotation) return;
            break;

            case HexagonType.Fragile:
                if (_currentAvailableNumberRotations - 1 <= 0) {
                    _hexagonDestroyControl.DestroyPlannedHexagon();

                    return;
                }
                _currentAvailableNumberRotations--;
            break;

            case HexagonType.Temporary:
                if (_currentAvailableNumberRotations - 1 <= 0) {
                    StopCoroutine(_hexagonRotationControl.IERandomHexagonRotation);

                    _hexagonDestroyControl.DestroyPlannedHexagon();

                    return;
                }
                _currentAvailableNumberRotations--;
            break;
        }

        // Set new object

        _hexagonRotationControl.StartRotation();
    }

    public void SetFirstObject(IHexagonObjectControl iHexagonObjectControl) {
        _currentObject = iHexagonObjectControl;

        //Set object parent and position
    }
}

public interface IHexagonControl {
    public void SetPositionAndID(Vector3 position, int id);
    public void SetHexagonTypeAndEnable(HexagonType hexagonType, bool rotateShadow = false);
    public void SetFirstObject(IHexagonObjectControl iHexagonObjectControl);
}