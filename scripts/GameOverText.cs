using Godot;
using System;

public partial class GameOverText : Label
{
	[Export]
	private GameState state { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Visible = state.lostGame;
	}
}
