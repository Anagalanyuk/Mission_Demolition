using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private GameObject _prefabProjecile;
    [SerializeField] private float _velocityMult = 8f;

    [Header("Set Dynamicaly")]
    [SerializeField] private GameObject _launchPoint;
    [SerializeField] private Vector3 _launchPos;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private bool _aimingMode;
    [SerializeField] private Rigidbody _projectileRigidBody;

    private void Awake()
    {
        _launchPoint.SetActive(false);
        _launchPos = _launchPoint.transform.position;
    }

    private void OnMouseEnter()
    {
        _launchPoint.SetActive(true);
        // Debug.Log("<color=red>OnMouseEnter</color>");
    }

    private void OnMouseExit()
    {
        _launchPoint.SetActive(false);
        // Debug.Log("<color=red>OnMouseExit</color>");
    }

    private void OnMouseDown()
    {
        _aimingMode = true;
        _projectile = Instantiate(_prefabProjecile) as GameObject;
        _projectile.transform.position = _launchPos;
        _projectileRigidBody = _projectile.GetComponent<Rigidbody>();
        _projectileRigidBody.isKinematic = true;
    }
    private void Update()
    {
        if(_aimingMode)
        {
            Vector3 mousePos2D = Input.mousePosition;
            mousePos2D.z = -Camera.main.transform.position.z;
            Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

            Vector3 mouseDelta = mousePos3D - _launchPos;
            float maxMagnitude = this.GetComponent<SphereCollider>().radius;
            if(mouseDelta.magnitude > maxMagnitude)
            {
                mouseDelta.Normalize();
                mouseDelta *= maxMagnitude;
            }

            Vector3 projPos = _launchPos + mouseDelta;
            _projectile.transform.position = projPos;
            if(Input.GetMouseButtonUp(0))
            {
                _aimingMode = false;
                _projectileRigidBody.isKinematic = false;
                _projectileRigidBody.velocity = -mouseDelta * _velocityMult;
                FollowCam.POI = _projectile;
                _projectile = null;
            }
        }
    }
}
