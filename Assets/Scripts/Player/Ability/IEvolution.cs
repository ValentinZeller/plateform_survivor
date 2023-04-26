namespace PlateformSurvivor.Player.Ability
{
    public interface IEvolution
    {
        public bool IsEvolved { get; set; }
        public void OnEvolution();
    }
}