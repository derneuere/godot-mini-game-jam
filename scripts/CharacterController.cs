using Godot;
using System;

public partial class CharacterController : CharacterBody2D
{
	[Export]
	public GameState state;
	
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	
	[Export]
	public double timeoutThreshold = 3.0;
	public double timeout = 0.0;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		
		if(Velocity.X == 0){
			if(timeout >= timeoutThreshold){
				state.looseGame();
			}
			timeout += delta;
		}
		else{
			timeout = 0.0;
		}
		
		if(GlobalPosition.Y > 1000){
			state.looseGame();
		}		

		Velocity = velocity;
		MoveAndSlide();
	}
}
