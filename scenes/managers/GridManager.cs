using Godot;
using System.Collections.Generic;
using System.Linq;
using Game.Component;

namespace Game.Managers;

public partial class GridManager : Node
{
    private HashSet<Vector2I> _occupiedCells = new();

    [Export] private TileMapLayer _highlightTileMapLayer;
    [Export] private TileMapLayer _baseTerrainTileMapLayer;

    public void MarkTileAsOccupied(Vector2I tilePosition)
    {
        _occupiedCells.Add(tilePosition);
    }

    public bool IsTilePositionValid(Vector2I tilePosition)
    {
        var custom_data = _baseTerrainTileMapLayer.GetCellTileData(tilePosition);

        if (custom_data == null)
        {
            return false;
        }

        if (!(bool)custom_data.GetCustomData("buildable"))
        {
            return false;
        }

        {
            return !_occupiedCells.Contains(tilePosition);
        }
    }

    private void HighlightValidTilesInRadius(Vector2I rootCell, int radius)
    {
        for (var x = rootCell.X - radius; x <= rootCell.X + radius; x++)
        {
            for (var y = rootCell.Y - radius; y <= rootCell.Y + radius; y++)
            {
                var tilePosition = new Vector2I(x, y);
                if (!IsTilePositionValid(tilePosition))
                {
                    continue;
                }

                _highlightTileMapLayer.SetCell(tilePosition, 0, Vector2I.Zero);
            }
        }
    }

    public void ClearHighlightedTiles()
    {
        _highlightTileMapLayer.Clear();
    }

    public Vector2I GetMouseGridCellPosition()
    {
        var mousePosition = _highlightTileMapLayer.GetGlobalMousePosition();
        var gridPosition = (mousePosition / 64).Floor();

        return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
    }

    public void HighlightBuildableTiles()
    {
        ClearHighlightedTiles();
        var buildingComponents = GetTree().GetNodesInGroup(nameof(BuildingComponent)).Cast<BuildingComponent>();

        foreach (var buildingComponent in buildingComponents)
        {
            HighlightValidTilesInRadius(buildingComponent.GetGridCellPosition(), buildingComponent.BuildableRadius);
        }
    }
}