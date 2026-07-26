using BoDraw;

public class Visualizer
{
    public Structure Structure;

    public double ForceScale = 1;
    public double ConstraintSize = 1;

    public Visualizer(Structure structure)
    {
        this.Structure = structure;
    }

    public void DrawSystem(IBoDraw bd)
    {
        // Elements
        foreach (Element element in this.Structure.Elements)
        {
            double x11 = element.Node1.Position[0];
            double x12 = element.Node1.Position[1];
            double x21 = element.Node2.Position[0];
            double x22 = element.Node2.Position[1];

            bd.Add(new Line(x11, x12, x21, x22).WithThickness(3));
        }

        // Constraints
        double a = this.ConstraintSize / 2;
        double b = 0.886 * this.ConstraintSize;

        foreach (Node node in this.Structure.Nodes)
        {
            double x1 = node.Position[0];
            double x2 = node.Position[1];

            if (node.Constraint.Fixed[0])
            {
                bd.Add(new Polygon(x1, x2, x1 - b, x2 - a, x1 - b, x2 + a));
            }
            if (node.Constraint.Fixed[1])
            {
                bd.Add(new Polygon(x1, x2, x1 - a, x2 - b, x1 + a, x2 - b));
            }
        }

        // Forces
        foreach (Node node in this.Structure.Nodes)
        {
            var f = node.Force.Components;

            if (f.Norm(2) != 0)
            {
                var p1 = node.Position;
                var p2 = p1 + this.ForceScale * f;

                bd.Add(new Arrow(p1[0], p1[1], p2[0], p2[1]).WithColor(Colors.Red).WithThickness(1.5));
            }
        }
    }
}