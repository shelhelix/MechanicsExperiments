using GameComponentAttributes.Attributes;
using UnityEngine;

namespace GameJamEntry.Gameplay.WeaponsMechanic {
	public class MachineGun : BaseGun {
		[NotNullReference, SerializeField] GameObject _bulletPrefab;
		[NotNullReference, SerializeField] Transform  _bulletSpawnPoint;
		
		[SerializeField] float DelayBetweenShots      = 0.1f;
		[SerializeField] float DelayForMagazineReload = 1f;

		public int AmmoInMagazineTotal = 30;

		public int AmmoInMagazineLeft;
		
		Timer _betweenFiringTimer = new();
		Timer _reloadTimer        = new();

		GunStatus _gunStatus = GunStatus.Idle;

		public override void PickUp() {
			// Do nothing
		}

		public override void Drop() {
			EndFire();
		}

		public override void StartFire() {
			if (_gunStatus != GunStatus.Idle) {
				return;
			}
			Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
			
			AmmoInMagazineLeft--;
			if (AmmoInMagazineLeft <= 0) {
				GoToReloadMagazine();
				return;
			}
			GoToDelayBetweenShots();
		}

		public override void EndFire() {
			_gunStatus = GunStatus.Idle;
			_betweenFiringTimer.Deinit();
			_reloadTimer.Deinit();
		}

		void Update() {
			_betweenFiringTimer.Tick(Time.deltaTime);
			_reloadTimer.Tick(Time.deltaTime);
		}

		#region DelayBetweenShotsBlock
		
		void GoToDelayBetweenShots() {
			_gunStatus = GunStatus.ReloadBetweenShots;
			_betweenFiringTimer.Init(TransitionFromDelayBetweenShotsToIdle, DelayBetweenShots, false);
		}

		void TransitionFromDelayBetweenShotsToIdle() {
			_gunStatus = GunStatus.Idle;
		}

		#endregion

		#region ReloadMagazineBlock
		
		void GoToReloadMagazine() {
			_gunStatus = GunStatus.ReloadMagazine;
			_betweenFiringTimer.Init(TransitionFromDelayBetweenShotsToIdle, DelayForMagazineReload, false);
		}

		void TransitionFromReloadMagazineToIdle() {
			AmmoInMagazineLeft = AmmoInMagazineTotal;
			_gunStatus         = GunStatus.Idle;
		}

		#endregion
	}
}