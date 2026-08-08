using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public float Speed = 300.0f;
	[Signal]
	public delegate void PlayerDeathEventHandler();
	public RemoteTransform2D CameraRemoteTransform;
	private AnimatedSprite2D body;
	
	
	public float Health = 100.0f;
	public HealthBar _HealthBar;
	
	public Vector2 lastDirection = new Vector2(50.0f, 0.0f);
	
	public Area2D attackArea;
	public float AttackRange = 50.0f;
	
	
	public override void _Ready()
	{
		CameraRemoteTransform = GetNode<RemoteTransform2D>("CameraRemoteTransform");
		body = GetNode<AnimatedSprite2D>("Body");
		attackArea = GetNode<Area2D>("AttackArea");
		_HealthBar = GetNode<HealthBar>("HealthBar");
		PlayerDeath  += OnPlayerDeath;
		
		_HealthBar.InitHealth(Health);
		attackArea.Position = lastDirection.Normalized() * AttackRange;
	}

	public override void _Process(double delta)
	{
		//LookAt(GetGlobalMousePosition());
		if(Input.IsActionJustPressed("attack"))
		{
			attackArea.Position = lastDirection.Normalized() * AttackRange;
			foreach (OverlapingBody in GetOverlappingBodies())
			{
				if(OverlapingBody is Enemy1)
				{
					OverlapingBody.TakeDamage(30.0f); //here
				}
			}
		}
		
		
		if (Input.IsActionJustPressed("quit game"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
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
			lastDirection = moveDir;
			Velocity = Speed * moveDir.Normalized();
			UpdateAnimation(moveDir);
		}
		else
		{
			Velocity = new Vector2(
				Mathf.MoveToward(Velocity.X, 0, Speed),
				Mathf.MoveToward(Velocity.Y, 0, Speed)
			);
			body.Stop();
		}

		MoveAndSlide();
	}
	
	private void UpdateAnimation(Vector2 direction)
	{
		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			body.Play(direction.X > 0 ? "Right" : "Left");
		}
		else
		{
			body.Play(direction.Y > 0 ? "Down" : "Up");
		}
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
		_HealthBar.Health = Health;

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
