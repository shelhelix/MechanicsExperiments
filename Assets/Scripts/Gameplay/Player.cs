using GameJamEntry.Gameplay.WeaponsMechanic;
using UnityEngine;

namespace GameJamEntry.Gameplay {
	public class Player : MonoBehaviour {
		BaseGun _gun;

		[SerializeField] float     MovementSpeed = 1f;
		[SerializeField] float     OverlapRadius = 1f;
		[SerializeField] Transform GunHolder;
		
		void Update() {
			TryPickUpWeapon();
			TryOperateGun();
			TryMove();
		}

		void TryMove() {
			var move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
			transform.position += move * MovementSpeed * Time.deltaTime;
		}
		
		void TryOperateGun() {
			if ( !_gun ) {
				return;
			}
			if ( Input.GetKeyDown(KeyCode.Mouse0) ) {
				_gun.StartFire();
			}
			if ( Input.GetKeyUp(KeyCode.Mouse0) ) {
				_gun.EndFire();
			}
		}

		void TryPickUpWeapon() {
			if ( !Input.GetKeyDown(KeyCode.E) ) {
				return;
			}
			var weapons         = Physics2D.OverlapCircleAll(transform.position, OverlapRadius, LayerMask.GetMask("Guns"));
			var weapon = FindWeaponCollision(weapons);
			if ( !weapon ) {
				return;
			}
			var gunComp = weapon.GetComponent<BaseGun>();
			if ( !gunComp ) {
				return;
			}
			var newGunPos = gunComp.transform.position;
			DropOldGun(newGunPos);
			PickUpGun(gunComp);
			
		}

		void PickUpGun(BaseGun gun) {
			_gun = gun;
			_gun.transform.SetParent(GunHolder);
			_gun.transform.position = GunHolder.position;
			_gun.PickUp();
		}

		void DropOldGun(Vector3 pos) {
			// drop old gun if exists
			if ( _gun ) {
				_gun.Drop();
				_gun.transform.SetParent(null);
				_gun.transform.position = pos;
			}
		}

		BaseGun FindWeaponCollision(Collider2D[] collisions) {
			foreach ( var collision in collisions ) {
				var gunComp = collision.GetComponent<BaseGun>();
				if ( gunComp && (gunComp != _gun) ) {
					return gunComp;
				}
			}
			return null;
		}
	}
}