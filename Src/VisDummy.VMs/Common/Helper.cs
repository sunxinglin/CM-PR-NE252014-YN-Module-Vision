using System.IO;
using VM.Core;

namespace VisDummy.VMs.Common
{
    public static class VmHelper
    {
        public static bool PreLoadSln(string sln)
        {
            if (!File.Exists(sln))
            {
                return false;
            }
            else
            {
                VmSolution.Load(sln);
                return true;
            }
        }

        public static void DisposeSln()
        {
            VmSolution.Instance?.Dispose();
        }
    }
}
