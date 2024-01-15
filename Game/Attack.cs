// Ignore Spelling: Cooldown

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game;
internal class Attack {
    public float AttackRange { get; set; }
    public float AttackDistance { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackCooldown { get; set; }
    public int AttackDamage { get; set; }
    public bool IsAttacking { get; set; }

    public float CooldownTimer { get; set; }
    public float AttackTimer { get; set; }

    public Attack(float attackRange, float attackDistance, float attackSpeed, float attackCooldown, int attackDamage) {
        this.AttackRange = attackRange;
        this.AttackDistance = attackDistance;
        this.AttackSpeed = attackSpeed;
        this.AttackCooldown = attackCooldown;
        this.AttackDamage = attackDamage;
    }

    public void Update(float deltaTime) {
        if (this.IsAttacking) {
            if (this.AttackTimer >= this.AttackSpeed) {
                this.FinishAttack();
            } else {
                this.UpdateAttackTimer(deltaTime);
            }
        }
        this.UpdateCooldownTimer(deltaTime);
    }

    public bool CanAttack() {
        return (this.CooldownTimer >= this.AttackCooldown) && (!this.IsAttacking);
    }

    public void ResetCooldownTimer() {
        this.CooldownTimer = 0;
    }
    public void ResetAttackTimer() {
        this.AttackTimer = 0;
    }

    public void UpdateCooldownTimer(float deltaTime) {
        if (this.CooldownTimer < this.AttackCooldown) {
            if (this.CooldownTimer + deltaTime > this.AttackCooldown) {
                this.CooldownTimer = this.AttackCooldown;
            } else {
                this.CooldownTimer += deltaTime;
            }
        }
    }

    public void UpdateAttackTimer(float deltaTime) {
        if ( this.AttackTimer < this.AttackSpeed ) {
            if ( this.AttackTimer + deltaTime > this.AttackSpeed ) {
                this.AttackTimer = this.AttackSpeed;
            } else {
                this.AttackTimer += deltaTime;
            }
        }
    }

    public void StartAttack() {
        if (this.CanAttack()) {
            this.ResetAttackTimer();
            this.IsAttacking = true;
        }
    }

    public void FinishAttack() {
        if (this.IsAttacking) {
            this.ResetCooldownTimer();
            this.AttackTimer = this.AttackSpeed;
            this.IsAttacking = false;
        }
    }
}
