[System.Serializable]
public class GameBonus {
    public BonusType Type;
    public float Value;

    public GameBonus(BonusType type, float value) {
        Type = type;
        Value = value;
    }
}