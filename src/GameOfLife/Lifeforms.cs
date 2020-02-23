using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public abstract class Lifeform
    {
        protected int[,] _tiles;

        public bool IsOccupied(int x, int y) => _tiles[y, x] == 1;

        public int Width() => _tiles.GetLength(1);

        public int Height() => _tiles.GetUpperBound(0) + 1;

        public static IEnumerable<Lifeform> Random(int count) => typeof(Lifeform)
            .Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Lifeform)))
            .OrderBy(_ => Guid.NewGuid())
            .Take(count)
            .Select(t => (Lifeform)Activator.CreateInstance(t));
    }

    #region Still Lifes

    public class Block : Lifeform
    {
        public Block() => _tiles = new [,]
        {
            { 1, 1 },
            { 1, 1 }
        };
    }

    public class BeeHive : Lifeform
    {
        public BeeHive() => _tiles = new[,]
        {
            { 0, 1, 1, 0 },
            { 1, 0, 0, 1 },
            { 0, 1, 1, 0 }
        };
    }

    public class Loaf : Lifeform
    {
        public Loaf() => _tiles = new[,]
        {
            { 0, 1, 1, 0 },
            { 1, 0, 0, 1 },
            { 0, 1, 0, 1 },
            { 0, 0, 1, 0 }
        };
    }

    public class Boat : Lifeform
    {
        public Boat() => _tiles = new[,]
        {
            { 1, 1, 0 },
            { 1, 0, 1 },
            { 0, 1, 0 }
        };
    }

    #endregion Still Lifes

    #region Oscillators

    public class Blinker : Lifeform
    {
        public Blinker() => _tiles = new[,] 
        {
            { 1, 1, 1 }
        };
    }

    public class Toad : Lifeform
    {
        public Toad() => _tiles = new [,]
        {
            { 0, 1, 1, 1 },
            { 1, 1, 1, 0 }
        };
    }

    public class Beacon : Lifeform
    {
        public Beacon() => _tiles = new [,]
        {
            { 1, 1, 0, 0 },
            { 1, 0, 0, 0 },
            { 0, 0, 0, 1 },
            { 0, 0, 1, 1 }
        };
    }

    public class Pulsar : Lifeform
    {
        public Pulsar() => _tiles = new[,]
        {
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 }
        };
    }

    public class PentaDecathalon : Lifeform
    {
        public PentaDecathalon() => _tiles = new[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 1, 0, 1 },
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 1, 0, 1 },
            { 0, 1, 0 },
            { 0, 1, 0 },
        };
    }

    #endregion Oscillators

    #region Spaceships

    public class Glider : Lifeform
    {
        public Glider() => _tiles = new[,]
        {
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 },
            { 0, 1, 1, 1 }
        };
    }

    public class LightWeightSpaceship : Lifeform
    {
        public LightWeightSpaceship() => _tiles = new[,]
        {
            { 1, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1 },
            { 0, 1, 1, 1, 1 }
        };
    }

    public class MiddleWeightSpaceship : Lifeform
    {
        public MiddleWeightSpaceship() => _tiles = new[,]
        {
            { 0, 0, 1, 0, 0, 0 },
            { 1, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1 },
            { 0, 1, 1, 1, 1, 1 }
        };
    }

    public class HeavyWeightSpaceship : Lifeform
    {
        public HeavyWeightSpaceship() => _tiles = new[,]
        {
            { 0, 0, 1, 1, 0, 0, 0 },
            { 1, 0, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 1 },
            { 0, 1, 1, 1, 1, 1, 1 }
        };
    }

    #endregion Spaceships
}
