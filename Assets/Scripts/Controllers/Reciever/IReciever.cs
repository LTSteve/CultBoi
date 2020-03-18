
public interface IReciever<T>
{
    T activeValue { get; }
    T defaultValue { get; }
}