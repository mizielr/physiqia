namespace Physiqia.TheLab.Constellation
{
    public interface IConstellation
    {
        ConstellationStar[] ConstellationStars { get; }
        int[][] Connections { get; }
        ConstellationMetadata Metadata { get; }
    }
}
