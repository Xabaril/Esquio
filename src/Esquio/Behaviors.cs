namespace Esquio
{
    public enum OnErrorBehavior : short
    {
        Throw = 0,
        SetAsNotActive = 1,
        SetAsActive = 2
    }

    public enum NotFoundBehavior : short
    {
        SetAsNotActive = 1,
        SetAsActive = 2
    }
}
