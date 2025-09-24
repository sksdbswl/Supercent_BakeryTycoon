public static class UnlockActionFactory
{
    public static IUnlockAction Create(UnlockType type)
    {
        switch (type)
        {
            case UnlockType.Open: return new OpenUnlockAction();
            case UnlockType.Upgrade: return new UpgradeUnlockAction();
            case UnlockType.Employment: return new EmploymentUnlockAction();
            default: throw new System.ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}