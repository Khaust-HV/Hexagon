using System.Collections;
using LevelObjectType;
using UnityEngine;
using UnityEngine.VFX;

namespace HexagonObjectControl {
    public sealed class AttackRangeHexagonObjectAura : HexagonObjectAura {
        [SerializeField] private Transform _trRingPosition;
        [SerializeField] private Transform _trRing;
        [SerializeField] private MeshRenderer[] _mrBrokenSpaces;
        [SerializeField] private VisualEffect _veBrokenSpaces;


        private MeshRenderer _mrRing;

        private bool _isAuraSpawnActive;

        private float _attackRangeAuraSpawnRingStartScale;
        private float _attackRangeAuraSpawnRingFinishScale;
        private Color _attackRangeAuraEmissionColor;
        private Color _attackRangeAuraIntensiveEmissionColor;

        private MaterialPropertyBlock _ringMaterialPropertyBlock;
        private MaterialPropertyBlock _brokenSpaceMaterialPropertyBlock;

        protected override void SetBaseConfiguration() {
            _mrRing = _trRing.GetComponent<MeshRenderer>();
            _mrRing.enabled = false;

            foreach(var brokenSpace in _mrBrokenSpaces) {
                brokenSpace.enabled = false;
            }

            _trRingPosition.localPosition = new Vector3 (
                _trRingPosition.position.x,
                _hexagonObjectConfigs.AttackRangeAuraRingHeight,
                _trRingPosition.position.z
            );

            switch (_hexagonObjectPartType) {
                case StatsAuraType.AttackRangePositiveAura:
                    _attackRangeAuraSpawnRingStartScale = _hexagonObjectConfigs.AttackRangePositiveAuraSpawnRingStartScale;
                    _attackRangeAuraSpawnRingFinishScale = _hexagonObjectConfigs.AttackRangePositiveAuraSpawnRingFinishScale;
                    _attackRangeAuraEmissionColor = _visualEffectsConfigs.AttackRangeAuraPositiveEmissionColor;
                    _attackRangeAuraIntensiveEmissionColor = _visualEffectsConfigs.AttackRangeAuraPositiveIntensiveEmissionColor;
                break;

                case StatsAuraType.AttackRangeNegativeAura:
                    _attackRangeAuraSpawnRingStartScale = _hexagonObjectConfigs.AttackRangeNegativeAuraSpawnRingStartScale;
                    _attackRangeAuraSpawnRingFinishScale = _hexagonObjectConfigs.AttackRangeNegativeAuraSpawnRingFinishScale;
                    _attackRangeAuraEmissionColor = _visualEffectsConfigs.AttackRangeAuraNegativeEmissionColor;
                    _attackRangeAuraIntensiveEmissionColor = _visualEffectsConfigs.AttackRangeAuraNegativeIntensiveEmissionColor;
                break;
            }

            SetVFXConfiguration();
            
            SetMaterial();
        }

        private void SetVFXConfiguration() {
            _veBrokenSpaces.visualEffectAsset = _visualEffectsConfigs.CreatingParticlesOnMeshAtTheMomentAndContinuousRandomVelocity;
            _veBrokenSpaces.SetVector4("EmissionColor", _attackRangeAuraEmissionColor);
            _veBrokenSpaces.SetMesh("ObjectMesh", _mrBrokenSpaces[3].GetComponent<MeshFilter>().sharedMesh);
            _veBrokenSpaces.SetFloat("SizeParticle", _levelConfigs.SizeAllObject * _visualEffectsConfigs.AttackRangeAuraSizeParticles);
            _veBrokenSpaces.SetFloat("LifeTimeParticle", _levelConfigs.DefaultDestroyTimeAllObject);
            _veBrokenSpaces.SetTexture("TextureParticle", _visualEffectsConfigs.AttackRangeAuraTextureParticle);
            _veBrokenSpaces.SetInt("NumberParticlesAtTheMoment", _visualEffectsConfigs.AttackRangeAuraNumberParticlesAtTheMoment);
            _veBrokenSpaces.SetInt("NumberParticlesContinuous", _visualEffectsConfigs.AttackRangeAuraNumberParticlesContinuous);
            _veBrokenSpaces.SetFloat("ContinuousMaxDelay", _visualEffectsConfigs.AttackRangeAuraContinuousMaxDelay);
            _veBrokenSpaces.SetVector3("MinVelocity", _visualEffectsConfigs.AttackRangeAuraMinVelocityParticles);
            _veBrokenSpaces.SetVector3("MaxVelocity", _visualEffectsConfigs.AttackRangeAuraMaxVelocityParticles);
            _veBrokenSpaces.SetFloat("LinearDrag", _visualEffectsConfigs.AttackRangeAuraLinearDrag);
            _veBrokenSpaces.SetFloat("TurbulencePawer", _visualEffectsConfigs.AttackRangeAuraTurbulencePawer);
        }

        private void SetMaterial() {
            Material ringMaterial = _visualEffectsConfigs.DissolveAndEmissionFullObjectGhost;
            _ringMaterialPropertyBlock = new MaterialPropertyBlock();
            _ringMaterialPropertyBlock.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _ringMaterialPropertyBlock.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);
            _ringMaterialPropertyBlock.SetFloat("_NoiseScale", _visualEffectsConfigs.DefaultDestroyNoiseScale);
            _ringMaterialPropertyBlock.SetFloat("_NoiseStrength", _visualEffectsConfigs.DefaultDestroyNoiseStrength);
            _ringMaterialPropertyBlock.SetFloat("_BaseAlpha", _visualEffectsConfigs.AttackRangeAuraBaseAlpha);

            Material brokenSpaceMaterial = _visualEffectsConfigs.DissolveFromMiddleAndEmissionFullObjectGhost;
            _brokenSpaceMaterialPropertyBlock = new MaterialPropertyBlock();
            _brokenSpaceMaterialPropertyBlock.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _brokenSpaceMaterialPropertyBlock.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);
            _brokenSpaceMaterialPropertyBlock.SetFloat("_NoiseScale", _visualEffectsConfigs.DefaultDestroyNoiseScale);
            _brokenSpaceMaterialPropertyBlock.SetFloat("_BaseAlpha", _visualEffectsConfigs.AttackRangeAuraBaseAlpha);

            switch (_hexagonObjectPartType) {
                case StatsAuraType.AttackRangePositiveAura:
                    _ringMaterialPropertyBlock.SetColor("_EmissionColor", _visualEffectsConfigs.AttackRangeAuraPositiveEmissionColor);
                    _ringMaterialPropertyBlock.SetColor("_FresnelColor", _visualEffectsConfigs.AttackRangeAuraPositiveEmissionFresnelColor);
                    _ringMaterialPropertyBlock.SetFloat("_FresnelPower", _visualEffectsConfigs.AttackRangeAuraPositiveEmissionFresnelPower);

                    _brokenSpaceMaterialPropertyBlock.SetColor("_EmissionColor", _visualEffectsConfigs.AttackRangeAuraPositiveEmissionColor);
                    _brokenSpaceMaterialPropertyBlock.SetColor("_FresnelColor", _visualEffectsConfigs.AttackRangeAuraPositiveEmissionFresnelColor);
                    _brokenSpaceMaterialPropertyBlock.SetFloat("_FresnelPower", _visualEffectsConfigs.AttackRangeAuraPositiveEmissionFresnelPower);
                break;

                case StatsAuraType.AttackRangeNegativeAura:
                    _ringMaterialPropertyBlock.SetColor("_EmissionColor", _visualEffectsConfigs.AttackRangeAuraNegativeEmissionColor);
                    _ringMaterialPropertyBlock.SetColor("_FresnelColor", _visualEffectsConfigs.AttackRangeAuraNegativeEmissionFresnelColor);
                    _ringMaterialPropertyBlock.SetFloat("_FresnelPower", _visualEffectsConfigs.AttackRangeAuraNegativeEmissionFresnelPower);

                    _brokenSpaceMaterialPropertyBlock.SetColor("_EmissionColor", _visualEffectsConfigs.AttackRangeAuraNegativeEmissionColor);
                    _brokenSpaceMaterialPropertyBlock.SetColor("_FresnelColor", _visualEffectsConfigs.AttackRangeAuraNegativeEmissionFresnelColor);
                    _brokenSpaceMaterialPropertyBlock.SetFloat("_FresnelPower", _visualEffectsConfigs.AttackRangeAuraNegativeEmissionFresnelPower);
                break;
            }

            _mrRing.material = ringMaterial;
            _mrRing.SetPropertyBlock(_ringMaterialPropertyBlock);

            foreach(var brokenSpace in _mrBrokenSpaces) {
                brokenSpace.material = brokenSpaceMaterial;
                brokenSpace.SetPropertyBlock(_brokenSpaceMaterialPropertyBlock);
            }
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
                StartCoroutine(SpawnRingStarted());
            } else {
                StopAllCoroutines();

                _veBrokenSpaces.SendEvent("StopContinuous");

                StartCoroutine(DestroyAuraStarted());
            }
        }

        private IEnumerator SpawnRingStarted() {
            _isAuraSpawnActive = true;

            float _spawnRingTime = _hexagonObjectConfigs.AttackRangeAuraSpawnRingTime;
            float inverseEffectTime = 1f / _spawnRingTime;
            float elapsedTime = 0f;

            Vector3 startLocalScale = new Vector3(_attackRangeAuraSpawnRingStartScale, _trRing.localScale.y, _attackRangeAuraSpawnRingStartScale);
            Vector3 finishLocalScale = new Vector3(_attackRangeAuraSpawnRingFinishScale, _trRing.localScale.y, _attackRangeAuraSpawnRingFinishScale);

            _mrRing.enabled = true;

            while (elapsedTime < _spawnRingTime) {
                Vector3 currentLocalScale = Vector3.Lerp(startLocalScale, finishLocalScale, elapsedTime * inverseEffectTime);

                _trRing.localScale = currentLocalScale;

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _trRing.localScale = finishLocalScale;

            StartCoroutine(FadingLightStarted());

            StartCoroutine(SpawnBrokenSpaceStarted());

            _veBrokenSpaces.SendEvent("AtTheMoment");
            _veBrokenSpaces.SendEvent("StartContinuous");
        }

        private IEnumerator DestroyAuraStarted() {
            MeshRenderer brokenSpace = _auraEfficiencyType switch {
                AuraEfficiencyType.LowEfficiency => _mrBrokenSpaces[0],
                AuraEfficiencyType.StandardEfficiency => _mrBrokenSpaces[1],
                AuraEfficiencyType.HighEfficiency => _mrBrokenSpaces[2],
                AuraEfficiencyType.ReallyHighEfficiency => _mrBrokenSpaces[3],
                _ => throw new System.Exception("Invalid aura efficiency type")
            };

            float _destroyAuraTime = _hexagonObjectConfigs.AttackRangeAuraDestroyTime;
            float inverseEffectTime = 1f / _destroyAuraTime;
            float elapsedTime = 0f;

            float startDestroyCutoffHeight  = _visualEffectsConfigs.AttackRangeAuraDestroyStartCutoffHeight;
            float finishDestroyCutoffHeight  = _visualEffectsConfigs.AttackRangeAuraDestroyFinishCutoffHeight;

            if (_isAuraSpawnActive) {
                brokenSpace.enabled = false;

                while (elapsedTime < _destroyAuraTime) {
                    float currentValue = Mathf.Lerp(startDestroyCutoffHeight, finishDestroyCutoffHeight, elapsedTime * inverseEffectTime);

                    _ringMaterialPropertyBlock.SetFloat("_CutoffHeight", currentValue);

                    _mrRing.SetPropertyBlock(_ringMaterialPropertyBlock);

                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                _ringMaterialPropertyBlock.SetFloat("_CutoffHeight", startDestroyCutoffHeight);
                _ringMaterialPropertyBlock.SetColor("_EmissionColor", _attackRangeAuraEmissionColor);

                _mrRing.SetPropertyBlock(_ringMaterialPropertyBlock);
            } else {
                _brokenSpaceMaterialPropertyBlock.SetFloat("_NoiseStrength", _visualEffectsConfigs.DefaultDestroyNoiseStrength);

                brokenSpace.SetPropertyBlock(_brokenSpaceMaterialPropertyBlock);

                while (elapsedTime < _destroyAuraTime) {
                    float currentValue = Mathf.Lerp(startDestroyCutoffHeight, finishDestroyCutoffHeight, elapsedTime * inverseEffectTime);

                    _ringMaterialPropertyBlock.SetFloat("_CutoffHeight", currentValue);
                    _brokenSpaceMaterialPropertyBlock.SetFloat("_CutoffHeight", currentValue);

                    _mrRing.SetPropertyBlock(_ringMaterialPropertyBlock);
                    brokenSpace.SetPropertyBlock(_brokenSpaceMaterialPropertyBlock);

                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                _ringMaterialPropertyBlock.SetFloat("_CutoffHeight", startDestroyCutoffHeight);
                _ringMaterialPropertyBlock.SetColor("_EmissionColor", _attackRangeAuraEmissionColor);
                _brokenSpaceMaterialPropertyBlock.SetFloat("_CutoffHeight", startDestroyCutoffHeight);

                _mrRing.SetPropertyBlock(_ringMaterialPropertyBlock);
                brokenSpace.SetPropertyBlock(_brokenSpaceMaterialPropertyBlock);

                brokenSpace.enabled = false;
            }

            _mrRing.enabled = false;

            _isAuraSpawnActive = false;
        }

        private IEnumerator FadingLightStarted() {
            MeshRenderer brokenSpace = _auraEfficiencyType switch {
                AuraEfficiencyType.LowEfficiency => _mrBrokenSpaces[0],
                AuraEfficiencyType.StandardEfficiency => _mrBrokenSpaces[1],
                AuraEfficiencyType.HighEfficiency => _mrBrokenSpaces[2],
                AuraEfficiencyType.ReallyHighEfficiency => _mrBrokenSpaces[3],
                _ => throw new System.Exception("Invalid aura efficiency type")
            };

            float intensiveEmissionTime = _hexagonObjectConfigs.AttackRangeAuraIntensiveEmissionTime;
            float inverseEffectTime = 1f / intensiveEmissionTime;
            float elapsedTime = 0f;

            while (elapsedTime < intensiveEmissionTime) {
                Color currentColor = Color.Lerp(_attackRangeAuraIntensiveEmissionColor, _attackRangeAuraEmissionColor, elapsedTime * inverseEffectTime);

                _ringMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);
                _brokenSpaceMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);
                _veBrokenSpaces.SetVector4("EmissionColor", currentColor);

                _mrRing.SetPropertyBlock(_ringMaterialPropertyBlock);
                brokenSpace.SetPropertyBlock(_brokenSpaceMaterialPropertyBlock);

                elapsedTime += Time.deltaTime;
                
                yield return null;
            }

            _ringMaterialPropertyBlock.SetColor("_EmissionColor", _attackRangeAuraEmissionColor);
            _brokenSpaceMaterialPropertyBlock.SetColor("_EmissionColor", _attackRangeAuraEmissionColor);
            _veBrokenSpaces.SetVector4("EmissionColor", _attackRangeAuraEmissionColor);

            _mrRing.SetPropertyBlock(_ringMaterialPropertyBlock);
            brokenSpace.SetPropertyBlock(_brokenSpaceMaterialPropertyBlock);
        }

        private IEnumerator SpawnBrokenSpaceStarted() {
            MeshRenderer brokenSpace = _auraEfficiencyType switch {
                AuraEfficiencyType.LowEfficiency => _mrBrokenSpaces[0],
                AuraEfficiencyType.StandardEfficiency => _mrBrokenSpaces[1],
                AuraEfficiencyType.HighEfficiency => _mrBrokenSpaces[2],
                AuraEfficiencyType.ReallyHighEfficiency => _mrBrokenSpaces[3],
                _ => throw new System.Exception("Invalid aura efficiency type")
            };

            brokenSpace.enabled = true;

            _brokenSpaceMaterialPropertyBlock.SetFloat("_NoiseStrength", _visualEffectsConfigs.AttackRangeAuraSpawnBrokenSpaceNoiseStrength);

            brokenSpace.SetPropertyBlock(_brokenSpaceMaterialPropertyBlock);

            float _spawnBrokenSpaceTime = _hexagonObjectConfigs.AttackRangeAuraSpawnBrokenSpaceTime;
            float inverseEffectTime = 1f / _spawnBrokenSpaceTime;
            float elapsedTime = 0f;

            float startSpawnCutoffHeight  = _visualEffectsConfigs.AttackRangeAuraSpawnBrokenSpaceStartCutoffHeight;
            float finishSpawnCutoffHeight  = _visualEffectsConfigs.AttackRangeAuraSpawnBrokenSpaceFinishCutoffHeight;

            while (elapsedTime < _spawnBrokenSpaceTime) {
                float currentValue = Mathf.Lerp(startSpawnCutoffHeight, finishSpawnCutoffHeight, elapsedTime * inverseEffectTime);

                _brokenSpaceMaterialPropertyBlock.SetFloat("_CutoffHeight", currentValue);

                brokenSpace.SetPropertyBlock(_brokenSpaceMaterialPropertyBlock);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _brokenSpaceMaterialPropertyBlock.SetFloat("_CutoffHeight", finishSpawnCutoffHeight);

            brokenSpace.SetPropertyBlock(_brokenSpaceMaterialPropertyBlock);

            _isAuraSpawnActive = false;
        }
    }
}