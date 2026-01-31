using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.Exceptions;
using VakantieparkenBL.Interfaces;

namespace VakantieparkenBL.Managers
{
    public class DataUploadManager
    {
        private IFileProcessor _fileProcessor;
        private IVakantieparkenRepository _vakantieparkenRepository;
        public DataUploadManager(IFileProcessor fileProcessor, IVakantieparkenRepository vakantieparkenRepository)
        {
            _fileProcessor = fileProcessor;
            _vakantieparkenRepository = vakantieparkenRepository;
        }

        public void UploadData(string faciliteitenFilePath, string huis_reservatiesFilePath, string huizenFilePath, string klanten_parkFilePath, string park_huizenFilePath, string parkenFilePath, string parken_faciliteitenFilePath, string reservatiesFilePath)
        {
            try
            {
                var bestandenNaarObjecten = _fileProcessor.LeesBestanden(faciliteitenFilePath, huis_reservatiesFilePath, huizenFilePath, klanten_parkFilePath, park_huizenFilePath, parkenFilePath, parken_faciliteitenFilePath, reservatiesFilePath);
                _vakantieparkenRepository.SchrijfBestanden(bestandenNaarObjecten);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(UploadData), ex);
            }
        }
    }
}
