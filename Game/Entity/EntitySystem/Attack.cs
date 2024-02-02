// Ignore Spelling: Cooldown

using System;

namespace GameJam14.Game.Entity.EntitySystem;
internal class Attack : IDisposable {
    public Attack(float attackRange, float attackDistance, double attackSpeed, double attackCooldown, int attackDamage) {
        this.AttackRange = attackRange;
        this.AttackDistance = attackDistance;
        this.AttackSpeed = attackSpeed;
        this.AttackCooldown = attackCooldown;
        this.AttackDamage = attackDamage;
        this._disposed = false;
    }

    public Attack() {
        this.AttackRange = 0.0f;
        this.AttackDistance = 0.0f;
        this.AttackSpeed = 0.0;
        this.AttackCooldown = 0.0;
        this.AttackDamage = 0;
    }

    public enum AttackType {
        Shoot,
        Squish,
        Body,
        None
    }

    public double AttackCooldown { get; set; }
    public int AttackDamage { get; set; }
    public float AttackDistance { get; set; }
    public float AttackRange { get; set; }
    public double AttackSpeed { get; set; }
    public double AttackTimer { get; set; }
    public double CooldownTimer { get; set; }
    public bool IsAttacking { get; set; }
    public AttackType Type { get; set; }
    public bool CanAttack() {
        return this.CooldownTimer >= this.AttackCooldown && !this.IsAttacking;
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void FinishAttack() {
        if ( this.IsAttacking ) {
            this.ResetCooldownTimer();
            this.AttackTimer = this.AttackSpeed;
            this.IsAttacking = false;
        }
    }

    public void ResetAttackTimer() {
        this.AttackTimer = 0.0;
    }

    public void ResetCooldownTimer() {
        this.CooldownTimer = 0.0;
    }

    public void StartAttack() {
        if ( this.CanAttack() ) {
            this.ResetAttackTimer();
            this.IsAttacking = true;
        }
    }

    public void Update(double deltaTime) {
        if ( this.IsAttacking ) {
            if ( this.AttackTimer >= this.AttackSpeed ) {
                this.FinishAttack();
            } else {
                this.UpdateAttackTimer(deltaTime);
            }
        }
        this.UpdateCooldownTimer(deltaTime);
    }

    public void UpdateAttackTimer(double deltaTime) {
        if ( this.AttackTimer < this.AttackSpeed ) {
            if ( this.AttackTimer + deltaTime > this.AttackSpeed ) {
                this.AttackTimer = this.AttackSpeed;
            } else {
                this.AttackTimer += deltaTime;
            }
        }
    }

    public void UpdateCooldownTimer(double deltaTime) {
        if ( this.CooldownTimer < this.AttackCooldown ) {
            if ( this.CooldownTimer + deltaTime > this.AttackCooldown ) {
                this.CooldownTimer = this.AttackCooldown;
            } else {
                this.CooldownTimer += deltaTime;
            }
        }
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this._disposed = true;
    }
    private bool _disposed;
}
