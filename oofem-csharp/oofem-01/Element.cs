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

    public override string ToString()
    {
        return $"{this.E,15:0}{this.A,15:0.0000##}";
    }


}