using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Services
{
    public interface IFileHelper
    {
        string LocalFilePath(string fileName);
    }
}
