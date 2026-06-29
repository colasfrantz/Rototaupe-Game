using Godot;

public partial class Minotaupe : CharacterBody2D
{
	[Export] public float BaseSpeed = 200.0f;
	public float SandstormMultiplier = 1.0f;
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		Velocity = direction * (BaseSpeed * SandstormMultiplier);
		MoveAndSlide();
	}


}
