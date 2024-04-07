using CsvHelper.Configuration.Attributes;

namespace AC2TractamentDeFitxers
{
    public class Gestor
    {
        public int Any {  get; set; }

        [Name("Codi comarca")]
        public int CodiComarca { get; set; }
        public string Comarca { get; set; }

        [Name("Població")]
        public int Poblacio { get; set; }

        [Name("Domèstic xarxa")]
        public  int DomesticXarxa { get; set; }

        [Name("Activitats econòmiques i fonts pròpies")]
        public int ActiEconom { get; set; }
        public int Total {  get; set; }

        [Name("Consum domèstic per càpita")]
        public double ConsDomCap {  get; set; }

        public Gestor(int any, int codiComarca, string comarca, int poblacio, int domesticXarxa, int actiEconom, int total, double consDomCap)
        {
            Any = any;
            CodiComarca = codiComarca;
            Comarca = comarca;
            Poblacio = poblacio;
            DomesticXarxa = domesticXarxa;
            ActiEconom = actiEconom;
            Total = total;
            ConsDomCap = consDomCap;
        }

        public Gestor() { }

    }
}
