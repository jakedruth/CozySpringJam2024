using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private AnimationPlayer _bodyAnimationPlayer;
    private AnimationPlayer _netAnimationPlayer;
    private ProgressBar _progressBar;

    [Export] private float _walkSpeed;
    [Export] private float _runSpeed;
    [Export] private double _maxStamina;
    private double _stamina;

    // Input Variables
    private Vector2 _input;
    private Vector2 _lastFacing;
    private bool _isRunning;
    private double _swingTime;

    public override void _Ready()
    {
        _bodyAnimationPlayer = GetNode<AnimationPlayer>("BodyAnimationPlayer");
        _netAnimationPlayer = GetNode<AnimationPlayer>("NetAnimationPlayer");
        _progressBar = GetNode<ProgressBar>("ProgressBar");

        _lastFacing = Vector2.Right;
        _bodyAnimationPlayer.Play("idle_R");
        _stamina = _maxStamina;
    }

    public override void _Process(double delta)
    {
        _input = Input.GetVector("walk_left", "walk_right", "walk_up", "walk_down");
        if (_input.LengthSquared() > 0.1)
            _lastFacing = _input;

        _isRunning = Input.IsActionPressed("run");
        if (_swingTime <= 0 && Input.IsActionJustPressed("swing"))
        {
            _swingTime = 0.75;
            StartSwing();
        }

        _progressBar.Value = _stamina / _maxStamina;
        _progressBar.Visible = _stamina != _maxStamina || _isRunning;

        HandleAnimations();
    }

    private void StartSwing()
    {
        if (_lastFacing.X > 0)
            _netAnimationPlayer.Play("swing_R");
        else if (_lastFacing.X < 0)
            _netAnimationPlayer.Play("swing_L");
        else if (_lastFacing.Y > 0)
            _netAnimationPlayer.Play("swing_D");
        else
            _netAnimationPlayer.Play("swing_U");
    }


    private void HandleAnimations()
    {
        bool moving = Velocity.LengthSquared() > 0;
        string anim = (moving ? "walk_" : "idle_") + (_lastFacing.X > 0 ? "R" : "L");

        _bodyAnimationPlayer.Play(anim);
    }


    public override void _PhysicsProcess(double delta)
    {
        if (_swingTime > 0)
            _swingTime -= delta;

        if (_isRunning && _input.LengthSquared() > 0)
            _stamina -= delta;
        else
            _stamina += delta * 0.5f;

        _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);

        float speed = _swingTime > 0
            ? 0
            : _isRunning && _stamina > 0
                ? _runSpeed
                : _walkSpeed;

        Velocity = _input * speed;
        MoveAndSlide();
    }

    public void _on_net_area_body_entered(Node2D other)
    {
        if (other is Butterfly butterfly)
        {
            butterfly.CallDeferred("free");
        }
    }
}
