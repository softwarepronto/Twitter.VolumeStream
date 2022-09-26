// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Extensions
{
    public static class EntitiesExtensions
    {
        public static IEnumerable<string> GetHashtagNames(this Entities entities)
        {
            return entities.hashtags.Select(h => h.tag);
        }
    }
}
