using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellets : BaseAmmo
{
    public override void InitializeDmgAmmo(Vector3 target, int damage, float moveSpeed, Tower sender)
    {
        this.tower = sender;
        this.target = target;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
    }

    public override void GetCollider()
    {
        _collider = GetComponent<CapsuleCollider2D>();
    }


    public override void SpecificBehaviour(Collider2D other)
    {
        other.GetComponent<Animal>().GetDamaged(damage);
    }
}
