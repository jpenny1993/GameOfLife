using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class World
    {
        private readonly int _xLimit;
        private readonly int _yLimit;

        private readonly bool[,] _previousFrame;
        private readonly bool[,] _nextFrame;

        private readonly IReadOnlyList<(int x, int y)> _drawOrder;

        public World(int size) : this(size, size) { }

        public World(int x, int y)
        {
            if (x < 1)
                throw new ArgumentException("X plane must be greater than 0.", nameof(x));

            if (y < 1)
                throw new ArgumentException("Y plane must be greater than 0.", nameof(y));

            _xLimit = x;
            _yLimit = y;
            _previousFrame = new bool[y, x];
            _nextFrame = new bool[y, x];

            // Generate a random order to drawing tiles
            // This is to avoid screen tearing when the world is overpopulated
            _drawOrder = Enumerable.Range(0, x)
                .SelectMany(xPos => Enumerable.Range(0, y)
                .Select(yPos => (xPos, yPos)))
                .OrderBy(_ => Guid.NewGuid())
                .ToArray();
        }

        /// <summary>
        /// Draws the provided lifeform in the next frame at the provided co-ordinates.
        /// </summary>
        public void AddLifeform(int x, int y, Lifeform lifeform) 
        {
            for (var oY = 0; oY < lifeform.Height(); oY++)
            for (var oX = 0; oX < lifeform.Width(); oX++)
            {
                var worldX = oX + x;
                var worldY = oY + y;

                if (!IsTileValid(x, y)) 
                    continue;

                SetEntity(worldX, worldY, lifeform.IsOccupied(oX, oY));
            }
        }

        /// <summary>
        /// Counts the number of occupied tiles surrounding the given co-ordinates.
        /// </summary>
        public int CountNeighbours(int x, int y)
        {
            int counter = 0;
            for (var oX = x - 1; oX < x + 2; oX++)
            for (var oY = y - 1; oY < y + 2; oY++)
            {
                // Skip self
                if (oX == x && oY == y)
                    continue;

                if (IsOccupied(oX, oY))
                    counter++;
            }

            return counter;
        }

        /// <summary>
        /// Gets entity from previously lived frame
        /// </summary>
        public bool GetEntity(int x, int y)
        {
            if (!IsTileValid(x, y))
                throw new InvalidOperationException($"({x}, {y}) is not a valid location, the boundaries are [(0, 0),({_xLimit - 1}, {_yLimit - 1})]");

            return _previousFrame[y, x];
        }

        /// <summary>
        /// Sets tile as occupied on next frame.
        /// </summary>
        public void SetEntity(int x, int y, bool occupied)
        {
            if (!IsTileValid(x, y))
                throw new InvalidOperationException($"({x}, {y}) is not a valid location, the boundaries are [(0, 0),({_xLimit - 1}, {_yLimit - 1})]");

            _nextFrame[y, x] = occupied;
        }

        /// <summary>
        /// Returns true if the provided co-ordinates are with the boundaries of the world.
        /// </summary>
        public bool IsTileValid(int x, int y)
        {
            return x >= 0 && x < _xLimit &&
                   y >= 0 && y < _yLimit;
        }

        /// <summary>
        /// Returns true if the population is less than 5.
        /// Used to know when the world can be reinitialised.
        /// </summary>
        public bool IsDead() 
        {
            // still life is a min of 4 blocks
            var counter = 0;

            for (var x = 0; x < _xLimit; x++)
            for (var y = 0; y < _yLimit; y++) 
            {
                if (GetEntity(x, y) && ++counter > 4)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if the tile is valid and was occupied on the previous frame.
        /// </summary>
        public bool IsOccupied(int x, int y)
        {
            return IsTileValid(x, y) && GetEntity(x, y);
        }

        /// <summary>
        /// Calculates the next frame based off of the previous frame.
        /// </summary>
        public void LiveFrame() 
        {
            for (var x = 0; x < _xLimit; x++)
            for (var y = 0; y < _yLimit; y++)
            {
                var isAlive = IsOccupied(x, y);
                var neighbours = CountNeighbours(x, y);

                if (isAlive && (neighbours < 2 || neighbours > 3))
                {
                    // Death, due to loneliness or overcrowding
                    SetEntity(x, y, false);
                }
                else if (!isAlive && neighbours > 2)
                {
                    // Birth
                    SetEntity(x, y, true);
                }
                else 
                {
                    // Maintain current state
                    SetEntity(x, y, isAlive);
                }
            }
        }

        /// <summary>
        /// Informs the UI which tile have changed and what they should be.
        /// </summary>
        /// <param name="drawTile">callback to draw a tile</param>
        public void DrawChanges(Action<(int x, int y), bool> drawTile)
        {
            foreach (var pos in _drawOrder)
            {
                var prev = _previousFrame[pos.y, pos.x];
                var next = _nextFrame[pos.y, pos.x];

                if (prev != next) 
                {
                    drawTile((pos.x, pos.y), next);
                }
            }
        }

        /// <summary>
        /// Copies the current frame to the previous frame.
        /// </summary>
        public void SaveFrame() 
        {
            for (var x = 0; x < _xLimit; x++)
            for (var y = 0; y < _yLimit; y++) 
            {
                _previousFrame[y, x] = _nextFrame[y, x];
            }
        }
    }
}
