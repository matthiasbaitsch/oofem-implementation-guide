public class Structure
{
    public List<Node> Nodes = [];
    public List<Element> Elements = [];

    public Node AddNode(double x1, double x2)
    {
        Node n = new Node(x1, x2);
        this.Nodes.Add(n);
        return n;
    }

    public Element AddElement(double e, double a, int n1, int n2)
    {
        Node node1 = this.Nodes[n1];
        Node node2 = this.Nodes[n2];
        Element element = new Element(e, a, node1, node2);
        this.Elements.Add(element);
        return element;
    }

    public void Print()
    {
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        Console.WriteLine("                          N O D E S");
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        Console.WriteLine($"         Position               Constraint       Force");
        Console.WriteLine($"         X           Y          Cx     Cy        Fx          Fy");
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        for (int i = 0; i < this.Nodes.Count; i++)
        {
            Console.WriteLine($"{i,3}{this.Nodes[i]}");
        }
        Console.WriteLine("──────────────────────────────────────────────────────────────────");

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        Console.WriteLine("                         E L E M E N T S");
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        Console.WriteLine($"                 E              A");
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        for (int i = 0; i < this.Elements.Count; i++)
        {
            Console.WriteLine($"{i,3}{this.Elements[i]}");
        }
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
    }
}