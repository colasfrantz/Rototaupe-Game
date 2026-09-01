using Godot;
using System;

public partial class Arena : Node2D
{
	private Player player;
	private Camera2D MainCamera;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		
		MainCamera = GetNode<Camera2D>("MainCamera");
		player = GetNode<Player>("Player");
		player.PlayerDeath  += OnPlayerDied;
		player.CameraRemoteTransform.RemotePath = MainCamera.GetPath();
	}

	private void OnPlayerDied()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		GD.Print("SandStorm Arena: game over - the player died");
		GetTree().CreateTimer(3).Timeout += () => GetTree().ChangeSceneToFile("res://Menu/Scenes/Menu.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
