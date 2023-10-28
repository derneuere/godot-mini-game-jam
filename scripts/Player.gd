extends Area2D

var input_queue = {}
var current_time = 0
var run_time

# Called when the node enters the scene tree for the first time.
func _ready():
	# get input queue from game manager
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	current_time += delta
	var time_slot = int(current_time / 1000)
	if (input_queue.has(time_slot)):
		var action = input_queue[time_slot]
		input_queue.erase(time_slot)
		match(action):
			"slide":
				slide()
			
	pass

func slide():
	# play slide animation
	# make hitbox smaller
	# check for proper ground
	pass
	
func die():
	pass
	
func start():
	pass
	
func finish():
	pass
	
	
