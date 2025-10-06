using Godot;
using System.Collections.Generic;

namespace Game.Managers;

public partial class GridManager : Node
{
    private HashSet<Vector2> _occupiedCells = new();

    [Export] private TileMapLayer _highlightTileMapLayer;
    [Export] private TileMapLayer _baseTerrainTileMapLayer;


    public override void _Ready()
    {
    }

    public void MarkTileAsOccupied(Vector2 tilePosition)
    {
        _occupiedCells.Add(tilePosition);
    }

    public bool IsTilePositionValid(Vector2 tilePosition)
    {
        return !_occupiedCells.Contains(tilePosition);
    }

    public void HighlightTilesInRadius(Vector2 rootCell, int radius)
    {
        _highlightTileMapLayer.Clear();

        for (var x = rootCell.X - radius; x <= rootCell.X + radius; x++)
        {
            for (var y = rootCell.Y - radius; y <= rootCell.Y + radius; y++)
            {
                if (!IsTilePositionValid(new Vector2(x, y)))
                {
                    continue;
                }

                _highlightTileMapLayer.SetCell(new Vector2I((int)x, (int)y), 0, Vector2I.Zero);
            }
        }
    }

    public void ClearHighlightedTiles()
    {
        _highlightTileMapLayer.Clear();
    }

    public Vector2 GetMouseGridCellPosition()
    {
        var mousePosition = _highlightTileMapLayer.GetGlobalMousePosition();
        var gridPosition = (mousePosition / 64).Floor();

        return gridPosition;
    }
}