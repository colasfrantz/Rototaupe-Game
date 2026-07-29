using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public float Speed = 300.0f;

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
	
	private void OnHitboxBodyEntered(Node2D body)
	{
		if(body is Enemy1)
		{
			GD.Print("touched the player");
		}
	}
	
	
}
