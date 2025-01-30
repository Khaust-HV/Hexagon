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
                    _baseMaterial = _visualEffectsConfigs.DissolveAndHitAndEmission1TextureWithUV;
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.RedCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.BlueCrystalBiome:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndHitAndEmission1TextureWithUV;
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.BlueCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GreenCrystalBiome:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndHitAndEmission1TextureWithUV;
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.GreenCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.ElectricalBiome:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndHitAndEmission1TextureWithUV;
                    _baseMaterialPropertyBlock.SetColor("_EmissionTextureColor", _visualEffectsConfigs.ElectricalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GlitcheBiome:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndHitAndEmission3TexturesWithUV;

                    _baseMaterialPropertyBlock.SetColor("_FirstEmissionColor", _visualEffectsConfigs.GlitcheFirstEmissionColor);

                    _baseMaterialPropertyBlock.SetColor("_SecondEmissionColor", _visualEffectsConfigs.GlitcheSecondEmissionColor);

                    _baseMaterialPropertyBlock.SetColor("_ThirdEmissionColor", _visualEffectsConfigs.GlitcheThirdEmissionColor);
                break;

                default:
                    _baseMaterial = _visualEffectsConfigs.DissolveAndHitWithUV;
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