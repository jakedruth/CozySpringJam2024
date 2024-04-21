using Godot;
using System;

public partial class PauseMenu : Control
{
    [Export] public LevelHandler levelHandler;

    public void Init()
    {
        Button first = GetNode<Button>("Panel/VBoxContainer/Continue");
        first.GrabFocus();
    }

    public void UnPauseGame()
    {
        levelHandler.PauseMenu();
    }

    public void QuitGame()
    {
        GetTree().Quit();
    }
}
