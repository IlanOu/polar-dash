using UnityEngine;

namespace SupanthaPaul
{
	public class PlayerAnimator : MonoBehaviour
	{
		private Rigidbody2D m_rb;
		private PlayerController m_controller;
		private Animator m_anim;
		private static readonly int Move = Animator.StringToHash("Move");
		private static readonly int JumpState = Animator.StringToHash("JumpState");
		private static readonly int IsJumping = Animator.StringToHash("IsJumping");
		private static readonly int WallGrabbing = Animator.StringToHash("WallGrabbing");
		private static readonly int IsDashing = Animator.StringToHash("IsDashing");
		private static readonly int IsSliding = Animator.StringToHash("IsSliding");

		private void Start()
		{
			m_anim = GetComponentInChildren<Animator>();
			m_controller = GetComponent<PlayerController>();
			m_rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			// Idle & Running animation
			m_anim.SetFloat(Move, Mathf.Abs(m_rb.velocity.x));

			// Jump state (handles transitions to falling/jumping)
			float verticalVelocity = m_rb.velocity.y;
			m_anim.SetFloat(JumpState, verticalVelocity);

			// Jump animation

			if (!m_controller.isGrounded)
			{
				m_anim.SetBool(IsJumping, true);
			}
			else
			{
				m_anim.SetBool(IsJumping, false);
			}
			
			// Debug.Log(m_controller.isSliding);
			m_anim.SetBool(IsSliding, m_controller.isSliding);
			// if (m_controller.isSliding){
			// }else{
			// 	m_anim.SetBool(IsSliding, true);
			// }


			// dash animation
			// m_anim.SetBool(IsDashing, m_controller.isDashing);
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
	}
}
