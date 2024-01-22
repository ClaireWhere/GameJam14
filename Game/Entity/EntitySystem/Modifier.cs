using System;

namespace GameJam14.Game.Entity.EntitySystem;
internal class Modifier : IDisposable {
    public Modifier(Stats stats, float duration) {
        Stats = stats;
        Duration = duration;
        TimeRemaining = duration;
        this._disposed = false;
    }

    public Stats Stats { get; private set; }
    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public bool IsExpired() {
        return TimeRemaining <= 0;
    }

    public void Update(float deltaTime) {
        TimeRemaining -= deltaTime;
    }

    protected virtual void Dispose(bool disposing) {
        if ( this._disposed ) {
            return;
        }
        this.Stats.Dispose();
        this._disposed = true;
    }

    private float Duration { get; set; }
    private float TimeRemaining { get; set; }
    public bool _disposed;
}
