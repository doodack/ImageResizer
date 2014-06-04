using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace DigitalCreations
{
	/// <summary>
	/// Provides methods for resizing images.
	/// </summary>
	public class ImageResizer : IDisposable
	{
		private readonly Image image;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageResizer"/> class.
		/// </summary>
		/// <param name="sourcePath">The path to the source image.</param>
		/// <exception cref="System.IO.FileLoadException">The requested image could not be loaded.</exception>
		public ImageResizer(string sourcePath)
		{
			try
			{
				this.image = new Bitmap(sourcePath);
			}
			catch (Exception ex)
			{
				throw new FileLoadException("The requested image could not be loaded.", ex);
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ImageResizer"/> class.
		/// </summary>
		/// <param name="source">The image to resize.</param>
		public ImageResizer(Image source)
		{
			this.image = source;
		}

		/// <summary>
		/// Resizes the image to the specified dimensions.
		/// If height is not set, it preserves the aspect ratio.
		/// </summary>
		/// <param name="width">Maximum width of the image.</param>
		/// <param name="height">Maximum height of the image.</param>
		public Image ResizeTo(int width, int? height = null)
		{
			int originalWidth = this.image.Width;
			int originalHeight = this.image.Height;

			int targetHeight = height ?? (width / originalWidth) * originalHeight;

			Bitmap target = new Bitmap(width, targetHeight);
			using (Graphics gfx = Graphics.FromImage(target))
			{
				gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gfx.SmoothingMode = SmoothingMode.HighQuality;
				gfx.DrawImage(this.image, 0, 0, width, targetHeight);
			}

			return target;
		}

		/// <summary>
		/// Resizes the image so it is not larger than specified dimensions.
		/// Preserves the aspect ratio.
		/// </summary>
		/// <param name="maxWidth">Maximum width of the image.</param>
		/// <param name="maxHeight">Maximum height of the image.</param>
		public Image ResizeToFit(int? maxWidth, int? maxHeight)
		{
			int originalWidth = this.image.Width;
			int originalHeight = this.image.Height;

			if ((!maxHeight.HasValue && !maxWidth.HasValue) || (originalHeight <= maxHeight.Value && originalWidth <= maxWidth.Value))
			{
				return this.image;
			}

			int targetHeight;
			int targetWidth;

			float widthResizeFactor = originalWidth / (float)maxWidth.GetValueOrDefault(int.MaxValue);
			float heightResizeFactor = originalHeight / (float)maxHeight.GetValueOrDefault(int.MaxValue);

			if (widthResizeFactor > heightResizeFactor)
			{
				targetWidth = maxWidth.Value;
				targetHeight = (int)(originalHeight / widthResizeFactor + 0.5);
			}
			else
			{
				targetHeight = maxHeight.Value;
				targetWidth = (int)(originalWidth / heightResizeFactor + 0.5);
			}

			Bitmap target = new Bitmap(targetWidth, targetHeight);
			using (Graphics gfx = Graphics.FromImage(target))
			{
				gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gfx.SmoothingMode = SmoothingMode.HighQuality;
				gfx.DrawImage(this.image, 0, 0, targetWidth, targetHeight);
			}

			return target;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (this.image != null)
			{
				this.image.Dispose();
			}

			GC.SuppressFinalize(this);
		}
	}
}
