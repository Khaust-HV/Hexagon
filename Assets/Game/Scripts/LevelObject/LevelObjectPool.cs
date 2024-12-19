using Hexagon;
using System.Collections.Generic;
using UnityEngine;

public sealed class LevelObjectPool : IBuildingsPool, IUnitsPool, IProjectilesPool {
    private List<IHexagonControl> _hexagonsList;
    private Dictionary<HexagonObjectsStorageKey, List<IHexagonControl>> _hexagonObjectsStorage;
    private Dictionary<UnitObjectsStorageKey, List<IHexagonControl>> _unitsStorage;
    private List<IHexagonControl> _squadsList;

    private Transform _trHexagonsPool;
    private Transform _trHexagonObjectsPool;
    private Transform _trUnitsPool;
    private Transform _trSquadsPool;

    public LevelObjectPool() {
        Transform objectPool = new GameObject("LevelObjectPool").transform;

        _trHexagonsPool = new GameObject("Hexagons").transform;
        _trHexagonsPool.SetParent(objectPool);

        _trHexagonObjectsPool = new GameObject("HexagonObjects").transform;
        _trHexagonObjectsPool.SetParent(objectPool);

        _trUnitsPool = new GameObject("Units").transform;
        _trUnitsPool.SetParent(objectPool);

        _trSquadsPool = new GameObject("Squads").transform;
        _trSquadsPool.SetParent(objectPool);

        _hexagonsList = new List<IHexagonControl>();
        _hexagonObjectsStorage = new Dictionary<HexagonObjectsStorageKey, List<IHexagonControl>>();
        _unitsStorage = new Dictionary<UnitObjectsStorageKey, List<IHexagonControl>>();
        _squadsList = new List<IHexagonControl>();
    }

    public IHexagonControl GetHexagonByID(int hexagonID) {
        return _hexagonsList[hexagonID];
    }

    public Transform GetHexagonTransformPool() {
        return _trHexagonsPool;
    }

    public void AddRangeHexagons(List<IHexagonControl> hexagonsList) {
        _hexagonsList.AddRange(hexagonsList);
    }
}

public interface IBuildingsPool {
    public IHexagonControl GetHexagonByID(int hexagonID);
    public Transform GetHexagonTransformPool();
    public void AddRangeHexagons(List<IHexagonControl> hexagonsList);
}

public interface IUnitsPool {
    
}

public interface IProjectilesPool {

}

public enum HexagonObjectsStorageKey {
    Mine,
    Heap,
    Core,
    BuildebleField,
    UnbuildebleField,
    SafeLiquid,
    DangerousLiquid
}

public enum UnitObjectsStorageKey {
    
}