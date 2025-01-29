using UnityEngine;
using System.Collections;
using TMPro;
using LevelObjectType;

namespace HexagonObjectControl {
    public class EmptyHexagonObjectElement : HexagonObjectElement {
        [SerializeField] private Transform _trText;
        [SerializeField] private TextMeshPro _text;

        private Transform _trCamera;
        private Animator _animator;

        protected override void SetBaseConfiguration() {
            base.SetBaseConfiguration();

            _trCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            _animator = GetComponent<Animator>();

            SetTextColor();
        }

        private void SetTextColor() {
            switch (_hexagonObjectPartType) {
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

                default:
                    throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType);
                // break;
            }
        }

        protected override void SetAnimationActive(bool isActive) {
            if (!_isObjectHaveAnimation) return;

            if (isActive) _animator.enabled = true;
            else _animator.enabled = false;
        }

        // protected override void SetHexagonObjectWorkActive(bool isActive) {
        //     if (isActive) {
        //         _text.text = _hexagonObjectPartType.ToString();

        //         StartCoroutine(TextLookedStarted());
        //     } else {
        //         _text.text = "";

        //         StopAllCoroutines();
        //     }
        // }

        private IEnumerator TextLookedStarted() {
            while (true) {
                _trText.transform.LookAt(_trCamera);
                
                yield return null;
            }
        }
    }
}