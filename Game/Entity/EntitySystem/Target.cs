using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity.EntitySystem;
internal class Target
{

    /// <summary>
    /// Types of targets for the enemy to attack.
    /// </summary>
    public enum TargetType
    {
        Player,
        Light,
        None
    }
    /// <summary>
    /// Gets or sets the target type.
    /// </summary>
    public TargetType Type { get; set; }
    /// <summary>
    /// Gets or sets the target range.
    /// </summary>
    public float TargetRange { get; set; }
    /// <summary>
    /// Gets or sets the target distance.
    /// </summary>
    public float TargetDistance { get; set; }
    /// <summary>
    /// Gets or sets the target angle.
    /// </summary>
    public float TargetAngle { get; set; }

    public Target(TargetType type, float targetRange, float targetDistance, float targetAngle)
    {
        Type = type;
        TargetRange = targetRange;
        TargetDistance = targetDistance;
        TargetAngle = targetAngle;
    }

    public Target()
    {
        Type = TargetType.None;
        TargetRange = 0;
        TargetDistance = 0;
        TargetAngle = 0;
    }

    public bool IsTargetType(Entity entity)
    {
        if (Type == TargetType.None)
        {
            return false;
        }
        if (Type == TargetType.Player)
        {
            return entity.GetType() == typeof(Player);
        }
        if (Type == TargetType.Light)
        {
            return entity.GetType() == typeof(Light);
        }
        return false;
    }
}
