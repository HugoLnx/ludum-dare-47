using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxMaster : MonoBehaviour
{
    private const int POOL_SIZE = 10;
    public GameObject instancePrefab;
    private ObjectsPool<VfxInstance> pool;
    public static VfxMaster Instance {get; private set;}

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        this.pool = new ObjectsPool<VfxInstance>("vfx-pool", POOL_SIZE, () => {
            return GameObject.Instantiate(instancePrefab).GetComponent<VfxInstance>();
        });
        this.pool.Parent = this.transform;
    }

    public static void Play(string stateName, Vector2 position) {
        Instance.pool.Next().Play(stateName, position);
    }
}
