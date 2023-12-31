﻿using System.Drawing;
using System.Numerics;

namespace Core;

/// <summary>
/// Константы для прорисовки
/// </summary>
/// <param name="Eye">Позиция камеры в мировом пространстве</param>
/// <param name="Target">Позиция цели, на которую направлена камера</param>
/// <param name="Up">Вектор, направленный вертикально вверх с точки зрения камеры</param>
/// <param name="LightDir">Вектор направление света</param>
/// <param name="Fov">Угол обзора камеры в радианах</param>
public record class ModelContext(
    Vector3 Eye,
    Vector3 Target,
    Vector3 Up,
    Vector3 LightDir,
    float Fov,
    int Zfar,
    Color FlatShadingColor);

