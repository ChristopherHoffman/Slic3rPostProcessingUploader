using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slic3rPostProcessingUploader.Services
{
    internal interface IBrowser
    {
        void Open(string url);
    }
}
