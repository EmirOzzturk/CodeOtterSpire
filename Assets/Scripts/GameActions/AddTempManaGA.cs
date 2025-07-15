using Action_System;
using UnityEngine;

public class AddTempManaGA : GameAction
{
    public int TempManaAmount { get; set; }

    public AddTempManaGA(int tempManaAmount)
    {
        TempManaAmount = tempManaAmount;
    }
}