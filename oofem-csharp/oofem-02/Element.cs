public class Element
{
    public double E;
    public double A;
    public Node Node1;
    public Node Node2;

    public Element(double e, double a, Node n1, Node n2)
    {
        this.E = e;
        this.A = a;
        this.Node1 = n1;
        this.Node2 = n2;
    }

    public double Length()
    {
        return (this.Node2.Position - this.Node1.Position).Norm(2);
    }

    public Vector D()
    {
        return 1 / this.Length() * (this.Node2.Position - this.Node1.Position);
    }

    public Vector T()
    {
        Vector e1 = this.D();
        return Vector.Build.DenseOfEnumerable((-e1).Concat(e1));
    }

    public Matrix K()
    {
        return this.E * this.A / this.Length() * Vector.OuterProduct(this.T(), this.T());
    }

    public override string ToString()
    {
        return $"{this.E,15:0}{this.A,15:0.0000##}{this.Length(),15:0.0000}";
    }

}