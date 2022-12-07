using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform camPivot;
    [SerializeField] private CinemachineVirtualCamera vcam;

    [Header("Game Stats (Resets on Play)")]
    [SerializeField] private Vector2 lookDirection;
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private float currentMaxMoveSpeed;
    [SerializeField] private bool sprintHeld;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float cameraPivotRotation;

    [Header("Player Stats")]
    public float maxMoveSpeed;
    public float sprintModifier;
    public float moveAcceleration;
    public float jumpForce;
    public float cameraSensitivity;
    public float interactRange;
    public LayerMask interactableLayers;
    public LayerMask fireLayers;

    [Header("Gun Setup")]
    [SerializeField] private Animator gunAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentMaxMoveSpeed = maxMoveSpeed;
        StartCoroutine(FootstepLoop());
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMove();
        ApplyLook();
    }

    #region Inputs

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            sprintHeld = true;
            UpdateCurrentMaxMoveSpeed();
        }

        if (context.canceled)
        {
            sprintHeld = false;
            UpdateCurrentMaxMoveSpeed();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        ApplyJump();
    }

    public void Look(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TryFireWeapon();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        TryInteract();
    }
    #endregion

    #region Input Application

    private void ApplyMove()
    {
        Vector3 movement = (transform.right * moveDirection.x) + (transform.forward * moveDirection.y);
        _rigidbody.AddForce(movement * moveAcceleration * (currentMaxMoveSpeed/maxMoveSpeed) * (Time.deltaTime * 60), ForceMode.Acceleration);
    }

    private void UpdateCurrentMaxMoveSpeed()
    {
        if (sprintHeld)
        {
            currentMaxMoveSpeed = maxMoveSpeed + sprintModifier;
        }
        else
        {
            currentMaxMoveSpeed = maxMoveSpeed;
        }
    }

    public void UpdateGroundedState(bool state)
    {
        isGrounded = state;
    }

    private void ApplyLook()
    {
        float lookX = lookDirection.x * cameraSensitivity;
        float lookY = lookDirection.y * cameraSensitivity;

        cameraPivotRotation -= lookY;
        cameraPivotRotation = Mathf.Clamp(cameraPivotRotation, -90f, 90f);
        transform.Rotate(Vector3.up, lookX);
        camPivot.localRotation = Quaternion.Euler(cameraPivotRotation, 0, 0);
    }

    private void ApplyJump()
    {
        if (!isGrounded)
        {
            return;
        }
        _rigidbody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
    }

    private void TryInteract()
    {
        RaycastHit hit;
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * interactRange, color:Color.blue);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRange, interactableLayers))
        {
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Interactable")
            {
                print("hit");
                hit.transform.GetComponentInChildren<Interactable>().InteractAction();
            }
        }
    }

    private void TryFireWeapon()
    {
        if(GameManager.Instance.GetAmmo() <= 0 || gunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fire"))
        {
            return;
        }
        
        gunAnimator.SetTrigger("Fire");
        
    }
    #endregion

    private void LimitMovement()
    {
        _rigidbody.velocity = Vector3.ClampMagnitude(new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z), currentMaxMoveSpeed) + new Vector3(0, _rigidbody.velocity.y, 0); 
    }

    public void PerformFireRaycast()
    {
        RaycastHit hit;
        AudioManager.Instance.PlayShot();
        GameManager.Instance.SetJustShot(true);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 200f, fireLayers))
        {
            // Hit Monster
            
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Monster")
            {
                print("hit");
                hit.transform.GetComponentInChildren<Monster>().damage();
            }
        }
    }

    private IEnumerator FootstepLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(sprintHeld ? 0.5f : 1.0f);

            if (moveDirection != Vector2.zero)
            {
                AudioManager.Instance.PlayFootstep();
            }
        }
    }
}
