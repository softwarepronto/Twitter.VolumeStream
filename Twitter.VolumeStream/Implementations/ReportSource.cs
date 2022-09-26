// Licensed to the softwarepronto.com blog under the GNU General Public License.

namespace Twitter.VolumeStream.Implementations
{
    public class ReportSource : IReportSource
    {
        static ReportSource()
        {
            Console.OutputEncoding = Encoding.Unicode;
        }

        public void Write(string output)
        {
            Console.WriteLine(output);
        }
    }
}
