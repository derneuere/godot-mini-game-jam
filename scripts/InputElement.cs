public class InputElement 
{
	public InputElement(double time, string inputEvent)
	{
		Time = time;
		InputEvent = inputEvent;
	}

	public double Time { get; }
	public string InputEvent { get; }

	public override string ToString() => $"({Time}, {InputEvent})";
}
