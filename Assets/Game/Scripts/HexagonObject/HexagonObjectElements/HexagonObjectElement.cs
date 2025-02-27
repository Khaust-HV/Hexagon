using System;
using System.Collections;
using System.Collections.Generic;
using GameConfigs;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace HexagonObjectControl {
    [RequireComponent(typeof(VisualEffect))]
    public class HexagonObjectElement : MonoBehaviour, IHexagonObjectPart {
        [Header("Renderer settings")]
        [SerializeField] private LODGroup[] _lODGroups;
        [SerializeField] private MeshRenderer[] _mrOtherObject;
        [SerializeField] protected bool _isObjectHaveAnimation;
        [Header("Dissolve effect settings")]
        [SerializeField] private float _spawnStartCutoffHeight;
        [SerializeField] private float _spawnFinishCutoffHeight;
        [SerializeField] private float _destroyStartCutoffHeight;
        [SerializeField] private float _destroyFinishCutoffHeight;

        protected List<MeshRenderer> _mrBaseObject = new();

        public event Action HexagonObjectPartIsRestored;
        public event Action HexagonObjectPartIsDestroyed;

        protected Enum _hexagonObjectPartType;

        protected float _spawnEffectTime;

        protected float _hitPoints;
        private bool _isHitEffectActive;
        private IEnumerator _hitEffectStarted;

        private bool _isHexagonObjectPartUsed;
        protected bool _isObjectHologram;
        private bool _isObjectWaitingToSpawn;

        private IEnumerator _spawnEffectStarted;

        protected Material _baseMaterial;
        protected MaterialPropertyBlock _baseMaterialPropertyBlock;
        private Material _hologramMaterial;
        protected MaterialPropertyBlock _hologramMaterialPropertyBlock;

        private VisualEffect _visualEffect;

        #region DI
            private IStorageTransformPool _iStorageTransformPool;
            protected VisualEffectsConfigs _visualEffectsConfigs;
            protected HexagonObjectConfigs _hexagonObjectConfigs;
            protected LevelConfigs _levelConfigs;
            CameraConfigs _cameraConfigs;
        #endregion

        [Inject]
        private void Construct (
            IStorageTransformPool iStorageTransformPool, 
            VisualEffectsConfigs visualEffectsConfigs, 
            HexagonObjectConfigs hexagonObjectConfigs,
            LevelConfigs levelConfigs,
            CameraConfigs cameraConfigs
            ) {
            // Set DI
            _iStorageTransformPool = iStorageTransformPool;

            // Set configurations
            _visualEffectsConfigs = visualEffectsConfigs;
            _hexagonObjectConfigs = hexagonObjectConfigs;
            _levelConfigs = levelConfigs;
            _cameraConfigs = cameraConfigs;

            // Set components
            _visualEffect = GetComponent<VisualEffect>();

            GetMeshRenderers();

            SetDestroyHexagonObjectVFXConfiguration();
        }

        private void GetMeshRenderers() {
            float lODObjectSize = _cameraConfigs.LODObjectSize;
            float lOD0Distance = _cameraConfigs.LOD0Distance;
            float lOD1Distance = _cameraConfigs.LOD1Distance;
            float lOD2Distance = _cameraConfigs.LOD2Distance;
            
            foreach (var lODGroup in _lODGroups) {
                var lODs = lODGroup.GetLODs();

                lODs[0].screenRelativeTransitionHeight = lOD0Distance;
                lODs[1].screenRelativeTransitionHeight = lOD1Distance;
                lODs[2].screenRelativeTransitionHeight = lOD2Distance;

                lODGroup.SetLODs(lODs);

                lODGroup.size = lODObjectSize;

                foreach (var lOD in lODs) {
                    foreach (Renderer renderer in lOD.renderers) {
                        if (renderer is MeshRenderer meshRenderer) _mrBaseObject.Add(meshRenderer);
                        else throw new Exception($"Problem in the hexagonObject {gameObject.name} renderer");
                    }
                }
            }

            foreach (var meshRenderer in _mrOtherObject) {
                _mrBaseObject.Add(meshRenderer);
            }
        }

        private void SetDestroyHexagonObjectVFXConfiguration() {
            _visualEffect.visualEffectAsset = _visualEffectsConfigs.CreatingParticlesOnMeshAtTheMomentAndContinuousRandomVelocity;
            _visualEffect.SetInt("NumberParticlesAtTheMoment", _visualEffectsConfigs.DefaultDestroyVFXNumberParticles);
            _visualEffect.SetTexture("TextureParticle", _visualEffectsConfigs.DefaultDestroyTextureParticle);
            _visualEffect.SetMesh("ObjectMesh", _mrBaseObject[0].GetComponent<MeshFilter>().sharedMesh);
            _visualEffect.SetFloat("LifeTimeParticle", _levelConfigs.DefaultDestroyTimeAllObject);
            _visualEffect.SetFloat("SizeParticle", _levelConfigs.SizeAllObject * _visualEffectsConfigs.DefaultDestroySizeParticles);
            _visualEffect.SetVector4("EmissionColor", _visualEffectsConfigs.DefaultDestroyVFXEmissionColor);
            _visualEffect.SetVector3("MaxVelocity", _visualEffectsConfigs.DefaultDestroyVFXMaxVelocity);
            _visualEffect.SetFloat("LinearDrag", _visualEffectsConfigs.DefaultDestroyVFXLinearDrag);
            _visualEffect.SetFloat("TurbulencePawer", _visualEffectsConfigs.DefaultDestroyVFXTurbulencePawer);
        }

        protected virtual void SetBaseConfiguration() {
            _spawnEffectTime = _levelConfigs.DefaultSpawnTimeAllObject;

            _baseMaterial = _visualEffectsConfigs.DissolveAndHitWithUV;
            _baseMaterialPropertyBlock = new MaterialPropertyBlock();
            _baseMaterialPropertyBlock.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _baseMaterialPropertyBlock.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _baseMaterial;
                mrObject.SetPropertyBlock(_baseMaterialPropertyBlock);
            }
        }

        protected virtual void SetAnimationActive(bool isActive) {
            // Overridden by an heir
        }

        protected virtual void SetColliderActive(bool isActive) {
            // Overridden by an heir
        }

        protected virtual void SetHexagonObjectWorkActive(bool isActive) {
            // Overridden by an heir
        }

        protected void TakeTheDamage(float damage) {
            if (_isHitEffectActive) StopCoroutine(_hitEffectStarted);

            StartCoroutine(_hitEffectStarted = HitEffectStarted());

            _hitPoints -= damage;

            if (_hitPoints <= 0f) HexagonObjectPartIsDestroyed?.Invoke();
        }

        private IEnumerator HitEffectStarted() {
            _isHitEffectActive = true;

            float hitEffectTime = _levelConfigs.DefaultHitEffectTime;
            float inverseEffectTime = 1f / hitEffectTime;
            float elapsedTime = 0f;

            Color startColor = _visualEffectsConfigs.DefaultHitIntensiveEmissionColor;
            Color finishColor = Color.black;

            while (elapsedTime < hitEffectTime) {
                Color currentColor = Color.Lerp(startColor, finishColor, elapsedTime * inverseEffectTime);

                _baseMaterialPropertyBlock.SetColor("_HitColor", currentColor);

                foreach (var mrObject in _mrBaseObject) {
                    mrObject.SetPropertyBlock(_baseMaterialPropertyBlock);
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _baseMaterialPropertyBlock.SetColor("_HitColor", finishColor);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.SetPropertyBlock(_baseMaterialPropertyBlock);
            }

            _isHitEffectActive = false;
        }


        public void SetHexagonObjectPartType<T>(T type) where T : Enum {
            _hexagonObjectPartType = type;

            SetBaseConfiguration();
        }

        public void SetHexagonObjectType<T>(T type) where T : Enum {
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"HexagonObjectElement is not aura {gameObject.name}");
        }

        public void SetParentObject(Transform parentObject) {
            _isHexagonObjectPartUsed = true;

            transform.SetParent(parentObject);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void SetAuraEfficiency(AuraEfficiencyType auraEfficiencyType){
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"HexagonObjectElement is not aura {gameObject.name}");
        }

        public void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart) {
            throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectPartType, $"HexagonObjectElement is not aura {gameObject.name}");
        }

        public void SpawnEffectEnable() {
            gameObject.SetActive(true);

            SetColliderActive(true);

            _baseMaterialPropertyBlock.SetFloat("_NoiseScale", _visualEffectsConfigs.DefaultSpawnNoiseScale);
            _baseMaterialPropertyBlock.SetFloat("_NoiseStrength", _visualEffectsConfigs.DefaultSpawnNoiseStrength);
            _baseMaterialPropertyBlock.SetFloat("_CutoffHeight", _spawnStartCutoffHeight);
            _baseMaterialPropertyBlock.SetFloat("_EdgeWidth", _visualEffectsConfigs.DefaultSpawnEdgeWidth);
            _baseMaterialPropertyBlock.SetColor("_EdgeColor", _visualEffectsConfigs.DefaultSpawnEdgeColor);
            _baseMaterialPropertyBlock.SetColor("_HitColor", Color.black);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.SetPropertyBlock(_baseMaterialPropertyBlock);
            }

            if (_isObjectWaitingToSpawn) StopCoroutine(_spawnEffectStarted);

            StartCoroutine(_spawnEffectStarted = SpawnEffectStarted(_baseMaterialPropertyBlock, _spawnEffectTime));
        }

        public void HologramSpawnEffectEnable() {
            gameObject.SetActive(true);

            _hologramMaterialPropertyBlock.SetFloat("_CutoffHeight", _spawnStartCutoffHeight);
            _baseMaterialPropertyBlock.SetColor("_HitColor", Color.black);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.SetPropertyBlock(_hologramMaterialPropertyBlock);
            }

            if (_isObjectWaitingToSpawn) StopCoroutine(_spawnEffectStarted);

            StartCoroutine(_spawnEffectStarted = SpawnEffectStarted(_hologramMaterialPropertyBlock, _levelConfigs.DefaultHologramSpawnTimeAllObject));
        }

        private IEnumerator SpawnEffectStarted(MaterialPropertyBlock materialPropertyBlock, float spawnEffectTime) {
            _isObjectWaitingToSpawn = true;

            SetAnimationActive(true);

            float elapsedTime = 0f;
            float inverseEffectTime = 1f / spawnEffectTime;

            while (elapsedTime < spawnEffectTime) {
                float currentValue = Mathf.Lerp(_spawnStartCutoffHeight, _spawnFinishCutoffHeight, elapsedTime * inverseEffectTime);

                materialPropertyBlock.SetFloat("_CutoffHeight", currentValue);

                foreach (var mrObject in _mrBaseObject) {
                    mrObject.SetPropertyBlock(materialPropertyBlock);
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            materialPropertyBlock.SetFloat("_CutoffHeight", _spawnFinishCutoffHeight);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.SetPropertyBlock(materialPropertyBlock);
            }

            SetHexagonObjectWorkActive(true);

            _isObjectWaitingToSpawn = false;
        }

        public void DestroyEffectEnable(bool _isFastDestroy) {
            SetHexagonObjectWorkActive(false);

            SetColliderActive(false);

            _baseMaterialPropertyBlock.SetFloat("_NoiseScale", _visualEffectsConfigs.DefaultDestroyNoiseScale);
            _baseMaterialPropertyBlock.SetFloat("_NoiseStrength", _visualEffectsConfigs.DefaultDestroyNoiseStrength);
            _baseMaterialPropertyBlock.SetFloat("_EdgeWidth", _visualEffectsConfigs.DefaultDestroyEdgeWidth);
            _baseMaterialPropertyBlock.SetColor("_EdgeColor", _visualEffectsConfigs.DefaultDestroyEdgeColor);
            _baseMaterialPropertyBlock.SetColor("_HitColor", Color.black);

            if (_isFastDestroy || _isObjectWaitingToSpawn) {
                _baseMaterialPropertyBlock.SetFloat("_CutoffHeight", _destroyFinishCutoffHeight);

                _visualEffect.SendEvent("AtTheMoment");

                if (_isObjectWaitingToSpawn) {
                    StopCoroutine(_spawnEffectStarted);

                    _isObjectWaitingToSpawn = false;
                }

                SetAnimationActive(false);

                foreach (var mrObject in _mrBaseObject) {
                    mrObject.SetPropertyBlock(_baseMaterialPropertyBlock);
                }

                StartCoroutine(DestroyEffectStarted(true));
            } else {
                _baseMaterialPropertyBlock.SetFloat("_CutoffHeight", _destroyStartCutoffHeight);

                foreach (var mrObject in _mrBaseObject) {
                    mrObject.SetPropertyBlock(_baseMaterialPropertyBlock);
                }

                StartCoroutine(DestroyEffectStarted(false));
            }
        }

        private IEnumerator DestroyEffectStarted(bool _isFastDestroy) {
            if (_isFastDestroy) yield return new WaitForSeconds(_levelConfigs.DefaultDestroyTimeAllObject);
            else {
                float elapsedTime = 0f;

                float destroyEffectTime = _levelConfigs.DefaultDestroyTimeAllObject;
                float inverseEffectTime = 1f / destroyEffectTime;

                while (elapsedTime <destroyEffectTime) {
                    float currentValue = Mathf.Lerp(_destroyStartCutoffHeight, _destroyFinishCutoffHeight, elapsedTime * inverseEffectTime);

                    _baseMaterialPropertyBlock.SetFloat("_CutoffHeight", currentValue);

                    foreach (var mrObject in _mrBaseObject) {
                        mrObject.SetPropertyBlock(_baseMaterialPropertyBlock);
                    }

                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                _baseMaterialPropertyBlock.SetFloat("_CutoffHeight", _destroyFinishCutoffHeight);

                foreach (var mrObject in _mrBaseObject) {
                    mrObject.SetPropertyBlock(_baseMaterialPropertyBlock);
                }

                SetAnimationActive(false);
            }

            RestoreAndHide();
        }

        public void MakeObjectHologram() {
            if (_hologramMaterialPropertyBlock == null) {
                _hologramMaterial = _visualEffectsConfigs.DissolveAndHologramGhost;
                _hologramMaterialPropertyBlock = new MaterialPropertyBlock();
                _hologramMaterialPropertyBlock.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
                _hologramMaterialPropertyBlock.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);
                _hologramMaterialPropertyBlock.SetFloat("_NoiseScale", _visualEffectsConfigs.DefaultSpawnNoiseScale);
                _hologramMaterialPropertyBlock.SetFloat("_NoiseStrength", _visualEffectsConfigs.DefaultSpawnNoiseStrength);
                _hologramMaterialPropertyBlock.SetFloat("_AnimationSpeed", _visualEffectsConfigs.DefaultHologramAnimationSpeed);
                _hologramMaterialPropertyBlock.SetFloat("_AnimationNoise", _visualEffectsConfigs.DefaultHologramAnimationNoise);
                _hologramMaterialPropertyBlock.SetColor("_EmissionColor", _visualEffectsConfigs.DefaultHologramEmissionColor);
                _hologramMaterialPropertyBlock.SetColor("_FresnelColor", _visualEffectsConfigs.DefaultHologramFresnelColor);
                _hologramMaterialPropertyBlock.SetFloat("_FresnelPower", _visualEffectsConfigs.DefaultHologramFresnelPower);
                _hologramMaterialPropertyBlock.SetColor("_EdgeColor", _visualEffectsConfigs.DefaultHologramEdgeColor);
                _hologramMaterialPropertyBlock.SetFloat("_EdgeWidth", _visualEffectsConfigs.DefaultHologramEdgeWidth);
            }

            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _hologramMaterial;
                mrObject.SetPropertyBlock(_hologramMaterialPropertyBlock);
            }

            _isObjectHologram = true;
        }

        public void MakeObjectNormal() {
            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _baseMaterial;
            }

            _isObjectHologram = false;
        }

        public void RestoreAndHide() {
            gameObject.SetActive(false);

            if (_isObjectHologram) {
                foreach (var mrObject in _mrBaseObject) {
                    mrObject.material = _baseMaterial;
                }

                _isObjectHologram = false;
            }

            transform.SetParent(_iStorageTransformPool.GetHexagonObjectTransformPool());
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            HexagonObjectPartIsRestored?.Invoke();

            _isHexagonObjectPartUsed = false;
        }

        public bool IsHexagonObjectPartUsed() {
            return _isHexagonObjectPartUsed;
        }
    }
}

public interface IHexagonObjectPart {
    public event Action HexagonObjectPartIsRestored;
    public event Action HexagonObjectPartIsDestroyed;
    public bool IsHexagonObjectPartUsed();
    public void SetHexagonObjectPartType<T>(T type) where T : Enum;
    public void SetHexagonObjectType<T>(T type) where T : Enum;
    public void SetParentObject(Transform parentObject);
    public void SetAuraEfficiency(AuraEfficiencyType auraEfficiencyType);
    public void ApplyAuraToHexagonObjectElement(IHexagonObjectPart iHexagonObjectPart);
    public void SpawnEffectEnable();
    public void DestroyEffectEnable(bool _isFastDestroy);
    public void RestoreAndHide();
    public void MakeObjectHologram();
    public void MakeObjectNormal();
    public void HologramSpawnEffectEnable();
}