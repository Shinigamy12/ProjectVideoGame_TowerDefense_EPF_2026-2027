namespace _Project.Scripts.Core.Placement
{   
    // Implémentée par les défenses posées, lue par la grille et le calcul du chemin
    public interface IGridOccupant
    {
        bool IsBlockingPath { get; }
    }
}