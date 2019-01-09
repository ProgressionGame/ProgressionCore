namespace Progression.IO.Encoders.Base
{
    public abstract class DerivedEncoderBase<TType, TRoot>:EncoderBase<TType, TRoot>
    {
        public override bool IsBaseEncoder => false;
    }
}