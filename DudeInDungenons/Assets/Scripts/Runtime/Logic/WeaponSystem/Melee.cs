namespace Runtime.Logic.WeaponSystem {
    public class Melee : Weapon {
        public override void Shoot(IDamagable target) {
            target.TakeDamage(Damage);
        }
    }
}
