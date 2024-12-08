using UnityEngine;
using Zenject;

public sealed class PlayerManager : MonoBehaviour, IPlayerManagerInput
{
    [Inject]
    private void Construct() {
        transform.SetParent(GameObject.Find("Managers").transform);
    }

    public void TapPositionCheck(Vector2 position) {
        //!
    }
}

public interface IPlayerManagerInput {
    public void TapPositionCheck(Vector2 position);
}