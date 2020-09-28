using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PoolConfig {
    public int size;
    public GameObject prefab;
}

public class ObjectPoolsManager<T> : MonoBehaviour
where T:MonoBehaviour
{
    private static ObjectPoolsManager<T> instance;
    public PoolConfig[] configs;
    private Dictionary<string, ObjectsPool<T>> pools = new Dictionary<string, ObjectsPool<T>>();

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }
    }
    private void Start() {
        foreach (var config in configs) {
            var pool = new ObjectsPool<T>($"pool-for-{config.prefab.name}", config.size, () => {
                return GameObject.Instantiate(config.prefab).GetComponent<T>();
            });
            pool.Parent = this.transform;
            pools[config.prefab.name] = pool;
        }
    }

    public static T Next(string name = null) => instance.NextInstance(name);

    public T NextInstance(string name = null) {
        if (name == null) name = configs[0].prefab.name;
        var pool = pools[name];
        return pool.Next();
    }
}
