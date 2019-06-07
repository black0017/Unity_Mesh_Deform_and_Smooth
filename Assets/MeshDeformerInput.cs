using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour
{

    public float force = 10f ;
    public float forceOffset = 0.1f;


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
        if (Input.GetMouseButton(1))
        {
            HandleInput2();
        }
    }

    private void HandleInput2()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            Mesh deformingMesh = hit.collider.GetComponent<MeshFilter>().mesh;
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformingMesh)
            {
                Vector3 point = hit.point;           //useless for now
                point += hit.normal * forceOffset;   //useless for now


                deformer.AddSmoothing();
                Debug.Log("AddSmoothing function has been called !!!!");
            }
        }
        else
        {
            Debug.Log("Failed to find Ray collider to tooth. WTF ???????");
        }

    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer)
            {
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point, force);
                Debug.Log("Force is applied ??");
            }
        }
        else
        {
            Debug.Log("Failed to find Ray collider to tooth. WTF ???????");
        }
        
    }



}