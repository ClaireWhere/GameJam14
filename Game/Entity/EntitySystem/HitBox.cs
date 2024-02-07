// Ignore Spelling: hitbox

using Microsoft.Xna.Framework;

namespace GameJam14.Game.Entity.EntitySystem;
public class HitBox {
    public Shape.Shape Shape { get; set; }
    public Vector2 Offset {
        get {
            return this._offset;
        }
        set {
            this._offset = value * this.Shape.Scale;
        }
    }
    private Vector2 _offset;

    public HitBox(Shape.Shape shape, Vector2 offset) {
        this.Shape = shape;
        this.Offset = offset * shape.Scale;
    }

    public HitBox(Shape.Shape shape) {
        this.Shape = shape;
        this.Offset = Vector2.Zero;
    }

    public void SetPosition(Vector2 position) {
        this.Shape.Position = position - this.Offset;
    }

    public bool CollidesWith(HitBox hitbox) {
        return this.Shape.Intersects(hitbox.Shape);
    }

    public override string ToString() {
        return "HitBox: " + this.Shape + " | Offset: " + this.Offset.ToString();
    }

    public void UpdateScale(float scale) {
        this.Shape.Scale = scale;
    }
}
