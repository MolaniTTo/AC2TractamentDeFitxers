using CsvHelper;
using System;
using System.Security.Cryptography.X509Certificates;
namespace AC2TractamentDeFitxers
{
    public class Program
    {
        public static void Main()
        {

            using var reader = new StreamReader("ConsAigua.csv");
            using var csv = new CsvReader (reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Gestor>();

            foreach (var record in records)
            {
                Console.WriteLine (record);
            }

           
        }

        public void PoblacioSuperior2000()
        {

        }
    }
}