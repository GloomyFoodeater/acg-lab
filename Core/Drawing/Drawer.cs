using System.Diagnostics;
using System.Drawing;
using Core.Elements;
using static System.Math;

namespace Core;

public class Drawer
{
    private readonly BufferedBitmap _buffer;

    public Drawer(Bitmap bitmap) => _buffer = new BufferedBitmap(bitmap);

    #region Вспомогательные функции

    // Алгоритм Брезенхема
    private void DrawLine(int xStart, int yStart, int xEnd, int yEnd)
    {
        // Длины проекции на оси абсцисс и ординат
        int dx = xEnd - xStart;
        int dy = yEnd - yStart;

        // Определение сторон сдвига
        int incx = Sign(dx); // -1 для справа налево и +1 для слеванаправо
        int incy = Sign(dy); // -1 для снизу вверх и +1 для сверху вниз

        // Получение абсолютных длин проекций
        dx = Abs(dx);
        dy = Abs(dy);


        int x;
        int y;
        int pdx;
        int pdy;
        int es;
        int el;
        int err;

        // Определение направления прохода в цикле в зависимости от вытянутости
        if (dx > dy)
        {
            // Отрезок более длинный, чем высокий
            pdx = incx;
            pdy = 0;

            es = dy;
            el = dx;
        }
        else
        {
            // Отрезок более высокий, чем длинный
            pdx = 0;
            pdy = incy;

            es = dx;
            el = dy;
        }

        x = xStart;
        y = yStart;
        err = el / 2;



        // Цикл растеризации
        _buffer[x, y] = Color.Black;
        for (int i = 0; i < el; i++)
        {
            err -= es;
            if (err < 0)
            {
                err += el;

                // Cдвинуть прямую (сместить вверх или вниз, если цикл проходит по иксам
                // или сместить влево-вправо, если цикл проходит по y)
                x += incx;
                y += incy;
            }
            else
            {
                // Продолжить тянуть прямую дальше (сдвинуть влево или вправо, если
                // цикл идёт по иксу; сдвинуть вверх или вниз, если по y)
                x += pdx;
                y += pdy;
            }
            _buffer[x, y] = Color.Black;
        }
    }

    #endregion

    public void DrawLab1(Model model)
    {
#if DEBUG
        var start = DateTime.Now;
#endif
        model.Recalculate(_buffer.Width, _buffer.Height);

        Parallel.ForEach(model.Faces, face =>
        {
            var v0 = model.ViewportVertices[face.Indeces[0].V];
            var v1 = model.ViewportVertices[face.Indeces[1].V];
            var v2 = model.ViewportVertices[face.Indeces[2].V];

            DrawLine((int)v0.X, (int)v0.Y, (int)v1.X, (int)v1.Y);
            DrawLine((int)v0.X, (int)v0.Y, (int)v2.X, (int)v2.Y);
            DrawLine((int)v2.X, (int)v2.Y, (int)v1.X, (int)v1.Y);

        });

        _buffer.Flush();
#if DEBUG
        Debug.WriteLine($"Drawn in {DateTime.Now - start}");
#endif
    }

    public void DrawLab2(Model model)
    {
        throw new NotImplementedException();
    }

    public void DrawLab3(Model model)
    {
        throw new NotImplementedException();
    }

    public void DrawLab4(Model model)
    {
        throw new NotImplementedException();
    }
}