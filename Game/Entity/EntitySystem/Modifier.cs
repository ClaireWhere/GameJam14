using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Entity.EntitySystem;
internal class Modifier : IDisposable
{
    public Stats Stats { get; private set; }
    private float Duration { get; set; }
    private float TimeRemaining { get; set; }

    public Modifier(Stats stats, float duration)
    {
        Stats = stats;
        Duration = duration;
        TimeRemaining = duration;
    }

    public void Update(float deltaTime)
    {
        TimeRemaining -= deltaTime;
    }

    public bool IsExpired()
    {
        return TimeRemaining <= 0;
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        this.Stats.Dispose();
        this.Dispose();
    }
}
