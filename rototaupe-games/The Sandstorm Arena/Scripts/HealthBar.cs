using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	public Timer timer;
	public ProgressBar DamageBar;
	
	private float _health = 0.0f;
	public float Health
	{
		get => _health;
		set => SetHealth(value);
	}
	
	public void SetHealth(float NewHealth)
	{
		float PrevHealth = _health;
		_health = (float)Mathf.Min(MaxValue, NewHealth);
		Value = _health;
		
		if(_health <= 0)
		{
			QueueFree();
		}
		
		if(_health < PrevHealth)
		{
			timer.Start();
		}
		else
		{
			DamageBar.Value = _health;
		}
	}
	
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		DamageBar = GetNode<ProgressBar>("DamageBar");
		timer.OneShot = true;
	}

	
	public override void _Process(double delta)
	{
	}
	
	public void InitHealth(float health)
	{
		MaxValue = health;
		Health = health;
		Value = health;
		DamageBar.MaxValue = health;
		DamageBar.Value = health;
	}
	
	public void _on_timer_timeout()
	{
		GD.Print("Timer timeout, DamageBar update to: " + _health);
		DamageBar.Value = _health;
	}
}
