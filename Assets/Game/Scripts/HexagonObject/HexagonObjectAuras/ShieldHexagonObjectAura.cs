using System.Collections;
using UnityEngine;

namespace HexagonObjectControl {
    public sealed class ShieldHexagonObjectAura : HexagonObjectAura {
        [SerializeField] private MeshRenderer _mrShieldAura;
        [SerializeField] private GameObject[] _shields;

        private ShieldAuraShieldControl[] _shildsControl;
        private MeshRenderer[] _mrShilds;

        private MaterialPropertyBlock _auraShieldMaterialPropertyBlock;
        private MaterialPropertyBlock _shieldMaterialPropertyBlock;

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
            Material shieldMaterial = _visualEffectsConfigs.EmissionFullObjectGhost;
            _shieldMaterialPropertyBlock = new MaterialPropertyBlock();
            _shieldMaterialPropertyBlock.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _shieldMaterialPropertyBlock.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);
            _shieldMaterialPropertyBlock.SetColor("_FresnelColor", _visualEffectsConfigs.ShieldAuraEmissionFresnelColor);
            _shieldMaterialPropertyBlock.SetFloat("_FresnelPower", _visualEffectsConfigs.ShieldAuraEmissionFresnelPower);
            _shieldMaterialPropertyBlock.SetColor("_EmissionColor", _visualEffectsConfigs.ShieldAuraEmissionColor);

            foreach (var shield in _mrShilds) {
                shield.material = shieldMaterial;
                shield.SetPropertyBlock(_shieldMaterialPropertyBlock);
            }

            Material auraShieldMaterial = _visualEffectsConfigs.DissolveAndEmissionFullObjectAndVerticalNoiceGhost;
            _auraShieldMaterialPropertyBlock = new MaterialPropertyBlock();
            _auraShieldMaterialPropertyBlock.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _auraShieldMaterialPropertyBlock.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);
            _auraShieldMaterialPropertyBlock.SetColor("_FresnelColor", _visualEffectsConfigs.ShieldAuraEmissionFresnelColor);
            _auraShieldMaterialPropertyBlock.SetFloat("_FresnelPower", _visualEffectsConfigs.ShieldAuraEmissionFresnelPower);
            _auraShieldMaterialPropertyBlock.SetColor("_EmissionColor", _visualEffectsConfigs.ShieldAuraEmissionColor);
            _auraShieldMaterialPropertyBlock.SetFloat("VerticalNoiceScale", _visualEffectsConfigs.ShieldAuraVerticalNoiceScale);
            _auraShieldMaterialPropertyBlock.SetFloat("DissolveNoiseScale", _visualEffectsConfigs.DefaultDestroyNoiseScale);
            _auraShieldMaterialPropertyBlock.SetFloat("NoiseStrength", _visualEffectsConfigs.DefaultDestroyNoiseStrength);

            _mrShieldAura.material = auraShieldMaterial;
            _mrShieldAura.SetPropertyBlock(_auraShieldMaterialPropertyBlock);
        }

        protected override void ApplyAuraEfficiency() {
            // In the process of coming up

            switch (_auraEfficiencyType) { 
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

        protected override void SetConfigurationFromHexagonObjectType() {
            // In the process of coming up
        }

        public override void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart) {
            // In the process of coming up
        }

        protected override void SetHexagonObjectWorkActive(bool isActive) {
            if (isActive) {
                switch (_auraEfficiencyType) {
                    case AuraEfficiencyType.LowEfficiency:
                        _shields[3].transform.localPosition = _hexagonObjectConfigs.ShieldAuraOneShieldPosition;

                        _mrShilds[3].enabled = true;
                        _mrShieldAura.enabled = true;

                        _shildsControl[3].SetMoveActive(true, _hexagonObjectConfigs.ShieldAuraMaxHeightLowEfficiency);

                        _shildsControl[3].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);

                        StartCoroutine(FadingLightStarted());
                        StartCoroutine(RaiseTheShield(_visualEffectsConfigs.ShieldAuraHeightLowEfficiency));
                    break;

                    case AuraEfficiencyType.StandardEfficiency:
                        _shields[0].transform.localPosition = _hexagonObjectConfigs.ShieldAuraOneShieldPosition;

                        _mrShilds[0].enabled = true;
                        _mrShieldAura.enabled = true;
                        
                        _shildsControl[0].SetMoveActive(true, _hexagonObjectConfigs.ShieldAuraMaxHeightStandardEfficiency);
                        _shildsControl[0].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);

                        StartCoroutine(FadingLightStarted());
                        StartCoroutine(RaiseTheShield(_visualEffectsConfigs.ShieldAuraHeightStandardEfficiency));
                    break;

                    case AuraEfficiencyType.HighEfficiency:
                        _shields[0].transform.localPosition = _hexagonObjectConfigs.ShieldAuraTwoShieldsPosition[0];
                        _shields[1].transform.localPosition = _hexagonObjectConfigs.ShieldAuraTwoShieldsPosition[1];

                        _mrShilds[0].enabled = true;
                        _mrShilds[1].enabled = true;
                        _mrShieldAura.enabled = true;

                        _shildsControl[0].SetMoveActive(true, _hexagonObjectConfigs.ShieldAuraMaxHeightHighEfficiency);
                        _shildsControl[1].SetMoveActive(true, _hexagonObjectConfigs.ShieldAuraMaxHeightHighEfficiency);

                        _shildsControl[0].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);
                        _shildsControl[1].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);

                        StartCoroutine(FadingLightStarted());
                        StartCoroutine(RaiseTheShield(_visualEffectsConfigs.ShieldAuraHeightHighEfficiency));
                    break;

                    case AuraEfficiencyType.ReallyHighEfficiency:
                        _shields[0].transform.localPosition = _hexagonObjectConfigs.ShieldAuraThreeShieldsPosition[0];
                        _shields[1].transform.localPosition = _hexagonObjectConfigs.ShieldAuraThreeShieldsPosition[1];
                        _shields[2].transform.localPosition = _hexagonObjectConfigs.ShieldAuraThreeShieldsPosition[2];

                        _mrShilds[0].enabled = true;
                        _mrShilds[1].enabled = true;
                        _mrShilds[2].enabled = true;
                        _mrShieldAura.enabled = true;

                        _shildsControl[0].SetMoveActive(true, _hexagonObjectConfigs.ShieldAuraMaxHeightReallyHighEfficiency);
                        _shildsControl[1].SetMoveActive(true, _hexagonObjectConfigs.ShieldAuraMaxHeightReallyHighEfficiency);
                        _shildsControl[2].SetMoveActive(true, _hexagonObjectConfigs.ShieldAuraMaxHeightReallyHighEfficiency);
                        
                        _shildsControl[0].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);
                        _shildsControl[1].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);
                        _shildsControl[2].ShieldEffectEnable(ShieldAuraEffectType.SpawnShield);

                        StartCoroutine(FadingLightStarted());
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

        private IEnumerator FadingLightStarted() {
            float intensiveEmissionTime = _hexagonObjectConfigs.ShieldAuraIntensiveEmissionTime;
            float inverseEffectTime = 1f / intensiveEmissionTime;
            float elapsedTime = 0f;

            Color startColor = _visualEffectsConfigs.ShieldAuraIntensiveEmissionColor;
            Color finishColor = _visualEffectsConfigs.ShieldAuraEmissionColor;

            while (elapsedTime < intensiveEmissionTime) {
                Color currentColor = Color.Lerp(startColor, finishColor, elapsedTime * inverseEffectTime);

                _shieldMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);
                _auraShieldMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);

                foreach (var mrShild in _mrShilds) {
                    mrShild.SetPropertyBlock(_shieldMaterialPropertyBlock);
                }

                _mrShieldAura.SetPropertyBlock(_auraShieldMaterialPropertyBlock);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _shieldMaterialPropertyBlock.SetColor("_EmissionColor", finishColor);
            _auraShieldMaterialPropertyBlock.SetColor("_EmissionColor", finishColor);

            foreach (var mrShild in _mrShilds) {
                mrShild.SetPropertyBlock(_shieldMaterialPropertyBlock);
            }

            _mrShieldAura.SetPropertyBlock(_auraShieldMaterialPropertyBlock);
        }

        private IEnumerator RaiseTheShield(float shieldAuraHeight) {
            _auraShieldMaterialPropertyBlock.SetFloat("_CutoffHeight", _visualEffectsConfigs.ShieldAuraDestroyStartCutoffHeight);

            _mrShieldAura.SetPropertyBlock(_auraShieldMaterialPropertyBlock);

            float spawnEffectTime = _hexagonObjectConfigs.ShieldAuraSpawnTime;
            float elapsedTime = 0f;

            while (elapsedTime < spawnEffectTime) {
                float currentValue = Mathf.Lerp(0f, shieldAuraHeight, elapsedTime / spawnEffectTime);

                _auraShieldMaterialPropertyBlock.SetFloat("_AuraHeight", currentValue);

                _mrShieldAura.SetPropertyBlock(_auraShieldMaterialPropertyBlock);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _auraShieldMaterialPropertyBlock.SetFloat("_AuraHeight", shieldAuraHeight);

            _mrShieldAura.SetPropertyBlock(_auraShieldMaterialPropertyBlock);
        }

        private IEnumerator LowerTheShield() {
            float destroyEffectTime = _hexagonObjectConfigs.ShieldAuraDestroyTime;
            float elapsedTime = 0f;

            float startDestroyCutoffHeight  = _visualEffectsConfigs.ShieldAuraDestroyStartCutoffHeight;
            float finishDestroyCutoffHeight  = _visualEffectsConfigs.ShieldAuraDestroyFinishCutoffHeight;

            while (elapsedTime < destroyEffectTime) {
                float currentValue = Mathf.Lerp(startDestroyCutoffHeight, finishDestroyCutoffHeight, elapsedTime / destroyEffectTime);

                _auraShieldMaterialPropertyBlock.SetFloat("_CutoffHeight", currentValue);

                _mrShieldAura.SetPropertyBlock(_auraShieldMaterialPropertyBlock);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            _auraShieldMaterialPropertyBlock.SetFloat("_CutoffHeight", finishDestroyCutoffHeight);

            _mrShieldAura.SetPropertyBlock(_auraShieldMaterialPropertyBlock);

            _mrShieldAura.enabled = false;
        }
    }
}