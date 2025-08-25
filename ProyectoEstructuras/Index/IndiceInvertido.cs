using BuscadorIndiceInvertido.Base;
using BuscadorIndiceInvertido.Calculos;
using BuscadorIndiceInvertido.Ordenamientos;
using BuscadorIndiceInvertido.Utilidades;

namespace BuscadorIndiceInvertido.Index
{
    internal class IndiceInvertido
    {
        private string[] palabras;
        private double[] IDFValores;
        private DoubleList<(Doc doc, int frec)>[] matrizFrec;
        private int palabrasCount;
        private TFIDFCalculador calculador;

        public IndiceInvertido()
        {
            palabras = new string[0];
            IDFValores = new double[0];
            matrizFrec = new DoubleList<(Doc doc, int freq)>[0];
            palabrasCount = 0;
            calculador = new TFIDFCalculador();
        }

        public void Build(DoubleList<Doc> docs)
        {
            if (docs == null || docs.Count == 0) return;

            int docsTotal = docs.Count;

            DoubleList<string> todasPalabras = ObtenerPalabras(docs);

            string[] arr = todasPalabras.ToArray();
            Array.Sort(arr, StringComparer.Ordinal);

            string[] palabrasUnicas = EliminarDuplicados(arr);

            InicializarAtributos(palabrasUnicas.Length);

            BuildVocab(palabrasUnicas);

            Doc[] docArr = docs.ToArray();
            BuildMatrizFrec(docArr);

            CalcularIDF(docsTotal);
        }

        private void BuildMatrizFrec(Doc[] arr)
        {
            int totalDocs = arr.Length;
            int[,] tempFrecs = new int[palabrasCount, totalDocs];

            // Contar frecuencias
            for (int docIndex = 0; docIndex < totalDocs; docIndex++)
            {
                Doc doc = arr[docIndex];
                foreach (string token in doc.tokens)
                {
                    int wordIndex = Utils.BusquedaBinaria(token, palabras);
                    if (wordIndex >= 0)
                        tempFrecs[wordIndex, docIndex]++;
                }
            }

            // Pasar a DoubleList
            for (int j = 0; j < palabrasCount; j++)
            {
                matrizFrec[j] = new DoubleList<(Doc doc, int freq)>();
                for (int k = 0; k < totalDocs; k++)
                {
                    if (tempFrecs[j, k] > 0)
                    {
                        matrizFrec[j].Add((arr[k], tempFrecs[j, k]));
                    }
                }
            }
        }

        private void CalcularIDF(int totalDocs)
        {
            for (int j = 0; j < palabrasCount; j++)
            {
                int df = matrizFrec[j].Count;
                IDFValores[j] = calculador.CalcularIDF(totalDocs, df);
            }
        }

        // metodos aux
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

        public string[] EliminarDuplicados(string[] arr)
        {
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

        private void InicializarAtributos(int Vocabcount)
        {
            palabras = new string[Vocabcount];
            IDFValores = new double[Vocabcount];
            matrizFrec = new DoubleList<(Doc doc, int freq)>[Vocabcount];
            palabrasCount = Vocabcount;
        }

        private void BuildVocab(string[] palabrasUnicas)
        {
            for (int i = 0; i < palabrasUnicas.Length; i++)
            {
                palabras[i] = palabrasUnicas[i];
            }
        }

        // metodos de acceso
        public DoubleList<(Doc doc, int freq)> GetPostings(string palabra)
        {
            int indice = Utils.BusquedaBinaria(palabra, palabras);
            if (indice >= 0)
                return matrizFrec[indice];

            return new DoubleList<(Doc doc, int freq)>();
        }

        public double GetIDF(string palabra)
        {
            int indice = Utils.BusquedaBinaria(palabra, palabras);
            if (indice >= 0)
                return IDFValores[indice];

            return 0.0;
        }

        public double GetTFIDF(string palabra, int frecuencia)
        {
            double idf = GetIDF(palabra);
            return calculador.CalcularTFIDF(frecuencia, idf);
        }
    }
}
