using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public float Speed = 300.0f;
	[Signal]
	public delegate void DamageTakenEventHandler();
	public RemoteTransform2D CameraRemoteTransform;
	
	public override void _Ready()
	{
		CameraRemoteTransform = GetNode<RemoteTransform2D>("CameraRemoteTransform");
		DamageTaken += OnDamageTaken;
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
			EmitSignal(SignalName.DamageTaken);
		}
	}
	
	private void OnDamageTaken()
	{
		GD.Print("Player took some damage");
		QueueFree();
	}
	
	
}
