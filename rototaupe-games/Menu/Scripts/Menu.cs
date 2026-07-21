using Godot;
using System;

public partial class Menu : Control
{
	public Label _title;
	public Button _game1Button;
	public Button _game2Button;
	public Button _game3Button;
	public Button _game4Button;
	public Button _aboutgamesButton;
	
	
	public override void _Ready()
	{
		_title = GetNode<Label>("Title");
		_game1Button = GetNode<Button>("Box/Row_1/Mini_Game_1");
		_game2Button = GetNode<Button>("Box/Row_1/Mini_Game_2");
		_game3Button = GetNode<Button>("Box/Row_2/Mini_Game_3");
		_game4Button = GetNode<Button>("Box/Row_2/Mini_Game_4");
		_aboutgamesButton = GetNode<Button>("Box/Row_3/About_the_Games");
		
		_game1Button.Pressed += OnGame1Pressed;
		_game2Button.Pressed += OnGame2Pressed;
		_game3Button.Pressed += OnGame3Pressed;
		_game4Button.Pressed += OnGame4Pressed;
		_aboutgamesButton.Pressed += OnAboutTheGamesPressed;
		
	}
	
	public void OnGame1Pressed()
	{
		GD.Print("The Sandstorm Arena Launches !");
		GetTree().ChangeSceneToFile("res://The Sandstorm Arena/Scenes/Arena.tscn");
	}
	
	public void OnGame2Pressed()
	{
		GD.Print("Game 2 Launches !");
		GetTree().ChangeSceneToFile("res://Game2/Scenes/Menu2.tscn");
	}
	
	public void OnGame3Pressed()
	{
		GD.Print("Game 3 Launches !");
		GetTree().ChangeSceneToFile("res://Game3/Scenes/Menu3.tscn");
	}
	
	public void OnGame4Pressed()
	{
		GD.Print("Game 4 Launches !");
		GetTree().ChangeSceneToFile("res://Game4/Scenes/Menu4.tscn");
	}
	
	public void OnAboutTheGamesPressed()
	{
		GD.Print("Learn More About The Games !");
		GetTree().ChangeSceneToFile("res://AboutGames/Scenes/About.tscn");
	}

	public override void _Process(double delta)
	{
	}
}
