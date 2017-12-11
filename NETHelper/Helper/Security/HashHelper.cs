﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Helper.Security
{
    public static class HashHelper
    {
        private const int Pbkdf2Count = 1000;

        private const int Pbkdf2SubkeyLength = 256 / 8;

        private const int SaltSize = 128 / 8;

        public static string GenerateSalt(int byteLength = SaltSize)
        {
            byte[] buffer = new byte[byteLength];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(buffer);
            }

            return Convert.ToBase64String(buffer);
        }

        public static string Hash(string input, string algorithm = "sha256")
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            return Hash(Encoding.UTF8.GetBytes(input), algorithm);
        }

        public static string Hash(byte[] input, string algorithm = "sha256")
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            using (HashAlgorithm alg = HashAlgorithm.Create(algorithm))
            {
                if (alg == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not supported hash alg", algorithm));
                }

                byte[] hashData = alg.ComputeHash(input);
                return BinaryToHex(hashData);
            }
        }

        public static string Sha1(string input)
        {
            return Hash(input, "sha1");
        }

        public static string Sha256(string input)
        {
            return Hash(input);
        }

        public static string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Pbkdf2Count))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(Pbkdf2SubkeyLength);
            }

            byte[] outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException("hashedPassword");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            if (hashedPasswordBytes.Length != (1 + SaltSize + Pbkdf2SubkeyLength) || hashedPasswordBytes[0] != (byte)0x00)
            {
                return false;
            }

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            var storedSubkey = new byte[Pbkdf2SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, Pbkdf2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Pbkdf2Count))
            {
                generatedSubkey = deriveBytes.GetBytes(Pbkdf2SubkeyLength);
            }

            return ByteArraysEqual(storedSubkey, generatedSubkey);
        }

        internal static string BinaryToHex(byte[] data)
        {
            var hex = new char[data.Length * 2];

            for (int iter = 0; iter < data.Length; iter++)
            {
                var hexChar = (byte)(data[iter] >> 4);
                hex[iter * 2] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
                hexChar = (byte)(data[iter] & 0xF);
                hex[(iter * 2) + 1] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
            }

            return new string(hex);
        }

        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool areSame = true;

            for (int i = 0; i < a.Length; i++)
            {
                areSame &= a[i] == b[i];
            }

            return areSame;
        }
    }
}
