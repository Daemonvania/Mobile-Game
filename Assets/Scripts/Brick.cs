using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Brick : MonoBehaviour
{
    [SerializeField] private Transform hitPoint;
    [Space]
    [SerializeField] private GameObject brickStart;
    [SerializeField] private GameObject brickEnd;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        MoveHitPoint();
    }

    public void MoveHitPoint()
    {
       hitPoint.transform.position = new Vector3(
            Random.Range(brickStart.transform.position.x, brickEnd.transform.position.x),
           hitPoint.transform.position.y,
            hitPoint.transform.position.z
        ); 
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
