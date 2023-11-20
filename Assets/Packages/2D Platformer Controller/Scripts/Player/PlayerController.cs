using System.Net.Sockets;
using UnityEngine;

namespace SupanthaPaul
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private float speed;
		[Header("Jumping")]
		[SerializeField] private float jumpForce;
		[SerializeField] private float fallMultiplier;
		[SerializeField] private Transform groundCheck;
		[SerializeField] private float groundCheckRadius;
		[SerializeField] private LayerMask whatIsGround;
		[SerializeField] private int extraJumpCount = 1;
		[SerializeField] private GameObject jumpEffect;
		[Header("Dashing")]
		[SerializeField] private float dashSpeed = 30f;
		[Tooltip("Amount of time (in seconds) the player will be in the dashing speed")]
		[SerializeField] private float startDashTime = 0.1f;
		[Tooltip("Time (in seconds) between dashes")]
		[SerializeField] private float dashCooldown = 0.2f;
		// [SerializeField] private GameObject dashEffect;

		// Access needed for handling animation in Player script and other uses
		[HideInInspector] public bool isGrounded;
		[HideInInspector] public float moveInput;
		[HideInInspector] public bool canMove = true;
		// [HideInInspector] public bool isDashing = false;
		[HideInInspector] public bool actuallyWallGrabbing = false;
		// controls whether this instance is currently playable or not
		[HideInInspector] public bool isCurrentlyPlayable = false;

		[Header("Wall grab & jump")]
		[Tooltip("Right offset of the wall detection sphere")]
		public Vector2 grabRightOffset = new Vector2(0.16f, 0f);
		public Vector2 grabLeftOffset = new Vector2(-0.16f, 0f);
		public float grabCheckRadius = 0.24f;
		public float slideSpeed = 2.5f;
		public Vector2 wallJumpForce = new Vector2(10.5f, 18f);
		public Vector2 wallClimbForce = new Vector2(4f, 14f);

		private Rigidbody2D m_rb;
		private ParticleSystem m_dustParticle;
		private bool m_facingRight = true;
		private readonly float m_groundedRememberTime = 0.25f;
		private float m_groundedRemember = 0f;
		private int m_extraJumps;
		private float m_extraJumpForce;


		void Start()
		{
			// create pools for particles
			// PoolManager.instance.CreatePool(dashEffect, 2);
			PoolManager.instance.CreatePool(jumpEffect, 2);

			// if it's the player, make this instance currently playable
			if (transform.CompareTag("Player"))
				isCurrentlyPlayable = true;

			m_extraJumps = extraJumpCount;
			m_extraJumpForce = jumpForce * 0.7f;

			m_rb = GetComponent<Rigidbody2D>();
			m_dustParticle = GetComponentInChildren<ParticleSystem>();

		}

		private void FixedUpdate()
		{
			// check if grounded
			isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
			var position = transform.position;
			
			// if this instance is currently playable
			if (isCurrentlyPlayable)
			{
				//* ++ Avancer en continue
				m_rb.velocity = new Vector2(speed, m_rb.velocity.y);
				
				// better jump physics
				if (m_rb.velocity.y < 0f)
				{
					m_rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
				}

			}
		}

		private void Update()
		{
			// horizontal input
			moveInput = InputSystem.HorizontalRaw();

			if (isGrounded)
			{
				m_extraJumps = extraJumpCount;
			}

			// grounded remember offset (for more responsive jump)
			m_groundedRemember -= Time.deltaTime;
			if (isGrounded)
				m_groundedRemember = m_groundedRememberTime;

			if (!isCurrentlyPlayable) return;

			// Jumping
			if(InputSystem.Jump() && m_extraJumps > 0 && !isGrounded)	// extra jumping
			{
				m_rb.velocity = new Vector2(m_rb.velocity.x, m_extraJumpForce); ;
				m_extraJumps--;
				// jumpEffect
				PoolManager.instance.ReuseObject(jumpEffect, groundCheck.position, Quaternion.identity);
				
				// ! Socket get 
				Debug.Log(UdpSocket.textRecieved);
			}
			else if(InputSystem.Jump() && (isGrounded || m_groundedRemember > 0f))	// normal single jumping
			{
				m_rb.velocity = new Vector2(m_rb.velocity.x, jumpForce);
				// jumpEffect
				PoolManager.instance.ReuseObject(jumpEffect, groundCheck.position, Quaternion.identity);
			}

		}
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
			Gizmos.DrawWireSphere((Vector2)transform.position + grabRightOffset, grabCheckRadius);
			Gizmos.DrawWireSphere((Vector2)transform.position + grabLeftOffset, grabCheckRadius);
		}
	}
}
