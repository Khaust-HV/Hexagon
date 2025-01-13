using UnityEngine;
using HexagonObjectControl;
using System.Collections;
using TMPro;
using LevelObjectType;

public class EmptyHexagonObject : HexagonObjectElement {
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private Transform _trText;

    private Transform _trCamera;

    protected override void SetHexagonObjectWorkActive(bool isActive) {
        base.SetHexagonObjectWorkActive(isActive);

        if (isActive) {
            if (_trCamera == null) { 
                _trCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

                switch (_hexagonObjectType) {
                    case DecorationHexagonObjectsType:
                        _text.color = _hexagonObjectConfigs.DecorationTypeTextColor;
                    break;

                    case MineHexagonObjectsType:
                        _text.color = _hexagonObjectConfigs.MineTypeTextColor;
                    break;

                    case BuildebleFieldHexagonObjectsType:
                        _text.color = _hexagonObjectConfigs.BuildebleFieldTypeTextColor;
                    break;

                    case UnBuildebleFieldHexagonObjectsType:
                        _text.color = _hexagonObjectConfigs.UnBuildebleFieldTypeTextColor;
                    break;

                    case CoreHexagonObjectsType:
                        _text.color = _hexagonObjectConfigs.CoreTypeTextColor;
                    break;

                    case HeapHexagonObjectsType:
                        _text.color = _hexagonObjectConfigs.HeapTypeTextColor;
                    break;

                    case RiverHexagonObjectsType:
                        _text.color = _hexagonObjectConfigs.RiverTypeTextColor;
                    break;
                }
            }

            _text.text = _hexagonObjectType.ToString();

            StartCoroutine(TextRotation());
        } else _text.text = "";
    }

    private IEnumerator TextRotation() {
        while (true) {
            _trText.transform.LookAt(_trCamera);
            
            yield return null;
        }
    }
}