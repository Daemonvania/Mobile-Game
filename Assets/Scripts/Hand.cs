using System;
using System.Collections;
using UnityEngine;
using Lean.Touch;
public class Hand : MonoBehaviour
{
    [SerializeField] private float hoverSpeed = 1f;
    [SerializeField] private float dropSpeed = 1f; 
    [SerializeField] private float timeToReset = 1f;
    [SerializeField] private float hitLeewayDist = 0.2f;
    [SerializeField] private float timeForStrong = 0.2f;
    [SerializeField] private float strongDuration = 0.2f;
    [Space]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;
    //todo change
    private Renderer renderer;
    
    [Space]
    
    [SerializeField] Material strongMaterial;
    Material baseMaterial;
    
    public delegate void OnHit();
    public delegate void OnMiss();
    
    public static event OnHit OnBrickHit;
    public static event OnMiss OnBrickMiss;
    
    private GameObject hitPoint;
    
    private Vector3 startPos;
  
    
    bool isDropping = false;
    private float hoverTime;
    private static bool isStrong = false;
    
    private Coroutine resetCoroutine;
    private Coroutine strongCoroutine;

    private void Start()
    {
        startPos = transform.position;
        hitPoint = GameObject.FindGameObjectWithTag("HitPoint");
        renderer = GetComponentInChildren<Renderer>();
        baseMaterial = renderer.material;
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
        
    }
    
    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= OnFingerDown;
    }

    //todo add InputManager so touch things arent here? 
    
    // Update is called once per frame
    void Update()
    {
        
        if (!GameManager.IsPlaying) return;
        if (isDropping)
        {
            HandleDropping();
            CheckHit();
        }
        else
        {
            HandleHovering();
        }
    }

    void HandleDropping()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y - (dropSpeed * Time.deltaTime),
            transform.position.z
        );
    }
    void CheckHit()
    {
        Vector3 hitPointPosY;
        hitPointPosY = new Vector3(transform.position.x, hitPoint.transform.position.y, transform.position.z);
        if (Vector3.Distance(gameObject.transform.position, hitPointPosY) < 0.1)
        {
            Debug.Log("Height!");
            Vector3 hitPointPosX;
            hitPointPosX = new Vector3(hitPoint.transform.position.x, transform.position.y, transform.position.z);
            if (Vector3.Distance(transform.position, hitPointPosX) < hitLeewayDist)
            {
                Debug.Log("Hit!");
                StopCoroutine(resetCoroutine);
                resetCoroutine = null;
                OnBrickHit?.Invoke();
            }
        }
    }

    void HandleHovering()
    {
        hoverTime += Time.deltaTime * hoverSpeed;

        // Calculate the X position using a sine wave
        float xPosition = Mathf.Lerp(minX, maxX, (Mathf.Sin(hoverTime) + 1f) / 2f);

        // Update the position of the Hand object
        transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
    }
    
    private void OnFingerDown(LeanFinger finger)
    {
        if (!isDropping)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            strongCoroutine = StartCoroutine(WaitForStrong());
        }
    }
    private void OnFingerUp(LeanFinger finger)
    {
        if (!isDropping)
        {
            isDropping = true;
            StopCoroutine(strongCoroutine);
            resetCoroutine = StartCoroutine(WaitToReset());
        }
    }

    private IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(timeToReset);
        OnBrickMiss?.Invoke();
        Reset();
    }

    private IEnumerator WaitForStrong()
    {
        yield return new WaitForSeconds(timeForStrong);
        isStrong = true;
        renderer.material = strongMaterial;
        yield return new WaitForSeconds(strongDuration);
        isStrong = false;
        renderer.material = baseMaterial;
    }
    
    public void Reset()
    {
        isDropping = false;
        transform.position = startPos;
        hoverTime = 0;
        renderer.material = baseMaterial;
    }

    public static bool IsStrong()
    {
        return isStrong;
    }
    
    public Vector2 GetPosRange() 
    {
        return new Vector2(minX, maxX);
    }
}