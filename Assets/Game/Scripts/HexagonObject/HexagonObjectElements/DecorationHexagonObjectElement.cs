using UnityEngine;
using LevelObjectType;

namespace HexagonObjectControl {
    public sealed class DecorationHexagonObjectElement : HexagonObjectElement {
        protected override void SetBaseConfiguration() {
            _spawnEffectTime = _levelConfigs.DefaultSpawnTimeAllObject;

            SetMaterial();
        }

        private void SetMaterial() {
            switch (_hexagonObjectPartType) {
                case DecorationHexagonObjectsType.RedCrystalBiome:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("_EmissionTexture", _visualEffectsConfigs.RedCrystalEmissionTexture);
                    _baseMaterial.SetColor("_EmissionTextureColor", _visualEffectsConfigs.RedCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.BlueCrystalBiome:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("_EmissionTexture", _visualEffectsConfigs.BlueCrystalEmissionTexture);
                    _baseMaterial.SetColor("_EmissionTextureColor", _visualEffectsConfigs.BlueCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GreenCrystalBiome:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("_EmissionTexture", _visualEffectsConfigs.GreenCrystalEmissionTexture);
                    _baseMaterial.SetColor("_EmissionTextureColor", _visualEffectsConfigs.GreenCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.ElectricalBiome:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("_EmissionTexture", _visualEffectsConfigs.ElectricalEmissionTexture);
                    _baseMaterial.SetColor("_EmissionTextureColor", _visualEffectsConfigs.ElectricalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GlitcheBiome:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveAndEmission3TexturesWithUV);

                    _baseMaterial.SetTexture("_FirstEmissionTexture", _visualEffectsConfigs.FirstGlitcheEmissionTexture);
                    _baseMaterial.SetColor("_FirstEmissionColor", _visualEffectsConfigs.FirstGlitcheEmissionColor);

                    _baseMaterial.SetTexture("_SecondEmissionTexture", _visualEffectsConfigs.SecondGlitcheEmissionTexture);
                    _baseMaterial.SetColor("_SecondEmissionColor", _visualEffectsConfigs.SecondGlitcheEmissionColor);

                    _baseMaterial.SetTexture("_ThirdEmissionTexture", _visualEffectsConfigs.ThirdGlitcheEmissionTexture);
                    _baseMaterial.SetColor("_ThirdEmissionColor", _visualEffectsConfigs.ThirdGlitcheEmissionColor);
                break;

                default:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveWithUV);
                break;
            }

            _baseMaterial.SetFloat("_Metallic", _visualEffectsConfigs.DefaultMetallic);
            _baseMaterial.SetFloat("_Smoothness", _visualEffectsConfigs.DefaultSmoothness);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _baseMaterial;
            }
        }
    }
}