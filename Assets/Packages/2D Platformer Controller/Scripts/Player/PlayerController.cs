using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using System.Collections.Generic;

namespace SupanthaPaul
{
	public class PlayerController : MonoBehaviour
	{
		[Header("Physics")]
		[Tooltip("Physics system")]
		[SerializeField] private float fallMultiplier;
		[SerializeField] private Transform groundCheck;
		[SerializeField] private float groundCheckRadius;
		[SerializeField] private LayerMask whatIsGround;

		[Header("Jumping")]
		[Tooltip("Jump system")]
		[SerializeField] private float jumpForce;
		[SerializeField] private int extraJumpCount = 0;
		[SerializeField] private GameObject jumpEffect;
		private bool isJumping = false;
		
		[Header("Running")]
		[Tooltip("Run system")]
		[SerializeField] private float speed;
		[HideInInspector] public bool canMove = true;
		// Access needed for handling animation in Player script and other uses
		[HideInInspector] public bool isGrounded;
		// controls whether this instance is currently playable or not
		[HideInInspector] public bool isCurrentlyPlayable = false;

		[HideInInspector] public bool isSliding = false;

		private Rigidbody2D m_rb;
		private ParticleSystem m_dustParticle;
		private readonly float m_groundedRememberTime = 0.25f;
		private float m_groundedRemember = 0f;
		private int m_extraJumps;
		private float m_extraJumpForce;

		private PlayerAnimator playerAnimator;

		float originalSizeX;
		float originalSizeY;

		BoxCollider2D boxCollider;

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

			playerAnimator = GetComponent<PlayerAnimator>();

			originalSizeX = boxCollider.size.x;
			originalSizeY = boxCollider.size.y;

			boxCollider = GetComponent<BoxCollider2D>() as BoxCollider2D;
/* 
			playerAnimator = GetComponent<playerAnimator>();
			if (playerAnimator == null)
			{
				Debug.LogError("Le script playerAnimator n'est pas attaché au même GameObject que PlayerController.");
			} 
*/
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
			// ! Socket get 
			Debug.Log(UdpSocket.textRecieved);
			InputSystem.Jump();
			
			isJumping = false;
			switch (UdpSocket.textRecieved)
			{
				case "left:jump":
					isJumping = true;
					break;
				default:
					isJumping = false;
					break;
			}
			//~ Temporary variables for debugging : Jump
			//~ ----------------------------------------
			isJumping = InputSystem.Jump();
			//~ ----------------------------------------

			if (isGrounded)
			{
				m_extraJumps = extraJumpCount;
			}

			// grounded remember offset (for more responsive jump)
			m_groundedRemember -= Time.deltaTime;
			if (isGrounded)
				m_groundedRemember = m_groundedRememberTime;

			if (!isCurrentlyPlayable) 
				return;

			// Jumping
			if(isJumping && m_extraJumps > 0 && !isGrounded)	// extra jumping
			{
				m_rb.velocity = new Vector2(m_rb.velocity.x, m_extraJumpForce); ;
				m_extraJumps--;
				// jumpEffect
				PoolManager.instance.ReuseObject(jumpEffect, groundCheck.position, Quaternion.identity);
			}
			else if(isJumping && (isGrounded || m_groundedRemember > 0f))	// normal single jumping
			{
				m_rb.velocity = new Vector2(m_rb.velocity.x, jumpForce);
				// jumpEffect
				PoolManager.instance.ReuseObject(jumpEffect, groundCheck.position, Quaternion.identity);
			}
			
			
			//* ++ Slide
			isSliding = Input.GetButtonDown("Fire1");
			if (isSliding && isGrounded)
			{
				// Réduire la taille du BoxCollider pendant la glissade
				boxCollider.size = new Vector2(boxCollider.size.x, originalSizeY / 2f);
				
				float slideAnimationDuration = playerAnimator.GetSlideAnimationDuration("Slide"); // Remplacez "YourSlideAnimationName" par le nom de votre animation

				// Lancer la coroutine pour réinitialiser la taille du collider après un certain délai
				StartCoroutine(ResetColliderSize(slideAnimationDuration));
			}
		}

		IEnumerator ResetColliderSize(float delay)
		{
			// Attendre pendant le délai spécifié
			yield return new WaitForSeconds(delay);

			// Remettre la taille du BoxCollider à la normale
			// float originalSizeY = // votre taille originale du BoxCollider en Y ici;
			boxCollider.size = new Vector2(boxCollider.size.x, originalSizeY);

			// Réactiver la possibilité de glisser
			isSliding = false;
		}

		
	}
}
