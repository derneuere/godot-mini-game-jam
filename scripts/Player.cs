using Godot;
using Godot.Collections;
using System;

public partial class Player : CharacterBody2D
{

	private Dictionary input_queue;
	private double current_time = 0;
	private double run_time = 0;

	[Export]
	private bool running = false;
	private bool sliding = false;

	public const float Speed = 160.0f;
	public const float JumpVelocity = -400.0f;


	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private Node2D collisionNode;
	//private Dictionary input_queue;
	private float current_time = 0;
	private float run_time = 0;

	public override void _Ready()
	{
		collisionNode = (Node2D)GetNode("CollisionShape2D");
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			GD.Print("jump");
			velocity.Y = JumpVelocity;
		}

		if (Input.IsActionJustPressed("slide"))
		{
			GD.Print("sliding");
			sliding = true;
			collisionNode.Scale = new Vector2(1.0f, 0.5f);
		} else if (Input.IsActionJustReleased("slide"))
		{
			GD.Print("stopped sliding");
			sliding = false;
			collisionNode.Scale = Vector2.One;
		}


		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		// Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		// if (direction != Vector2.Zero)
		// {
		// 	velocity.X = direction.X * Speed;
		// }
		// else
		// {
		// 	velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		// }

		// GD.Print("Speed: " + (Speed * (float)delta));
		if (running)
			// velocity.X = Speed * (float)delta;
			velocity.X = Speed;
		else
			velocity.X = Velocity.X;

		// Velocity = Vector2.Right * Speed;
		Velocity = velocity;
		MoveAndSlide();
	}
}
