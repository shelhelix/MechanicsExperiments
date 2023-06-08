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

		bool _isPressedFiring;

		GunStatus _gunStatus {
			get => _state;
			set {
				Debug.Log($"Change state from {_state} to {value}");
				_state = value;
			}
		}

		GunStatus _state;

		void Start() {
			AmmoInMagazineLeft = AmmoInMagazineTotal;
		}

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
			if (AmmoInMagazineLeft <= 0) {
				GoToReloadMagazine();
				return;
			}
			_isPressedFiring = true;
			FireBullet();
		}

		public override void EndFire() {
			_gunStatus       = GunStatus.Idle;
			_isPressedFiring = false;
			_betweenFiringTimer.Deinit();
			_reloadTimer.Deinit();
		}

		void FireBullet() {
			if (AmmoInMagazineLeft <= 0) {
				GoToReloadMagazine();
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

		void Update() {
			_betweenFiringTimer.Tick(Time.deltaTime);
			_reloadTimer.Tick(Time.deltaTime);
			if ( _isPressedFiring && (_gunStatus == GunStatus.Idle) ) {
				FireBullet();
			}
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
			_reloadTimer.Init(TransitionFromReloadMagazineToIdle, DelayForMagazineReload, false);
		}

		void TransitionFromReloadMagazineToIdle() {
			AmmoInMagazineLeft = AmmoInMagazineTotal;
			_gunStatus         = GunStatus.Idle;
			StartFire();
		}

		#endregion
	}
}