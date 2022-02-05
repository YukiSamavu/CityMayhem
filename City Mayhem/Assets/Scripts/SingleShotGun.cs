using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitposition, Vector3 hitnormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitposition, 0.3f);
        if(colliders.Length != 0)
        {
            GameObject bullerImpactObj = Instantiate(bullerImpactPrefab, hitposition + hitnormal * 0.0001f, Quaternion.LookRotation(hitnormal, Vector3.up) * bullerImpactPrefab.transform.rotation);
            Destroy(bullerImpactObj, 5f);
            bullerImpactObj.transform.SetParent(colliders[0].transform);
        }
    }
}
