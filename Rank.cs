namespace ItemLuck;

public enum Tier
{
    S,
    A,
    B,
    C,
    D,
    F,
    Unspecified
}

public static class TierExtensions
{
    public static float ToMultiplier(this Tier tier)
    {
        return tier switch
        {
            Tier.S => 1,
            Tier.A => 0.6f,
            Tier.B => 0.2f,
            Tier.C => -0.2f,
            Tier.D => -0.6f,
            Tier.F => -1,
            Tier.Unspecified => 1f
        };
    }
}

public class Rank<T>
{
    public T Def { get; }
    public Tier Tier { get; }

    public Rank(T index, Tier tier)
    {
        Def = index;
        Tier = tier;
    }
}
