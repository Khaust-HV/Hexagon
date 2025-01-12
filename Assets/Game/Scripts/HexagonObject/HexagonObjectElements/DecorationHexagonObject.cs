using UnityEngine;
using LevelObjectType;

namespace HexagonObjectControl {
    public sealed class DecorationHexagonObject : HexagonObjectElement {
        [SerializeField] private DecorationHexagonObjectsType _decorationHexagonObjectType;

        protected override void SetBaseConfiguration() {
            _spawnEffectTime = _materialConfigs.SpawnEffectTime;

            if (IsObjectHaveEmission) {
                switch (_decorationHexagonObjectType) {
                    case DecorationHexagonObjectsType.RedCrystalBiome:
                        _baseMaterial = new Material(_materialConfigs.DissolveShaderEffectWithUVAndEmission);
                        _baseMaterial.SetTexture("EmissionTexture", _materialConfigs.RedCrystalEmissionTexture);
                        _baseMaterial.SetColor("EmissionColor", _materialConfigs.RedCrystalEmissionColor);
                    break;

                    case DecorationHexagonObjectsType.BlueCrystalBiome:
                        _baseMaterial = new Material(_materialConfigs.DissolveShaderEffectWithUVAndEmission);
                        _baseMaterial.SetTexture("EmissionTexture", _materialConfigs.BlueCrystalEmissionTexture);
                        _baseMaterial.SetColor("EmissionColor", _materialConfigs.BlueCrystalEmissionColor);
                    break;

                    case DecorationHexagonObjectsType.GreenCrystalBiome:
                        _baseMaterial = new Material(_materialConfigs.DissolveShaderEffectWithUVAndEmission);
                        _baseMaterial.SetTexture("EmissionTexture", _materialConfigs.GreenCrystalEmissionTexture);
                        _baseMaterial.SetColor("EmissionColor", _materialConfigs.GreenCrystalEmissionColor);
                    break;

                    case DecorationHexagonObjectsType.GlitcheBiome:
                        _baseMaterial = new Material(_materialConfigs.DissolveShaderEffectWithUVAndEmission3Textures);
                        _baseMaterial.SetFloat("_Metallic", _materialConfigs.BaseMetallic);
                        _baseMaterial.SetFloat("_Smoothness", _materialConfigs.BaseSmoothness);

                        _baseMaterial.SetTexture("FirstEmissionTexture", _materialConfigs.FirstGlitcheEmissionTexture);
                        _baseMaterial.SetColor("FirstEmissionColor", _materialConfigs.FirstGlitcheEmissionColor);

                        _baseMaterial.SetTexture("SecondEmissionTexture", _materialConfigs.SecondGlitcheEmissionTexture);
                        _baseMaterial.SetColor("SecondEmissionColor", _materialConfigs.SecondGlitcheEmissionColor);

                        _baseMaterial.SetTexture("ThirdEmissionTexture", _materialConfigs.ThirdGlitcheEmissionTexture);
                        _baseMaterial.SetColor("ThirdEmissionColor", _materialConfigs.ThirdGlitcheEmissionColor);
                    break;

                    default:
                        throw new LevelObjectException(LevelObjectErrorType.InvalidHexagonObjectType);
                    // break;
                }
            } else _baseMaterial = new Material(_materialConfigs.DissolveShaderEffectWithUV);

            _baseMaterial.SetFloat("_Metallic", _materialConfigs.BaseMetallic);
            _baseMaterial.SetFloat("_Smoothness", _materialConfigs.BaseSmoothness);

            foreach (var mrObject in MRBaseObject) {
                mrObject.material = _baseMaterial;
            }
        }
    }
}