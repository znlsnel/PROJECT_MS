using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserData : IManager
{
    public Inventory Inventory {get; private set;}

    public void Init()
    {
        Inventory = new Inventory();
    }
    
    public void Clear()
    {

    }
}
