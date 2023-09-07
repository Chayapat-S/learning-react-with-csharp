using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace api.Infrastructure
{
    public static class ExtensionMethods
    {
        public static string MakingZipFile(string startingFilePath)
        {
            string returnFileZipPath = string.Empty;
            try
            {
                FileInfo file = new FileInfo(startingFilePath);
                using (FileStream originalFileStream = file.OpenRead())
                {
                    if ((File.GetAttributes(file.FullName) &
                         FileAttributes.Hidden) != FileAttributes.Hidden & file.Extension != ".zip")
                    {
                        using (FileStream compressedFileStream = File.Create(file.FullName + ".zip"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                                CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }

                        returnFileZipPath = startingFilePath + ".zip";
                        FileInfo info = new FileInfo(startingFilePath + ".zip");
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                returnFileZipPath = string.Empty;
            }

            return returnFileZipPath;
        }

        public static T Next<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            bool flag = false;

            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (flag) return enumerator.Current;

                    if (predicate(enumerator.Current))
                    {
                        flag = true;
                    }
                }
            }

            return default(T);
        }

        public static IEnumerable<Triplet<T>> Tripletize<T>(this IEnumerable<T> source)
        {
            using (IEnumerator<T> iter = source.GetEnumerator())
            {
                // read the first triple
                if (!iter.MoveNext()) yield break;
                T x = iter.Current;
                if (!iter.MoveNext()) yield break;
                T y = iter.Current;
                if (!iter.MoveNext()) yield break;
                T z = iter.Current;

                yield return new Triplet<T>(x, y, z);
                while (iter.MoveNext())
                {
                    x = y;
                    y = z;
                    z = iter.Current;
                    yield return new Triplet<T>(x, y, z);
                }
            }
        }

        public readonly struct Triplet<T>
        {
            public Triplet(T previous, T current, T next)
            {
                Previous = previous;
                Current = current;
                Next = next;
            }

            public T Previous { get; }
            public T Current { get; }
            public T Next { get; }
        }


        public static int ToInt32(this string intString)
        {
            return Convert.ToInt32(intString);
        }

        //public async static Task<string> GetImageAsBase64Url(string url)
        //{
        //    var credentials = new NetworkCredential(user, pw);
        //    using (var handler = new HttpClientHandler { Credentials = credentials })
        //    using (var client = new HttpClient(handler))
        //    {
        //        var bytes = await client.GetByteArrayAsync(url);
        //        return "image/jpeg;base64," + Convert.ToBase64String(bytes);
        //    }
        //}

        public static decimal ConvertSatangToBaht(this decimal satang)
        {
            return satang / 100.00m;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static bool ValidateToken(this string token, string secretKey)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(secretKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public static bool ContainsAllItems<T>(List<T> a, List<T> b)
        {
            return !b.Except(a).Any();
        }

    }
}