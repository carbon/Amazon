namespace Amazon.Scheduling;

public interface IException
{
    bool IsTransient { get; }
}