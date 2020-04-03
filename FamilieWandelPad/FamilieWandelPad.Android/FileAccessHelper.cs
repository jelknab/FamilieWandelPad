using System;
using System.IO;
using FamilieWandelPad.Droid;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(FileAccessHelper))]
namespace FamilieWandelPad.Droid
{
    public class FileAccessHelper : IFileAccessHelper
    {

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

        string IFileAccessHelper.MakeAssetAvailable(string fileName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var outputPath = Path.Combine(path, fileName);

            CopyFile(outputPath, fileName);

            return outputPath;
        }
    }
}