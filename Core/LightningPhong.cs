
using System.Numerics;
using System.Drawing;
using static System.Math;
using System.Runtime.CompilerServices;

namespace Core.Elements
{
    public class LightingPhong
    {

        // Интенсивность фонового освещения
        Vector3 iamb = new(0f, 1.0f, 0f);
        Vector3 idif = new(1.0f, 0f, 0f);
        Vector3 ispec = new(0f, 0f, 1.0f);

        // Коффициент фонового освещения(свойство материала)
        Vector3 kAmbient = new(0.1f, 0.1f, 0.1f);
        Vector3 kDiffuse = new(0.5f, 0.5f, 0.5f);
        Vector3 kSpecular = new(1.0f, 1.0f, 1.0f);


        public static Vector3[] lightPos = {
            new Vector3(0f, 0f, 15f),
            new Vector3(10f, 0f, 15f),
            new Vector3(-10f, 0f, 15f),
            new Vector3(20f, 0f, 15f)
        };

        double specular_power = 32; // Коффициент блеска поверхности

        // Интенсивности освещения
        Color La = Color.FromArgb(0, 0, 0);
        Color Ld = Color.FromArgb(0, 0, 0);
        Color Ls = Color.FromArgb(0, 0, 0);

        // Обратный вектор направления света для первого источника света
        Vector3 lightDirection = Vector3.Normalize(new(lightPos[0].X, lightPos[0].Y, lightPos[0].Z));

        public LightingPhong()
        {
        }

        // Реализация цветов освещений

        // Фоновое освещение
        private void Ambient() => La = ToColor(255 * iamb * kAmbient);

        // Фонга 
        /*
         * Передоваемая нормаль уже нормализована и в мировых координатах
         * TODO реализвать совокупность нескольких источников света
         */
        private void Diffuse(Vector3 normal)
        {
            // Попытка рассчётов в Vector3


            float temp = Max(Vector3.Dot(normal, lightDirection), 0.0f);// Косинус угла между нормалью и вектором направления света
            Ld = ToColor(temp * idif * kDiffuse);
        }

        // Бликовое
        //TODO требует доработки
        private void Specular(Vector3 worldPos, Vector3 normal)
        {

            // Вычисление R: R = L - 2 * (L * N) * N
            // Reflect -- возвращает отражение вектора от поверхности, которая имеет заданную нормаль.
            var reflection = Vector3.Normalize(Vector3.Reflect(-lightDirection, normal));
            var viewDirection = Vector3.Normalize(-worldPos);// Вектор направленный в камеру от точки(0,0,0)

            float RV = Max(Vector3.Dot(reflection, viewDirection), 0.0f);
            float temp = (float)Pow(RV, specular_power);

            Ls = ToColor(temp * ispec * kSpecular);
        }

        /* Итоговое получение цвета на основе трёх компонентов освещения
         * vertex -- координата точки
         * normal -- координата её нормали        
         */
        public Color GetColor(Vector3 worldPos, Vector3 normal)
        {
            // Перевод в V3 и нормализация
            worldPos = Vector3.Normalize(worldPos);
            normal = Vector3.Normalize(normal);

            //viewDir = Vector3.Normalize(view - vertexByV3);

            Ambient();
            // Передаётся уже нормализованное значение нормали 
            Diffuse(normal);
            Specular(worldPos, normal);

            int Lred = Min((La.R + Ld.R + Ls.R), 255);
            int Lgreen = Min((La.G + Ld.G + Ls.G), 255);
            int Lblue = Min((La.B + Ld.B + Ls.B), 255);
            return Color.FromArgb(Lred, Lgreen, Lblue);
        }

        private static Color ToColor(Vector3 v) => Color.FromArgb(Min((int)v.X, 255), Min((int)v.Y, 255), Min((int)v.Z, 255));

    }
}
