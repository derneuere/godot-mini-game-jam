using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{

	// private Dictionary input_queue;
	public Queue<InputElement> inputQueue;
	private double current_time = 0;
	private double run_time = 0;

	[Export]
	public GameState state;
	[Export]
	public double timeoutThreshold = 3.0;
	public double timeout = 0.0;

	[Export]
	private Label timeLabel;

	[Export]
	private bool running = false;
	private bool sliding = false;

	public const float Speed = 160.0f;
	public const float JumpVelocity = -400.0f;

	private InputElement nextInput = null;


	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private Node2D collisionNode;
	

	public override void _Ready()
	{
		collisionNode = (Node2D)GetNode("CollisionShape2D");
		inputQueue = new Queue<InputElement>();
		inputQueue.Enqueue(new InputElement(2.5, "slide_start"));
		inputQueue.Enqueue(new InputElement(5.5, "slide_stop"));
		inputQueue.Enqueue(new InputElement(6.5, "jump"));
	}
	public override void _PhysicsProcess(double delta)
	{
		double old_time = current_time;
		bool jump = false;
		bool slide_start = false;
		bool slide_stop = false;
		current_time += delta;
		timeLabel.Text = String.Format("Time: {0}", current_time);

		if(nextInput == null) 
		{
			if (inputQueue.Count > 0)
				nextInput = inputQueue.Dequeue();
		}
		else
		{
			if ((old_time <= nextInput.Time) && (nextInput.Time < current_time))
			{
				switch(nextInput.InputEvent)
				{
					case "jump":
						jump = true;
						break;
					case "slide_start":
						slide_start = true;
						break;
					case "slide_stop":
						slide_stop = true;
						break;
					default:
						break;
				}
				nextInput = null;
			}
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

		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		// if (Input.IsActionJustPressed("jump") && IsOnFloor())
		if(jump && IsOnFloor())
		{
			GD.Print("jump");
			velocity.Y = JumpVelocity;
			jump = false;
		}

		// Tween tween = GetTree().CreateTween();
		// if (Input.IsActionJustPressed("slide"))
		if(slide_start)
		{
			GD.Print("sliding");
			sliding = true;
			collisionNode.Scale = new Vector2(1.0f, 0.5f);
			slide_start = false;
		// } else if (Input.IsActionJustReleased("slide"))
		} else if (slide_stop)
		{
			GD.Print("stopped sliding");
			sliding = false;
			collisionNode.Scale = Vector2.One;
			slide_stop = false;
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
