using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State
    {
        FLIPPING,
        GROUNDED,
        CUTTING
    }

    public Action OnStateChanged;
    public KitchenHandler kitchenHandler;
    public Transform playerRenderer;
    public Transform targetForward;
    public Collider bladeCollider;
    public Collider bodyCollider;

    public float startRotation = 110f;

    public float jumpForce = 10f;
    public float moveSpeed = 10f;
    public float rotateDuration = 0.75f;
    public float acceleration = -100f;
    public float collideForce = 10f;

    private bool isStart = false;
    private Vector3 velocity = Vector3.zero;
    private float realRotateDuration = 1f;
    private Vector3 addForce = Vector3.zero;

    private State state = State.GROUNDED;
    private State prevState = State.GROUNDED;
    public State CurrentState
    {
        get
        {
            return state;
        }
        set
        {
            if (state == value) return;

            prevState = state;
            state = value;
            OnStateChanged?.Invoke();
        }
    }

    private int lastScore = 0;

    void Start()
    {
        isStart = false;
        startTimer = false;
    }

    public void Init(Vector3 startPosition)
    {
        transform.position = startPosition;

        var angle = new Vector3(startRotation, 0, 0);
        playerRenderer.localEulerAngles = angle;

        state = State.GROUNDED;
        isStart = false;
        velocity = Vector3.zero;
        addForce = Vector3.zero;
        lastScore = 0;

        GameManager.Instance.OnStateChange -= OnGameStateChanged;
        GameManager.Instance.OnStateChange += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.PLAY)
        {
            StartFlip();
            TimeManager.Instance.StartTimer();
            InputController.OnTouchBegan -= OnTouchBegan;
            InputController.OnTouchBegan += OnTouchBegan;
            kitchenHandler.ResetKitchen();
        }
        else
        {
            // InputController.OnTouchBegan -= OnTouchBegan;
        }
    }

    private void OnTouchBegan(Vector3 position)
    {
        if (isStart == false)
        {
            return;
        }

        if (InputController.Instance.IsOnUI()) return;

        Jump();
    }

    float yVelocityForce = 0f;
    float zVelocityForce = 0f;
    float cuttingVelocityForce = 0f;
    

    void Update()
    {
        if (isStart == false) return;

        var position = transform.position;

        if (flipTimer > 0f && (CurrentState == State.CUTTING))
        {
            flipTimer -= Time.deltaTime;
            if (flipTimer <= 0f)
            {
                CurrentState = State.FLIPPING;
            }
        }

        if (CurrentState == State.FLIPPING || (CurrentState == State.CUTTING))
        {
            velocity.y += acceleration * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, -25, 30f);

            if (CurrentState == State.CUTTING)
            {
                velocity.z = Mathf.SmoothDamp(velocity.z, 0f, ref cuttingVelocityForce, 0.1f);
                velocity.y = Mathf.Clamp(velocity.y, -20, 30f);
            }

            velocity.z = Mathf.Clamp(velocity.z, -30, 30f);

            position.y += (velocity.y + addForce.y) * Time.deltaTime;
            position.z += (velocity.z + addForce.z) * Time.deltaTime;

            transform.position = position;

            if (shouldRotate)
            {
                rotateTime += Time.deltaTime;
            }

            var t = Mathf.Clamp((rotateTime / realRotateDuration), 0, 1f);

            var rotation = startRot * Quaternion.AngleAxis(t * angleBetween, Vector3.right);
            playerRenderer.rotation = rotation;

            if ((rotateTime >= realRotateDuration) && (CurrentState == State.FLIPPING))
            {
                CalculateTargetRotation(rotateDuration, playerRenderer.forward);
            }
        }

        if (CurrentState == State.FLIPPING || (CurrentState == State.CUTTING))
        {
            flippingTime += Time.deltaTime;
        }
        else
        {
            flippingTime = 0f;
        }

        if (addForce.y != 0f)
        {
            addForce.y = Mathf.SmoothDamp(addForce.y, 0f, ref yVelocityForce, 0.25f);
        }

        if (addForce.z != 0f)
        {
            addForce.z = Mathf.SmoothDamp(addForce.z, 0f, ref zVelocityForce, 0.25f);
        }
    }

    public void OnEnable()
    {
        EventManager.Instance.OnGameStart += StartTimer;
    }

    public void OnDisable()
    {
        EventManager.Instance.OnGameStart -= StartTimer;
    }
    public bool startTimer = false;
    public void StartFlip()
    {
        if (!startTimer)
        {
            StartTimer();
        }
        isStart = true;
        prevState = CurrentState;
        CurrentState = State.FLIPPING;

        Jump();
    }

   public void StartTimer()
    {
        startTimer = true;
        TimeManager.Instance.StartTimer();
        //EventManager.Instance.OnGameStart();
    }
    //public void stoptimer()
    //{
    //    startTimer = false;
    //}

    Quaternion startRot;
    float rotateTime = 0f;
    float angleBetween;
    bool shouldRotate = true;

    private void CalculateTargetRotation(float duration, Vector3 target, bool add360 = true, int round = 1)
    {
        startRot = playerRenderer.rotation;
        rotateTime = 0f;

        angleBetween = Vector3.SignedAngle(targetForward.forward, target, Vector3.right);

        if (angleBetween >= 0)
        {
            angleBetween = 360f - angleBetween;
        }
        else
        {
            angleBetween = Mathf.Abs(angleBetween) + (add360 ? (360f * round) : 0f);
        }

        realRotateDuration = duration;
    }

    
    public void Jump()
    {
        CurrentState = State.FLIPPING;

        velocity.z = moveSpeed;
        velocity.y = jumpForce;
        shouldRotate = true;
        addForce = Vector3.zero;

        SetActiveCollider(true);

        flippingTime = 0f;

        CalculateTargetRotation(rotateDuration, playerRenderer.forward);

        SoundManager.Instance.PlayRandomJumpClip();
    }

    private float flippingTime = 0f;
    private bool canGround
    {
        get
        {
            return flippingTime >= 0.05f;
        }
    }

    private void SetActiveCollider(bool active)
    {
        bladeCollider.enabled = active;
        bodyCollider.enabled = active;
    }

    public void OnCollisionTriggerEnter(Collider thisCollider, Collision other)
    {
        if (other == null) return;
        if (other.collider == null) return;

        if (other.collider.CompareTag("Water") && canGround)
        {
            CurrentState = State.GROUNDED;
            velocity.z = 0f;

            flippingTime = 0f;
            flipTimer = 0f;
            //SetActiveCollider(false);

            GameManager.Instance.Lose();
            return;
        }

        if (other.collider.CompareTag("Item") && (CurrentState != State.GROUNDED))
        {
            if (velocity.y < 0)
            {
                if (CurrentState != State.CUTTING) lastScore = GameManager.Instance.Score;
                CurrentState = State.CUTTING;
            }

            flipTimer = 0f;
            addForce = Vector3.zero;
            return;
        }

        if (other.collider.CompareTag("Ground") && (CurrentState != State.GROUNDED))
        {
            var collidePoint = other.collider.ClosestPoint(thisCollider.transform.position);
            var dir = (this.transform.position - collidePoint).normalized;
            var distance = (this.transform.position - collidePoint).magnitude;

            if (thisCollider.CompareTag("Blade") && canGround)
            {
                CurrentState = State.GROUNDED;
                velocity.z = 0f;

                flippingTime = 0f;
                flipTimer = 0f;
                //SetActiveCollider(false);

                if (GameManager.Instance.IsReachEnd)
                {
                    GameManager.Instance.Win();
                }
                else
                {
                    var diff = GameManager.Instance.Score - lastScore;
                    lastScore = GameManager.Instance.Score;
                    GameManager.Instance.CheckShowWowText(diff);
                }

            }
            else if (thisCollider.CompareTag("Body") && (dir.z * velocity.z) < 0)
            {
                addForce.y = collideForce * dir.y;
                addForce.z = collideForce * dir.z;
                velocity.z = (dir.z < 0f) ? 0f : velocity.z;

                // Debug.DrawLine(collidePoint, collidePoint + dir * distance, Color.red, 1f);
            }
        }
    }

    float flipTimer = 0f;

    public void OnCollisionTriggerExit(Collider thisCollider, Collision other)
    {
        if (other == null) return;

        // Debug.Log("OnCollisionTriggerExit! " + thisCollider.tag + ", other: " + other.collider.tag + ", other: " + other.gameObject.name + ", CurrentState: " + CurrentState);

        if (thisCollider.CompareTag("Blade") && other.collider.CompareTag("Item"))
        {
            if (CurrentState == State.CUTTING)
            {
                // CurrentState = State.FLIPPING;
                flipTimer = 0.25f;
            }
        }
    }

    public void OnCollisionTriggerStay(Collider thisCollider, Collision other)
    {
        if (other == null) return;

    }

}
