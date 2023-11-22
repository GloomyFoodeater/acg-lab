using System.Numerics;
using Core.Elements;
using static System.Globalization.CultureInfo;

namespace Core;

public class ObjParser
{
    // Объект для создания новой модели из внешних по отношению к файлу данных
    private readonly ModelContext _context;
    public ObjParser(ModelContext context) => _context = context;

    private static Vector3 ParseVertice(string[] lexems)
    {
        if (lexems.Length < 4) throw new IOException("Invalid geometric vertices syntax");

        var x = float.Parse(lexems[1], InvariantCulture);
        var y = float.Parse(lexems[2], InvariantCulture);
        var z = float.Parse(lexems[3], InvariantCulture);

        return new Vector3(x, y, z);
    }

    private static Vector3 ParseNormal(string[] lexems)
    {
        if (lexems.Length < 4) throw new IOException("Invalid vertex normal syntax");

        var i = float.Parse(lexems[1], InvariantCulture);
        var j = float.Parse(lexems[2], InvariantCulture);
        var k = float.Parse(lexems[3], InvariantCulture);

        return new Vector3(i, j, k);
    }

    // Перевод индексов вершин в файле в индексы списка заданной длины
    private static int ShiftIndex(int index, int length) => index > 0 ? index - 1 : length + index;

    private static Face ParseFace(string[] lexems, List<Vector3> listV, List<Vector3> listVn)
    {
        if (lexems.Length > 4) throw new NotSupportedException("Faces can only be triangles");
        if (lexems.Length < 4) throw new IOException("Invalid face syntax");

        var idx = 0;
        var entries = new Vertice[3];
        foreach (var lexem in lexems.Skip(1))
        {
            // Разбить лексему на тройку индексов
            var indeces = lexem
                .Split('/', StringSplitOptions.TrimEntries)
                .ToArray();

            if (indeces.Length < 2) throw new NotSupportedException("Faces must contain normals");

            var v = ShiftIndex(int.Parse(indeces[0]), listV.Count); // Поиск v
            var vn = ShiftIndex(int.Parse(indeces[2]), listVn.Count); // Поиск vn

            // Запись новой вершины
            entries[idx++] = new Vertice(v, vn);
        }

        return new Face(entries);
    }

    public Model Parse(string path)
    {
        var listF = new List<Face>();
        var listV = new List<Vector3>();
        var listVn = new List<Vector3>();

        string? line;
        using var sw = new StreamReader(path);
        while ((line = sw.ReadLine()) != null)
        {
            // Игнор комментария
            var commentStart = line.IndexOf('#');
            if (commentStart != -1) line = line[..commentStart];

            // Деление по лексемам с пустыми лексемами и без пробельных
            var lexems = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (lexems.Length == 0) continue;

            switch (lexems[0])
            {
                case "v":
                    listV.Add(ParseVertice(lexems));
                    break;
                case "vn":
                    listVn.Add(ParseNormal(lexems));
                    break;
                case "f":
                    listF.Add(ParseFace(lexems, listV, listVn));
                    break;
                default:
                    break;
            }
        }
        return new Model(_context, listV, listVn, listF);
    }
}
