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
                    _baseMaterial.SetTexture("EmissionTexture", _visualEffectsConfigs.RedCrystalEmissionTexture);
                    _baseMaterial.SetColor("EmissionColor", _visualEffectsConfigs.RedCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.BlueCrystalBiome:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("EmissionTexture", _visualEffectsConfigs.BlueCrystalEmissionTexture);
                    _baseMaterial.SetColor("EmissionColor", _visualEffectsConfigs.BlueCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GreenCrystalBiome:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("EmissionTexture", _visualEffectsConfigs.GreenCrystalEmissionTexture);
                    _baseMaterial.SetColor("EmissionColor", _visualEffectsConfigs.GreenCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.ElectricalBiome:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("EmissionTexture", _visualEffectsConfigs.ElectricalEmissionTexture);
                    _baseMaterial.SetColor("EmissionColor", _visualEffectsConfigs.ElectricalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GlitcheBiome:
                    _baseMaterial = new Material(_visualEffectsConfigs.DissolveAndEmission3TexturesWithUV);

                    _baseMaterial.SetTexture("FirstEmissionTexture", _visualEffectsConfigs.FirstGlitcheEmissionTexture);
                    _baseMaterial.SetColor("FirstEmissionColor", _visualEffectsConfigs.FirstGlitcheEmissionColor);

                    _baseMaterial.SetTexture("SecondEmissionTexture", _visualEffectsConfigs.SecondGlitcheEmissionTexture);
                    _baseMaterial.SetColor("SecondEmissionColor", _visualEffectsConfigs.SecondGlitcheEmissionColor);

                    _baseMaterial.SetTexture("ThirdEmissionTexture", _visualEffectsConfigs.ThirdGlitcheEmissionTexture);
                    _baseMaterial.SetColor("ThirdEmissionColor", _visualEffectsConfigs.ThirdGlitcheEmissionColor);
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