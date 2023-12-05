using System.Collections;
using UnityEngine;

namespace SupanthaPaul
{
	public class PlayerAnimator : MonoBehaviour
	{
		public SpriteRenderer spriteRenderer;
		private Rigidbody2D m_rb;
		private PlayerController m_controller;
		private Animator m_anim;
		private DyingSystem dyingSystem;
		private static readonly int Move = Animator.StringToHash("Move");
		private static readonly int JumpState = Animator.StringToHash("JumpState");
		private static readonly int IsJumping = Animator.StringToHash("IsJumping");
		private static readonly int WallGrabbing = Animator.StringToHash("WallGrabbing");
		private static readonly int IsSliding = Animator.StringToHash("IsSliding");
		private static readonly int IsDying = Animator.StringToHash("IsDying");


		private void Start()
		{
			m_anim = GetComponentInChildren<Animator>();
			m_controller = GetComponent<PlayerController>();
			dyingSystem = GetComponent<DyingSystem>();
			m_rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			if (PlayerHealth.instance.isTakingDamage && PlayerHealth.instance.currentHealth > 0)
			{
				StartCoroutine(DamageAnimation());
			}
			// Idle & Running animation
			m_anim.SetFloat(Move, Mathf.Abs(m_rb.velocity.x));

			// Jump state (handles transitions to falling/jumping)
			float verticalVelocity = m_rb.velocity.y;
			m_anim.SetFloat(JumpState, verticalVelocity);

			// Jump animation

			if (!m_controller.isGrounded)
			{
				m_anim.SetBool(IsJumping, true);
				SoundsManager.instance.playJumpSound();
				SoundsManager.instance.playJumpSound();
			}
			else
			{
				m_anim.SetBool(IsJumping, false);
			}
			
			m_anim.SetBool(IsSliding, m_controller.isSliding);
			if (m_controller.isSliding){
				SoundsManager.instance.playSlideSound();
			}


			m_anim.SetBool(IsDying, DyingSystem.instance.isDead);
			if (DyingSystem.instance.isDead){
				SoundsManager.instance.playDeadSound();
			}
		}

		public float GetSlideAnimationDuration(string animationName)
		{
			// Récupérer l'état de l'animation par son nom
			AnimatorStateInfo state = m_anim.GetCurrentAnimatorStateInfo(0);

			// Vérifier si l'animation spécifiée est en cours
			if (state.IsName(animationName))
			{
				// Retourner la durée totale de l'animation
				return state.length;
			}
			else
			{
				// Retourner une valeur par défaut si l'animation n'est pas en cours
				return 0f;
			}
		}

		private IEnumerator DamageAnimation()
		{
			int nbTimeAnimation = 5;
			float time = 0.1f;
			for (int i = 0 ; i < nbTimeAnimation ; i++)
			{
				spriteRenderer.enabled = false;
				yield return new WaitForSeconds(time);
				spriteRenderer.enabled = true;
				yield return new WaitForSeconds(time);
			}
		}
	}
}
