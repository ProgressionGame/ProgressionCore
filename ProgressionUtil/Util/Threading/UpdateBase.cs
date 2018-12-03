namespace Progression.Util.Threading
{
    public abstract class UpdateBase<TInterface, TUpdate>
        where TInterface : ThreadingInterface<TInterface, TUpdate> 
        where TUpdate : UpdateBase<TInterface, TUpdate>
    {
        public abstract void Execute(TInterface on);
    }
}