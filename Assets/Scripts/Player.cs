using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventHandler> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventHandler : EventArgs
    {
        public ClearCounter SelectedCounter;
    }
    
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask counterLayerMask;

    private bool _isWalking;
    private Vector3 _lastInteractDir;
    private ClearCounter _selectedCounter;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more then 1 player");
        }
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
            _selectedCounter.Interact();
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
            _lastInteractDir = moveDir;
        
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != _selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
                SetSelectedCounter(null);
        }
        else
            SetSelectedCounter(null);

        //Debug.Log(_selectedCounter);
    }

    void HandleMovement()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = Time.deltaTime * moveSpeed;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //cant move towards moveDir
            
            //attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized; 
            canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius, moveDirX, moveDistance);

            if (canMove) //can move only on the x
                moveDir = moveDirX;
            else //cant move only on the x
            {
                //attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized; 
                canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius, moveDirZ, moveDistance);
                
                if (canMove) //can move only on the Z
                    moveDir = moveDirZ;
                else
                {
                    //cant move in any dir
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
            
        }

        _isWalking = moveDir != Vector3.zero;
        
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    
    public bool IsWalking()
    {
        return _isWalking;
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this,new OnSelectedCounterChangedEventHandler{SelectedCounter = _selectedCounter});
    }
    
}
