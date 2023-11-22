using System.Numerics;
using static Core.Utils.VectorMath;


namespace Core.Elements;

public partial class Model
{
    private readonly List<Vector3> _worldVertices;
    private readonly List<Vector3> _observerVertices;
    private readonly List<Vector3> _projectionVertices;
    private readonly List<Vector3> _viewportVertices;

    private readonly List<Vector3> _worldNormals;
    private readonly List<Vector3> _observerNormals;
    private readonly List<Vector3> _projectionNormals;

    public Model(
        ModelContext context,
        List<Vector3> listV,
        List<Vector3> listVn,
        List<Face> faces)
    {
        Context = context;

        ModelVertices = listV;
        _worldVertices = new(listV);
        _observerVertices = new(listV);
        _projectionVertices = new(listV);
        _viewportVertices = new(listV);

        ModelNormals = listVn;
        _worldNormals = new(listVn);
        _observerNormals = new(listVn);
        _projectionNormals = new(listVn);

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
    public IReadOnlyList<Vector3> ModelVertices { get; }
    public IReadOnlyList<Vector3> WorldVertices => _worldVertices;
    public IReadOnlyList<Vector3> ObserverVertices => _observerVertices;
    public IReadOnlyList<Vector3> ProjectionVertices => _projectionVertices;
    public IReadOnlyList<Vector3> ViewportVertices => _viewportVertices;
    #endregion

    #region Координаты нормалей вершин в разных пространствах
    public IReadOnlyList<Vector3> ModelNormals { get; }
    public IReadOnlyList<Vector3> WorldNormals => _worldNormals;
    public IReadOnlyList<Vector3> ObserverNormals => _observerNormals;
    public IReadOnlyList<Vector3> ProjectionNormals => _projectionNormals;
    #endregion

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

            _worldNormals[i] = ToWorldFromModel(
                ModelNormals[i],
                ShiftX,
                ShiftY,
                RotationOfXInRadians,
                RotationOfYInRadians,
                Scale);
            _observerNormals[i] = ToObserverFromWorld(
                WorldNormals[i],
                Context.Eye,
                Context.Target,
                Context.Up);
            _projectionNormals[i] = ToProjectionFromObserver(
                ObserverNormals[i],
                Context.Fov,
                aspect,
                zNear,
                Context.Zfar);
        });
    }
}
