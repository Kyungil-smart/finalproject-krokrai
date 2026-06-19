using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bakery_ShopTableSO", menuName = "Scriptable Objects/Bakery_ShopTableSO")]
public class Bakery_ShopTableSO : ScriptableObject
{
    [SOInject(0)]public string Shop_Index;
    [SOInject(1)]public Shop_TypeEnum Shop_Type;
    [SOInject(2)]public int Shot_number;
    [SOInject(3)]public int Shop_buyCost;
    [SOInject(4)]public int Shop_buyLimit;
}
