using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.Extras;

[RequireComponent(typeof(SteamVR_LaserPointer))]
public class LaserPointer : MonoBehaviour
{
    #region Debug

    private bool _mockLaserActive = false;
    private Camera _cam;

    #endregion

    public bool LaserPointerEnabled
    {
        set
        {
            if (SteamVR.active) _laserPointer.active = value;
            else _mockLaserActive = value;
        }
    }

    public UnityEvent<GameObject> onPointerClick;
    public UnityEvent<Avatar, SteamVR_LaserPointer> onAvatarHovered;
    private SteamVR_LaserPointer _laserPointer;

    private void Start()
    {
        _laserPointer = GetComponent<SteamVR_LaserPointer>();
        _laserPointer.PointerIn += OnPointerIn;
        _laserPointer.PointerOut += OnPointerOut;
        _laserPointer.PointerClick += OnPointerClick;
        LaserPointerEnabled = false;
    }

    private void Update()
    {
        if (!_mockLaserActive) return;
        
        if(_cam == null) _cam = Camera.main;
        
        var ray = _cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out var hit))
            {
                onPointerClick?.Invoke(hit.transform.gameObject);
            }
        }

    }


    private void OnPointerIn(object sender, PointerEventArgs e)
    {
        if (e.target.CompareTag("Avatar"))
        {
            onAvatarHovered?.Invoke(e.target.GetComponent<Avatar>(), _laserPointer);
        }        
    }
    
    private void OnPointerOut(object sender, PointerEventArgs e)
    {
        _laserPointer.color = Color.black;
    }
    
    private void OnPointerClick(object sender, PointerEventArgs e)
    {
        onPointerClick?.Invoke(e.target.gameObject);
    }

    
}
