using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5;
    [SerializeField] private float JumpForce = 5;
    [SerializeField] private float AccelerationTime = 0.5f;
    [SerializeField] private float DecelerationTime = 0.5f;
    [SerializeField] private float JumpCooldownDuration = 0.5f;
    [SerializeField] private Rigidbody Body;
    [SerializeField] private Transform FeetTransform;
    [SerializeField] private Animator Animator;
    [SerializeField] private ParticleSystem ThrusterParticles;

    private Vector2 inputVector;
    private Vector3 dampSpeed;
    public bool isJumpingButtonDown;
    public bool isJumping;
    public bool isGrounded;
    public bool isJumpActivated;
    public Vector3 velocity;
    public Vector3 targetVelocity;

    private void Start()
    {
        
    }

    private void Update()
    {
        GetInputs();
        targetVelocity.x = inputVector.x * MoveSpeed;
        targetVelocity.z = inputVector.y * MoveSpeed;
    }

    private void GetInputs()
    {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputVector.Normalize();
        isJumpingButtonDown = Input.GetButton("Jump");

        isGrounded = Physics.Raycast(FeetTransform.position, Vector3.down, 0.1f);
    }

    private void FixedUpdate()
    {
        float verticalVelocity = velocity.y;
        velocity = CalculateVelocity();
        velocity.y = verticalVelocity;
        Vector3 movement = velocity * Time.deltaTime;

        Body.MovePosition(transform.position + movement);
        if (isJumpingButtonDown && isGrounded && !isJumpActivated)
        {
            isJumping = true;
            Animator.SetBool("IsThrustersActivated", true);
            StartCoroutine(JumpTimerCoroutine());
        }
        if (isJumping && !isJumpingButtonDown)
        {
            Animator.SetBool("IsThrustersActivated", false);
            isJumpActivated = false;
        }
        if (isJumpActivated)
        {
            Vector3 velocityBeforeJump = Body.velocity;
            velocityBeforeJump.y = JumpForce;
            Body.velocity = velocityBeforeJump;
        }
    }

    private Vector3 CalculateVelocity()
    {
        if (velocity.sqrMagnitude >= targetVelocity.sqrMagnitude)
        {
            return CustomVelocityInterpolation(velocity, targetVelocity, DecelerationTime);
        }
        return CustomVelocityInterpolation(velocity, targetVelocity, AccelerationTime);
    }

    private Vector3 CustomVelocityInterpolation(Vector3 current, Vector3 target, float smoothTime)
    {
        Vector3 interpolatedValue = Vector3.SmoothDamp(current, target, ref dampSpeed, smoothTime);

        if (interpolatedValue.sqrMagnitude < 0.001f)
        {
            return Vector3.zero;
        }
        return interpolatedValue;
    }

    private IEnumerator JumpTimerCoroutine()
    {
        isJumpActivated = true;
        yield return new WaitForSeconds(JumpCooldownDuration);
        isJumpActivated = false;
        Animator.SetBool("IsThrustersActivated", false);
    }

    public void StopThrusters()
    {
        ThrusterParticles.Stop();
    }    
    
    public void StartThrusters()
    {
        ThrusterParticles.Play();
    }
}
