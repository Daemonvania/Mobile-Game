using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Hand hand;
    [SerializeField] private Brick brick;
    
    public static readonly bool IsPlaying = true;
    private void OnEnable()
    {
        Hand.OnBrickHit += OnHit;
        Hand.OnBrickMiss += OnMiss;
    }
    
    private void OnDisable()
    {
        Hand.OnBrickHit -= OnHit;
        Hand.OnBrickMiss -= OnMiss;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         Debug.Log(IsPlaying);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnHit()
    {
        hand.Reset();
        brick.MoveHitPoint();
    }
    
    private void OnMiss()
    {
        hand.Reset();
        brick.MoveHitPoint();
    }
}
