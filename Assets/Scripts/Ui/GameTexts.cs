using UnityEngine;

public static class GameTexts {
    public static string NewBonus(BonusType bType, float val) {
        string str = "";
        str += "Boss defeated! ";
        if (bType == BonusType.ResourceGainFlat) {
            int intVal = (int)val;
            str += "You will gain ";
            if (intVal == 1) 
                str += "1 extra unit ";
            else
                str += $"{intVal} extra units ";
            str += "of resources when a worker returns trash to the base!";
        } else if (bType == BonusType.BaseToughness) {
            str += $"Your base gains an additional {val*100f} % base damage resistance!";
        }
        return str;
    }
}