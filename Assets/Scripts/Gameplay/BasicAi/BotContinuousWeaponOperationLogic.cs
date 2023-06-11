using GameJamEntry.Gameplay.WeaponsMechanic;

namespace GameJamEntry.Gameplay.BasicAi {
	public class BotContinuousWeaponOperationLogic : BaseBotWeaponOperationLogic {
		bool _isFireStarted;

		public override void Init(BaseGun gun) {
			base.Init(gun);
			_isFireStarted = false;
		}

		public override void Tick() {
			if ( _isFireStarted ) {
				return;
			}
			Gun.StartFire();
			_isFireStarted = true;
		}
	}
}