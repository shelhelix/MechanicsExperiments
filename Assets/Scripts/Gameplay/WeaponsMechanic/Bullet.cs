using UnityEngine;

namespace GameJamEntry.Gameplay.WeaponsMechanic {
	public class Bullet : MonoBehaviour {
		[SerializeField] float Speed = 10;
		
		void Update() {
			transform.position += transform.up * Time.deltaTime * Speed;
		}
	}
}