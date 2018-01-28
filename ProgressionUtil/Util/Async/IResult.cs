namespace Progression.Util.Async
{
    public interface IResult<out T> : IResult
    {
        new T Result { get;}
    }
    public interface IResult
    {
        bool IsCompleted { get;}
        object Result { get;}
    }
}