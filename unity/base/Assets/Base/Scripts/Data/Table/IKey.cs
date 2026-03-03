
public interface IKey
{

}

public interface IKey<T> : IKey
{
    T Key { get; }
}
