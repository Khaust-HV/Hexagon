using UnityEngine;
using LevelObjectType;

namespace HexagonObjectControl {
    public sealed class DecorationHexagonObjectElement : HexagonObjectElement {
        protected override void SetBaseConfiguration() {
            _spawnEffectTime = _materialConfigs.SpawnEffectTime;

            SetMaterial();
        }

        private void SetMaterial() {
            switch (_hexagonObjectPartType) {
                case DecorationHexagonObjectsType.RedCrystalBiome:
                    _baseMaterial = new Material(_materialConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("EmissionTexture", _materialConfigs.RedCrystalEmissionTexture);
                    _baseMaterial.SetColor("EmissionColor", _materialConfigs.RedCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.BlueCrystalBiome:
                    _baseMaterial = new Material(_materialConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("EmissionTexture", _materialConfigs.BlueCrystalEmissionTexture);
                    _baseMaterial.SetColor("EmissionColor", _materialConfigs.BlueCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GreenCrystalBiome:
                    _baseMaterial = new Material(_materialConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("EmissionTexture", _materialConfigs.GreenCrystalEmissionTexture);
                    _baseMaterial.SetColor("EmissionColor", _materialConfigs.GreenCrystalEmissionColor);
                break;

                case DecorationHexagonObjectsType.ElectricalBiome:
                    _baseMaterial = new Material(_materialConfigs.DissolveAndEmissionTextureWithUV);
                    _baseMaterial.SetTexture("EmissionTexture", _materialConfigs.ElectricalEmissionTexture);
                    _baseMaterial.SetColor("EmissionColor", _materialConfigs.ElectricalEmissionColor);
                break;

                case DecorationHexagonObjectsType.GlitcheBiome:
                    _baseMaterial = new Material(_materialConfigs.DissolveAndEmission3TexturesWithUV);

                    _baseMaterial.SetTexture("FirstEmissionTexture", _materialConfigs.FirstGlitcheEmissionTexture);
                    _baseMaterial.SetColor("FirstEmissionColor", _materialConfigs.FirstGlitcheEmissionColor);

                    _baseMaterial.SetTexture("SecondEmissionTexture", _materialConfigs.SecondGlitcheEmissionTexture);
                    _baseMaterial.SetColor("SecondEmissionColor", _materialConfigs.SecondGlitcheEmissionColor);

                    _baseMaterial.SetTexture("ThirdEmissionTexture", _materialConfigs.ThirdGlitcheEmissionTexture);
                    _baseMaterial.SetColor("ThirdEmissionColor", _materialConfigs.ThirdGlitcheEmissionColor);
                break;

                default:
                    _baseMaterial = new Material(_materialConfigs.DissolveWithUV);
                break;
            }

            _baseMaterial.SetFloat("_Metallic", _materialConfigs.BaseMetallic);
            _baseMaterial.SetFloat("_Smoothness", _materialConfigs.BaseSmoothness);

            foreach (var mrObject in _mrBaseObject) {
                mrObject.material = _baseMaterial;
            }
        }
    }
}