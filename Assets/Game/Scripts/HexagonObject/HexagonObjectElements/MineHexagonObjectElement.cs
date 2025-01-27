using System.Collections;
using LevelObjectType;
using UnityEngine;
using Zenject;

namespace HexagonObjectControl {
    public sealed class MineHexagonObjectElement : HexagonObjectElement {
        private bool _isSource;
        
        private ResourceType _resourceCreated;
        private int _minResourceNumber;
        private int _maxResourceNumber;
        private float _creationTime;

        private Animator _animator;

        #region DI
            private IWorkingWithHexagonObject _iWorkingWithHexagonObject;
        #endregion

        [Inject]
        private void Construct(IWorkingWithHexagonObject iWorkingWithHexagonObject) {
            // Set DI
            _iWorkingWithHexagonObject = iWorkingWithHexagonObject;

            // Set componenets
            _animator = GetComponent<Animator>();
        }

        protected override void SetBaseConfiguration() {
            SetMaterial();

            SetMineTypeConfiguration();
        }

        private void SetMaterial() {
            _baseMaterialPropertyBlock = new MaterialPropertyBlock();

            switch (_hexagonObjectPartType) {
                case MineHexagonObjectsType.RedCrystalSource:
                case MineHexagonObjectsType.RedCrystalMining:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndEmissionTextureWithUV;
                    _baseMaterialPropertyBlock.SetTexture("_EmissionTexture", _visualEffectsConfigs.RedCrystalEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.RedCrystalEmissionColor);
                break;

                case MineHexagonObjectsType.BlueCrystalSource:
                case MineHexagonObjectsType.BlueCrystalMining:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndEmissionTextureWithUV;
                    _baseMaterialPropertyBlock.SetTexture("_EmissionTexture", _visualEffectsConfigs.BlueCrystalEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.BlueCrystalEmissionColor);
                break;

                case MineHexagonObjectsType.GreenCrystalSource:
                case MineHexagonObjectsType.GreenCrystalMining:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndEmissionTextureWithUV;
                    _baseMaterialPropertyBlock.SetTexture("_EmissionTexture", _visualEffectsConfigs.GreenCrystalEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.GreenCrystalEmissionColor);
                break;

                case MineHexagonObjectsType.ElectricalSource:
                case MineHexagonObjectsType.ElectricalMining:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndEmissionTextureWithUV;
                    _baseMaterialPropertyBlock.SetTexture("_EmissionTexture", _visualEffectsConfigs.ElectricalEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.ElectricalEmissionColor);
                break;

                case MineHexagonObjectsType.GlitcheSource:
                case MineHexagonObjectsType.GlitcheMining:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndEmission3TexturesWithUV;

                    _baseMaterialPropertyBlock.SetTexture("_FirstEmissionTexture", _visualEffectsConfigs.FirstGlitcheEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_FirstEmissionColor", _visualEffectsConfigs.FirstGlitcheEmissionColor);

                    _baseMaterialPropertyBlock.SetTexture("_SecondEmissionTexture", _visualEffectsConfigs.SecondGlitcheEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_SecondEmissionColor", _visualEffectsConfigs.SecondGlitcheEmissionColor);

                    _baseMaterialPropertyBlock.SetTexture("_ThirdEmissionTexture", _visualEffectsConfigs.ThirdGlitcheEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_ThirdEmissionColor", _visualEffectsConfigs.ThirdGlitcheEmissionColor);
                break;

                default:
                    _baseMaterial = _visualEffectsConfigs.DissolveWithUV;
                break;
            }

            _baseMaterialPropertyBlock.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _baseMaterialPropertyBlock.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _baseMaterial;
                mrObject.SetPropertyBlock(_baseMaterialPropertyBlock);
            }
        }

        private void SetMineTypeConfiguration() {
            switch (_hexagonObjectPartType) {
                case MineHexagonObjectsType.TreeSource:
                case MineHexagonObjectsType.StoneSource:
                case MineHexagonObjectsType.MetalSource:
                case MineHexagonObjectsType.ElectricalSource:
                case MineHexagonObjectsType.OilSource:
                case MineHexagonObjectsType.RedCrystalSource:
                case MineHexagonObjectsType.BlueCrystalSource:
                case MineHexagonObjectsType.GreenCrystalSource:
                case MineHexagonObjectsType.GlitcheSource:
                    _spawnEffectTime = _levelConfigs.DefaultSpawnTimeAllObject;
                    _isSource = true;
                break;

                case MineHexagonObjectsType.TreeMining:
                    _resourceCreated = ResourceType.Wood;
                    _creationTime = _hexagonObjectConfigs.TreeMiningResourceCreationTime;
                    _minResourceNumber = _hexagonObjectConfigs.TreeMiningMinResourceNumber;
                    _maxResourceNumber = _hexagonObjectConfigs.TreeMiningMaxResourceNumber;
                    _spawnEffectTime = _hexagonObjectConfigs.TreeMiningTimeSpawn;
                break;

                case MineHexagonObjectsType.StoneMining:
                    _resourceCreated = ResourceType.Stone;
                    _creationTime = _hexagonObjectConfigs.StoneMiningResourceCreationTime;
                    _minResourceNumber = _hexagonObjectConfigs.StoneMiningMinResourceNumber;
                    _maxResourceNumber = _hexagonObjectConfigs.StoneMiningMaxResourceNumber;
                    _spawnEffectTime = _hexagonObjectConfigs.StoneMiningTimeSpawn;
                break;

                case MineHexagonObjectsType.MetalMining:
                    _resourceCreated = ResourceType.Metal;
                    _creationTime = _hexagonObjectConfigs.MetalMiningResourceCreationTime;
                    _minResourceNumber = _hexagonObjectConfigs.MetalMiningMinResourceNumber;
                    _maxResourceNumber = _hexagonObjectConfigs.MetalMiningMaxResourceNumber;
                    _spawnEffectTime = _hexagonObjectConfigs.MetalMiningTimeSpawn;
                break;

                case MineHexagonObjectsType.ElectricalMining:
                    _resourceCreated = ResourceType.Electrical;
                    _creationTime = _hexagonObjectConfigs.ElectricalMiningResourceCreationTime;
                    _minResourceNumber = _hexagonObjectConfigs.ElectricalMiningMinResourceNumber;
                    _maxResourceNumber = _hexagonObjectConfigs.ElectricalMiningMaxResourceNumber;
                    _spawnEffectTime = _hexagonObjectConfigs.ElectricalMiningTimeSpawn;
                break;

                case MineHexagonObjectsType.OilMining:
                    _resourceCreated = ResourceType.Oil;
                    _creationTime = _hexagonObjectConfigs.OilMiningResourceCreationTime;
                    _minResourceNumber = _hexagonObjectConfigs.OilMiningMinResourceNumber;
                    _maxResourceNumber = _hexagonObjectConfigs.OilMiningMaxResourceNumber;
                    _spawnEffectTime = _hexagonObjectConfigs.OilMiningTimeSpawn;
                break;

                case MineHexagonObjectsType.RedCrystalMining:
                    _resourceCreated = ResourceType.RedCrystal;
                    _creationTime = _hexagonObjectConfigs.RedCrystalMiningResourceCreationTime;
                    _minResourceNumber = _hexagonObjectConfigs.RedCrystalMiningMinResourceNumber;
                    _maxResourceNumber = _hexagonObjectConfigs.RedCrystalMiningMaxResourceNumber;
                    _spawnEffectTime = _hexagonObjectConfigs.RedCrystalMiningTimeSpawn;
                break;

                case MineHexagonObjectsType.BlueCrystalMining:
                    _resourceCreated = ResourceType.BlueCrystal;
                    _creationTime = _hexagonObjectConfigs.BlueCrystalMiningResourceCreationTime;
                    _minResourceNumber = _hexagonObjectConfigs.BlueCrystalMiningMinResourceNumber;
                    _maxResourceNumber = _hexagonObjectConfigs.BlueCrystalMiningMaxResourceNumber;
                    _spawnEffectTime = _hexagonObjectConfigs.BlueCrystalMiningTimeSpawn;
                break;

                case MineHexagonObjectsType.GreenCrystalMining:
                    _resourceCreated = ResourceType.GreenCrystal;
                    _creationTime = _hexagonObjectConfigs.GreenCrystalMiningResourceCreationTime;
                    _minResourceNumber = _hexagonObjectConfigs.GreenCrystalMiningMinResourceNumber;
                    _maxResourceNumber = _hexagonObjectConfigs.GreenCrystalMiningMaxResourceNumber;
                    _spawnEffectTime = _hexagonObjectConfigs.GreenCrystalMiningTimeSpawn;
                break;

                case MineHexagonObjectsType.GlitcheMining:
                    _resourceCreated = ResourceType.Glitche;
                    _creationTime = _hexagonObjectConfigs.GlitcheMiningResourceCreationTime;
                    _minResourceNumber = _hexagonObjectConfigs.GlitcheMiningMinResourceNumber;
                    _maxResourceNumber = _hexagonObjectConfigs.GlitcheMiningMaxResourceNumber;
                    _spawnEffectTime = _hexagonObjectConfigs.GlitcheMiningTimeSpawn;
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

        protected override void SetHexagonObjectWorkActive(bool isActive) {
            if (_isSource || _isObjectHologram) return;

            if (isActive) StartCoroutine(CreateResourceStarted());
            else StopAllCoroutines();
        }

        private IEnumerator CreateResourceStarted() {
            while (true) {
                yield return new WaitForSeconds(_creationTime);

                int resourceNumber = Random.Range(_minResourceNumber, _maxResourceNumber);
                
                _iWorkingWithHexagonObject.ResourceCredit(_resourceCreated, resourceNumber);
            }
        }
    }
}

public enum ResourceType {
    Wood,
    Stone,
    Metal,
    Electrical,
    Oil,
    RedCrystal,
    BlueCrystal,
    GreenCrystal,
    Glitche
}