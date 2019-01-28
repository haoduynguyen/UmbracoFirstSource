using System;
using System.Collections.Generic;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

/// <summary>
/// Summary description for ImageResize
/// </summary>
/// 
namespace Customs.Helper
{
    public class ImageResize
    {
        private string filepath = "";
        private int maxWidth;
        private int maxHeight;
        private System.Drawing.Image origImage = null;
        private System.Drawing.Image thumbnail = null;
        public ImageResize(string files)
        {
            this.filepath = HttpContext.Current.Server.MapPath(files);
            if (!System.IO.File.Exists(this.filepath)) this.filepath = null;
        }
        public void Save(string filename)
        {
            if (thumbnail != null)
            {
                try
                {
                    FileInfo fs = new FileInfo(filename);

                    if (fs.Extension.ToLower() == ".jpg" || fs.Extension.ToLower() == ".jpeg")
                    {
                        EncoderParameters encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(Encoder.Compression, 100);
                        thumbnail.Save(HttpContext.Current.Server.MapPath(filename), ImageCodecInfo.GetImageEncoders()[1], encoderParameters);
                    }
                    else
                    {
                        thumbnail.Save(HttpContext.Current.Server.MapPath(filename), thumbnail.RawFormat);
                    }
                }
                finally
                {
                    thumbnail.Dispose();
                    origImage.Dispose();
                }
            }
        }
        public void ImageResizer(int intNewWidth, int intNewHeight)
        {
            if (!string.IsNullOrWhiteSpace(this.filepath))
            {
                origImage = System.Drawing.Image.FromFile(this.filepath);
                decimal lnRatio;
                if (origImage.Width <= intNewWidth && origImage.Height <= intNewHeight)
                    this.thumbnail = origImage;
                else
                {
                    if (intNewWidth > intNewHeight)
                    {
                        lnRatio = (decimal)intNewWidth / origImage.Width;
                        decimal lnTemp = origImage.Height * lnRatio;
                        intNewHeight = (int)lnTemp;

                    }
                    else
                    {
                        lnRatio = (decimal)intNewHeight / origImage.Height;
                        decimal lnTemp = origImage.Width * lnRatio;
                        intNewWidth = (int)lnTemp;

                    }
                    thumbnail = new System.Drawing.Bitmap(intNewWidth, intNewHeight);
                    System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(thumbnail);

                    //set quality properties
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphic.DrawImage(origImage, new Rectangle(0, 0, intNewWidth, intNewHeight));
                    graphic.Dispose();
                }
            }
        }
        public void ImageResizerH(int intNewHeight)
        {
            if (!string.IsNullOrWhiteSpace(this.filepath))
            {
                origImage = System.Drawing.Image.FromFile(this.filepath);
                int intNewWidth = 0;
                if (origImage.Height > intNewHeight)
                {
                    float xscale = (float)origImage.Width / intNewWidth;
                    float yscale = (float)origImage.Height / intNewHeight;
                    intNewWidth = (int)(origImage.Width * (1 / yscale));
                    intNewHeight = (int)(origImage.Height * (1 / yscale));

                    thumbnail = new System.Drawing.Bitmap(intNewWidth, intNewHeight);
                    System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(thumbnail);

                    //set quality properties
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphic.DrawImage(origImage, 0, 0, intNewWidth, intNewHeight);
                    graphic.Dispose();
                }
                else
                {
                    this.thumbnail = origImage;
                }
            }

        }
        public void ImageResizer(int intNewWidth)
        {
            if (!string.IsNullOrWhiteSpace(this.filepath))
            {
                origImage = System.Drawing.Image.FromFile(this.filepath);
                int intNewHeight = 0;
                if (origImage.Width > intNewWidth)
                {
                    float xscale = (float)origImage.Width / intNewWidth;
                    float yscale = (float)origImage.Height / intNewHeight;
                    intNewWidth = (int)(origImage.Width * (1 / xscale));
                    intNewHeight = (int)(origImage.Height * (1 / xscale));

                    thumbnail = new System.Drawing.Bitmap(intNewWidth, intNewHeight);
                    System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(thumbnail);

                    //set quality properties
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphic.DrawImage(origImage, 0, 0, intNewWidth, intNewHeight);
                    graphic.Dispose();
                }
                else
                {
                    this.thumbnail = origImage;
                }
            }
        }
        public void AdaptiveResize(int intNewWidth, int intNewHeight)
        {
            if (!string.IsNullOrWhiteSpace(this.filepath))
            {
                origImage = GetImage(this.filepath);
                decimal lnRatio;
                if (origImage.Width <= intNewWidth && origImage.Height <= intNewHeight)
                    this.thumbnail = origImage;
                else
                {
                    maxWidth = intNewWidth;
                    maxHeight = intNewHeight;
                    if (intNewWidth > intNewHeight)
                    {
                        lnRatio = (decimal)intNewWidth / origImage.Width;
                        decimal lnTemp = origImage.Height * lnRatio;
                        maxHeight = (int)lnTemp;

                    }
                    else
                    {
                        lnRatio = (decimal)intNewHeight / origImage.Height;
                        decimal lnTemp = origImage.Width * lnRatio;
                        maxWidth = (int)lnTemp;

                    }
                    int cropX = intNewWidth > maxWidth ? (int)(intNewWidth - maxWidth) : 2;
                    int cropY = intNewHeight > maxHeight ? (int)(intNewHeight - maxHeight) : 2;

                    thumbnail = new System.Drawing.Bitmap(intNewWidth, intNewHeight);

                    System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(thumbnail);

                    //set quality properties
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphic.DrawImage(origImage, 0, 0, maxWidth, maxHeight);
                    graphic.Dispose();
                }
            }
        }

        //The crop image sub
        public void CropImage(int Width, int Height, int StartAtX, int StartAtY)
        {
            if (!string.IsNullOrWhiteSpace(this.filepath))
            {
                try
                {
                    origImage = System.Drawing.Image.FromFile(this.filepath);
                    //check the image height against our desired image height
                    if (origImage.Height < Height)
                    {
                        Height = origImage.Height;
                    }
                    if (origImage.Width < Width)
                    {
                        Width = origImage.Width;
                    }
                    thumbnail = new System.Drawing.Bitmap(Width, Height);

                    System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(thumbnail);
                    System.Drawing.Imaging.ImageCodecInfo[] Info = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                    System.Drawing.Imaging.EncoderParameters Params = new System.Drawing.Imaging.EncoderParameters(1);
                    Params.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

                    //set quality properties
                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    graphic.DrawImage(origImage, new Rectangle(0, 0, Width, Height), StartAtX, StartAtY, Width, Height, GraphicsUnit.Pixel);
                    graphic.Dispose();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error cropping image, the error was: " + ex.Message);
                }
            }
        }
        private System.Drawing.Image GetImage(string sFilePath)
        {

            System.Drawing.Image img = System.Drawing.Image.FromFile(this.filepath);
            /*FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs, true, true);
            fs.Close();*/
            return img;
        }
    }
}