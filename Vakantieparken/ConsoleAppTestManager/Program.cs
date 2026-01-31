using VakantieparkenBL.Interfaces;
using VakantieparkenBL.Managers;
using VakantieparkenBL.Model;
using VakantieparkenDL_File;
using VakantieparkenDL_SQL;

namespace ConsoleAppTestManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string conn = @"Data Source=Gaith\SQLEXPRESS01;Initial Catalog=DB_Vakantieparken;Integrated Security=True;Trust Server Certificate=True";

            string parkenFilePath = @"C:\Users\Gaith Alsahaf\Desktop\HoGent\Graduaat\2de jaar\Sem 1\Gevorderd 1\Eindopdracht\Opgave\parkData\parken.txt";
            string faciliteitenFilePath = @"C:\Users\Gaith Alsahaf\Desktop\HoGent\Graduaat\2de jaar\Sem 1\Gevorderd 1\Eindopdracht\Opgave\parkData\faciliteiten.txt";
            string klantenFilePath = @"C:\Users\Gaith Alsahaf\Desktop\HoGent\Graduaat\2de jaar\Sem 1\Gevorderd 1\Eindopdracht\Opgave\parkData\klanten_park.txt";
            string huizenFilePath = @"C:\Users\Gaith Alsahaf\Desktop\HoGent\Graduaat\2de jaar\Sem 1\Gevorderd 1\Eindopdracht\Opgave\parkData\huizen.txt";
            string reservatiesFilePath = @"C:\Users\Gaith Alsahaf\Desktop\HoGent\Graduaat\2de jaar\Sem 1\Gevorderd 1\Eindopdracht\Opgave\parkData\reservaties.txt";
            string Park_faciliteitFilePath = @"C:\Users\Gaith Alsahaf\Desktop\HoGent\Graduaat\2de jaar\Sem 1\Gevorderd 1\Eindopdracht\Opgave\parkData\parken_faciliteiten.txt";
            string park_huizenFilePath = @"C:\Users\Gaith Alsahaf\Desktop\HoGent\Graduaat\2de jaar\Sem 1\Gevorderd 1\Eindopdracht\Opgave\parkData\park_huizen.txt";
            string huis_reservatiesFilePath = @"C:\Users\Gaith Alsahaf\Desktop\HoGent\Graduaat\2de jaar\Sem 1\Gevorderd 1\Eindopdracht\Opgave\parkData\huis_reservaties.txt";

            IFileProcessor fileProcessor = new FileProcessor();
            IVakantieparkenRepository vakantieparkenRepository = new VakantieparkenRepository(conn);
            DataUploadManager dataUploadManager = new DataUploadManager(fileProcessor, vakantieparkenRepository);
            dataUploadManager.UploadData(faciliteitenFilePath, huis_reservatiesFilePath, huizenFilePath, klantenFilePath, park_huizenFilePath, parkenFilePath, Park_faciliteitFilePath, reservatiesFilePath);

            Console.WriteLine("Great job!");
        }
    }
}
