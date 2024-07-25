namespace QuoteList.Extensions;

public static class SymmetricThickness
{
    public static Thickness Symmetric(this Thickness thickness, double value)
    {
        return new Thickness
        {
            Left = value,
            Right = value
        };
    }
}