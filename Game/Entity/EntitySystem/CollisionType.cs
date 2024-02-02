namespace GameJam14.Game.Entity.EntitySystem;
public class CollisionType {
    /*
     * ---
     * Each CollisionType has a SolidType, which determines how it interacts with other solids.
     * ---
     * Solid: Collides with walls and blockades, but not other solids
     * NonSolid: Collides with solids, walls, and compatible non-solids, but not with blockades
     * Wall: Collides with solids and non-solids
     * Blockade: Collides with solids, but not with non-solids
     * None: Does not collide with anything
     *
     * ---
     * Each CollisionType has a LightType, which determines how it interacts with light.
     * ---
     * Light: Collides with Darkness
     * Darkness: Collides with Light
     * None: Only collides with other compatible None types
     *
     * Each CollisionType has a PlayerCollision and EnemyCollision, which determines how it interacts with entities. A collision may have both, one, or neither of these.
     * ----
     * PlayerCollision: Is able to collide with Player entities
     * EntityCollision: Is able to collide with Enemy entities
     *
     * ---
     * At least one of the compatible non-solid conditions must be true for a collision to occur.
     * Compatible non-solids:
     *  - Light:
     *    - Light collides with Darkness
     *    - Darkness collides with Light
     *    - None collides with None
     *  - Entity:
     *    - PlayerCollision collides with Player
     *    - EnemyCollision collides with Enemy
     *
     *
    */

    /// <summary>
    ///   Initializes a new instance of the <see cref="CollisionType" /> class.
    /// </summary>
    /// <param name="solidType">
    ///   The solid collision type.
    /// </param>
    /// <param name="lightType">
    ///   The light collision type.
    /// </param>
    /// <param name="entityType">
    ///   The entity collision type.
    /// </param>
    /// <param name="playerCollision">
    ///   If true, this collision type has player collision.
    /// </param>
    /// <param name="enemyCollision">
    ///   If true, this collision type has enemy collision.
    /// </param>
    public CollisionType(SolidType solidType, LightType lightType, EntityType entityType = EntityType.Other, bool playerCollision = false, bool enemyCollision = false) {
        this._solidType = solidType;
        this._lightType = lightType;
        this._entityType = entityType;
        this._playerCollision = playerCollision;
        this._enemyCollision = enemyCollision;
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="CollisionType" /> class with no collision.
    /// </summary>
    public CollisionType() {
        this._solidType = SolidType.None;
        this._lightType = LightType.None;
        this._entityType = EntityType.Other;
        this._playerCollision = false;
        this._enemyCollision = false;
    }

    /// <summary>
    ///   The type of entity this collision type represents.
    /// </summary>
    public enum EntityType {
        Player,
        Enemy,
        Other
    }

    /// <summary>
    ///   The type of light collision.
    /// </summary>
    public enum LightType {
        Light,
        Darkness,
        None
    }

    /// <summary>
    ///   The type of solid collision.
    /// </summary>
    public enum SolidType {
        Solid,
        NonSolid,
        Wall,
        Blockade,
        None
    }

    public bool CheckEntityCollision(CollisionType collision) {
        return ( this._playerCollision && collision.IsPlayer() )
            || ( this._enemyCollision && collision.IsEnemy() )
            || ( collision._playerCollision && this.IsPlayer() )
            || ( collision._enemyCollision && this.IsEnemy() );
    }

    public bool CheckLightCollision(CollisionType collision) {
        return this.NoLightCollision() || collision.NoLightCollision()
            ? this._lightType == collision._lightType
            : ( this.IsLight() && collision.IsDarkness() ) || ( this.IsDarkness() && collision.IsLight() );
    }

    public bool CheckSolidCollision(CollisionType collision) {
        if ( this.NoSolidCollision() || collision.NoSolidCollision() ) {
            return false;
        }

        // Check wall collision
        if ( this.IsWall() || collision.IsWall() ) {
            return true;
        }

        // Check blockade collision
        if ( this.IsBlockade() || collision.IsBlockade() ) {
            return this.IsSolid() || collision.IsSolid();
        }

        // Check solid/non-solid collision
        if ( this.IsSolid() && collision.IsSolid() ) {
            return false;
        }

        if ( this.IsNonSolid() && collision.IsNonSolid() ) {
            return this.CheckLightCollision(collision) || this.CheckEntityCollision(collision);
        }

        // Otherwise, one is a solid and one is a non-solid, which is a collision
        return true;
    }

    public bool Collides(CollisionType collision) {
        return this.CheckSolidCollision(collision);
    }

    /// <summary>
    ///   Checks whether this collision type is a blockade.
    /// </summary>
    /// <returns>
    ///   True if the collision type is a blockade, false otherwise
    /// </returns>
    public bool IsBlockade() {
        return this._solidType == SolidType.Blockade;
    }

    /// <summary>
    ///   Checks whether this collision type represents darkness.
    /// </summary>
    /// <returns>
    ///   True if the collision type represents darkness.
    /// </returns>
    public bool IsDarkness() {
        return this._lightType == LightType.Darkness;
    }

    /// <summary>
    ///   Checks whether this collision type represents a enemy.
    /// </summary>
    /// <returns>
    ///   True if the collision type represents an enemy, false otherwise
    /// </returns>
    public bool IsEnemy() {
        return this._entityType == EntityType.Enemy;
    }

    /// <summary>
    ///   Checks whether this collision type represents light.
    /// </summary>
    /// <returns>
    ///   True if the collision type represents light.
    /// </returns>
    public bool IsLight() {
        return this._lightType == LightType.Light;
    }

    /// <summary>
    ///   Checks whether this collision type is non-solid.
    /// </summary>
    /// <returns>
    ///   True if the collision type is non-solid, false otherwise
    /// </returns>
    public bool IsNonSolid() {
        return this._solidType == SolidType.NonSolid;
    }

    /// <summary>
    ///   Checks whether this collision type represents a player.
    /// </summary>
    /// <returns>
    ///   True if the collision type represents a player, false otherwise
    /// </returns>
    public bool IsPlayer() {
        return this._entityType == EntityType.Player;
    }

    /// <summary>
    ///   Checks whether this collision type is solid.
    /// </summary>
    /// <returns>
    ///   True if the collision type is solid, false otherwise
    /// </returns>
    public bool IsSolid() {
        return this._solidType == SolidType.Solid;
    }

    /// <summary>
    ///   Checks whether this collision type is a wall.
    /// </summary>
    /// <returns>
    ///   True if the collision type is a wall, false otherwise
    /// </returns>
    public bool IsWall() {
        return this._solidType == SolidType.Wall;
    }

    /// <summary>
    ///   Checks whether this collision type does not represent any kind of light.
    /// </summary>
    /// <returns>
    ///   True if the collision type does not represent any kind of light.
    /// </returns>
    public bool NoLightCollision() {
        return this._lightType == LightType.None;
    }

    /// <summary>
    ///   Checks whether this collision type has no solid collision.
    /// </summary>
    /// <returns>
    ///   True if the collision type has no solid collision, false otherwise
    /// </returns>
    public bool NoSolidCollision() {
        return this._solidType == SolidType.None;
    }

    private readonly bool _enemyCollision;
    private readonly EntityType _entityType;
    private readonly LightType _lightType;
    private readonly bool _playerCollision;
    private readonly SolidType _solidType;
}
