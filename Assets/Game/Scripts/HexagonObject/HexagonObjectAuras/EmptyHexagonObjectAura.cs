using UnityEngine;
using System.Collections;
using TMPro;
using LevelObjectType;

namespace HexagonObjectControl {
    public sealed class EmptyHexagonObjectAura : HexagonObjectAura {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private Transform _trText;

        private Transform _trCamera;

        protected override void SetBaseConfiguration() {
            base.SetBaseConfiguration();

            _trCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

            SetTextColor();
        }

        private void SetTextColor() {
            switch (_hexagonObjectPartType) {
                case ElementAuraType:
                    _text.color = _hexagonObjectConfigs.ElementAuraTypeTextColor;
                break;

                case StatsAuraType:
                    _text.color = _hexagonObjectConfigs.StatsAuraTypeTextColor;
                break;

                case BuildAuraType:
                    _text.color = _hexagonObjectConfigs.BuildAuraTypeTextColor;
                break;

                case TrailAuraType:
                    _text.color = _hexagonObjectConfigs.TrailAuraTypeTextColor;
                break;

                default:
                    throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType);
                // break;
            }
        }

        protected override void SetHexagonObjectWorkActive(bool isActive) {
            base.SetHexagonObjectWorkActive(isActive);

            if (isActive) {
                _text.text = _hexagonObjectPartType.ToString();

                StartCoroutine(StartTextLooked());
            } else _text.text = "";
        }

        private IEnumerator StartTextLooked() {
            while (true) {
                _trText.transform.LookAt(_trCamera);
                
                yield return null;
            }
        }
    }
}