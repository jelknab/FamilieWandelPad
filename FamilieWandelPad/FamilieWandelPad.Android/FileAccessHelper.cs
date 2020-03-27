using System;
using System.IO;
using Android.App;

namespace FamilieWandelPad.Droid
{
    public class FileAccessHelper
    {
        public static string MakeAssetAvailable(string filename)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(path, filename);

            var names = Application.Context.Assets.List("");

            CopyFile(dbPath, filename);

            return dbPath;
        }

        private static void CopyFile(string dbPath, string filename)
        {
            using var br = new BinaryReader(Application.Context.Assets.Open(filename));
            using var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create));

            var buffer = new byte[2048];
            var length = 0;
            while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
            {
                bw.Write(buffer, 0, length);
            }
        }
    }
}