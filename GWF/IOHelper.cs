using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace GWF
{
    /// <summary>
    /// 텍스트 파일 Helper
    /// 
    /// 작성자 : Elian Song, wiluby@gmail.com
    /// </summary>
    public class IOHelper
    {
        /// <summary>
        /// 주어진 경로에 텍스트 파일을 읽음
        /// </summary>
        /// <param name="sPath">읽어올 파일 경로</param>
        /// <param name="enc">읽어올 파일의 인코딩</param>
        /// <returns>파일 내용</returns>
        public static string TextFileRead(string sPath, Encoding enc = null)
        {
            string sFileText = string.Empty;

            try
            {
                if (File.Exists(sPath))
                {
                    StreamReader sr = new StreamReader(sPath, enc ?? Encoding.Default);

                    sFileText = sr.ReadToEnd();

                    sr.Close();
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return sFileText;
        }

        /// <summary>
        /// 주어진 경로에 문자열을 텍스트 파일로 저장
        /// </summary>
        /// <param name="sPath">저장 경로</param>
        /// <param name="sMsg">저장 내용</param>
        public static bool TextFileWrite(string sPath, string sMsg, Encoding enc = null)
        {
            try
            {
                FileStream aFile = new FileStream(sPath, FileMode.OpenOrCreate);
                aFile.Position = aFile.Length;
                StreamWriter sw = new StreamWriter(aFile, enc ?? Encoding.Default);

                sw.Write(sMsg);
                sw.Close();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// 주어진 경로에 문자열을 텍스트 파일로 저장 비동기
        /// </summary>
        /// <param name="sPath">저장 경로</param>
        /// <param name="sMsg">저장 내용</param>
        public static async Task<bool> TextFileWriteAsync(string sPath, string sMsg, Encoding enc = null)
        {
            try
            {
                FileStream aFile = new FileStream(sPath, FileMode.OpenOrCreate);
                aFile.Position = aFile.Length;
                StreamWriter sw = new StreamWriter(aFile, enc ?? Encoding.Default);

                await sw.WriteAsync(sMsg);
                sw.Close();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// 파일을 복사
        /// </summary>
        /// <param name="copyFile"></param>
        /// <param name="copyFolderPath"></param>
        /// <param name="overwrite"></param>
        public static void CopyFile(FileInfo copyFile, string copyFolderPath, bool overwrite = true)
        {
            if (!Directory.Exists(copyFolderPath))
                Directory.CreateDirectory(copyFolderPath);

            copyFile.CopyTo(Path.Combine(copyFolderPath, copyFile.Name), overwrite);
        }

        /// <summary>
        /// 소스 폴더의 서브 폴더 및 파일들을 타켓 폴더로 복사한다.
        /// 파일이 있으면 덮어씌움
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="targetFolder"></param>
        /// <param name="overwrite"></param>
        public static void CopyFolder(string sourceFolder, string targetFolder, bool overwrite = true)
        {
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            foreach(string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(targetFolder, name);

                if (File.Exists(dest) && overwrite == false)
                    continue;

                File.Copy(file, dest, overwrite);
            }

            foreach(string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(targetFolder, name);
                CopyFolder(folder, dest, overwrite);
            }
        }

        /// <summary>
        /// 폴더내의 디렉토리 및 파일들을 삭제한다.
        /// </summary>
        /// <param name="folderPath"></param>
        public static void DeleteFolder(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);
            string[] folders = Directory.GetDirectories(folderPath);

            foreach(string file in files)
            {
                File.Delete(file);
            }

            foreach(string folder in folders)
            {
                DeleteFolder(folder);
            }

            if (Directory.GetFiles(folderPath).Length == 0)
                Directory.Delete(folderPath);
        }

        public static long getFileSize(string filefullPath)
        {
            if(!File.Exists(filefullPath))
            {
                return 0;
            }

            return (new FileInfo(filefullPath)).Length;
        }
    }
}
