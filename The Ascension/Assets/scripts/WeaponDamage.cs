using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    private int damage;
    private int poiseDamage;
    [SerializeField] private Collider myCollider;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) { return; }

       if (alreadyCollidedWith.Contains(other)) { return; }
        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage, poiseDamage);
        }
    }

    public void SetAttack(int damage, int poiseDamage)
    {
        this.damage = damage;
        this.poiseDamage = poiseDamage;
        alreadyCollidedWith.Clear();
    }

}

