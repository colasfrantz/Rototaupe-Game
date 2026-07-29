using Godot;
using System;

public partial class Arena : Node2D
{
	private Player player;
	private Camera2D MainCamera;

	public override void _Ready()
	{
		MainCamera = GetNode<Camera2D>("MainCamera");
		player = GetNode<Player>("Player");
		player.DamageTaken += OnPlayerDied;
		player.CameraRemoteTransform.RemotePath = MainCamera.GetPath();
	}

	private void OnPlayerDied()
	{
		GD.Print("game over");
		GetTree().CreateTimer(3).Timeout += () => GetTree().ChangeSceneToFile("res://Menu/Scenes/Menu.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
