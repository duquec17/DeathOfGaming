using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// taken from
// https://discussions.unity.com/t/950094
using System.Runtime.CompilerServices;
using System;

public readonly struct Weighted<T>
{
    public readonly float weight;
    public readonly T payload;

    public Weighted(float weight, T payload) : this() 
        => (this.weight, this.payload) = (weight, payload);
    
}

//---------------------------------------------------------------------------------
// Probability menu

public class ProbMenu<T> : IList<Weighted<T>>
{

    List<Weighted<T>> _list;
    IEqualityComparer<T> _eqComp = EqualityComparer<T>.Default;

    float _wsum = 0f;
    public float WeightSum => _wsum;

    public int Capacity
    {
        get => _list.Capacity;
        set => _list.Capacity = value;
    }

    private ProbMenu(IEqualityComparer<T> eqComp, int _)
      => _eqComp = eqComp ?? _eqComp;

    public ProbMenu(IEqualityComparer<T> equalityComparer = null) : this(equalityComparer, 0)
      => _list = new();

    public ProbMenu(int capacity, IEqualityComparer<T> equalityComparer = null) : this(equalityComparer, 0)
      => _list = new(capacity);

    //---------------------------------------------------------------------------------
    // Binary search for autosorting

    static int descending_weight(Weighted<T> a, Weighted<T> b)
      => b.weight.CompareTo(a.weight);

    static int binarySearch(IList<Weighted<T>> list, Weighted<T> value, Comparison<Weighted<T>> comparison)
    {
        int lower = 0;
        int upper = list.Count - 1;

        while (lower <= upper)
        {
            int middle = lower + (upper - lower) / 2;
            int result = comparison(value, list[middle]);
            if (result == 0) return middle;
            else if (result < 0) upper = middle - 1;
            else lower = middle + 1;
        }

        return ~lower;
    }

    //---------------------------------------------------------------------------------
    // Core operations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int insert(Weighted<T> item)
    {
        if (item.weight <= 0f) return -1;

        int index = binarySearch(_list, item, descending_weight);
        if (index < 0) index = ~index;

        _list.Insert(index, item);
        _wsum += item.weight;

        return index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void removeAt(int index)
    {
        _wsum -= this[index].weight;
        _list.RemoveAt(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int updateAt(int index, Weighted<T> item)
    {
        removeAt(index);
        return insert(item);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int indexOf(T payload, int start)
    {
        for (int i = start; i < Count; i++)
            if (_eqComp.Equals(this[i].payload, payload))
                return i;

        return -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void clear()
    {
        _list.Clear();
        _wsum = 0f;
    }

    //---------------------------------------------------------------------------------
    // Public interface

    public Weighted<T> this[int index]
    {
        get => _list[index];
        set => updateAt(index, value);
    }

    public int Count => _list.Count;
    public bool IsReadOnly => false;

    public bool Contains(T payload, int start = 0)
      => IndexOf(payload, start) >= 0;

    public int Add(T payload, float weight = 1f)
      => insert(new(weight, payload));

    public bool Remove(T payload, int start = 0)
    {
        int index = indexOf(payload, start);
        if (index < 0) return false;
        removeAt(index);
        return true;
    }

    public void RemoveAt(int index) => removeAt(index);

    public void UpdateAt(int index, T payload)
      => _list[index] = new(this[index].weight, payload);

    public int UpdateAt(int index, float weight)
      => UpdateAt(index, weight, this[index].payload);

    public int UpdateAt(int index, float weight, T payload)
      => updateAt(index, new(weight, payload));

    public int IndexOf(T payload, int start = 0)
      => indexOf(payload, start);

    public void CopyTo(Weighted<T>[] array, int arrayIndex)
      => _list.CopyTo(array, arrayIndex);

    public void Clear() => clear();
    public void TrimExcess() => _list.TrimExcess();

    public IEnumerator<Weighted<T>> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    //---------------------------------------------------------------------------------
    // Probability

    public float GetProbability(int index)
      => this[index].weight / WeightSum;

    //---------------------------------------------------------------------------------
    // ToString

    public override string ToString()
      => ToString("\n");

    public string ToString(string delimiter, bool inclProbabilities = false, string format = "F2")
    {
        System.Text.StringBuilder sb = new();

        for (int i = 0; i < _list.Count; i++)
        {
            if (i > 0) sb.Append(delimiter);
            sb.Append($"'{this[i].payload.ToString()}'");
            sb.Append($" [W{(this[i].weight).ToString(format)}]");
            if (inclProbabilities) sb.Append($" [{(GetProbability(i) * 1E2f).ToString(format)} %]");
        }

        return sb.ToString();
    }

    //---------------------------------------------------------------------------------
    // Interface fulfillment

    void ICollection<Weighted<T>>.Add(Weighted<T> item) => insert(item);
    bool ICollection<Weighted<T>>.Remove(Weighted<T> item) => Remove(item.payload);
    bool ICollection<Weighted<T>>.Contains(Weighted<T> item) => Contains(item.payload);
    int IList<Weighted<T>>.IndexOf(Weighted<T> item) => IndexOf(item.payload);
    void IList<Weighted<T>>.Insert(int index, Weighted<T> item) => insert(item);

}

