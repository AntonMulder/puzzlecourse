using Godot;

namespace Game;

public partial class Main : Node2D
{
    private Sprite2D _sprite;
    private PackedScene _buildingScene;

    public override void _Ready()
    {
        _buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");
        _sprite = GetNode<Sprite2D>("Cursor");
    }

    public override void _UnhandledInput(InputEvent evt)
    {
        if (evt.IsActionPressed("left_click")) PlaceBuildingAtMousePosition();
    }

    public override void _Process(double delta)
    {
        var gridPosition = GetMouseGridCellPosition();
        _sprite.GlobalPosition = gridPosition * 64;
    }

    private Vector2 GetMouseGridCellPosition()
    {
        var mousePosition = GetGlobalMousePosition();
        var gridPosition = (mousePosition / 64).Floor();

        return gridPosition;
    }

    private void PlaceBuildingAtMousePosition()
    {
        var building = _buildingScene.Instantiate<Node2D>();
        AddChild(building);

        var gridPosition = GetMouseGridCellPosition();
        building.GlobalPosition = gridPosition * 64;
    }
}