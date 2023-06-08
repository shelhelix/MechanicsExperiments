using GameJamEntry.Gameplay.WeaponsMechanic;
using UnityEngine;

namespace GameJamEntry.Gameplay {
	public class Player : MonoBehaviour {
		[SerializeField] BaseGun   Gun;
		[SerializeField] Transform GunHolder;
		
		void Update() {
			TryPickUpWeapon();
			TryOperateGun();
		}
		
		void TryOperateGun() {
			if ( !Gun ) {
				return;
			}
			if ( Input.GetKeyDown(KeyCode.Mouse0) ) {
				Gun.StartFire();
			}
			if ( Input.GetKeyUp(KeyCode.Mouse0) ) {
				Gun.EndFire();
			}
		}

		void TryPickUpWeapon() {
			if ( !Input.GetKeyDown(KeyCode.E) ) {
				return;
			}
			var weapon = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Default"));
			if ( !weapon ) {
				return;
			}
			var gunComp = weapon.GetComponent<BaseGun>();
			if ( !gunComp ) {
				return;
			}
			var newGunPos = gunComp.transform.position;
			// drop old gun if exists
			if ( Gun ) {
				Gun.Drop();
				Gun.transform.SetParent(null);
				Gun.transform.position = newGunPos;
			}
			
			Gun = gunComp;
			Gun.transform.SetParent(GunHolder);
			Gun.transform.position = GunHolder.position;
			Gun.PickUp();
		}
	}
}