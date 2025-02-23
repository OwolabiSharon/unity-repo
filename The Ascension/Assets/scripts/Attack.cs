using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attack 
{

    [field: SerializeField] public string AnimationName {get; private set;}
    [field: SerializeField] public float TransitionDuration {get; private set;}
    [field: SerializeField] public int ComboStateIndex {get; private set;} = -1;
    [field: SerializeField] public float ComboAttackTime {get; private set;}
    [field: SerializeField] public float AttackForce {get; private set;}
    [field: SerializeField] public float AttackForceTime {get; private set;}
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int PoiseDamage { get; private set; }
    [field: SerializeField] public int WeaponIndex { get; private set; }
} 
