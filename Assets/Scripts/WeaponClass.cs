using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// child class for Defensive Building class
/// </summary>
public class WeaponClass : MonoBehaviour
{
    /// <summary>
    /// the following parameters will be inherited from parent class
    /// </summary>
    private float Hp;           //health of the building
    private float Cost;         //cost for building and/or upgrading
    private float Level;        //level of the building
    
    /// <summary>
    /// the following parameters will be for this class
    /// </summary>
    private float Damage;

    /// <summary>
    /// constructor, initializes parameter values
    /// </summary>
    public WeaponClass()
    {
        this.Hp = 200;
        this.Damage = 100;
        this.Level = 1;
        this.Cost = this.Cost + (this.Level * 25);
    }

    /// <summary>
    /// behaviour when upgrading
    /// </summary>
    public void Upgrade()
    {
        this.Damage += 50;
        this.Hp += 100;
        this.Level++;
        this.Cost = this.Cost + (this.Level * 25);
    }

    /// <summary>
    /// behaviour when selling
    /// </summary>
    public void Sell()
    {
        Destroy(this);
    }

}
