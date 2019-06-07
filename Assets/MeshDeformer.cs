using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    // code for smoothing
    [System.Serializable]
    enum FilterType
    {
        Laplacian, HC
    };

    MeshFilter filter;

    [SerializeField, Range(0f, 1f)] float intensity = 0.5f;
    [SerializeField] FilterType type;
    [SerializeField, Range(0, 20)] int times = 3;
    [SerializeField, Range(0f, 1f)] float hcAlpha = 0.5f;
    [SerializeField, Range(0f, 1f)] float hcBeta = 0.5f;
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;

    public float springForce = 20f;
    public float damping = 5f;


    void Start()
    {
        filter = GetComponent<MeshFilter>();
        deformingMesh = GetComponent<MeshFilter>().mesh;


        

        originalVertices = deformingMesh.vertices; // get vertices from defined class
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
        vertexVelocities = new Vector3[originalVertices.Length];

        Debug.Log("Deformer Initialized !!!! ");

    }

    public void AddDeformingForce(Vector3 point, float force)
    {
        point = transform.InverseTransformPoint(point); // world to local space 
        Debug.DrawLine(Camera.main.transform.position, point);
        Debug.Log("Line should appear !!!");

        for (int i=0 ; i< displacedVertices.Length ; i++ )
        {
            AddForceToVertex(i, point, force);
        }
    }

    private void AddForceToVertex(int i, Vector3 point, float force)
    {
        Vector3 pointToVertex = displacedVertices[i] - point ;
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;

    }

    public void AddSmoothing()
    {
        switch (type)
        {
            case FilterType.Laplacian:
                //filter.mesh = MeshSmoothing.LaplacianFilter(deformingMesh, times);
                deformingMesh = MeshSmoothing.LaplacianFilter(deformingMesh, times);
                
                break;
            case FilterType.HC:
                //filter.mesh = MeshSmoothing.HCFilter(deformingMesh, times, hcAlpha, hcBeta);
                deformingMesh = MeshSmoothing.HCFilter(deformingMesh, times, hcAlpha, hcBeta);
                break;
        }
        displacedVertices = deformingMesh.vertices;
        Debug.Log("Smoothing Teeth activated !!!!!");
        Debug.Log("Smoothing Teeth activated !!!!!");

    }

    void Update()
    {
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        deformingMesh.vertices = displacedVertices; // update Vertices of Mesh
        deformingMesh.RecalculateNormals();
    }

    private void UpdateVertex(int i)
    {
        float dt = Time.deltaTime;
        Vector3 velocity = vertexVelocities[i];
        Vector3 displacement = displacedVertices[i] - originalVertices[i];  
        //velocity = velocity - displacement * springForce * dt;
        velocity = velocity - velocity * damping * dt;

        vertexVelocities[i] = velocity;
        displacedVertices[i] += velocity * dt;
    }
}