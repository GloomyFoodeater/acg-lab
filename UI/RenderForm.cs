using Core;
using Core.Elements;
using static System.Math;

namespace UI;

public partial class RenderForm : Form
{
    #region Переменные для интеракции с формой
    private int previousX;
    private int previousY;
    private bool rotateMode;
    #endregion

    #region Ограничения на изменение положения модели на сцене
    private const float _deltaShiftX = 0.2f;
    private const float _deltaShiftY = 0.2f;
    private const float _deltaScale = 0.1f;
    private const float _minScale = 0.001f;
    private const float _maxScale = 2.0f;
    #endregion

    private readonly ModelContext _context = new(
        Eye: new(0, 0, 10),
        Target: new(0, 0, -1),
        Up: new(0, 1, 0),
        LightDir: new(0, 0, 1),
        Fov: (float)(20 * PI / 180),
        Zfar: 100,
        FlatShadingColor: Color.FromArgb(120, 170, 255));

    private Drawer? _drawer;
    private Model? _model;

    public RenderForm()
    {
        InitializeComponent();
        LabComboBox.SelectedIndex = 2;
        ResizeImage();
    }

    private void ResizeImage()
    {
        try
        {
            var bitmap = new Bitmap(PictureBox.Width, PictureBox.Height);
            PictureBox.Image = bitmap;
            _drawer = new(bitmap);
        }
        catch
        {
            PictureBox.Image = null;
            _drawer = null;
        }
    }

    private void Redraw()
    {
        if (_model == null || _drawer == null) return;

        ScaleValue.Text = $"Масштаб: {_model.Scale:F3}";
        PoligonSize.Text = $"Количество полигонов: {_model.Faces.Count}";

        switch (LabComboBox.SelectedIndex)
        {
            case 0: _drawer.DrawLab1(_model); break;
            case 1: _drawer.DrawLab2(_model); break;
            case 2: _drawer.DrawLab3(_model); break;
            default: throw new Exception("Unexpected combobox value");
        }
        PictureBox.Refresh();
    }

    private void ResetModelPosition()
    {
        if (_model == null) return;

        _model.Scale = _minScale;
        _model.ShiftX = 0;
        _model.ShiftY = 0;
        _model.RotationOfXInRadians = 1;
        _model.RotationOfYInRadians = 1;
    }

    #region Обработчики событий

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        if (_model == null) return;
        var newScale = _model.Scale + _deltaScale * Sign(e.Delta) * _model.Scale;
        _model.Scale = Min(_maxScale, Max(_minScale, newScale));
        Redraw();
    }

    private void PictureBox_MouseDown(object sender, MouseEventArgs e)
    {
        if (_model == null) return;

        rotateMode = true;

        // Сохранение предыдущих координат мыши
        previousX = e.X;
        previousY = e.Y;
    }

    private void PictureBox_MouseMove(object sender, MouseEventArgs e)
    {
        if (!rotateMode || _model == null) return;

        var width = PictureBox.Image.Width;
        var height = PictureBox.Image.Height;

        // Поворот модели
        _model.RotationOfXInRadians += (e.X - previousX) / (float)(width / 2);
        _model.RotationOfYInRadians += (e.Y - previousY) / (float)(height / 2);

        // Сохранение предыдущих координат мыши
        previousX = e.X;
        previousY = e.Y;

        Redraw();
    }

    private void PictureBox_MouseUp(object sender, MouseEventArgs e)
    {
        if (_model == null) return;

        rotateMode = false;

        Redraw();
    }

    private void PictureBox_MoveModel(object sender, KeyEventArgs e)
    {
        if (_model == null) return;

        // Сдвиги модели по WASD
        switch ((Keys)e.KeyValue)
        {
            case Keys.W:
                {
                    _model.ShiftY += _deltaShiftY;
                    break;
                }
            case Keys.S:
                {
                    _model.ShiftY -= _deltaShiftY;
                    break;
                }
            case Keys.A:
                {
                    _model.ShiftX -= _deltaShiftX;
                    break;
                }
            case Keys.D:
                {
                    _model.ShiftX += _deltaShiftX;
                    break;
                }
            case Keys.Q:
                {
                    _model.Scale = _minScale;
                    break;
                }
            case Keys.F:
                {
                    _model.Scale = _maxScale;
                    break;
                }
            case Keys.F1:
                {
                    ResetModelPosition();
                    break;
                }
        }
        Redraw();
    }

    private void OpenButton_Click(object sender, EventArgs e)
    {
        try
        {
            var openDialog = new OpenFileDialog() { Filter = "OBJ geometry format(*.obj)|*.obj" };
            if (openDialog.ShowDialog() != DialogResult.OK) return;

            var path = openDialog.FileName;


            var parser = new ObjParser(_context);
            _model = parser.Parse(path);
            ResetModelPosition();

            FilePath.Text = $"Путь: {path}";
            Redraw();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void StretchTextbox_OnChange(object sender, EventArgs e)
    {
        var textBox = (TextBox)sender;
        var size = TextRenderer.MeasureText(textBox.Text, textBox.Font);
        textBox.Width = size.Width;
        textBox.Height = size.Height;
    }

    private void StyleComboBox_SelectedIndexChanged(object sender, EventArgs e) => Redraw();

    private void PictureBox_SizeChanged(object sender, EventArgs e)
    {
        ResizeImage();
        Redraw();
    }
    #endregion
}
