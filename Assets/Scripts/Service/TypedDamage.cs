namespace PlateformSurvivor.Service
{
    public class TypedDamage
    {
        public string type;
        public float value;

        public TypedDamage(string newType, float newValue)
        {
            type = newType;
            value = newValue;
        }
    }
}