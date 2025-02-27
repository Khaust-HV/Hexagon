using UnityEngine;
using System.Collections;
using TMPro;
using LevelObjectType;

namespace HexagonObjectControl {
    public sealed class EmptyHexagonObjectAura : HexagonObjectAura {
        [SerializeField] private MeshRenderer _mrAura;
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private Transform _trText;

        private Transform _trCamera;

        protected override void SetBaseConfiguration() {
            _trCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

            _mrAura.enabled = false;

            SetMaterial();

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

        private void SetMaterial() {
            MaterialPropertyBlock _baseMaterialPropertyBlock = new MaterialPropertyBlock();
            _baseMaterialPropertyBlock.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _baseMaterialPropertyBlock.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);

            _mrAura.material = _visualEffectsConfigs.DissolveAndHitWithUV;
            _mrAura.SetPropertyBlock(_baseMaterialPropertyBlock);
        }

        protected override void SetHexagonObjectWorkActive(bool isActive) {
            if (isActive) {
                _text.text = _hexagonObjectPartType.ToString();

                _mrAura.enabled = true;

                StartCoroutine(TextLookedStarted());
            } else {
                _text.text = "";

                _mrAura.enabled = false;

                StopAllCoroutines();
            }
        }

        private IEnumerator TextLookedStarted() {
            while (true) {
                _trText.transform.LookAt(_trCamera);
                
                yield return null;
            }
        }
    }
}