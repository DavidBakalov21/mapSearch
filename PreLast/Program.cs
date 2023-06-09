using System.Diagnostics;
using System.Globalization;
using PreLast;



using System.Diagnostics;
Rect ToRect(double Radius, double latitude, double longitude)
{
    var resRect = new Rect(new Point("name", latitude - Radius, longitude - Radius),
        new Point("name", latitude + Radius, longitude + Radius));
    return resRect;
}


var sw = new Stopwatch();
sw.Start();

var words = File.ReadAllLines("C:\\Users\\Давід\\RiderProjects\\PreLast\\PreLast\\ukraine_poi.csv");
var latitude1 = Console.ReadLine();
var longitude1 = Console.ReadLine();
var radius = Console.ReadLine();
var RadiusRect = ToRect(double.Parse(radius), double.Parse(latitude1), double.Parse(longitude1));
var latList = new List<double>();
var longList = new List<double>();
var Points = new List<Point>();
for (int i = 0; i < words.Length; i++)
{
    
    var split = words[i].Split(";");
   
   
    var latitude = split[0];
    var longitude = split[1];
    if (latitude != "" && longitude != "" && latitude != null && longitude != null)
    {
        var t = split.Length;
        var name = "";
        while (split[t-1]=="")
        {
            if (split[t-1]!="")
            {
                break;
            }

            t -= 1;
        }
       
            name = split[t-1];
      
        latList.Add(double.Parse(latitude));
        longList.Add(double.Parse(longitude));
        Points.Add(new Point(name, double.Parse(latitude),double.Parse(longitude)));
    }
}

var bottom_leftPoint= new Point("bottom_left", latList.Min(),longList.Min() );
var top_rightPoint= new Point("top_right", latList.Max(),longList.Max() );
KdTree buildTree(List<Point> pointList, Rect StartRect)
{
    KdTree kdTree = new KdTree();
    kdTree.BuildTree(pointList, StartRect);
    return kdTree;
}

var StartRect = new Rect(bottom_leftPoint, top_rightPoint);
var a = buildTree(Points,StartRect);
var FinList = a.Traverse(RadiusRect, new Point("name", double.Parse(latitude1), double.Parse(longitude1)), double.Parse(radius));

foreach (var VARIABLE in FinList)
{
    var output = VARIABLE.name+" " + VARIABLE.latitude+" "+ VARIABLE.longitude;
     Console.WriteLine(output);
}
sw.Stop();
Console.WriteLine($"Elapsed time: {sw.Elapsed}");
class Node
{
    public List<Point> value { get; set; }
   
    public Rect rectvalue { get; set; }
    public Node left { get; set; }
    public Node right { get; set; }

    public Node( Rect RectVal)
    {
        rectvalue = RectVal;
        value = new List<Point>();
        left = null;
        right = null;
    }
}
class KdTree
{
    public Node Root { get; set; }

    public KdTree()
    {
        Root = null;
    }

    public void BuildTree(List<Point> pointList, Rect strtRect)
    {
        Root = BuildTreeRecursive(pointList, 0, strtRect);
    }

    private Node BuildTreeRecursive(List<Point> pointList, int depth, Rect startRect)
    {
        if (pointList.Count == 0)
            return null;

        if (pointList.Count <= 2)
        {
            Node leafNode = new Node(startRect);
            leafNode.value.AddRange(pointList);
            return leafNode;
        }

        int axis = depth % 2;
        pointList.Sort((a, b) => axis == 0 ? a.longitude.CompareTo(b.longitude) : a.latitude.CompareTo(b.latitude));

        int medianIndex = pointList.Count / 2;
        Point medianPoint = pointList[medianIndex];

        Rect newRectL, newRectR;
        if (axis == 0)
        {
            newRectL = new Rect(startRect.BottomLeft, new Point("name", medianPoint.latitude, startRect.TopRight.longitude));
            newRectR = new Rect(new Point("name", medianPoint.latitude, startRect.BottomLeft.longitude), startRect.TopRight);
        }
        else
        {
            newRectL = new Rect(new Point("name", startRect.BottomLeft.latitude, medianPoint.longitude), startRect.TopRight);
            newRectR = new Rect(startRect.BottomLeft, new Point("name", startRect.TopRight.latitude, medianPoint.longitude));
        }

        Node node = new Node(startRect);
        node.left = BuildTreeRecursive(pointList.GetRange(0, medianIndex), depth + 1, newRectL);
        node.right = BuildTreeRecursive(pointList.GetRange(medianIndex + 1, pointList.Count - medianIndex - 1), depth + 1, newRectR);

        return node;
    }

    public List<Point> Traverse(Rect rect, Point point, double radius)
    {
        if (Root == null || !rect.Intersect(Root.rectvalue))
        {
            return new List<Point>();
        }

        List<Point> resultList = new List<Point>();

        TraverseRecursive(rect, point, Root, radius, resultList);

        return resultList;
    }

    private void TraverseRecursive(Rect rect, Point point, Node node, double radius, List<Point> resultList)
    {
        if (node == null || !rect.Intersect(node.rectvalue))
        {
            return;
        }

        if (node.left == null && node.right == null)
        {
            foreach (var p in node.value)
            {
                double distance = CalculateDistance(point.latitude, point.longitude, p.latitude, p.longitude);
                if (distance / 1000 <= radius)
                {
                    resultList.Add(p);
                }
            }
        }
        else
        {
            TraverseRecursive(rect, point, node.left, radius, resultList);
            TraverseRecursive(rect, point, node.right, radius, resultList);
        }
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double rLat = (lat2 - lat1) * (Math.PI / 180);
        double rLon = (lon2 - lon1) * (Math.PI / 180);
        double a = Math.Sin(rLat / 2) * Math.Sin(rLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(rLon / 2) * Math.Sin(rLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = 6371e3 * c;
        return distance;
    }
}