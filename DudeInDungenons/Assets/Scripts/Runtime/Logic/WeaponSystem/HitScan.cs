namespace Runtime.Logic.WeaponSystem {
    public class HitScan : Weapon {
        public override void Shoot(IDamagable target) {
            target.TakeDamage(Damage);
        }
    }
}
