using Godot;

namespace Game;

public partial class Main : Node2D
{
    private Sprite2D _cursor;
    private PackedScene _buildingScene;
    private Button _placeBuildingButton;

    public override void _Ready()
    {
        _buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");
        _cursor = GetNode<Sprite2D>("Cursor");
        _placeBuildingButton = GetNode<Button>("PlaceBuildingButton");

        _cursor.Visible = false;

        _placeBuildingButton.Pressed += OnPlaceBuildingButtonPressed;
    }

    public override void _UnhandledInput(InputEvent evt)
    {
        if (_cursor.Visible && evt.IsActionPressed("left_click"))
        {
            PlaceBuildingAtMousePosition();
            _cursor.Visible = false;
        }
    }

    public override void _Process(double delta)
    {
        var gridPosition = GetMouseGridCellPosition();
        _cursor.GlobalPosition = gridPosition * 64;
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

    private void OnPlaceBuildingButtonPressed()
    {
        _cursor.Visible = true;
    }
}