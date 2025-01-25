using System.Collections;
using UnityEngine;

namespace HexagonObjectControl {
    public sealed class ShieldHexagonObjectAura : HexagonObjectAura {
        [SerializeField] private MeshRenderer _mrShieldAura;
        [SerializeField] private GameObject[] _shields;
        [Header("Dissolve effect settings")]
        [SerializeField] private float _destroyStartCutoffHeight;
        [SerializeField] private float _destroyFinishCutoffHeight;

        private ShieldAuraShieldControl[] _shildsControl;
        private MeshRenderer[] _mrShilds;

        private Material _shieldMaterial;
        private Material _auraShieldMaterial;

        private AuraEfficiencyType _auraEfficiencyType;

        protected override void SetBaseConfiguration() {
            _mrShieldAura = _mrShieldAura.GetComponent<MeshRenderer>();

            _shildsControl = new ShieldAuraShieldControl[_shields.Length];
            _mrShilds = new MeshRenderer[_shields.Length];

            for (int i = 0; i < _shields.Length; i++) {
                _shildsControl[i] = _shields[i].GetComponent<ShieldAuraShieldControl>();
                (_mrShilds[i] = _shields[i].GetComponent<MeshRenderer>()).enabled = false;
            }

            _mrShieldAura.enabled = false;

            SetMaterial();
        }

        private void SetMaterial() {
            _shieldMaterial = new Material(_visualEffectsConfigs.EmissionFullObject);
            _shieldMaterial.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _shieldMaterial.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);
            _shieldMaterial.SetColor("_FresnelColor", _visualEffectsConfigs.ShieldAuraEmissionFresnelColor);
            _shieldMaterial.SetFloat("_FresnelPower", _visualEffectsConfigs.ShieldAuraEmissionFresnelPower);
            _shieldMaterial.SetColor("_EmissionColor", _visualEffectsConfigs.ShieldAuraEmissionColor);

            foreach (var shield in _mrShilds) {
                shield.material = _shieldMaterial;
            }

            _auraShieldMaterial = new Material(_visualEffectsConfigs.DissolveAndEmissionFullObjectAndVerticalNoice);
            _auraShieldMaterial.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _auraShieldMaterial.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);
            _auraShieldMaterial.SetColor("_FresnelColor", _visualEffectsConfigs.ShieldAuraEmissionFresnelColor);
            _auraShieldMaterial.SetFloat("_FresnelPower", _visualEffectsConfigs.ShieldAuraEmissionFresnelPower);
            _auraShieldMaterial.SetColor("_EmissionColor", _visualEffectsConfigs.ShieldAuraEmissionColor);
            _auraShieldMaterial.SetFloat("VerticalNoiceScale", _visualEffectsConfigs.ShieldAuraVerticalNoiceScale);
            _auraShieldMaterial.SetFloat("DissolveNoiseScale", _visualEffectsConfigs.DefaultDestroyNoiseScale);
            _auraShieldMaterial.SetFloat("NoiseStrength", _visualEffectsConfigs.DefaultDestroyNoiseStrength);

            _mrShieldAura.material = _auraShieldMaterial;
        }

        public override void SetAuraEfficiency(AuraEfficiencyType auraEfficiencyType) {
            _auraEfficiencyType = auraEfficiencyType;

            // Set aura efficiency
            switch (_auraEfficiencyType) { // FIX IT !
                case AuraEfficiencyType.LowEfficiency:

                break;

                case AuraEfficiencyType.StandardEfficiency:

                break;

                case AuraEfficiencyType.HighEfficiency:

                break;

                case AuraEfficiencyType.ReallyHighEfficiency:

                break;
            }
        }

        protected override void SetHexagonObjectWorkActive(bool isActive) {
            if (isActive) {
                switch (_auraEfficiencyType) {
                    case AuraEfficiencyType.LowEfficiency:
                        _shields[3].transform.localPosition = _hexagonObjectConfigs.OneShieldPosition;

                        _mrShilds[3].enabled = true;
                        _mrShieldAura.enabled = true;

                        _shildsControl[3].SetMoveActive(true, _hexagonObjectConfigs.MaxHeightShieldAuraLowEfficiency);

                        _shildsControl[3].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);

                        StartCoroutine(RaiseTheShield(_visualEffectsConfigs.ShieldAuraHeightLowEfficiency));
                    break;

                    case AuraEfficiencyType.StandardEfficiency:
                        _shields[0].transform.localPosition = _hexagonObjectConfigs.OneShieldPosition;

                        _mrShilds[0].enabled = true;
                        _mrShieldAura.enabled = true;
                        
                        _shildsControl[0].SetMoveActive(true, _hexagonObjectConfigs.MaxHeightShieldAuraStandardEfficiency);
                        _shildsControl[0].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);

                        StartCoroutine(RaiseTheShield(_visualEffectsConfigs.ShieldAuraHeightStandardEfficiency));
                    break;

                    case AuraEfficiencyType.HighEfficiency:
                        _shields[0].transform.localPosition = _hexagonObjectConfigs.TwoShieldsPosition[0];
                        _shields[1].transform.localPosition = _hexagonObjectConfigs.TwoShieldsPosition[1];

                        _mrShilds[0].enabled = true;
                        _mrShilds[1].enabled = true;
                        _mrShieldAura.enabled = true;

                        _shildsControl[0].SetMoveActive(true, _hexagonObjectConfigs.MaxHeightShieldAuraHighEfficiency);
                        _shildsControl[1].SetMoveActive(true, _hexagonObjectConfigs.MaxHeightShieldAuraHighEfficiency);

                        _shildsControl[0].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);
                        _shildsControl[1].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);

                        StartCoroutine(RaiseTheShield(_visualEffectsConfigs.ShieldAuraHeightHighEfficiency));
                    break;

                    case AuraEfficiencyType.ReallyHighEfficiency:
                        _shields[0].transform.localPosition = _hexagonObjectConfigs.ThreeShieldsPosition[0];
                        _shields[1].transform.localPosition = _hexagonObjectConfigs.ThreeShieldsPosition[1];
                        _shields[2].transform.localPosition = _hexagonObjectConfigs.ThreeShieldsPosition[2];

                        _mrShilds[0].enabled = true;
                        _mrShilds[1].enabled = true;
                        _mrShilds[2].enabled = true;
                        _mrShieldAura.enabled = true;

                        _shildsControl[0].SetMoveActive(true, _hexagonObjectConfigs.MaxHeightShieldAuraReallyHighEfficiency);
                        _shildsControl[1].SetMoveActive(true, _hexagonObjectConfigs.MaxHeightShieldAuraReallyHighEfficiency);
                        _shildsControl[2].SetMoveActive(true, _hexagonObjectConfigs.MaxHeightShieldAuraReallyHighEfficiency);
                        
                        _shildsControl[0].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);
                        _shildsControl[1].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);
                        _shildsControl[2].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);

                        StartCoroutine(RaiseTheShield(_visualEffectsConfigs.ShieldAuraHeightReallyHighEfficiency));
                    break;
                }
            } else {
                StopAllCoroutines();

                StartCoroutine(LowerTheShield());

                switch (_auraEfficiencyType) {
                    case AuraEfficiencyType.LowEfficiency:
                        _mrShilds[3].enabled = false;

                        _shildsControl[3].SetMoveActive(false);

                        _shildsControl[3].ShieldEffectEnable(ShieldAuraEffectType.DestroyShield);
                    break;

                    case AuraEfficiencyType.StandardEfficiency:
                        _mrShilds[0].enabled = false;
                        
                        _shildsControl[0].SetMoveActive(false);
                        _shildsControl[0].ShieldEffectEnable(ShieldAuraEffectType.DestroyShield);
                    break;

                    case AuraEfficiencyType.HighEfficiency:
                        _mrShilds[0].enabled = false;
                        _mrShilds[1].enabled = false;

                        _shildsControl[0].SetMoveActive(false);
                        _shildsControl[1].SetMoveActive(false);

                        _shildsControl[0].ShieldEffectEnable(ShieldAuraEffectType.DestroyShield);
                        _shildsControl[1].ShieldEffectEnable(ShieldAuraEffectType.DestroyShield);
                    break;

                    case AuraEfficiencyType.ReallyHighEfficiency:
                        _mrShilds[0].enabled = false;
                        _mrShilds[1].enabled = false;
                        _mrShilds[2].enabled = false;

                        _shildsControl[0].SetMoveActive(false);
                        _shildsControl[1].SetMoveActive(false);
                        _shildsControl[2].SetMoveActive(false);
                        
                        _shildsControl[0].ShieldEffectEnable(ShieldAuraEffectType.DestroyShield);
                        _shildsControl[1].ShieldEffectEnable(ShieldAuraEffectType.DestroyShield);
                        _shildsControl[2].ShieldEffectEnable(ShieldAuraEffectType.DestroyShield);
                    break;
                }
            }
        }

        private IEnumerator RaiseTheShield(float shieldAuraHeight) {
            _auraShieldMaterial.SetFloat("_CutoffHeight", _destroyStartCutoffHeight);

            float elapsedTime = 0f;

            float spawnEffectTime = _visualEffectsConfigs.ShieldAuraSpawnTime;

            while (elapsedTime < spawnEffectTime) {
                float currentValue = Mathf.Lerp(0f, shieldAuraHeight, elapsedTime / spawnEffectTime);

                _auraShieldMaterial.SetFloat("_AuraHeight", currentValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _auraShieldMaterial.SetFloat("_AuraHeight", shieldAuraHeight);
        }

        private IEnumerator LowerTheShield() {
            float elapsedTime = 0f;

            float destroyEffectTime = _visualEffectsConfigs.ShieldAuraDestroyTime;

            while (elapsedTime < destroyEffectTime) {
                float currentValue = Mathf.Lerp(_destroyStartCutoffHeight, _destroyFinishCutoffHeight, elapsedTime / destroyEffectTime);

                _auraShieldMaterial.SetFloat("_CutoffHeight", currentValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _auraShieldMaterial.SetFloat("_CutoffHeight", _destroyFinishCutoffHeight);

            _mrShieldAura.enabled = false;
        }
    }
}