using Godot;
using System;

public partial class Enemy1 : CharacterBody2D
{
	[Signal]
	public delegate void Enemy1DeathEventHandler();
	
	public Player player = null;
	public float Speed = 200.0f;
	public float Health = 100.0f;
	
	public Vector2 direction = Vector2.Zero;
	public float stop_distance = 20.0f;
	
	public float DamageAmount = 5.0f;
	private Timer AttackTimer;
	private bool TouchingPlayer = false;
	
	public HealthBar _HealthBar;
	
	
	public override void _Ready()
	{
		AttackTimer = GetNode<Timer>("AttackTimer");
		_HealthBar = GetNode<HealthBar>("HealthBar");
		_HealthBar.InitHealth(Health);
		AttackTimer.WaitTime = 2;
		AttackTimer.OneShot = false;
		AttackTimer.Timeout += OnAttackTimerTimeout;
		
		Enemy1Death  += OnEnemy1Death;
	}

	public override void _Process(double delta)
	{
		if (player != null){
			LookAt(player.GlobalPosition);
		}
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		if(player != null)
		{
			var enemy_to_player = (player.GlobalPosition - this.GlobalPosition);
			if(enemy_to_player.Length() > stop_distance)
			{
				direction = enemy_to_player.Normalized();
			}
			else
			{
				direction = Vector2.Zero;
			}
			
			if (direction != Vector2.Zero)
			{
				Velocity = Speed * direction;
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
		
	}
	
	private void _on_player_detector_body_entered(Node2D body)
	{
		if (body is Player)
		{
			if (player == null)
			{
				player = body as Player;
				GD.Print(Name + " found the player\n" + Name + " is following the Player");
			}
		}
	}
	
	private void _on_player_detector_body_exited(Node2D body)
	{
		if (body is Player)
		{
			if (player != null)
			{
				player = null;
				GD.Print(Name + " lost the player...");
			}
		}
	}
	
	private void _on_attack_hitbox_body_entered(Node2D body)
	{
		if (body is Player AttackedPlayer)
		{
			TouchingPlayer = true;
			AttackedPlayer.TakeDamage(DamageAmount);
			AttackTimer.Start();
		}
	}

	private void _on_attack_hitbox_body_exited(Node2D body)
	{
		if (body is Player)
		{
			TouchingPlayer = false;
			AttackTimer.Stop();
		}
	}

	private void OnAttackTimerTimeout()
	{
		if (TouchingPlayer && player != null)
		{
			GD.Print(Name + " is attacking the Player");
			player.TakeDamage(DamageAmount);
		}
	}
	
	public void TakeDamage(float amount)
	{
		Health -= amount;
		GD.Print(Name + "'s Health: " + Health);
		_HealthBar.Health = Health;
		
		if (Health <= 0)
		{
			EmitSignal(SignalName.Enemy1Death);
		}
	}
	
	private void OnEnemy1Death()
	{
		GD.Print(Name + " is dead");
		QueueFree();
	}
}
