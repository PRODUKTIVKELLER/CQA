
namespace Produktivkeller.CodeQualityAssurance.Editor.Logging
{
    public interface IOutput
    {
        void Write(LogLevel logLevel, string message);
    }
}

