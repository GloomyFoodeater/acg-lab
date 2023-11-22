using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static System.Drawing.Image;

namespace Core;

internal class BufferedBitmap
{
    private readonly int depth;         // Число байт на пиксель
    
    private readonly byte[] buffer;     // Буфер для работы с битмапом
    
    private readonly double[,] zBuffer; // Буфер для отсекания невидимых полигонов

    private readonly Bitmap _bitmap;

    public int Width { get; }

    public int Height { get; }

    public BufferedBitmap(Bitmap bitmap)
    {
        _bitmap = bitmap;
        Width = _bitmap.Width;
        Height = _bitmap.Height;
        depth = GetPixelFormatSize(_bitmap.PixelFormat) / 8;
        
        buffer = new byte[Width * Height * depth];
        zBuffer = new double[Width, Height];

        Reset();
    }

    private void Reset()
    {
        Array.Fill<byte>(buffer, 255);
        for (int i = 0; i < Height; i++)
            for (int j = 0; j < Width; j++)
                zBuffer[j, i] = int.MinValue;
    }

    public Color this[int x, int y]
    {
        get
        {
            var offset = (y * Width + x) * depth;
            return Color.FromArgb(buffer[offset], buffer[offset + 1], buffer[offset + 2]);
        }
        set
        {

            if (x > 0 && x < Width && y > 0 && y < Height)
            {
                var offset = (y * Width + x) * depth;
                buffer[offset] = value.R;
                buffer[offset + 1] = value.G;
                buffer[offset + 2] = value.B;
            }
        }
    }

    public bool PutZValue(int x, int y, double z)
    {
        double existedZ = zBuffer[x, y];
        if (existedZ < z)
        {
            zBuffer[x, y] = z;
            return true;
        }
        return false;
    }

    // Перенос данных из буфера в битмап
    public void Flush()
    {
        var data = _bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, _bitmap.PixelFormat);
        Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
        _bitmap.UnlockBits(data);

        Reset();
    }
}
