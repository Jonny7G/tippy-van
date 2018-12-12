using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float min, max;
    [SerializeField] private float speed;
    [SerializeField] private Vector3Reference direction;
    private bool straight = true;

    private void Start()
    {
        gameObject.SetActive(true);
        transform.parent = null;
    }

    public void MoveCam(float speed)
    {
        //transform.position += new Vector3(direction.value.x * Time.deltaTime * speed, 0, 0);

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, min, max), transform.position.y, transform.position.z);
    }
}