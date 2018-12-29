using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    [SerializeField]private Renderer myRend;
    [SerializeField] private Vector3Reference worldDirection;
    [SerializeField] private FloatReference worldSpeed;
    [Range(0,1)]
    [SerializeField] private float moveFactor;
    [Header("References")]
    [SerializeField] private BoolReference gameActive;
    private string textName;
    private Vector2 uvOffset = Vector2.zero;
    float x = 0;
    private void Start()
    {
        textName = myRend.sharedMaterial.mainTexture.name;
    }

    void Update()
    {
        if (gameActive.value)
        {
            uvOffset -= (Vector2)worldDirection.value.normalized * Time.deltaTime * worldSpeed.value * moveFactor;
            myRend.material.mainTextureOffset = (uvOffset);
        }
    }
}