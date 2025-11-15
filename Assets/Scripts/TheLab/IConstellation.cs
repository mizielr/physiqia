namespace Physiqia.TheLab
{
    public interface IConstellation
    {
        ConstellationStar[] ConstellationStars { get; }
        int[][] Connections { get; }
        ConstellationMetadata Metadata { get; }
    }
}
