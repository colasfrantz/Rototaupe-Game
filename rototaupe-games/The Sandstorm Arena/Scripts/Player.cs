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
	public Vector2 aimDirection = new Vector2(50.0f, 0.0f);
	
	public Area2D attackArea;
	public float attackRange = 50.0f;
	public Sprite2D cursor;
	public float cursorRange = 100.0f;
	public bool inAttack = false;
	
	
	public override void _Ready()
	{
		CameraRemoteTransform = GetNode<RemoteTransform2D>("CameraRemoteTransform");
		body = GetNode<AnimatedSprite2D>("Body");
		attackArea = GetNode<Area2D>("AttackArea");
		_HealthBar = GetNode<HealthBar>("HealthBar");
		cursor = GetNode<Sprite2D>("Cursor");
		
		_HealthBar.InitHealth(Health);
		attackArea.Position = lastDirection.Normalized() * attackRange;
		cursor.Position = lastDirection.Normalized() * cursorRange;
		
		body.AnimationFinished += OnAttackAnimationFinished;
		PlayerDeath  += OnPlayerDeath;
	}

	public override void _Process(double delta)
	{
		Vector2 toMouse = GetGlobalMousePosition() - GlobalPosition;
		cursor.Position = toMouse.Normalized() * cursorRange;
		cursor.Rotation = toMouse.Angle();
		
		aimDirection =  GetGlobalMousePosition() - GlobalPosition;
		
		
		if(Input.IsActionJustPressed("attack"))
		{
			var bodies = attackArea.GetOverlappingBodies();
			GD.Print("Bodies in attack area: --" + bodies.Count + "--");
			GD.Print("You are attacking them, dealing 30 dmg");
			AnimationAttack(aimDirection);
			foreach (Node2D OverlapingBodies in attackArea.GetOverlappingBodies())
			{
				if(OverlapingBodies is Enemy1 enemy)
				{
					enemy.TakeDamage(30.0f);
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
			UpdateAnimationMove(moveDir);
		}
		else
		{
			Velocity = new Vector2(
				Mathf.MoveToward(Velocity.X, 0, Speed),
				Mathf.MoveToward(Velocity.Y, 0, Speed)
			);
			if (!inAttack)
			{
				body.Stop();
			}
		}

		MoveAndSlide();
		attackArea.Position = aimDirection.Normalized() * attackRange;
		attackArea.Rotation = aimDirection.Angle();
	}
	
	private void UpdateAnimationMove(Vector2 direction)
	{
		if (!inAttack)
		{
			float angle = Mathf.RadToDeg(direction.Angle());
			int ang = Mathf.RoundToInt(angle / 45f) * 45;
			switch(ang)
			{
				case(0):
					body.Play("Right");
					break;
				case(45):
					body.Play("DownRight");
					break;
				case(90):
					body.Play("Down");
					break;
				case(135):
					body.Play("DownLeft");
					break;
				case(180):
					body.Play("Left");
					break;
				case(-180):
					body.Play("Left");
					break;
				case(-135):
					body.Play("UpLeft");
					break;
				case(-90):
					body.Play("Up");
					break;
				case(-45):
					body.Play("UpRight");
					break;
				default:
					break;
			}
		}
	}
	
	public void AnimationAttack(Vector2 direction)
	{
		inAttack = true;
		float angle = Mathf.RadToDeg(direction.Angle());
		int ang = Mathf.RoundToInt(angle / 45f) * 45;
		switch(ang)
		{
			case(0):
				body.Play("Attack_Right");
				break;
			case(45):
				body.Play("Attack_Down_Right");
				break;
			case(90):
				body.Play("Attack_Down");
				break;
			case(135):
				body.Play("Attack_Down_Left");
				break;
			case(180):
				body.Play("Attack_Left");
				break;
			case(-180):
				body.Play("Attack_Left");
				break;
			case(-135):
				body.Play("Attack_Up_Left");
				break;
			case(-90):
				body.Play("Attack_Up");
				break;
			case(-45):
				body.Play("Attack_Up_Right");
				break;
			default:
				break;
		}
	}
	
	private void OnAttackAnimationFinished()
	{
		if (inAttack)
		{
			inAttack = false;
		}
	}
	
	
	private void _on_hitbox_body_entered(Node2D body)
	{
		if (body is Enemy1)
		{
			GD.Print("An Enemy1 is touching the Player");
		}
	}
	
	
	public void TakeDamage(float amount)
	{
		Health -= amount;
		GD.Print("Taking Damage \nPlayer's Health: " + Health);
		_HealthBar.Health = Health;

		if (Health <= 0)
		{
			EmitSignal(SignalName.PlayerDeath);
		}
	}
	
	private void OnPlayerDeath()
	{
		GD.Print("The Player is dead");
		QueueFree();
	}
	
	
}
