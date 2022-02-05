using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Items
{
    public abstract override void Use();

    public GameObject bullerImpactPrefab;
}
