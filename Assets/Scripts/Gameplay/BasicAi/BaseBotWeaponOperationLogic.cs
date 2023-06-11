using GameJamEntry.Gameplay.WeaponsMechanic;

namespace GameJamEntry.Gameplay.BasicAi {
	public abstract class BaseBotWeaponOperationLogic {
		protected BaseGun Gun;
		
		public virtual void Init(BaseGun gun) {
			Gun = gun;
		}

		public void Deinit() {
			Gun.EndFire();
		}

		public abstract void Tick();
	}
}