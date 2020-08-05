using Priority_Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace zadanie_5
{
    public class Koleje
    {
        private int ilosc_miast;
        private Zdarzenie[] zdarzenia;
        private Zapytanie[] zapytania;
        private List<Stacja>[] lista_incydencji;
        
        public class Data
        {
            public int rok;
            public int miesiac;
            public int dzien;
            public Data(int rok, int miesiac, int dzien)
            {
                this.rok = rok;
                this.miesiac = miesiac;
                this.dzien = dzien;
            }

            public override string ToString()
            {
                return String.Format($"{rok}-{miesiac}-{dzien}");
            }
        }

        public class Zdarzenie
        {
            public Data data_zdarzenia;
            public int nr_miasta1;
            public int nr_miasta2;
            public char rodzaj;
            public int predkosc;
            public int odleglosc;

            public Zdarzenie(Data data_zdarzenia, int nr_miasta1, int nr_miasta2, char rodzaj, int predkosc, int odleglosc = 0)
            {
                this.data_zdarzenia = data_zdarzenia;
                this.nr_miasta1 = nr_miasta1;
                this.nr_miasta2 = nr_miasta2;
                this.rodzaj = rodzaj;
                this.predkosc = predkosc;
                if (rodzaj == 'b')
                {
                    this.odleglosc = odleglosc;
                }
            }

            public override string ToString()
            {
                return String.Format($"{data_zdarzenia} {nr_miasta1} {nr_miasta2} {rodzaj} {predkosc} {odleglosc}");
            }
        }
        public class Zapytanie
        {
            public int nr_miasta1, nr_miasta2, czas;
            public bool czy_zakonczone;
            public string odpowiedz;
            public Stacja2[] tablica_odleglosci;
            public List<int> aktualna_sciezka;
            public Zapytanie(int nr_miasta1, int nr_miasta2, int czas, int rozmiar)
            {
                this.nr_miasta1 = nr_miasta1;
                this.nr_miasta2 = nr_miasta2;
                this.czas = czas;

                czy_zakonczone = false;
            }
            public void zakoncz()
            {
                czy_zakonczone = true;
            }
            public void Setodpowiedz(string odpowiedz)
            {
                this.odpowiedz = odpowiedz;
            }
            public string Getodpowiedz()
            {
                return odpowiedz;
            }
            public override string ToString()
            {
                return String.Format($"{nr_miasta1} {nr_miasta2 } {czas}");
            }
        }
        public class Stacja2
        {
            public int czas, poprzednik;
        }

        public class Stacja
        {
            public int czas, nr_miasta, odleglosc;

            public Stacja(int nr_miasta, int czas, int odleglosc)
            {
                this.czas = czas;
                this.nr_miasta = nr_miasta;
                this.odleglosc = odleglosc;
            }

            public override bool Equals(object obj)
            {
                var stacja = obj as Stacja;
                return stacja != null && nr_miasta == stacja.nr_miasta;

            }
        }

        public Koleje()
        {
            StreamReader s = new StreamReader("C:/Users/marci/Desktop/in.txt");
            string value = s.ReadLine();
            string[] p = value.Split(null);
            ilosc_miast = Convert.ToInt32(p[0]) + 1;
            zdarzenia = new Zdarzenie[Convert.ToInt32(p[1])];
            zapytania = new Zapytanie[Convert.ToInt32(p[2])];
            for (int i = 0; i < zdarzenia.Length; i++)
            {
                value = s.ReadLine();
                p = value.Split(null);
                String[] data = p[0].Split("-");
                Data dataa = new Data(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]));
                if (p[1] == "b")
                    zdarzenia[i] = new Zdarzenie(dataa, Convert.ToInt32(p[2]), Convert.ToInt32(p[3]), Convert.ToChar(p[1]),
                        Convert.ToInt32(p[4]), 60 * Convert.ToInt32(p[5]));
                else
                {
                    zdarzenia[i] = new Zdarzenie(dataa, Convert.ToInt32(p[2]), Convert.ToInt32(p[3]), Convert.ToChar(p[1]),
                    Convert.ToInt32(p[4]));
                }
            }
            for (int i = 0; i < zapytania.Length; i++)
            {
                value = s.ReadLine();
                p = value.Split(null);
                zapytania[i] = new Zapytanie(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]), ilosc_miast);

            }
            lista_incydencji = new List<Stacja>[ilosc_miast];
            for (int i = 0; i < ilosc_miast; i++)
            {
                lista_incydencji[i] = new List<Stacja>();
            }
        }
        public void Wypisz()
        {
            Console.WriteLine($"{ilosc_miast} {zdarzenia.Length} {zapytania.Length}");
            foreach (Zdarzenie z in zdarzenia)
            {
                Console.WriteLine(z);

            }
            foreach (Zapytanie z in zapytania)
            {
                Console.WriteLine(z);
            }
        }
        public void PrzetwarzajZdarzenia()
        {
            foreach (Zdarzenie z in zdarzenia)
            {
                int stary_czas = 0, nowy_czas = 0;
                
                if (z.rodzaj == 'b')
                {

                    Stacja s = new Stacja(z.nr_miasta2, (z.odleglosc / z.predkosc), z.odleglosc);
                    lista_incydencji[z.nr_miasta1].Add(s);
                  
                    s = new Stacja(z.nr_miasta1, (z.odleglosc / z.predkosc), z.odleglosc);
                    lista_incydencji[z.nr_miasta2].Add(s);
                    
                }
                else
                {

                    foreach (Stacja s in lista_incydencji[z.nr_miasta1])
                    {
                        if (s.nr_miasta == z.nr_miasta2)
                        {
                            stary_czas = s.czas;
                            s.czas = s.odleglosc / z.predkosc;
                           
                            nowy_czas = s.czas;
                            break;
                        }
                    }
                    foreach (Stacja s in lista_incydencji[z.nr_miasta2])
                    {
                        if (s.nr_miasta == z.nr_miasta1)
                        {
                            s.czas = s.odleglosc / z.predkosc;
                            
                            break;
                        }
                    }
                }
                int licznik = 0;
                foreach (Zapytanie zap in zapytania)
                {
                    
                    if (zap.czy_zakonczone == false)
                    {
                        licznik++;
                        
                        if (lista_incydencji[zap.nr_miasta1].Count != 0 && lista_incydencji[zap.nr_miasta2].Count != 0)
                        {

                            
                            if (zap.tablica_odleglosci != null && zap.tablica_odleglosci[zap.nr_miasta2].czas
                                < zap.tablica_odleglosci[z.nr_miasta1].czas &&
                                zap.tablica_odleglosci[zap.nr_miasta2].czas
                                < zap.tablica_odleglosci[z.nr_miasta2].czas)//jesli odleglosc do punktow jest za duza
                            {
                                
                                continue;
                            }
                            else if (zap.aktualna_sciezka != null && ((zap.aktualna_sciezka.Contains(z.nr_miasta2) && zap.tablica_odleglosci[z.nr_miasta2].poprzednik == z.nr_miasta1)
                                    || (zap.aktualna_sciezka.Contains(z.nr_miasta1) && zap.tablica_odleglosci[z.nr_miasta1].poprzednik == z.nr_miasta2)))
                            {//jesli punkty juz sa na sciezce
                                
                                int roznica_czasu = stary_czas - nowy_czas;
                                zap.tablica_odleglosci[zap.nr_miasta2].czas -= roznica_czasu;
                                if (zap.tablica_odleglosci[zap.nr_miasta2].czas <= zap.czas)
                                {
                                    zap.czy_zakonczone = true;
                                    zap.odpowiedz = z.data_zdarzenia.ToString();

                                }
                            }

                            else
                            {
                                zap.tablica_odleglosci = Dijkstra(zap.nr_miasta1, zap.nr_miasta2);
                                int poprzedni = zap.tablica_odleglosci[zap.nr_miasta2].poprzednik;
                                zap.aktualna_sciezka = new List<int>();
                                zap.aktualna_sciezka.Add(zap.nr_miasta2);
                                while (poprzedni != 0)
                                {
                                    zap.aktualna_sciezka.Add(poprzedni);
                                    poprzedni = zap.tablica_odleglosci[poprzedni].poprzednik;
                                }
                                if (zap.tablica_odleglosci[zap.nr_miasta2].czas <= zap.czas)
                                {
                                    zap.czy_zakonczone = true;
                                    zap.odpowiedz = z.data_zdarzenia.ToString();

                                }
                            }

                        }
                    }
                }
                if (licznik == 0) break;
            }
            foreach (Zapytanie zap in zapytania)
            {
                if (zap.czy_zakonczone == false)
                {
                    zap.odpowiedz = "NIE";
                }
            }
        }
        public void WypiszWynik()
        {
            foreach (Zapytanie zap in zapytania)
            {
                Console.WriteLine(zap.odpowiedz);
            }
        }
        private Stacja2[] Dijkstra(int miastopocz, int miastokon)
        {
            
            SimplePriorityQueue<int> kolejka = new SimplePriorityQueue<int>();
            bool[] odwiedzone = new bool[ilosc_miast];
            bool check = false;
            Stacja2[] tablica_odl = new Stacja2[ilosc_miast];
            
            for (int i = 0; i < ilosc_miast; i++)
            {
                if (lista_incydencji[i].Count != 0)
                {
                    odwiedzone[i] = true;
                    check = true;

                }
                tablica_odl[i] = new Stacja2();
                tablica_odl[i].czas = int.MaxValue;
            }
            if (check == false) return null;
            tablica_odl[miastopocz].czas = -5;
            kolejka.Enqueue(miastopocz, -5);
            
            odwiedzone[miastopocz] = false;
            while (kolejka.Count != 0)
            {
                int aktualne = kolejka.First;
                kolejka.Dequeue();
                foreach (Stacja s in lista_incydencji[aktualne])
                {
                    if (tablica_odl[aktualne].czas + s.czas +5
                        < tablica_odl[s.nr_miasta].czas)
                    {
                        tablica_odl[s.nr_miasta].czas = tablica_odl[aktualne].czas + s.czas + 5;
                        tablica_odl[s.nr_miasta].poprzednik = aktualne;
                        
                        if (odwiedzone[s.nr_miasta] == true)
                        {
                            odwiedzone[s.nr_miasta] = false;
                            kolejka.Enqueue(s.nr_miasta, tablica_odl[s.nr_miasta].czas);


                        }
                        else
                        {
                            kolejka.TryUpdatePriority(s.nr_miasta, tablica_odl[s.nr_miasta].czas);
                        }
                    }
                   

                }


            }
            
            return tablica_odl;
        }
      
    }
}
