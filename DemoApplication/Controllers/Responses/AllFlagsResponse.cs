using Microsoft.Extensions.Primitives;

namespace DemoApplication.Controllers;

public class AllFlagsResponse
{
    public string VisitorCode { get; set; } = "";

    public IEnumerable<FeatureInfo> FeaturesInfo { get; set; } = Enumerable.Empty<FeatureInfo>();

    public struct FeatureInfo
    {
        public string FeatureKey { get; set; }
        public bool Active { get; set; }
    }
}
