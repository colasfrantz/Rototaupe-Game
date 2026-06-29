using Godot;
using System;

public partial class About : Control
{
	public Label _title;
	public Button _back;
	
	public override void _Ready()
	{
		_title = GetNode<Label>("Title");
		_back = GetNode<Button>("Back");
		
		_back.Pressed += OnBackPressed;
		
	}
	
	public void OnBackPressed()
	{
		GD.Print("Going back to the menu");
		GetTree().ChangeSceneToFile("res://Menu/Scenes/Menu.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
