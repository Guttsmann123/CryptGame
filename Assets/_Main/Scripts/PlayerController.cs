using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 2.0f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float comboTimerDuration = 2f;
    public float scaledHeight = 0f;
    public float scaledRadius = 0f;
    public Transform weaponHolder;
    public GameObject inventoryCanvas;
    public CameraController cameraController;

    private Rigidbody rb;
    private Animator animator;
    private bool isJumping = false;
    private bool isGrounded;
    private bool isAttacking = false;
    private float lastAttackTime;
    private GameObject equippedWeapon;
    private bool isMouseLookEnabled = true;

    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int ComboAttackTrigger = Animator.StringToHash("ComboAttack");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (inventoryCanvas != null)
        {
            inventoryCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("No inventory canvas is assigned in PlayerController.");
        }

        if (cameraController == null)
        {
            Debug.LogError("No CameraController script is assigned in PlayerController.");
        }
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 playerInput = new Vector3(moveHorizontal, 0, moveVertical);
        Vector3 playerDirection = Camera.main.transform.TransformDirection(playerInput).normalized;
        playerDirection.y = 0;

        if (Input.GetKeyUp(KeyCode.W))
        {
            rb.velocity = Vector3.zero;
        }
        else if (isJumping && isGrounded)
        {
            rb.velocity = new Vector3(playerDirection.x * speed, jumpForce, playerDirection.z * speed);
            isJumping = false;
        }
        else
        {
            rb.velocity = new Vector3(playerDirection.x * speed, rb.velocity.y, playerDirection.z * speed);
        }

        animator.SetFloat("Speed", rb.velocity.magnitude);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsGrounded", isGrounded);

        if (isMouseLookEnabled && Input.GetMouseButtonDown(0))
        {
            if (isAttacking && (Time.time - lastAttackTime) <= comboTimerDuration)
            {
                animator.SetTrigger(ComboAttackTrigger);
            }
            else
            {
                animator.SetTrigger(AttackTrigger);
            }

            lastAttackTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 2f);

            foreach (Collider collider in nearbyColliders)
            {
                if (collider.CompareTag("Weapon"))
                {
                    PickUpWeapon(collider.gameObject);
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropWeapon();
        }

        float stepOffset = scaledHeight + (scaledRadius * 2);
        float requiredValue = scaledHeight + (scaledRadius * 2);

        if (stepOffset > requiredValue)
        {
            Debug.LogError("Step Offset must be less than or equal to scaled Height + scaled Radius * 2");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryCanvas != null)
            {
                inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            if (cameraController != null)
            {
                cameraController.enabled = !cameraController.enabled;
                isMouseLookEnabled = cameraController.enabled;
            }
        }
    }

    public void AttackAnimationComplete()
    {
        isAttacking = false;
        animator.SetBool(IsAttacking, isAttacking);
        animator.ResetTrigger(AttackTrigger);
        animator.ResetTrigger(ComboAttackTrigger);
    }

    public void AttackAnimationStart()
    {
        isAttacking = true;
        animator.SetBool(IsAttacking, isAttacking);
    }

    public void PickUpWeapon(GameObject weapon)
    {
        if (equippedWeapon != null)
        {
            DropWeapon();
        }

        weapon.transform.SetParent(null);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localScale = Vector3.one;

        weapon.transform.SetParent(weaponHolder);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        Collider weaponCollider = weapon.GetComponent<Collider>();
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }

        equippedWeapon = weapon;
    }

    public void DropWeapon()
    {
        if (equippedWeapon != null)
        {
            Collider weaponCollider = equippedWeapon.GetComponent<Collider>();
            if (weaponCollider != null)
            {
                weaponCollider.enabled = true;
            }

            equippedWeapon.transform.SetParent(null);

            equippedWeapon = null;
        }
    }
}
