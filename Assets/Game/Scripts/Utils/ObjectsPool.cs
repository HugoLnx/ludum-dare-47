using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool<T>
where T:MonoBehaviour
{
    private GameObject root;
    private string name;
    private T[] pool;
    private int nextInx = 0;
    public Transform Parent {
        get => this.root.transform.parent;
        set => this.root.transform.SetParent(value);
    }
    public T Current => pool[CurrentInx];
    public int CurrentInx => (nextInx+(pool.Length-1)) % pool.Length;

    public ObjectsPool(GameObject root, int size, System.Func<T> recipe) {
        this.root = root;
        pool = new T[size];
        for (var i = 0; i < size; i++) {
            this.pool[i] = recipe();
            this.pool[i].transform.SetParent(this.root.transform);
        }
    }

    public ObjectsPool(string name, int size, System.Func<T> recipe) :
        this(new GameObject($"pool-{name}"), size, recipe) {}

    public T Next() {
        var el = pool[nextInx];
        nextInx = (nextInx + 1) % pool.Length;
        return el;
    }

    public T GetByInx(int inx)
    {
        return pool[inx];
    }
}
