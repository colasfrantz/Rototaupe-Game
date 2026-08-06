using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public float Speed = 300.0f;
	[Signal]
	public delegate void PlayerDeathEventHandler();
	public RemoteTransform2D CameraRemoteTransform;
	public float Health = 100.0f;
	
	public override void _Ready()
	{
		CameraRemoteTransform = GetNode<RemoteTransform2D>("CameraRemoteTransform");
		PlayerDeath  += OnPlayerDeath;
	}

	public override void _Process(double delta)
	{
		LookAt(GetGlobalMousePosition());
		
		
		if (Input.IsActionJustPressed("quit game"))
		{
			GetTree().ChangeSceneToFile("res://Menu/Scenes/Menu.tscn");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 moveDir = new Vector2(
			Input.GetAxis("move_left", "move_right"),
			Input.GetAxis("move_up", "move_down")
		);

		if (moveDir != Vector2.Zero)
		{
			Velocity = Speed * moveDir.Normalized();
		}
		else
		{
			Velocity = new Vector2(
				Mathf.MoveToward(Velocity.X, 0, Speed),
				Mathf.MoveToward(Velocity.Y, 0, Speed)
			);
		}

		MoveAndSlide();
	}
	
	private void _on_hitbox_body_entered(Node2D body)
	{
		if (body is Enemy1)
		{
			GD.Print("touch an Enemy1");
		}
	}
	
	
	public void TakeDamage(float amount)
	{
		Health -= amount;
		GD.Print("Health: " + Health);

		if (Health <= 0)
		{
			EmitSignal(SignalName.PlayerDeath);
		}
	}
	
	private void OnPlayerDeath()
	{
		GD.Print("Player is dead");
		QueueFree();
	}
	
	
}
