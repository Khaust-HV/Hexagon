using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelObjectFactory : ILevelObjectFactory {
    #region DI
        private readonly DiContainer _container;
    #endregion

    public LevelObjectFactory(DiContainer container) {
        // Set DI
        _container = container;
    }

    public List<I> CreateObjects<I>(GameObject prefab, int number, Transform trParentObject, float size = 0f) where I : class {
        List<I> objectsList = new List<I>();

        for (int i = 0; i < number; i++) {
            GameObject obj = _container.InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, trParentObject);

            if (size > 0) obj.transform.localScale = new Vector3(size, size, size);

            obj.SetActive(false);

            Component[] components = obj.GetComponents<Component>();
            foreach (Component comp in components) {
                if (comp is I interfaceComponent) {
                    objectsList.Add(interfaceComponent);
                    break;
                }
            }
        }

        return objectsList;
    }

    public List<I> CreateRandomObjects<I>(GameObject[] prefabs, int number, Transform trParentObject, float size = 0f) where I : class {
        List<I> objectsList = new List<I>();

        for (int i = 0; i < number; i++) {
            int randomObject = Random.Range(0, prefabs.Length);

            GameObject obj = _container.InstantiatePrefab(prefabs[randomObject], Vector3.zero, Quaternion.identity, trParentObject);

            if (size > 0) obj.transform.localScale = new Vector3(size, size, size);

            obj.SetActive(false);

            Component[] components = obj.GetComponents<Component>();
            foreach (Component comp in components) {
                if (comp is I interfaceComponent) {
                    objectsList.Add(interfaceComponent);
                    break;
                }
            }
        }

        return objectsList;
    }
}

public interface ILevelObjectFactory {
    List<I> CreateObjects<I> (
        GameObject prefab, 
        int number, 
        Transform trParentObject, 
        float size = 0f
    ) where I : class;
    List<I> CreateRandomObjects<I> (
        GameObject[] prefabs, 
        int number, 
        Transform trParentObject, 
        float size = 0f
    ) where I : class;
}