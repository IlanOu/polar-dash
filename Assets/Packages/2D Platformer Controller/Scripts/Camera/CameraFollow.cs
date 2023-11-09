using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SupanthaPaul
{
	public class CameraFollow : MonoBehaviour
	{
	    [SerializeField]
		private Transform target;
		[SerializeField]
		private float smoothSpeed = 0.125f;
		public Vector3 offset;
		[Header("Camera bounds")]
		public Vector3 minCamerabounds;
		public Vector3 maxCamerabounds;


		private void FixedUpdate()
		{
			Vector3 desiredPosition = target.localPosition + offset;
			var localPosition = transform.localPosition;
			Vector3 smoothedPosition = Vector3.Lerp(localPosition, desiredPosition, smoothSpeed);
			localPosition = smoothedPosition;

			// clamp camera's position between min and max
			localPosition = new Vector3(
				Mathf.Clamp(localPosition.x, minCamerabounds.x, maxCamerabounds.x),
				Mathf.Clamp(localPosition.y, minCamerabounds.y, maxCamerabounds.y),
				Mathf.Clamp(localPosition.z, minCamerabounds.z, maxCamerabounds.z)
				);
			transform.localPosition = localPosition;
		}

		public void SetTarget(Transform targetToSet)
		{
			target = targetToSet;
		}
	}
}
