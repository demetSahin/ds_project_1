using System;

namespace PROJECT_1
{
    class MainClass
    {
        static void Main(string[] args)
        {
            ////////// 1 - a) //////////
            double[,] matrix = CreateMatrix();

            ////////// 1 - b) //////////
            CreateDistanceMatrix(matrix);

            ////////// 2 - a) //////////
            Console.Write("\n\n\n2. sorunun a) şıkkının cevabı için lütfen ENTER'a basın.");
            Console.ReadLine();

            /// Verilen linkten indirilen vserisetinin programımıza tanıtılması.
            string[] training_data = System.IO.File.ReadAllLines(@"/Users/demetsahinyenigul/Desktop/data_banknote_authentication.txt"); //5 adet değer içeren satırlar tek tek bir diziye aktarılıyor           
            ValidateBanknote(training_data);

            ////////// 2 - b) //////////
            Console.Write("\n\n\n2. sorunun b) şıkkının cevabı için lütfen ENTER'a basın.");
            Console.ReadLine();
            KNearestNeighbors(training_data);

            ////////// 2 - c) //////////
            Console.Write("\n\n\n2. sorunun c) şıkkının cevabı için lütfen ENTER'a basın.");
            Console.ReadLine();
            SuccessRate(training_data);

            ////////// 2 - d) //////////
            Console.Write("\n\n\n2. sorunun d) şıkkının cevabı için lütfen ENTER'a basın.");
            Console.ReadLine();
            Listing(training_data);

            Console.ReadLine();
        }

        private static double[,] CreateMatrix()
        {
            Console.Write("Lütfen Öklid uzayının genişliğini giriniz : ");
            double width = double.Parse(Console.ReadLine());

            Console.Write("Lütfen Öklid uzayının yüksekliğini giriniz : ");
            double height = double.Parse(Console.ReadLine());

            Console.Write("Lütfen kaç adet nokta oluşturulacağını belirtiniz : ");
            int n = int.Parse(Console.ReadLine());

            double[,] matrix = new double[n, 2];

            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                matrix[i, 0] = (random.NextDouble()) * width;
                matrix[i, 1] = (random.NextDouble()) * height;
            }

            Console.WriteLine($"\n\n{width}x{height} Öklid uzayındaki rastgele noktalardan oluşan {n}x2 matris :\n");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 2; j++)
                    Console.Write(String.Format("{0:0.0}", matrix[i, j]) + "\t");
                Console.WriteLine("\n");
            }

            return matrix;
        }

        private static void CreateDistanceMatrix(double[,] matrix)
        {
            double[,] distance_matrix = new double[matrix.GetLength(0), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    distance_matrix[i, j] = Math.Sqrt(Math.Pow(matrix[i, 0] - matrix[j, 0], 2) + Math.Pow(matrix[i, 1] - matrix[j, 1], 2));
                }
            }

            Console.WriteLine($"\n\n{distance_matrix.GetLength(0)}x{distance_matrix.GetLength(1)} uzaklık matrisi :\n");

            for (int i = 0; i < distance_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < distance_matrix.GetLength(1); j++)
                    Console.Write(String.Format("{0:0.0}", distance_matrix[i, j]).Replace(",", ".") + "\t");
                Console.WriteLine("\n");
            }
        }

        private static void ValidateBanknote(string[] training_data)
        {
            Console.Write("\n\n\nLütfen banknotun varyans değerini giriniz (örnek : 3.6216, -1.3971, 0.39012, ...) : ");
            double varyans = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.Write("Lütfen banknotun çarpıklık değerini giriniz (örnek : 8.6661, -2.6383, -0.14279, ...) : ");
            double carpiklik = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.Write("Lütfen banknotun basıklık değerini giriniz (örnek : -2.8073, -0.031994, 7.8929, ...) : ");
            double basiklik = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.Write("Lütfen banknotun entropi değerini giriniz (örnek : -0.44699, 0.10645, 0.96765, ...) : ");
            double entropi = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.Write("Lütfen \"k\" değerini giriniz : ");
            int k = int.Parse(Console.ReadLine());

            ///Her bir satırında uzaklık ve tür bilgisini tutan 1372x2'lik matrisin boyutlandırılması.
            double[,] resultMatrix = new double[training_data.Length, 2];

            double[,] kNNMatrix = new double[k, 2];// kx2'lik matris en yakın komşulukların bulunması için yaratılır.

            ///Her bir satırında uzaklık ve tür bilgisini tutan 1372x2'lik matrisin oluşturulması.
            for (int i = 0; i < training_data.Length; i++)
            {
                string[] subs = training_data[i].Split(',');//Verisetinin her bir satırı virgüllerden parçalanarak oluşan 5 değer yeni bir diziye aktarılıyor
                double distance = Math.Sqrt(Math.Pow(double.Parse(subs[0].Replace(".", ",")) - varyans, 2) //1. değer varyans
                    + Math.Pow(double.Parse(subs[1].Replace(".", ",")) - carpiklik, 2) //2. değer çarpıklık
                    + Math.Pow(double.Parse(subs[2].Replace(".", ",")) - basiklik, 2) //3. değer basıklık
                    + Math.Pow(double.Parse(subs[3].Replace(".", ",")) - entropi, 2)); //4. değer entropi
                resultMatrix[i, 0] = distance;
                resultMatrix[i, 1] = double.Parse(subs[4]);//5. değer tür
            }

            ///Bulunan matrisin uzaklık değerlerine göre küçükten büyüğe sıralanması
            double temp1, temp2;
            for (int i = 0; i < resultMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < resultMatrix.GetLength(0) - 1 - i; j++)
                {
                    if (resultMatrix[j, 0] > resultMatrix[j + 1, 0]) // column 1 entry comparison
                    {
                        temp1 = resultMatrix[j, 0];              // swap both column 0 and column 1
                        temp2 = resultMatrix[j, 1];

                        resultMatrix[j, 0] = resultMatrix[j + 1, 0];
                        resultMatrix[j, 1] = resultMatrix[j + 1, 1];

                        resultMatrix[j + 1, 0] = temp1;
                        resultMatrix[j + 1, 1] = temp2;
                    }
                }
            }

            ///// Oluşturulan uzaklık matrisinin yazdırılması
            //for (int i = 0; i < resultMatrix.GetLength(0); i++)
            //{
            //    for (int j = 0; j < 2; j++)
            //        Console.Write(String.Format("{0:0.0}", resultMatrix[i, j]) + " ");
            //    Console.WriteLine();
            //}

            ///Girilen k değeri kadar satır ve her satırında uzaklık ve tür değerini içeren matrisin oluşturulması.
            for (int i = 0; i < k; i++)
            {
                kNNMatrix[i, 0] = resultMatrix[i, 0];
                kNNMatrix[i, 1] = resultMatrix[i, 1];
            }

            ///// k değerine bağlı olarak oluşturulan matrisin yazdırılması
            //for (int i = 0; i < kNNMatrix.GetLength(0); i++)
            //{
            //    for (int j = 0; j < 2; j++)
            //        Console.Write(String.Format("{0:0.0}", kNNMatrix[i, j]) + " ");
            //    Console.WriteLine();
            //}

            int count_real = 0; // Geçerli banknot sayısı
            int count_fake = 0; // Sahte banknot sayısı

            for (int i = 0; i < kNNMatrix.GetLength(0); i++)
            {
                if ((int)kNNMatrix[i, 1] == 0) count_fake += 1;
                else count_real += 1;
            }

            //Console.Write($"Gerçek sayısı: {count_real}\tSahte sayısı : {count_fake}");
            if (count_fake == count_real)
            {
                if ((int)kNNMatrix[0, 1] == 0) Console.Write("Bu banknot sahtedir.");
                else Console.Write("Bu banknot geçerlidir.");
            }
            else if (count_fake > count_real) Console.Write("Bu banknot sahtedir.");
            else Console.Write("Bu banknot geçerlidir.");
        }

        private static void KNearestNeighbors(string[] training_data)
        {
            Console.Write("\n\n\nLütfen banknotun varyans değerini giriniz (örnek : 3.6216, -1.3971, 0.39012, ...) : ");
            double varyans = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.Write("Lütfen banknotun çarpıklık değerini giriniz (örnek : 8.6661, -2.6383, -0.14279, ...) : ");
            double carpiklik = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.Write("Lütfen banknotun basıklık değerini giriniz (örnek : -2.8073, -0.031994, 7.8929, ...) : ");
            double basiklik = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.Write("Lütfen banknotun entropi değerini giriniz (örnek : -0.44699, 0.10645, 0.96765, ...) : ");
            double entropi = double.Parse(Console.ReadLine().Replace(".", ","));

            Console.Write("Lütfen \"k\" değerini giriniz : ");
            int k = int.Parse(Console.ReadLine());

            double[,] resultMatrix = new double[training_data.Length, 2];

            double[,] kNNMatrix = new double[k, 6];

            for (int i = 0; i < training_data.Length; i++)
            {
                string[] subs = training_data[i].Split(',');
                double distance = Math.Sqrt(Math.Pow(double.Parse(subs[0].Replace(".", ",")) - varyans, 2)
                    + Math.Pow(double.Parse(subs[1].Replace(".", ",")) - carpiklik, 2)
                    + Math.Pow(double.Parse(subs[2].Replace(".", ",")) - basiklik, 2)
                    + Math.Pow(double.Parse(subs[3].Replace(".", ",")) - entropi, 2));
                resultMatrix[i, 0] = distance;
                resultMatrix[i, 1] = i;
            }

            ///Bulunan matrisin uzaklık değerlerine göre küçükten büyüğe sıralanması
            double temp1, temp2;
            for (int i = 0; i < resultMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < resultMatrix.GetLength(0) - 1 - i; j++)
                {
                    if (resultMatrix[j, 0] > resultMatrix[j + 1, 0]) 
                    {
                        temp1 = resultMatrix[j, 0];              
                        temp2 = resultMatrix[j, 1];

                        resultMatrix[j, 0] = resultMatrix[j + 1, 0];
                        resultMatrix[j, 1] = resultMatrix[j + 1, 1];

                        resultMatrix[j + 1, 0] = temp1;
                        resultMatrix[j + 1, 1] = temp2;
                    }
                }
            }

            for (int i = 0; i < k; i++)
            {
                string[] subs = training_data[(int)resultMatrix[i, 1]].Split(',');
                kNNMatrix[i, 0] = double.Parse(subs[0].Replace(".", ","));
                kNNMatrix[i, 1] = double.Parse(subs[1].Replace(".", ","));
                kNNMatrix[i, 2] = double.Parse(subs[2].Replace(".", ","));
                kNNMatrix[i, 3] = double.Parse(subs[3].Replace(".", ","));
                kNNMatrix[i, 4] = resultMatrix[i, 0];
                kNNMatrix[i, 5] = double.Parse(subs[4].Replace(".", ","));
            }

            int count_real = 0; // Geçerli banknot sayısı
            int count_fake = 0; // Sahte banknot sayısı
            string banknote_class;

            for (int i = 0; i < kNNMatrix.GetLength(0); i++)
            {
                if ((int)kNNMatrix[i, 5] == 0) count_fake += 1;
                else count_real += 1;
            }

            if (count_fake == count_real)
            {
                if ((int)kNNMatrix[0, 5] == 0) banknote_class = "0";
                else banknote_class = "1";
            }
            else if (count_fake > count_real) banknote_class = "0";
            else banknote_class = "1";

            Console.Write("\n\n\nÖzellikleri belirtilen banknota ait değerler :\nVaryans \tÇarpıklık \tBasıklık \tEntropi \tTahminlenen Tür \n");
            Console.Write($"{varyans.ToString().Replace(",", ".")}\t\t{carpiklik.ToString().Replace(",", ".")}" +
                $"\t\t{basiklik.ToString().Replace(",", ".")}\t\t{entropi.ToString().Replace(",", ".")}\t\t{banknote_class}\n");
            if (banknote_class == "0") Console.Write("\n\nBu banknot sahtedir.");
            else Console.Write("\n\nBu banknot geçerlidir.");

            Console.Write($"\n\n\n{k} adet en yakın komşuluktaki banknotlara ait değerler :\nVaryans \tÇarpıklık \tBasıklık \tEntropi \tUzaklık \tTür \n");
            for (int i = 0; i < kNNMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < kNNMatrix.GetLength(1); j++)
                {
                    if (j == 5) Console.Write((int)kNNMatrix[i, j] + "\t\t");
                    else Console.Write(String.Format("{0:0.0}", kNNMatrix[i, j]).Replace(",", ".") + "\t\t");
                    //else Console.Write(String.Format("{0:0.0}", kNNMatrix[i, j]) + "\t\t");
                }
                Console.WriteLine("\n");
            }

        }

        private static void SuccessRate(string[] training_data)
        {
            Console.Write("Lütfen \"k\" değerini giriniz : ");
            int k = int.Parse(Console.ReadLine());

            string[] training_data_test = new string[200];

            int first_1_index = 0;
            int dogru_tahmin = 0;
            double basari_orani = 0;

            for (int i = 0; i < training_data.Length; i++)
            {
                string[] subs = training_data[i].Split(',');
                if (subs[4].Equals("1"))
                {
                    first_1_index = i;
                    break;
                }
            }

            Array.Copy(training_data, first_1_index - 100, training_data_test, 0, 100);
            Array.Copy(training_data, training_data.Length - 100, training_data_test, 100, 100);

            for (int i = 0; i < training_data_test.Length; i++)
            {
                double[,] result_matrix = new double[training_data.Length, 2];

                double[,] kNNMatrix = new double[k, 6];

                string[] subs_test = training_data_test[i].Split(',');

                for (int j = 0; j < training_data.Length; j++)
                {
                    string[] subs = training_data[j].Split(',');
                    double distance = Math.Sqrt(Math.Pow(double.Parse(subs[0].Replace(".", ",")) - double.Parse(subs_test[0].Replace(".", ",")), 2)
                        + Math.Pow(double.Parse(subs[1].Replace(".", ",")) - double.Parse(subs_test[1].Replace(".", ",")), 2)
                        + Math.Pow(double.Parse(subs[2].Replace(".", ",")) - double.Parse(subs_test[2].Replace(".", ",")), 2)
                        + Math.Pow(double.Parse(subs[3].Replace(".", ",")) - double.Parse(subs_test[3].Replace(".", ",")), 2));
                    result_matrix[j, 0] = distance;
                    result_matrix[j, 1] = j;
                }

                ///Bulunan matrisin uzaklık değerlerine göre küçükten büyüğe sıralanması
                double temp1, temp2;
                for (int ix = 0; ix < result_matrix.GetLength(0); ix++)
                {
                    for (int j = 0; j < result_matrix.GetLength(0) - 1 - ix; j++)
                    {
                        if (result_matrix[j, 0] > result_matrix[j + 1, 0]) // column 1 entry comparison
                        {
                            temp1 = result_matrix[j, 0];              // swap both column 0 and column 1
                            temp2 = result_matrix[j, 1];

                            result_matrix[j, 0] = result_matrix[j + 1, 0];
                            result_matrix[j, 1] = result_matrix[j + 1, 1];

                            result_matrix[j + 1, 0] = temp1;
                            result_matrix[j + 1, 1] = temp2;
                        }
                    }
                }

                for (int ix = 0; ix < k; ix++)
                {
                    string[] subs = training_data[(int)result_matrix[ix, 1]].Split(',');
                    kNNMatrix[ix, 0] = double.Parse(subs[0].Replace(".", ","));
                    kNNMatrix[ix, 1] = double.Parse(subs[1].Replace(".", ","));
                    kNNMatrix[ix, 2] = double.Parse(subs[2].Replace(".", ","));
                    kNNMatrix[ix, 3] = double.Parse(subs[3].Replace(".", ","));
                    kNNMatrix[ix, 4] = result_matrix[ix, 0];
                    kNNMatrix[ix, 5] = double.Parse(subs[4].Replace(".", ","));
                }

                int count_real = 0; // Geçerli banknot sayısı
                int count_fake = 0; // Sahte banknot sayısı
                string banknote_class;

                for (int ix = 0; ix < kNNMatrix.GetLength(0); ix++)
                {
                    if ((int)kNNMatrix[ix, 5] == 0) count_fake += 1;
                    else count_real += 1;
                }

                if (count_fake == count_real)
                {
                    if ((int)kNNMatrix[0, 5] == 0) banknote_class = "0";
                    else banknote_class = "1";
                }
                else if (count_fake > count_real) banknote_class = "0";
                else banknote_class = "1";

                if (subs_test[4].Equals(banknote_class)) dogru_tahmin += 1;

                Console.Write("\n\n\nÖzellikleri belirtilen banknota ait değerler :\nVaryans \tÇarpıklık \tBasıklık \tEntropi \tGerçek Tür \tTahminlenen Tür\n");
                Console.Write($"{String.Format("{0:0.0000}", subs_test[0])}\t\t{String.Format("{0:0.0000}", subs_test[1])}" +
                    $"\t\t{String.Format("{0:0.0000}", subs_test[2])}\t\t{String.Format("{0:0.0000}", subs_test[3])}" +
                    $"\t\t{subs_test[4]}\t\t{banknote_class}\n");
                if (banknote_class == "0") Console.Write("\n\nBu banknot sahtedir.");
                else Console.Write("\n\nBu banknot geçerlidir.");

                ///Test verisetindeki x. banknot için ...
                Console.Write($"\n\n\n{k} adet en yakın komşuluktaki banknotlara ait değerler :\nVaryans \tÇarpıklık \tBasıklık \tEntropi \tUzaklık \tTür \n");
                for (int ix = 0; ix < kNNMatrix.GetLength(0); ix++)
                {

                    for (int j = 0; j < kNNMatrix.GetLength(1); j++)
                    {
                        if (j == 5) Console.Write((int)kNNMatrix[ix, j]);
                        else Console.Write(String.Format("{0:0.00}", kNNMatrix[ix, j]).Replace(",", ".") + "\t\t");
                        //else Console.Write(String.Format("{0:0.0}", kNNMatrix[i, j]) + "\t\t");
                    }
                    Console.WriteLine("\n");
                }

                Console.Write("\n\n-------------------------------------------------");
            }

            basari_orani = dogru_tahmin / training_data_test.Length;

            Console.Write($"\n\n\nBaşarı oranı : {basari_orani}");
        }

        private static void Listing(string[] training_data)
        {
            Console.WriteLine("Varyans\tÇarpıklık\tBasıklık\tEntropi\tTür");
            for (int i = 0; i < training_data.Length; i++)
            {
                string[] subs = training_data[i].Split(',');
                Console.WriteLine($"{subs[0]}\t{subs[1]}\t{subs[2]}\t{subs[3]}\t{subs[4]}");
            }
        }

    }
}
