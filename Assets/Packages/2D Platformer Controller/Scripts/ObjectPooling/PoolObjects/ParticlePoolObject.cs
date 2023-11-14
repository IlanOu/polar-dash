using UnityEngine;

namespace SupanthaPaul
{
	public class ParticlePoolObject : PoolObject
	{
		ParticleSystem _particle;

		public override void OnObjectReuse()
		{
			if(!_particle)
				_particle = GetComponent<ParticleSystem>();
			_particle.Play();
		}
	}
}
