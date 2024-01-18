using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Data;
static class StatData {
    public static Stats GetEnemyStats(EnemyData.EnemyType enemyType) => enemyType switch {
        EnemyData.EnemyType.Tree => new Stats(health: 100, attack: 1, defense: 1, idleSpeed: 10, runSpeed: 20),
        EnemyData.EnemyType.BigTree => new Stats(health: 100, attack: 1, defense: 1, idleSpeed: 10, runSpeed: 20),
        EnemyData.EnemyType.TeensyMushroom => new Stats(health: 100, attack: 1, defense: 1, idleSpeed: 10, runSpeed: 20),
        EnemyData.EnemyType.Mushroom => new Stats(health: 100, attack: 1, defense: 1, idleSpeed: 10, runSpeed: 20),
        EnemyData.EnemyType.ThiccMushroom => new Stats(health: 100, attack: 1, defense: 1, idleSpeed: 10, runSpeed: 20),
        EnemyData.EnemyType.LongMushroom => new Stats(health: 100, attack: 1, defense: 1, idleSpeed: 10, runSpeed: 20),
        EnemyData.EnemyType.SquidParasite => new Stats(health: 100, attack: 1, defense: 1, idleSpeed: 10, runSpeed: 20),
        _ => new Stats(health: 0, attack: 0, defense: 0, idleSpeed: 0, runSpeed: 0)
    };

    public static Stats PlayerStats {
        get {
            return new Stats(health: 100, attack: 1, defense: 1, idleSpeed: 200, runSpeed: 400);
        }
    }
}
