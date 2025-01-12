using UnityEngine;
using Zenject;

public sealed class Bootstrap : MonoBehaviour {
    #region DI
        IGenerateLevel _iGenerateLevel;
        ISwitchGameplayInput _iSwitchGameplayInput;
    #endregion

    [Inject]
    private void Construct(IGenerateLevel iGenerateLevel, ISwitchGameplayInput iSwitchGameplayInput) {
        // Set DI
        _iGenerateLevel = iGenerateLevel;
        _iSwitchGameplayInput = iSwitchGameplayInput;
    }

    private void Awake() {
        // Generate level
        _iGenerateLevel.GenerateLevel();

        // Set control
        _iSwitchGameplayInput.SetGameplayInputActionMapActive(true);
        _iSwitchGameplayInput.SetAllGameplayActive(true);
    }
}