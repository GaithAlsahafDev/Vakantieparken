using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.Model;

namespace VakantieparkenBL.Interfaces
{
    public interface IFileProcessor
    {
        (List<Park>, List<Klant>) LeesBestanden(string faciliteitenFilePath, string huis_reservatiesFilePath, string huizenFilePath, string klanten_parkFilePath, string park_huizenFilePath, string parkenFilePath, string parken_faciliteitenFilePath, string reservatiesFilePath);
    }
}
