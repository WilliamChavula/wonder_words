namespace FormFields.lib;

public interface IInput
{
    bool IsInputValid { get; }
    bool IsInputPure { get; }
}