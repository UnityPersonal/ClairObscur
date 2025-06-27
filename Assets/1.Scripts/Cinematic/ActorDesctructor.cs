using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ActorDesctructor : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Collider[] colliders;
    private Rigidbody[] rigidbodies;
    
    private Transform rootTransform;
    
    Dictionary<Collider, Vector3> rigPositions = new Dictionary<Collider, Vector3>();
    Dictionary<Collider, Quaternion> rigRotations = new Dictionary<Collider, Quaternion>();
    
        
    void Awake()
    {
        colliders = GetComponentsInChildren<Collider>(true);
        rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        

        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        
        foreach (var rb in rigidbodies)
        {
            rb.useGravity = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyActor();
            EditorApplication.isPaused = true; // For testing purposes, pause the game after destruction
        }
        else if (StartDestruct)
        {
            //StartDestruct = false;
            foreach (var col in colliders)
            {
                col.transform.position = rigPositions[col];
                col.transform.rotation = rigRotations[col];
                //col.enabled = true;
            }
            
            // all rigidbodies
            /*foreach (var rb in rigidbodies)
            {
                rb.useGravity = true;
                rb.isKinematic = false; // Make sure the rigidbody does not respond to physics
            }*/
        }
    }


    bool StartDestruct = false;
    public void DestroyActor()
    {
        StartDestruct = true;
                
        // all colliders
        foreach (var col in colliders)
        {
            rigPositions[col] = col.transform.position;
            rigRotations[col] = col.transform.rotation;
            
        }
        animator.runtimeAnimatorController = null;
        
       
    }
    
}
