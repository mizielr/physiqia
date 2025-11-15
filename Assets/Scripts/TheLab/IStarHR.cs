namespace Physiqia.TheLab
{
    public interface IStarHR
    {
        string HR { get; }
        string Bayer { get; }
        string Name { get; }
        double? VisualMagnitude { get; }
        string AbsoluteMagnitude { get; }
        string DistanceParsec { get; }
        string DistanceAl { get; }
        string SpectralType { get; }
    }
}
