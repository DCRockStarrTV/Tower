using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
        
    [SerializeField] private float moveSpeed = 1f;
    private Actions actions;

    private void Awake()
    {
        actions = new Actions();
    }

    private void OnEnable()
    {
        if (LastCube == null)
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();

        CurrentCube = this;
    }

    void Start()
    {
        actions.Enable();
        actions.Cubitos.Stop.performed += Stop;
    }


    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }
    //Al click el cubo se detiene 
    private void Stop(InputAction.CallbackContext context)
    {
        moveSpeed = 0;
        float Sig = transform.position.z - LastCube.transform.position.z;

        SplitCubeOnZ(Sig);
    }

    private void SplitCubeOnZ(float Sig)
    {
        //Recorte del bloque -
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(Sig);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (Sig / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        //Caida del bloque --
        float cubeEdge = transform.position.z + (newZSize / 2f);
        float fallingBlockZposition = cubeEdge + fallingBlockSize / 2f;

        SpawnDropCube(fallingBlockZposition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZposition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(transform.localScale.x, transform.position.y, fallingBlockSize);
        cube.transform.localScale = new Vector3(transform.localScale.x, transform.position.y, fallingBlockSize);
    }
}
