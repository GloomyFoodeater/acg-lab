using System.Numerics;
using static System.Numerics.Matrix4x4;

namespace Core.Utils;

public static class VectorMath
{
    public static Vector3 Vector4to3(Vector4 v) => new(v.X, v.Y, v.Z);

    public static Vector3 ToWorldFromModel(
        Vector3 modelVector,
        float ShiftX,
        float ShiftY,
        float RotationOfXInRadians,
        float RotationOfYInRadians,
        float Scale)
    {
        // Сначала вращение, затем повороты, лишь потом масштабирование
        var worldMatrix = CreateTranslation(ShiftX, ShiftY, 0)
            * CreateRotationY(RotationOfXInRadians)
            * CreateRotationX(RotationOfYInRadians)
            * CreateRotationZ(0)
            * CreateScale(Scale);

        return Vector3.Transform(modelVector, worldMatrix);
    }

    /// <summary>
    /// Преобразование из мирового простанства в пространство наблюдателя
    /// </summary>
    /// <param name="eye">Позиция камеры в мировом пространстве</param>
    /// <param name="target">Позиция цели, на которую направлена камера</param>
    /// <param name="up">Вектор, направленный вертикально вверх с точки зрения камеры</param>
    public static Vector3 ToObserverFromWorld(
        Vector3 worldVector,
        Vector4 eye,
        Vector4 target,
        Vector4 up)
    {
        var zAxis = Vector4.Normalize(eye - target);
        var v3Cross = Vector3.Cross(Vector4to3(up), Vector4to3(zAxis));
        var xAxis = Vector4.Normalize(new Vector4(v3Cross, 1));
        var yAxis = up;
        var viewMatrix = new Matrix4x4
        {
            M11 = xAxis.X,
            M12 = yAxis.X,
            M13 = zAxis.X,
            M21 = xAxis.Y,
            M22 = yAxis.Y,
            M23 = zAxis.Y,
            M31 = xAxis.Z,
            M32 = yAxis.Z,
            M33 = zAxis.Z,
            M41 = -Vector4.Dot(xAxis, eye),
            M42 = -Vector4.Dot(yAxis, eye),
            M43 = -Vector4.Dot(zAxis, eye),
            M44 = 1
        };
        return Vector3.Transform(worldVector, viewMatrix);
    }

    // Преобразование из пространства наблюдателя в пространство проекции.
    public static Vector3 ToProjectionFromObserver(
        Vector3 observerVector,
        float fov,
        float aspect,
        float zNear,
        float zFar)
    {

        var sx = (float)(1 / Math.Tan(fov / 2) / aspect);
        var sy = (float)(1 / Math.Tan(fov / 2));
        var sz = zFar / (zNear - zFar);
        var dz = zNear * zFar / (zNear - zFar);

        var projectionMatrix = new Matrix4x4
        {
            M11 = sx,
            M22 = sy,
            M33 = sz,
            M43 = dz,
            M34 = -1
        };

        var projectionVector = Vector4.Transform(observerVector, projectionMatrix);
        projectionVector.X /= projectionVector.W;
        projectionVector.Y /= projectionVector.W;
        projectionVector.Z /= projectionVector.W;
        projectionVector.W /= projectionVector.W;

        return Vector4to3(projectionVector);
    }

    // Преобразование из пространства проекции в пространство окна просмотра.
    public static Vector3 ToViewportFromProjection(
        Vector3 projectionVector,
        float width,
        float height)
    {
        var viewportMatrix = new Matrix4x4
        {
            M11 = width / 2,
            M41 = width / 2,
            M33 = 1,
            M22 = -height / 2,
            M42 = height / 2,
            M44 = 1
        };

        return Vector3.Transform(projectionVector, viewportMatrix);
    }
}
