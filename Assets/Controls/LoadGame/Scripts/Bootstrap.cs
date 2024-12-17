using UnityEngine;
using Zenject;

public sealed class Bootstrap : MonoBehaviour
{
    #region DI
        IGenerateLevel _iGenerateLevel;
        IBuildingsCreate _iBuildingsCreate;
        IUnitsCreate _iUnitsCreate;
        ISwitchGameplayInput _iSwitchGameplayInput;
    #endregion

    [Inject]
    private void Construct(IGenerateLevel iGenerateLevel, IBuildingsCreate iBuildingsCreate, IUnitsCreate iUnitsCreate, ISwitchGameplayInput iSwitchGameplayInput) {
        // Set DI
        _iGenerateLevel = iGenerateLevel;
        _iBuildingsCreate = iBuildingsCreate;
        _iUnitsCreate = iUnitsCreate;
        _iSwitchGameplayInput = iSwitchGameplayInput;
    }

    private void Awake() {
        // Generate level
        int sumNumberHexagons = _iBuildingsCreate.CreateHexagons();
        _iGenerateLevel.SetSumNumberHexagons(sumNumberHexagons);
        _iGenerateLevel.GenerateLevel();

        // Set control
        _iSwitchGameplayInput.SetGameplayInputActionMapActive(true);
        _iSwitchGameplayInput.SetAllGameplayActive(true);
    }
}