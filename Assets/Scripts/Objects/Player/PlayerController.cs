using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    [Header("Animation")]
    [SerializeField] private Animator _anim;

    [Header("GoundCheck")]
    [SerializeField] private Transform _trfGroundCheck;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundCheckMask;

    private PlayerInput _playerInput;

    private InputActionMap _actionMap;

    private InputAction _moveAction;
    private InputAction _jumpAction;

    private Vector2 _inputMoveVec;
    private Rigidbody2D _rigid;

    private bool _isGrounded;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _actionMap = _playerInput.actions.FindActionMap("PlayerActionMap");
        _moveAction = _actionMap.FindAction("MoveAction");
        _jumpAction = _actionMap.FindAction("JumpAction");

        _moveAction.performed += OnMoveAction;
        _moveAction.canceled += OnMoveStopAction;
        _jumpAction.started += OnJumpAction;

        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGround();
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
    }
    private void OnDestroy()
    {
        _moveAction.performed -= OnMoveAction;
        _moveAction.canceled -= OnMoveStopAction;
        _jumpAction.started -= OnJumpAction;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }
    private void OnMoveAction(InputAction.CallbackContext ctx)
    {
        _inputMoveVec = ctx.ReadValue<Vector2>();
        SetMoveAnim(_inputMoveVec.sqrMagnitude);
    }

    private void OnMoveStopAction(InputAction.CallbackContext ctx)
    {
        _inputMoveVec = Vector2.zero;
        SetMoveAnim(0);   
    }

    private void MovePlayer()
    {
        Vector2 nextMove = _inputMoveVec * _moveSpeed;
        _rigid.linearVelocity = new Vector2(nextMove.x, _rigid.linearVelocity.y);
    }

    private void OnJumpAction(InputAction.CallbackContext ctx)
    {
         if (_isGrounded == false)
            return;

        _rigid.linearVelocity = new Vector3(_rigid.linearVelocity.x, _jumpForce);
        _anim.SetTrigger("Jump");
    }

    private void SetMoveAnim(float moveInput)
    {
        _anim?.SetFloat("MoveInput", moveInput);
        SetMoveRotation(_inputMoveVec.x);
    }

    private void SetMoveRotation(float moveX)
    {
        float yRotation = moveX < 0 ? 180f : 0f;
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, yRotation, transform.localRotation.z);
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(_trfGroundCheck.position, Vector2.down, _groundCheckDistance, _groundCheckMask);
        _isGrounded = hit;
        _anim.SetBool("isGrounded", _isGrounded);
    }

    public void BounceUp()
    {
        _rigid.linearVelocity = new Vector3(_rigid.linearVelocity.x, _jumpForce);
        _anim.SetTrigger("Jump");
    }
}
