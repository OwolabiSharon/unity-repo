using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;

    public void EnableWeapon(int index)
    {
        weapons[index].SetActive(true);
    }

    public void DisableWeapon(int index)
    {
        weapons[index].SetActive(false);
    }
}

