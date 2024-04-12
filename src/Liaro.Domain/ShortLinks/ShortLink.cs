namespace Liaro.Domain.ShortLinks;


public class ShortLink : BaseClass, IEntity
{
    public required string Source { get; set; }
    public required string Target { get; set; }
    public int VisitedCount { get; set; } = 0;
    public ShortLinkType Type { get; set; }
    public int CreatorUserId { get; set; }
}