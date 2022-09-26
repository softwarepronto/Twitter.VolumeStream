// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Tests.TestUtilities
{
    public class TweatArchive
    {
        public const string TweetArchiveFolderName = "TweetArchive";

        protected static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().Location;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);

                if (path == null)
                {
                    throw new NullException(
                        $"Null returned by Uri.UnescapeDataString({uri.Path})");
                }

                var assemblyDirectory = Path.GetDirectoryName(path);

                if (assemblyDirectory == null)
                {
                    throw new NullException(
                        $"Null returned by Path.GetDirectoryName({path})");
                }

                return assemblyDirectory;
            }
        }

        protected static string TweetArchiveFolderPath => 
                            Path.Combine(AssemblyDirectory, TweetArchiveFolderName);
    }
}
