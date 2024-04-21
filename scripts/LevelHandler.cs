using Godot;
using System;

public partial class LevelHandler : Node2D
{
    [Export] public PauseMenu pauseMenu;
    public bool isPaused;

    public override void _Ready()
    {
        pauseMenu.Hide();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("pause"))
            PauseMenu();
    }

    public void PauseMenu()
    {
        if (isPaused)
        {
            // Hide Menu
            pauseMenu.Hide();
            Engine.TimeScale = 1;
        }
        else
        {
            // Show Menu
            
            pauseMenu.Show();
            pauseMenu.Init();
            Engine.TimeScale = 0;
        }

        isPaused = !isPaused;
    }
}
