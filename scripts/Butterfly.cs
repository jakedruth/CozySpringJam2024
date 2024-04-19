using Godot;
using System;

public partial class Butterfly : CharacterBody2D
{
    [Export] CompressedTexture2D[] _textures;
    [Export] float _speed;
    [Export] float _acc;
    [Export] public double _minChangeDirTime;
    [Export] public double _maxChangeDirTime;
    private double _changeDirTimer;

    public Vector2 direction;
    Image image;
    byte[] data;
    double t;

    public override void _Ready()
    {
        Sprite2D sprite2D = GetChild<Sprite2D>(1);
        sprite2D.Texture = _textures[GD.Randi() % _textures.Length];

        ShaderMaterial material = sprite2D.Material as ShaderMaterial;
        sprite2D.Material = material.Duplicate() as ShaderMaterial;
        (sprite2D.Material as ShaderMaterial).SetShaderParameter("hashOffset", GD.Randf() * 6); // 6 is close enough to 2 pi

        direction = RandomUnitVector();
        Velocity = direction * _speed;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        const float bounds = 1000;
        if (GlobalPosition.X < -bounds ||
            GlobalPosition.X > bounds ||
            GlobalPosition.Y < -bounds ||
            GlobalPosition.Y > bounds)
        {
            CallDeferred("free");
            return;
        }

        _changeDirTimer -= delta;
        if (_changeDirTimer <= 0)
        {
            ResetChangeDirectionTimer();
            UpdateDirection();
        }

        Velocity = Velocity.MoveToward(direction * _speed, _acc * (float)delta);

        LookAt(Position + Velocity.Normalized());
        MoveAndSlide();
    }

    private void UpdateDirection()
    {
        direction = (direction * 2 + RandomUnitVector()).Normalized();
    }

    private Vector2 RandomUnitVector(int maxAttempts = 3)
    {
        Vector2 result = Vector2.Zero;
        while (maxAttempts > 0)
        {
            result = new Vector2(
                GD.Randf() * 2 - 1,
                GD.Randf() * 2 - 1);

            if (result.LengthSquared() <= 1)
                return result.Normalized();

            maxAttempts--;
        }

        return result.Normalized();
    }

    private void ResetChangeDirectionTimer()
    {
        _changeDirTimer = _minChangeDirTime + GD.Randf() * (_maxChangeDirTime - _minChangeDirTime);
    }
}
