using UnityEngine;

public static class UnlockActionFactory
{
    public static ParticleSystem OpenParticle;
    
    public static IUnlockAction Create(UnlockType type)
    {
        switch (type)
        {
            case UnlockType.Open: return new OpenUnlockAction(OpenParticle);
            case UnlockType.Upgrade: return new UpgradeUnlockAction();
            case UnlockType.Employment: return new EmploymentUnlockAction();
            default: throw new System.ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}