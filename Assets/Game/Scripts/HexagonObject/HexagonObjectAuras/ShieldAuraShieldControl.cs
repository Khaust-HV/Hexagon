using System.Collections;
using GameConfigs;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace HexagonObjectControl {
    public sealed class ShieldAuraShieldControl : MonoBehaviour {
        #region AuraConfigs
            private float _orbitSpeed;
            private float _orbitRadius;
            private float _verticalSpeed;
            private float _maxHeight;
            private float _minHeight;
        #endregion

        private bool _isRises;

        private VisualEffect _visualEffect;

        private IEnumerator _moveStarted;

        #region DI
            private VisualEffectsConfigs _visualEffectsConfigs;
            private LevelConfigs _levelConfigs;
        #endregion

        [Inject]
        private void Construct (
            HexagonObjectConfigs hexagonObjectConfigs,
            VisualEffectsConfigs visualEffectsConfigs, 
            LevelConfigs levelConfigs
            ) {
            // Set configurations

            _visualEffectsConfigs = visualEffectsConfigs;
            _levelConfigs = levelConfigs;

            _orbitSpeed = hexagonObjectConfigs.ShieldAuraOrbitSpeed;
            _orbitRadius = hexagonObjectConfigs.ShieldAuraOrbitRadius;
            _verticalSpeed = hexagonObjectConfigs.ShieldAuraVerticalSpeed;
            _minHeight = hexagonObjectConfigs.ShieldAuraMinHeight;

            _visualEffect = GetComponent<VisualEffect>();
            SetVFXConfiguration();
        }

        private void SetVFXConfiguration() {
            _visualEffect.visualEffectAsset = _visualEffectsConfigs.ShieldAuraShieldSpawn;
            _visualEffect.SetFloat("Metallic", _visualEffectsConfigs.DefaultMetallic);
            _visualEffect.SetFloat("Smoothness", _visualEffectsConfigs.DefaultSmoothness);
            _visualEffect.SetFloat("NoiseScale", _visualEffectsConfigs.DefaultDestroyNoiseScale);
            _visualEffect.SetFloat("NoiseStrength", _visualEffectsConfigs.DefaultDestroyNoiseStrength);
            _visualEffect.SetVector4("FresnelColor", _visualEffectsConfigs.ShieldAuraEmissionFresnelColor);
            _visualEffect.SetFloat("FresnelPower", _visualEffectsConfigs.ShieldAuraEmissionFresnelPower);
            _visualEffect.SetVector4("EmissionColor", _visualEffectsConfigs.ShieldAuraEmissionColor);
            _visualEffect.SetMesh("ShieldMesh", GetComponent<MeshFilter>().sharedMesh);
            _visualEffect.SetFloat("ShieldSizeParticle", _levelConfigs.SizeAllObject);
            _visualEffect.SetFloat("FragmentSizeParticle", _levelConfigs.SizeAllObject * _visualEffectsConfigs.ShieldAuraSizeParticles);
            _visualEffect.SetFloat("LifeTimeParticle", _levelConfigs.DefaultDestroyTimeAllObject);
            _visualEffect.SetInt("ParticlesNumberForShieldDestroy", _visualEffectsConfigs.ShieldAuraParticlesNumberForShieldDestroy);
            _visualEffect.SetTexture("DestroyShieldTextureParticle", _visualEffectsConfigs.ShieldAuraTextureParticle);
            _visualEffect.SetVector3("MaxVelocity", _visualEffectsConfigs.ShieldAuraMaxVelocityParticles);
            _visualEffect.SetFloat("LinearDrag", _visualEffectsConfigs.ShieldAuraLinearDrag);
            _visualEffect.SetFloat("TurbulencePawer", _visualEffectsConfigs.ShieldAuraTurbulencePawer);
            _visualEffect.SetFloat("BaseAlpha", _visualEffectsConfigs.ShieldAuraBaseAlpha);
        }

        public void ShieldEffectEnable(ShieldAuraEffectType shieldAuraEffectType) {
            _visualEffect.SetVector3("ObjectRotation", transform.eulerAngles);

            switch (shieldAuraEffectType) {
                case ShieldAuraEffectType.SpawnShield:
                    _visualEffect.SendEvent("ShieldSpawn");

                    StartCoroutine(DestroyShieldEffectStarted());
                break;

                case ShieldAuraEffectType.DestroyShield:
                    _visualEffect.SendEvent("ShieldDestroy");
                break;
            }
        }

        private IEnumerator DestroyShieldEffectStarted() {
            float destroyEffectTime = _levelConfigs.DefaultDestroyTimeAllObject;
            float elapsedTime = 0f;

            float startDestroyCutoffHeight  = _visualEffectsConfigs.ShieldAuraShieldDestroyStartCutoffHeight;
            float finishDestroyCutoffHeight  = _visualEffectsConfigs.ShieldAuraShieldDestroyFinishCutoffHeight;

            while (elapsedTime < destroyEffectTime) {
                float currentValue = Mathf.Lerp(startDestroyCutoffHeight, finishDestroyCutoffHeight, elapsedTime / destroyEffectTime);

                _visualEffect.SetFloat("CutoffHeight", currentValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _visualEffect.SetFloat("CutoffHeight", finishDestroyCutoffHeight);
        }

        public void SetMoveActive(bool isActive, float maxHeightShieldAura = 0f) {
            if (isActive) {
                _maxHeight = maxHeightShieldAura;

                StartCoroutine(_moveStarted = MoveStarted());
            } else {
                StopCoroutine(_moveStarted);
            }
        }

        private IEnumerator MoveStarted() {
            Vector3 currentPos = transform.localPosition;
            float currentAngle = Mathf.Atan2(currentPos.z, currentPos.x) * Mathf.Rad2Deg;

            while (true) {
                float deltaTime = Time.deltaTime;

                currentAngle -= _orbitSpeed * deltaTime;

                float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * _orbitRadius;

                float y = transform.localPosition.y;

                if (_isRises) {
                    y += _verticalSpeed * deltaTime;

                    if (y >= _maxHeight) {
                        y = _maxHeight;

                        _isRises = false;
                    }
                } else {
                    y -= _verticalSpeed * deltaTime;

                    if (y <= _minHeight) {
                        y = _minHeight;

                        _isRises = true;
                    }
                }

                float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * _orbitRadius;

                Vector3 newPosition = new Vector3(x, y, z);

                transform.localPosition = newPosition;

                float angleY = Mathf.Atan2(newPosition.x, newPosition.z) * Mathf.Rad2Deg;

                transform.localRotation = Quaternion.Euler(0, angleY + 90f, 0);

                yield return null;
            }
        }
    }

    public enum ShieldAuraEffectType {
        SpawnShield,
        DestroyShield
    }
}