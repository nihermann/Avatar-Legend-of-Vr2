using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Vector3 position => transform.position;
    public Quaternion quaternion => transform.rotation;

    public Color DefaultColor { get; private set ; }

    public Field prevField;
    public Field nextField;
    
    void Start(){
        DefaultColor = this.GetComponent<Renderer>().material.color;
    }

}
