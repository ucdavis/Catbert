namespace Catbert4.Core.Domain
{
    public interface ITrackable
    {
        bool IsTracked();

        bool ArePropertiesTracked();
    }
}