using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace GWF
{
    /// <summary>
    /// 썸네일 이미지 헬퍼
    /// 
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 주어진 이미지와 주어진 사이즈에 대한 썸네일 반환
        /// 
        /// 주의! 가로/세로 비율 유지함
        /// </summary>
        /// <param name="imgPhoto">대상 이미지</param>
        /// <param name="Width">조정할 가로 사이즈</param>
        /// <param name="Height">조정할 세로 사이즈</param>
        /// <returns>섬네일 이미지</returns>
        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
            {
                grPhoto.Clear(Color.White);
                grPhoto.InterpolationMode =
                        InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);
            }
            return bmPhoto;
        }

        /// <summary>
        /// 이미지 퍼센트단위로 줄이기
        /// </summary>
        /// <param name="imgPhoto">이미지 원본</param>
        /// <param name="percent">줄이고싶은 퍼센트단위</param>
        /// <returns></returns>
        public static Image FixedSize(Image imgPhoto, float percent)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            float nPercent = percent / 100;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
            {
                grPhoto.Clear(Color.White);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.DrawImage(imgPhoto,new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
            }

            return bmPhoto;
        }


        /// <summary>
        /// 확장자에 따른 이미지 파일 여부 반환
        /// </summary>
        /// <param name="name">파일명</param>
        /// <returns>이미지 파일이면 true</returns>
        public static bool IsImageFile(string name)
        {
            string ret = name.ToLower();
            if (ret.EndsWith(".jpeg"))
                return true;
            else if (ret.EndsWith(".jpg"))
                return true;
            else if (ret.EndsWith(".png"))
                return true;
            else if (ret.EndsWith(".gif"))
                return true;

            return false;
        }

        /// <summary>
        /// 주어진 파일명으로 썸네일 이미지 파일명을 리턴(파일명만 있는 경우 사용)
        /// </summary>
        /// <param name="imagefilename"></param>
        /// <returns></returns>
        public static string GetThumbnailFileName(string imagefilename)
        {
            return "tmb_" + Path.GetFileNameWithoutExtension(imagefilename) + ".jpg";
        }

        /// <summary>
        /// 주어진 경로와 파일명으로 썸네일 이미지 패스를 리턴(패스와 파일명이 모두 있는 경우)
        /// </summary>
        /// <param name="fullpathfile"></param>
        /// <returns></returns>
        public static string GetThumbnailFullPathFileName(string fullpathfile)
        {
            return Path.Combine(Path.GetDirectoryName(fullpathfile), "tmb_" + Path.GetFileNameWithoutExtension(fullpathfile) + ".jpg");
        }

        /// <summary>
        /// 주어진 경로의 이미지를 압축(스케일링)
        /// </summary>
        /// <param name="fullpath">정체 경로</param>
        /// <param name="quality">퀄리티(1~100)</param>
        /// <param name="THUMBNAIL_IMAGE_SCALE_PERCENT">스케일 비율</param>
        /// <returns>저장 성공 시 저장된 파일 경로</returns>
        public static string ImageCompress(string fullpath, int quality, int THUMBNAIL_IMAGE_SCALE_PERCENT)
        {
            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            if (string.IsNullOrEmpty(fullpath) || !File.Exists(fullpath))
                return string.Empty;

            string path = Path.GetDirectoryName(fullpath);
            string filename = Path.GetFileNameWithoutExtension(fullpath);

            try
            {
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(fullpath))
                {
                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    myEncoder = Encoder.Quality;

                    string targetfile = Path.Combine(path, "tmb_" + filename + ".jpg");
                    using (myEncoderParameters = new EncoderParameters(1))
                    {
                        myEncoderParameter = new EncoderParameter(myEncoder, ConvertHelper.ToInt64(THUMBNAIL_IMAGE_SCALE_PERCENT, 0 < quality ? (quality < 101 ? quality : 100) : 1));
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        img.Save(targetfile, myImageCodecInfo, myEncoderParameters);
                    }
                    return targetfile;
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.ErrorFormat("image scale compr err : {0}", ex.ToString());
            }
            return string.Empty;
        }

        /// <summary>
        /// 주어진 경로의 이미지의 썸네일 생성
        /// </summary>
        /// <param name="fullpath">썸네일을 생성할 이미지 파일</param>
        /// <param name="width">비율을 유지하는 가로 사이즈(0보다 커야함)</param>
        /// <param name="height">비율을 유지하는 세로 사이즈(0보다 커야함)</param>
        /// <param name="originalWidth">원본 이미지 가로 사이즈</param>
        /// <param name="originalHeight">원본 이미지 세로 사이즈</param>
        /// <param name="THUMBNAIL_IMAGE_SCALE_PERCENT">스케일 비율</param>
        /// <returns>생성된 썸네일 전체 경로</returns>
        public static string ImageThumbnail(string fullpath, int width, int height, out int originalWidth, out int originalHeight, int THUMBNAIL_IMAGE_SCALE_PERCENT)
        {
            originalWidth = 0;
            originalHeight = 0;

            if (!File.Exists(fullpath))
                return string.Empty;

            string thumbnailfile = GetThumbnailFullPathFileName(fullpath);

            using (Image imgSrc = Image.FromFile(fullpath))
            {
                using (Image imgThumb = FixedSize(imgSrc, width, height))
                {
                    ImageCodecInfo myImageCodecInfo;
                    Encoder myEncoder;
                    EncoderParameter myEncoderParameter;
                    EncoderParameters myEncoderParameters;

                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    myEncoder = Encoder.Quality;

                    using (myEncoderParameters = new EncoderParameters(1))
                    {
                        myEncoderParameter = new EncoderParameter(myEncoder, ConvertHelper.ToInt64(THUMBNAIL_IMAGE_SCALE_PERCENT, 80));
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        imgThumb.Save(thumbnailfile, myImageCodecInfo, myEncoderParameters);
                    }

                    originalWidth = imgSrc.Width;
                    originalHeight = imgSrc.Height;

                    return thumbnailfile;
                }
            }
        }


        /// <summary>
        /// 주어진 경로의 이미지의 썸네일 생성
        /// </summary>
        /// <param name="fullpath">썸네일을 생성할 이미지 파일</param>
        /// <param name="percent">줄이고싶은 사이즈 퍼센트</param>
        /// <param name="Querity">퀄리티</param>
        /// <returns>생성된 썸네일 전체 경로</returns>
        public static bool ImageThumbnail(string ImageFullPath, int ScalePercent, int Querity, out string tmbFilePath, out string tmbFileName, out long tmbSize)
        {
            tmbFilePath = string.Empty;
            tmbFileName = string.Empty;
            tmbSize = 0;

            if (!File.Exists(ImageFullPath))
                return false;

            string ThumbnailFullPath = GetThumbnailFullPathFileName(ImageFullPath);
            
            using (Image imgSrc = Image.FromFile(ImageFullPath))
            {
                using (Image imgThumb = FixedSize(imgSrc, ScalePercent))
                {
                    ImageCodecInfo myImageCodecInfo;
                    Encoder myEncoder;
                    EncoderParameter myEncoderParameter;
                    EncoderParameters myEncoderParameters;

                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    myEncoder = Encoder.Quality;

                    using (myEncoderParameters = new EncoderParameters(1))
                    {
                        myEncoderParameter = new EncoderParameter(myEncoder, ConvertHelper.ToInt64(Querity, 80));
                        myEncoderParameters.Param[0] = myEncoderParameter;

                        if (File.Exists(ThumbnailFullPath))
                            File.Delete(ThumbnailFullPath);
                                                                                            
                        imgThumb.Save(ThumbnailFullPath, myImageCodecInfo, myEncoderParameters);
                    }

                    tmbFilePath = ThumbnailFullPath;
                    tmbFileName = Path.GetFileName(ThumbnailFullPath);
                    tmbSize = IOHelper.getFileSize(ThumbnailFullPath);
                }
            }

            return true;
        }

        /// <summary>
        /// mimetype에 대한 이미지 처리 코덱 정보 리턴
        /// </summary>
        /// <param name="mimeType">MineType</param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}
