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
            private MaterialConfigs _materialConfigs;
            private VisualEffectConfigs _visualEffectConfigs;
            private LevelConfigs _levelConfigs;
        #endregion

        [Inject]
        private void Construct (
            HexagonObjectConfigs hexagonObjectConfigs,
            MaterialConfigs materialConfigs, 
            VisualEffectConfigs visualEffectConfigs,
            LevelConfigs levelConfigs
            ) {
            // Set configurations

            _materialConfigs = materialConfigs;
            _visualEffectConfigs = visualEffectConfigs;
            _levelConfigs = levelConfigs;

            _orbitSpeed = hexagonObjectConfigs.OrbitSpeedShieldAura;
            _orbitRadius = hexagonObjectConfigs.OrbitRadiusShieldAura;
            _verticalSpeed = hexagonObjectConfigs.VerticalSpeedShieldAura;
            _minHeight = hexagonObjectConfigs.MinHeightShieldAura;

            _visualEffect = GetComponent<VisualEffect>();
            SetShieldAuraShieldSpawnConfiguration();
        }

        private void SetShieldAuraShieldSpawnConfiguration() {
            _visualEffect.visualEffectAsset = _visualEffectConfigs.ShieldAuraShieldSpawn;
            _visualEffect.SetFloat("Metallic", _materialConfigs.BaseMetallic);
            _visualEffect.SetFloat("Smoothness", _materialConfigs.BaseSmoothness);
            _visualEffect.SetFloat("NoiseScale", _materialConfigs.DestroyNoiseScale);
            _visualEffect.SetFloat("NoiseStrength", _materialConfigs.DestroyNoiseStrength);
            _visualEffect.SetVector4("FresnelColor", _materialConfigs.ShieldAuraEmissionFresnelColor);
            _visualEffect.SetFloat("FresnelPower", _materialConfigs.ShieldAuraEmissionFresnelPower);
            _visualEffect.SetVector4("EmissionColor", _materialConfigs.ShieldAuraEmissionColor);
            _visualEffect.SetMesh("ShieldMesh", GetComponent<MeshFilter>().sharedMesh);
            _visualEffect.SetMesh("ShieldFragmentMesh", _visualEffectConfigs.DestroyVFXParticleMesh);
            _visualEffect.SetFloat("SizeParticle", _levelConfigs.HexagonObjectSize * transform.localScale.x);
            _visualEffect.SetFloat("LifeTimeParticle", _materialConfigs.DestroyEffectTime);
            _visualEffect.SetInt("ParticlesNumberForShieldDestroy", _visualEffectConfigs.ShieldAuraParticlesNumberForShieldDestroy);
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
            float elapsedTime = 0f;

            float destroyEffectTime = _materialConfigs.DestroyEffectTime * 0.85f;

            float destroyStartCutoffHeight = _visualEffectConfigs.ShieldAuraDestroyStartCutoffHeight;
            float destroyFinishCutoffHeight = _visualEffectConfigs.ShieldAuraDestroyFinishCutoffHeight;

            while (elapsedTime < destroyEffectTime) {
                float currentValue = Mathf.Lerp(destroyStartCutoffHeight, destroyFinishCutoffHeight, elapsedTime / destroyEffectTime);

                _visualEffect.SetFloat("CutoffHeight", currentValue);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _visualEffect.SetFloat("CutoffHeight", destroyFinishCutoffHeight);
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