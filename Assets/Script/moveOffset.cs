using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveOffset : MonoBehaviour
{

    private         Material    material;
    public          float       velx, vely;
    public          float       incremento;
    private         float       offset;
    
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += incremento;
        // setturexture = modificar o offset da minha textura
        material.SetTextureOffset("_MainTex", new Vector2(offset * velx, offset * vely));
    }
}
