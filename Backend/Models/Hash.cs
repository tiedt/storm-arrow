using System;
using System.Security.Cryptography;
using System.Text;

namespace SIMP.Models{

    public static class Hash{
        
        // Tentativa 1
        // Método não utilizado, pois para um mesmo salt, ele gerava resultados diferentes
        //private string CreateSalt(int size){
        //    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        //    byte[] buff = new byte[size];
        //    rng.GetBytes(buff);
        //    return Convert.ToBase64String(buff);
        //}
        
        // Tentativa 2
        private static string CreateSalt(string salt){
            //return encryptString(size.ToString());

            HashAlgorithm hashAlgorithm = SHA512.Create();
            var encodedValue = Encoding.UTF8.GetBytes(salt);
            var encryptedPassword = hashAlgorithm.ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var caracter in encryptedPassword){
                sb.Append(caracter.ToString("X2"));
            }

            return sb.ToString();
        }

        private static string EncryptString(string txt){
            
            HashAlgorithm hashAlgorithm = SHA512.Create();
            var encodedValue = Encoding.UTF8.GetBytes(txt);
            var encryptedPassword = hashAlgorithm.ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var caracter in encryptedPassword){
                sb.Append(caracter.ToString("X2"));
            }
            return sb.ToString();
        }

        public static string EncryptStringSalt(string txt, string salt){
            return EncryptString(txt) + CreateSalt(salt);
        }

        public static bool CompareStrings(string txt1, string txt2, string salt){            
            if (string.IsNullOrEmpty(txt1))
                return false;
            return EncryptStringSalt(txt1, salt).Equals(txt2);
        }

    }
}
