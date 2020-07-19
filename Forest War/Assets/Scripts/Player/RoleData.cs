using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RoleData
{
    private const string prefix_prefab = "Prefabs/";

    public RoleType RoleType { get; private set; }
    public GameObject RolePrefab { get; private set; }
    public GameObject ArrowPrefab { get; private set; }
    public Vector3 Birthplace { get; private set; }
    public GameObject ExplosionEffect { get; private set; }

    public RoleData(RoleType roleType, string rolePfbPath, string arrowPfbPath, string explosionEftPath,Transform birthplace)
    {
        RoleType = roleType;
        RolePrefab = Resources.Load(prefix_prefab + rolePfbPath) as GameObject;
        ExplosionEffect = Resources.Load(prefix_prefab + explosionEftPath) as GameObject;
        ArrowPrefab = Resources.Load(prefix_prefab + arrowPfbPath) as GameObject;
        ArrowPrefab.GetComponent<Arrow>().explosionEffect = ExplosionEffect;
        Birthplace = birthplace.position;
    }

}
