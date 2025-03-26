[System.Serializable]
public class AttackPattern
{
    public string animationTriggerName; // The trigger name for the animation
    public float damage; // How much damage this attack deals
    public float attackDelay; // Delay before the attack happens (to sync with animation)
}