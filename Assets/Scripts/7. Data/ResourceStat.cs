using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class ResourceStat : Stat
{
    public float Current { get; private set; }
    public float Maximum;
    
    public ResourceStat(float baseValue) : base(baseValue)
    {
        Maximum = baseValue;
        Current = baseValue;
    } 

    public event Action<float, float> onResourceChanged; // (current, max)
    
    public void Add(float amount)
    {
        Current += amount;
        Current = Mathf.Clamp(Current, 0, Maximum);
        onResourceChanged?.Invoke(Current, Maximum);
    }
    
    public void Subtract(float amount)
    {
        Current -= amount;
        Current = Mathf.Clamp(Current, 0, Maximum);
        onResourceChanged?.Invoke(Current, Maximum);
    }

    public void Modify(float amount)
    {
        Current = Mathf.Clamp(Current, amount, Maximum); 
        onResourceChanged?.Invoke(Current, Maximum);
    }

    public void SetMaximum(float newMax)
    {
        Maximum = newMax;
        Current = Mathf.Clamp(Current, 0, Maximum);
        onResourceChanged?.Invoke(Current, Maximum);
    }
}
