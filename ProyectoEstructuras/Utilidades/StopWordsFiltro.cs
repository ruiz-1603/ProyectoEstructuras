﻿using BuscadorIndiceInvertido.Ordenamientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscadorIndiceInvertido.Utilidades
{
    internal class StopWordsFiltro
    {
        private string[] StopWords = new string[] {
                "a", "al", "algo", "algunas", "algunos", "ante", "antes", "como", "con",
                "contra", "cual", "cuando", "de", "del", "desde", "donde", "durante",
                "e", "el", "ella", "ellas", "ellos", "en", "entre", "era", "erais", "eran",
                "eras", "es", "esa", "esas", "ese", "eso", "esos", "esta", "estaba",
                "estabais", "estaban", "estabas", "estad", "estada", "estadas", "estado",
                "estados", "estais", "estan", "estar", "estaremos", "estaria", "estarias",
                "este", "esto", "estos", "estoy", "fue", "fueron", "fui", "fuimos", "ha",
                "habeis", "haber", "habiamos", "habia", "habias", "han", "has", "hasta",
                "hay", "haya", "he", "hemos", "hice", "hicieron", "hizo", "hoy", "la",
                "las", "le", "les", "lo", "los", "me", "mi", "mis", "mas", "nada", "no",
                "nos", "nosotros", "o", "os", "otra", "otras", "otro", "otros", "para",
                "pero", "poco", "por", "porque", "que", "quien", "quienes", "se",
                "sea", "sean", "ser", "si", "sin", "sobre", "sois", "son", "soy", "su",
                "sus", "suya", "suyas", "suyo", "suyos", "te", "tiene", "tienes", "todo",
                "todos", "tu", "tus", "un", "una", "uno", "unos", "vos", "vosotros", "y",
                "ya", "yo"
        };

        public StopWordsFiltro()
        {
            Utils.OrdenarAlfab(StopWords, 0, StopWords.Length - 1);
        }

        public DoubleList<string> FiltrarStopWords(string[] palabras)
        {
            DoubleList<string> tokens = new DoubleList<string>();

            foreach (string palabra in palabras)
            {
                if (!IsStopWord(palabra))
                    tokens.Add(palabra);
            }

            return tokens;
        }

        private bool IsStopWord(string palabra)
        {
            // BinarySearch devuelve -1 si no encuentra el elemento
            return Utils.BusquedaBinaria(palabra, StopWords) != -1;
        }
    }
}
