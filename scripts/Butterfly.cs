using Godot;
using System;

public partial class Butterfly : CharacterBody2D
{
    [Export] float _speed;
    private Vector2 _direction;
    Image image;
    byte[] data;
    double t;

    public override void _Ready()
    {
        _direction = RandomUnitVector();
        var texture = new NoiseTexture2D();
        texture.Noise = new FastNoiseLite();
        texture.Changed += () =>
        {
            image = texture.GetImage();
            data = image.GetData();
        };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = _direction * _speed;

        LookAt(Position + _direction);

        MoveAndSlide();
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
}
