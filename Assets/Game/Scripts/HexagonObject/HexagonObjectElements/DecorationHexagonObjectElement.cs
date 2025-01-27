using UnityEngine;
using LevelObjectType;

namespace HexagonObjectControl {
    public sealed class DecorationHexagonObjectElement : HexagonObjectElement {
        protected override void SetBaseConfiguration() {
            _spawnEffectTime = _levelConfigs.DefaultSpawnTimeAllObject;

            SetMaterial();
        }

        private void SetMaterial() {
            _baseMaterialPropertyBlock = new MaterialPropertyBlock();

            switch (_hexagonObjectPartType) {
                case DecorationHexagonObjectsType.RedCrystalBiome:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndEmissionTextureWithUV;
                    _baseMaterialPropertyBlock.SetTexture("_EmissionTexture", _visualEffectsConfigs.RedCrystalEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.RedCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.BlueCrystalBiome:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndEmissionTextureWithUV;
                    _baseMaterialPropertyBlock.SetTexture("_EmissionTexture", _visualEffectsConfigs.BlueCrystalEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.BlueCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GreenCrystalBiome:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndEmissionTextureWithUV;
                    _baseMaterialPropertyBlock.SetTexture("_EmissionTexture", _visualEffectsConfigs.GreenCrystalEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.GreenCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.ElectricalBiome:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndEmissionTextureWithUV;
                    _baseMaterialPropertyBlock.SetTexture("_EmissionTexture", _visualEffectsConfigs.ElectricalEmissionTexture);
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.ElectricalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GlitcheBiome:
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
    }
}