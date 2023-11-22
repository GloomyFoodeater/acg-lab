using System.Numerics;
using static Core.Utils.VectorMath;


namespace Core.Elements;

public partial class Model
{
    private readonly List<Vector4> _worldVertices;
    private readonly List<Vector4> _observerVertices;
    private readonly List<Vector4> _projectionVertices;
    private readonly List<Vector4> _viewportVertices;

    public Model(
        ModelContext context,
        List<Vector4> listV,
        List<Vector3> listVn,
        List<Face> faces)
    {
        Context = context;

        ModelVertices = listV;
        _worldVertices = new(listV);
        _observerVertices = new(listV);
        _projectionVertices = new(listV);
        _viewportVertices = new(listV);

        Normals = listVn;

        Faces = faces;
    }

    public ModelContext Context { get; }

    #region Позиция на сцене
    public float ShiftX { get; set; } = 0;

    public float ShiftY { get; set; } = 0;

    public float RotationOfXInRadians { get; set; } = 0;

    public float RotationOfYInRadians { get; set; } = 0;

    public float Scale { get; set; } = 1;
    #endregion

    #region Координаты геометрических вершин в разных пространствах
    public IReadOnlyList<Vector4> ModelVertices { get; }
    public IReadOnlyList<Vector4> WorldVertices => _worldVertices;
    public IReadOnlyList<Vector4> ObserverVertices => _observerVertices; 
    public IReadOnlyList<Vector4> ProjectionVertices => _projectionVertices;
    public IReadOnlyList<Vector4> ViewportVertices => _viewportVertices;
    #endregion

    public IReadOnlyList<Vector3> Normals { get; }

    public IReadOnlyList<Face> Faces { get; }

    /// <summary>
    /// Пересчёт координат с учётом произведённых переносов
    /// </summary>
    /// <param name="width">Текущая ширина экрана</param>
    /// <param name="height">Текущая высота экрана</param>
    public void Recalculate(int width, int height)
    {
        float aspect = width / (float)height;
        int zNear = width;

        var res = Parallel.For(0, ModelVertices.Count, i =>
        {
            _worldVertices[i] = ToWorldFromModel(
                ModelVertices[i],
                ShiftX,
                ShiftY,
                RotationOfXInRadians,
                RotationOfYInRadians,
                Scale);
            _observerVertices[i] = ToObserverFromWorld(
                WorldVertices[i],
                Context.Eye,
                Context.Target,
                Context.Up);
            _projectionVertices[i] = ToProjectionFromObserver(
                ObserverVertices[i],
                Context.Fov,
                aspect,
                zNear,
                Context.Zfar);
            _viewportVertices[i] = ToViewportFromProjection(
                ProjectionVertices[i],
                width,
                height);
        });
    }
}
