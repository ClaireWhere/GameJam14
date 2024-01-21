// Ignore Spelling: Cooldown

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity;
internal class Attack
{
    public enum AttackType
    {
        Shoot,
        Squish,
        Body,
        None
    }
    public float AttackRange { get; set; }
    public float AttackDistance { get; set; }
    public double AttackSpeed { get; set; }
    public double AttackCooldown { get; set; }
    public int AttackDamage { get; set; }
    public bool IsAttacking { get; set; }

    public double CooldownTimer { get; set; }
    public double AttackTimer { get; set; }
    public AttackType Type { get; set; }

    public Attack(float attackRange, float attackDistance, double attackSpeed, double attackCooldown, int attackDamage)
    {
        AttackRange = attackRange;
        AttackDistance = attackDistance;
        AttackSpeed = attackSpeed;
        AttackCooldown = attackCooldown;
        AttackDamage = attackDamage;
    }

    public Attack()
    {
        AttackRange = 0.0f;
        AttackDistance = 0.0f;
        AttackSpeed = 0.0;
        AttackCooldown = 0.0;
        AttackDamage = 0;
    }

    public void Update(double deltaTime)
    {
        if (IsAttacking)
        {
            if (AttackTimer >= AttackSpeed)
            {
                FinishAttack();
            }
            else
            {
                UpdateAttackTimer(deltaTime);
            }
        }
        UpdateCooldownTimer(deltaTime);
    }

    public bool CanAttack()
    {
        return CooldownTimer >= AttackCooldown && !IsAttacking;
    }

    public void ResetCooldownTimer()
    {
        CooldownTimer = 0.0;
    }
    public void ResetAttackTimer()
    {
        AttackTimer = 0.0;
    }

    public void UpdateCooldownTimer(double deltaTime)
    {
        if (CooldownTimer < AttackCooldown)
        {
            if (CooldownTimer + deltaTime > AttackCooldown)
            {
                CooldownTimer = AttackCooldown;
            }
            else
            {
                CooldownTimer += deltaTime;
            }
        }
    }

    public void UpdateAttackTimer(double deltaTime)
    {
        if (AttackTimer < AttackSpeed)
        {
            if (AttackTimer + deltaTime > AttackSpeed)
            {
                AttackTimer = AttackSpeed;
            }
            else
            {
                AttackTimer += deltaTime;
            }
        }
    }

    public void StartAttack()
    {
        if (CanAttack())
        {
            ResetAttackTimer();
            IsAttacking = true;
        }
    }

    public void FinishAttack()
    {
        if (IsAttacking)
        {
            ResetCooldownTimer();
            AttackTimer = AttackSpeed;
            IsAttacking = false;
        }
    }
}
