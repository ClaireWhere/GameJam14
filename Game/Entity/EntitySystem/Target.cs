namespace GameJam14.Game.Entity.EntitySystem;
internal class Target {
    public Target(TargetType type, float targetRange, float targetDistance, float targetAngle) {
        this.Type = type;
        this.TargetRange = targetRange;
        this.TargetDistance = targetDistance;
        this.TargetAngle = targetAngle;
    }

    public Target() {
        this.Type = TargetType.None;
        this.TargetRange = 0;
        this.TargetDistance = 0;
        this.TargetAngle = 0;
    }

    /// <summary>
    ///   Types of targets for the enemy to attack.
    /// </summary>
    public enum TargetType {
        Player,
        Light,
        None
    }

    /// <summary>
    ///   Gets or sets the target angle.
    /// </summary>
    public float TargetAngle { get; set; }

    /// <summary>
    ///   Gets or sets the target distance.
    /// </summary>
    public float TargetDistance { get; set; }

    /// <summary>
    ///   Gets or sets the target range.
    /// </summary>
    public float TargetRange { get; set; }

    /// <summary>
    ///   Gets or sets the target type.
    /// </summary>
    public TargetType Type { get; set; }

    public bool IsTargetType(Entity entity) {
        return this.Type != TargetType.None
&& ( this.Type == TargetType.Player
            ? entity.GetType() == typeof(Player)
            : this.Type == TargetType.Light && entity.GetType() == typeof(Light) );
    }
}
