using System.Collections.Generic;

public interface IInventory
{
    void AddItem(IItem item);
    void RemoveItem(IItem item);
    float CurrentWeight { get; }
    float MaxWeight { get; }
}