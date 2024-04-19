using Godot;
using System;
using System.Data;
using System.Linq;

public partial class ButterflySpawner : Node
{
    private Node2D[] _spawnPoints;

    [Export] PackedScene butterflyScene { get; set; }


    [Export] private int _spawnCount;
    [Export] private double _minSpawnTime;
    [Export] private double _maxSpawnTime;
    private double _spawnTimer;

    public override void _Ready()
    {
        _spawnPoints = GetChildren().Select(c => { return (Node2D)c; }).ToArray();
    }

    public override void _Process(double delta)
    {
        _spawnTimer -= delta;
        if (_spawnTimer <= 0)
        {
            _spawnTimer = _minSpawnTime + GD.Randf() * (_maxSpawnTime - _minSpawnTime);
            SpawnButterflies();
        }
    }

    private void SpawnButterflies()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            int index = Mathf.FloorToInt(GD.Randf() * _spawnPoints.Length);
            var spawnPoint = _spawnPoints[index];
            Butterfly b = butterflyScene.Instantiate<Butterfly>();
            b.GlobalPosition = spawnPoint.GlobalPosition;
            b.direction = Vector2.Right.Rotated(spawnPoint.Rotation).Normalized();
            AddChild(b);
        }
    }
}
