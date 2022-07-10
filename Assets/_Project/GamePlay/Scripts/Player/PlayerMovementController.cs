using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    protected Vector3 m_Velocity;

    protected Vector3 m_TargetVelocity;
    protected Vector3 m_TargetJumpVelocity;

    [SerializeField] protected float m_MaxSpeed;

    [SerializeField] protected float m_AccelerationSpeed;

    [SerializeField] protected float m_DeccelerationSpeed;

    [SerializeField] protected float m_JumpSpeed;

    [SerializeField] protected float m_JumpSafetyTime;
    [SerializeField] protected float m_DropDownHoldPulseTime;
    [SerializeField] protected float m_JumpHoldPulseTime;

    [SerializeField] protected LayerMask m_CollisionLayers;

    [SerializeField] protected LayerMask m_OneWayDown;
    [SerializeField] protected LayerMask m_OneWayUp;
    [SerializeField] protected LayerMask m_OneWayLeft;
    [SerializeField] protected LayerMask m_OneWayRight;

    private float m_LastHorizontalMovement;

    [SerializeField] protected GameObject m_CollisionBox;

    protected float m_DropDownHorizontalAxisThreshold = 0.5f;

    protected bool m_DropDown;
    protected float m_DropDownTime = 0;
    protected bool m_HoldingDropDown;
    protected float m_DropDownHoldTime = 0;
    protected bool m_HoldingJump;
    protected float m_JumpHoldTime = 0;

    protected bool m_IsGrounded;
    protected float m_LastGroundedTime = 0;

    private bool _isInteractable = false;

    public void SetInteractable(bool isInteractable)
    {
        _isInteractable = isInteractable;
    }

    public Vector3 Velocity
	{
		get
		{
            return m_Velocity;
        }
	}

	public float MaxSpeed
	{
		get
		{
            return m_MaxSpeed;
        }
        set
        {
            m_MaxSpeed = value;
        }
	}

	public bool IsGrounded
	{
		get
		{
            return m_IsGrounded;
        }
	}

    void Update()
    {
        if(PauseController.IsPaused) return;

        float horizontalMovement = InputController.Instance.GetAxis(InputController.eAxis.Horizontal).Value;

        if (!_isInteractable)
        {
            horizontalMovement = 0;
        }

        if (m_LastHorizontalMovement < 0 && horizontalMovement > 0)
        {
            m_TargetVelocity.x = 0;
        }
        else if (m_LastHorizontalMovement > 0 && horizontalMovement < 0)
        {
            m_TargetVelocity.x = 0;
        }

        if (horizontalMovement < 0)
        {
            m_TargetVelocity.x -= m_AccelerationSpeed * Time.deltaTime;
            PlayerHealthController.Instance.SetHealthDrainPaused(false);
        }
        else if (horizontalMovement > 0)
        {
            m_TargetVelocity.x += m_AccelerationSpeed * Time.deltaTime;
            PlayerHealthController.Instance.SetHealthDrainPaused(false);
        }
        else
        {
            m_TargetVelocity.x = Mathf.Lerp(m_TargetVelocity.x, 0, m_DeccelerationSpeed * Time.deltaTime);
        }

        m_TargetVelocity.x = Mathf.Clamp(m_TargetVelocity.x, -m_MaxSpeed, m_MaxSpeed);

        if (_isInteractable && InputController.Instance.GetButtonDown(InputController.eButtons.Jump))
        {
            if (m_IsGrounded || m_LastGroundedTime < m_JumpSafetyTime)
            {
                m_TargetJumpVelocity.y = m_JumpSpeed;
            }
        }

        if (_isInteractable && InputController.Instance.GetButton(InputController.eButtons.Jump))
        {
            m_HoldingJump = true;
        }
        else
        {
            m_HoldingJump = false;
            m_JumpHoldTime = 0;
        }

        if (m_HoldingJump)
        {
            if (m_JumpHoldTime >= m_JumpHoldPulseTime)
            {
                if ((m_IsGrounded || m_LastGroundedTime < m_JumpSafetyTime) && m_Velocity.y <= 0)
                {
                    m_TargetJumpVelocity.y = m_JumpSpeed;
                }

                m_JumpHoldTime = 0;
            }
            else
            {
                m_JumpHoldTime += Time.deltaTime;
            }
        }

        m_LastHorizontalMovement = horizontalMovement;

        if (m_DropDownTime > 0)
        {
            m_DropDownTime -= Time.deltaTime;

            if (m_DropDownTime <= 0)
            {
                m_DropDown = false;
            }
        }

        if (m_IsGrounded && !m_DropDown && _isInteractable && IsDroppingDown())
        {
            m_DropDown = true;
            m_DropDownTime = 0.2f;
        }

        if (_isInteractable && (InputController.Instance.GetButton(InputController.eButtons.DropDown) || IsDroppingDown()))
        {
            m_HoldingDropDown = true;
        }
        else
        {
            m_HoldingDropDown = false;
            m_DropDownHoldTime = 0;
        }

        if (m_HoldingDropDown)
        {
            if (m_DropDownHoldTime >= m_DropDownHoldPulseTime)
            {
                if (m_IsGrounded && !m_DropDown)
                {
                    m_DropDown = true;
                    m_DropDownTime = 0.2f;
                }

                m_DropDownHoldTime = 0;
            }
            else
            {
                m_DropDownHoldTime += Time.deltaTime;
            }
        }

        if (m_IsGrounded && m_TargetJumpVelocity.y <= 0)
        {
            m_LastGroundedTime = 0;
        }
        else
        {
            m_LastGroundedTime += Time.deltaTime;
        }
    }

    private bool IsDroppingDown()
    {
        bool dropDownPressed = InputController.Instance.GetButtonDown(InputController.eButtons.DropDown);

        bool dropDownHeld = InputController.Instance.GetButton(InputController.eButtons.DropDown);

        float verticalAxis = InputController.Instance.GetAxis(InputController.eAxis.Vertical).Value;
        bool isVerticalAxisNegative = verticalAxis < 0;

        float horizontalAxis = InputController.Instance.GetAxis(InputController.eAxis.Horizontal).Value;

        verticalAxis = Mathf.Abs(verticalAxis);
        horizontalAxis = Mathf.Abs(horizontalAxis);

        bool isDropDownController = false;

        if (verticalAxis > horizontalAxis + m_DropDownHorizontalAxisThreshold)
        {
            isDropDownController = true;
        }

        if (!dropDownPressed && !dropDownHeld && isVerticalAxisNegative)
        {
            return isDropDownController;
        }

        return dropDownPressed;
    }

    void FixedUpdate()
    {
        if (PauseController.IsPaused) return;
        
        m_IsGrounded = false;

        RaycastHit hit;

        float safetyDistance = 0.2f;
        float rampTestDistance = 0.2f;

        Quaternion floorRotation = Quaternion.identity;
        Vector3 colliderExtents = GetComponent<BoxCollider>().bounds.extents;
        Vector3 detectorExtents = colliderExtents * 0.99f;
        Vector3 originalTargetVelocity = m_TargetVelocity;

        LayerMask downMask = m_CollisionLayers | m_OneWayDown;

        if (m_DropDown)
        {
            downMask = m_CollisionLayers;
        }

        LayerMask upMask = m_CollisionLayers | m_OneWayUp;
        LayerMask leftMask = m_CollisionLayers | m_OneWayLeft;
        LayerMask rightMask = m_CollisionLayers | m_OneWayRight;

        bool onRamp = false;

        if (Physics.BoxCast(transform.position + (safetyDistance * Vector3.up), detectorExtents, Vector3.down, out hit, Quaternion.identity, rampTestDistance + safetyDistance, downMask))
        {
            if (Vector3.Angle(Vector3.up, hit.normal) <= 45)
            {
                if (hit.normal != Vector3.up)
                {
                    onRamp = true;
                }
            }

            floorRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            m_IsGrounded = true;
        }

        m_TargetVelocity.y = 0;
        m_TargetVelocity.z = 0;

        if (onRamp)
        {
            m_TargetVelocity = floorRotation * m_TargetVelocity;

            leftMask = leftMask | m_OneWayDown;
            rightMask = rightMask | m_OneWayDown;
        }

        m_TargetJumpVelocity.y += Physics.gravity.y * Time.fixedDeltaTime;
        m_TargetJumpVelocity.y = Mathf.Clamp(m_TargetJumpVelocity.y, -m_JumpSpeed, m_JumpSpeed);

        m_Velocity = Vector3.Lerp(m_Velocity, m_TargetVelocity, Time.fixedDeltaTime * 20);
        m_Velocity.y = Mathf.Lerp(m_Velocity.y, m_TargetVelocity.y, Time.fixedDeltaTime * 20) + m_TargetJumpVelocity.y;

        Vector3 targetPosition = transform.position + m_Velocity;

        m_CollisionBox.transform.position = targetPosition;
        m_CollisionBox.transform.rotation = floorRotation;

        Vector3 boxCastDistance = targetPosition - transform.position;
        boxCastDistance.x = Mathf.Abs(boxCastDistance.x);
        boxCastDistance.y = Mathf.Abs(boxCastDistance.y);

        if (onRamp)
        {
            if (m_Velocity.y < 0)
            {
                if (Physics.BoxCast(transform.position + (Vector3.up * safetyDistance), detectorExtents,
                    Vector3.down, out hit, Quaternion.identity, safetyDistance + boxCastDistance.y, downMask))
                {
                    m_TargetJumpVelocity.y = 0;
                    m_Velocity.y = 0;

                    Vector3 newTargetPosition = targetPosition;
                    newTargetPosition = hit.point + colliderExtents;

                    targetPosition.y = newTargetPosition.y;
                }
            }
            else if (m_Velocity.y > 0)
            {
                if (Physics.BoxCast(transform.position + (Vector3.down * safetyDistance), detectorExtents,
                    Vector3.up, out hit, Quaternion.identity, safetyDistance + boxCastDistance.y, upMask))
                {
                    m_TargetJumpVelocity.y = 0;
                    m_Velocity.y = 0;

                    Vector3 newTargetPosition = targetPosition;
                    newTargetPosition = hit.point - colliderExtents;

                    targetPosition.y = newTargetPosition.y;
                }
            }

            if (m_Velocity.x > 0)
            {
                if (Physics.BoxCast(transform.position + (Vector3.left * safetyDistance), detectorExtents,
                    Vector3.right, out hit, Quaternion.identity, safetyDistance + boxCastDistance.x, rightMask))
                {
                    RaycastHit rampHit;
                    if (m_TargetJumpVelocity.y <= 0 && Physics.BoxCast(targetPosition + (Vector3.up * safetyDistance), detectorExtents,
                        Vector3.down, out rampHit, Quaternion.identity, safetyDistance + boxCastDistance.y, downMask))
                    {
                        Vector3 newTargetPosition = rampHit.point + colliderExtents;
                        targetPosition.y = newTargetPosition.y;
                    }
                }
            }
            else if (m_Velocity.x < 0)
            {
                if (Physics.BoxCast(transform.position + (Vector3.right * safetyDistance), detectorExtents,
                    Vector3.left, out hit, Quaternion.identity, safetyDistance + boxCastDistance.x, leftMask))
                {
                    RaycastHit rampHit;
                    if (m_TargetJumpVelocity.y <= 0 && Physics.BoxCast(targetPosition + (Vector3.up * safetyDistance), detectorExtents,
                        Vector3.down, out rampHit, Quaternion.identity, safetyDistance + boxCastDistance.y, downMask))
                    {
                        Vector3 newTargetPosition = rampHit.point + colliderExtents;
                        targetPosition.y = newTargetPosition.y;
                    }
                }
            }
        }
        else
        {
            if (m_Velocity.y < 0)
            {
                if (Physics.BoxCast(transform.position + (Vector3.up * safetyDistance), detectorExtents,
                    Vector3.down, out hit, Quaternion.identity, safetyDistance + boxCastDistance.y, downMask))
                {
                    m_TargetJumpVelocity.y = 0;
                    m_Velocity.y = 0;

                    Vector3 newTargetPosition = targetPosition;
                    newTargetPosition = hit.point + colliderExtents;

                    targetPosition.y = newTargetPosition.y;
                }
            }
            else if (m_Velocity.y > 0)
            {
                if (Physics.BoxCast(transform.position + (Vector3.down * safetyDistance), detectorExtents,
                    Vector3.up, out hit, Quaternion.identity, safetyDistance + boxCastDistance.y, upMask))
                {
                    m_TargetJumpVelocity.y = 0;
                    m_Velocity.y = 0;

                    Vector3 newTargetPosition = targetPosition;
                    newTargetPosition = hit.point - colliderExtents;

                    targetPosition.y = newTargetPosition.y;
                }
            }

            if (m_Velocity.x > 0)
            {
                if (Physics.BoxCast(transform.position + (Vector3.left * safetyDistance), detectorExtents,
                    Vector3.right, out hit, Quaternion.identity, safetyDistance + boxCastDistance.x, rightMask))
                {
                    m_Velocity.x = 0;

                    Vector3 newTargetPosition = targetPosition;
                    newTargetPosition = hit.point - colliderExtents;

                    targetPosition.x = newTargetPosition.x;
                }
            }
            else if (m_Velocity.x < 0)
            {
                if (Physics.BoxCast(transform.position + (Vector3.right * safetyDistance), detectorExtents,
                    Vector3.left, out hit, Quaternion.identity, safetyDistance + boxCastDistance.x, leftMask))
                {
                    m_Velocity.x = 0;

                    Vector3 newTargetPosition = targetPosition;
                    newTargetPosition = hit.point + colliderExtents;

                    targetPosition.x = newTargetPosition.x;
                }
            }
        }

        Collider[] colliders = Physics.OverlapBox(targetPosition, colliderExtents, Quaternion.identity, m_CollisionLayers);

        List<Vector3> directions = new List<Vector3>();

        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 direction;
            float distance;

            if (Physics.ComputePenetration(GetComponent<BoxCollider>(), targetPosition, Quaternion.identity, colliders[i], colliders[i].transform.position, colliders[i].transform.rotation, out direction, out distance))
            {
                if (direction != null)
                {
                    directions.Add(direction * distance);
                }
            }
        }

        for (int i = 0; i < directions.Count; i++)
        {
            targetPosition += directions[i];
        }

        transform.position = targetPosition;
        m_TargetVelocity = originalTargetVelocity;
    }
}
