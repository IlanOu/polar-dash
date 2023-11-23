using UnityEngine;

namespace SupanthaPaul
{
	public class PlayerAnimator : MonoBehaviour
	{
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
			// Idle & Running animation
			m_anim.SetFloat(Move, Mathf.Abs(m_rb.velocity.x));

			// Jump state (handles transitions to falling/jumping)
			float verticalVelocity = m_rb.velocity.y;
			m_anim.SetFloat(JumpState, verticalVelocity);

			// Jump animation

			if (!m_controller.isGrounded)
			{
				m_anim.SetBool(IsJumping, true);
				Sounds_Manager.instance.playJumpSound();
			}
			else
			{
				m_anim.SetBool(IsJumping, false);
			}
			
			m_anim.SetBool(IsSliding, m_controller.isSliding);
			if (m_controller.isSliding){
				Sounds_Manager.instance.playSlideSound();
			}


			m_anim.SetBool(IsDying, dyingSystem.isDead);
			if (dyingSystem.isDead){
				Sounds_Manager.instance.playDeadSound();
			}
		}

		public float GetSlideAnimationDuration(string animationName)
		{
			AnimatorStateInfo state = m_anim.GetCurrentAnimatorStateInfo(0);

			if (state.IsName(animationName))
			{
				return state.length;
			}
			else
			{
				return 0f;
			}
		}
	}
}
