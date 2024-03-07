using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
    }

    void Start()
    {
        actions.Enable();
        actions.Cubitos.Stop.performed += Stop;
    }


    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;

        if (CurrentCube != null)
            //Stop-----------------------------------------------------------------------------------------------------------------------------------------------------------

            FindObjectOfType<CubeSpawn>().SpawnCube();

    }

    

    //Al click el cubo se detiene 
    private void Stop(InputAction.CallbackContext context)
    {
        moveSpeed = 0;
        float Sig = transform.position.z - LastCube.transform.position.z;

        if (Mathf.Abs(Sig) >= LastCube.transform.localScale.z)
        {
            LastCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(0);

        }

        float direction = Sig > 0 ? 1f : -1f;
        SplitCubeOnZ(Sig, direction);

        

    }

    private void SplitCubeOnZ(float Sig, float direction)
    {
        //Recorte del bloque -
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(Sig);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (Sig / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        //Caida del bloque --
        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZposition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockZposition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZposition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
        cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZposition);

        cube.AddComponent<Rigidbody>();
        Destroy(cube.gameObject, 1f);
    }

    

   
}
