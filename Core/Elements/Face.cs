
using Core.Elements;

namespace Core;

public class Face
{
    public Vertice[] Indeces { get; }

    public Face(IEnumerable<Vertice> vertices) => Indeces = vertices.ToArray();
}
