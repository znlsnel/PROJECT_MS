using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

[Serializable]
public abstract class ResourceStat : NetworkBehaviour
{
    public readonly SyncVar<float> Current = new SyncVar<float>();
    public readonly SyncVar<float> Maximum = new SyncVar<float>();

    public event Action<float, float> OnResourceChanged; // (current, max)

    [ServerRpc(RequireOwnership = false)]
    protected void Init(float maxValue, float currentValue)
    {
        Maximum.Value = maxValue;
        Current.Value = Mathf.Clamp(currentValue, 0, maxValue);
    }

    public override void OnStartNetwork()
    {
        Current.OnChange += OnCurrentChanged;
        Maximum.OnChange += OnMaximumChanged;
    }

    public override void OnStopNetwork()
    {
        Current.OnChange -= OnCurrentChanged;
        Maximum.OnChange -= OnMaximumChanged;
    }

    private void OnCurrentChanged(float prev, float next, bool asServer)
    {
        OnResourceChanged?.Invoke(next, Maximum.Value);
    }

    private void OnMaximumChanged(float prev, float next, bool asServer)
    {
        OnResourceChanged?.Invoke(Current.Value, next);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void Add(float amount)
    {
        Current.Value += amount;
        Current.Value = Mathf.Clamp(Current.Value, 0, Maximum.Value);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void Subtract(float amount)
    {
        Current.Value -= amount;
        Current.Value = Mathf.Clamp(Current.Value, 0, Maximum.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void Modify(float amount)
    {
        Current.Value = Mathf.Clamp(Current.Value, amount, Maximum.Value); 
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetMaximum(float newMax)
    {
        Maximum.Value = newMax;
        Current.Value = Mathf.Clamp(Current.Value, 0, Maximum.Value);
    }
}
