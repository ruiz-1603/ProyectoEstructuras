using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Utilidades;
using System;

namespace BuscadorIndiceInvertido.Index
{
    // Clase para almacenar la palabra y su frecuencia global
    internal class TerminoFrecuencia
    {
        public string Termino { get; set; }
        public int Frecuencia { get; set; }
    }

    internal class Zipf
    {
        // Método principal para filtrar el vocabulario
        public string[] FiltrarVocabulario(DoubleList<Doc> docs, double percentil)
        {
            // 1. Obtener todas las palabras de los documentos
            DoubleList<string> todasPalabras = ObtenerPalabras(docs);

            // 2. Crear un vocabulario único y ordenado alfabéticamente
            string[] vocabularioUnico = CrearVocabularioUnico(todasPalabras);

            // 3. Calcular las frecuencias globales de cada palabra
            int[] frecuenciasGlobales = CalcularFrecuenciasGlobales(docs, vocabularioUnico);

            // 4. Crear un array de TerminoFrecuencia para ordenar
            TerminoFrecuencia[] listaFrecuencias = new TerminoFrecuencia[vocabularioUnico.Length];
            for (int i = 0; i < vocabularioUnico.Length; i++)
            {
                listaFrecuencias[i] = new TerminoFrecuencia { Termino = vocabularioUnico[i], Frecuencia = frecuenciasGlobales[i] };
            }

            // 5. Ordenar el array por frecuencia descendente
            Array.Sort(listaFrecuencias, (x, y) => y.Frecuencia.CompareTo(x.Frecuencia));

            // 6. Filtrar el vocabulario
            int totalWords = listaFrecuencias.Length;
            int wordsToRemove = (int)(totalWords * percentil);

            DoubleList<string> vocabularioFiltrado = new DoubleList<string>();
            for (int i = wordsToRemove; i < totalWords; i++)
            {
                // Omitir palabras con frecuencia <= 1
                if (listaFrecuencias[i].Frecuencia > 1)
                {
                    vocabularioFiltrado.Add(listaFrecuencias[i].Termino);
                }
            }

            // 7. Devolver el vocabulario filtrado como un array
            string[] resultado = new string[vocabularioFiltrado.Count];
            vocabularioFiltrado.CopyTo(resultado, 0);
            return resultado;
        }

        private DoubleList<string> ObtenerPalabras(DoubleList<Doc> docs)
        {
            DoubleList<string> tokens = new DoubleList<string>();
            foreach (Doc doc in docs)
            {
                foreach (string token in doc.tokens)
                {
                    tokens.Add(token);
                }
            }
            return tokens;
        }

        private string[] CrearVocabularioUnico(DoubleList<string> todasPalabras)
        {
            string[] arr = new string[todasPalabras.Count];
            todasPalabras.CopyTo(arr, 0);
            Array.Sort(arr, StringComparer.Ordinal);
            
            if (arr.Length == 0) return arr;

            int j = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                if (!arr[i].Equals(arr[j], StringComparison.Ordinal))
                {
                    j++;
                    arr[j] = arr[i];
                }
            }

            string[] palabrasUnicas = new string[j + 1];
            for (int k = 0; k <= j; k++)
            {
                palabrasUnicas[k] = arr[k];
            }

            return palabrasUnicas;
        }

        private int[] CalcularFrecuenciasGlobales(DoubleList<Doc> docs, string[] vocabularioUnico)
        {
            int[] frecuencias = new int[vocabularioUnico.Length];
            foreach (var doc in docs)
            {
                foreach (var token in doc.tokens)
                {
                    int index = Array.BinarySearch(vocabularioUnico, token, StringComparer.Ordinal);
                    if (index >= 0)
                    {
                        frecuencias[index]++;
                    }
                }
            }
            return frecuencias;
        }
    }
}